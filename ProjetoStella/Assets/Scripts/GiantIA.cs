using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GiantIA : MonoBehaviour
{
    private GameManager _gameManager;
    private Animator animator;

    [Header("Enemy Config")]
    public int HP;
    private bool isDie;

    public enemyState state;

    //public const float idleWaitTime = 5f;
    //public const float patrolWaitTime = 20f;

    public GameObject rockPrefab;
    public GameObject poeiraPrefab;

    //IA 
    private bool isWalk;
    private bool isAlert;
    private NavMeshAgent agent;
    private Vector3 destination;
    private int idWayPoint;
    private bool isPlayerVisible;
    private bool isAttack;


    // Start is called before the first frame update
    void Start()
    {
        _gameManager = FindObjectOfType(typeof(GameManager)) as GameManager;
        agent = GetComponent<NavMeshAgent>();

        animator = GetComponent<Animator>();
        ChangeState(state);

        agent.speed = 1.0f;
        agent.acceleration = 4f;     
        agent.angularSpeed = 120f;


    }

    // Update is called once per frame
    void Update()
    {
        stateManager();

        if (agent.desiredVelocity.magnitude >= 0.1f)
        {
            isWalk = true;
            //print(isWalk);
        }
        else 
        {
            isWalk = false;
            //print(isWalk);
        }
        
        animator.SetBool("isWalk", isWalk);
        animator.SetBool("isAlert", isAlert);

    }

    IEnumerator Died()
    {
        isDie = true;
        yield return new WaitForSeconds(0f);

        Vector3 pos = transform.position + Vector3.up * 1.5f;
        Quaternion rot = transform.rotation;

        Destroy(this.gameObject);

        GameObject rock = Instantiate(rockPrefab, pos, rot);

        Rigidbody rb = rock.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;

        GameObject poeiraInst = Instantiate(poeiraPrefab, pos + Vector3.down * 1.75f, Quaternion.identity);
        Destroy(poeiraInst, 3f);

    }



    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            isPlayerVisible = true;
            if(state == enemyState.IDLE || state == enemyState.PATROL)
            {
                ChangeState(enemyState.ALERT);
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            isPlayerVisible = false;
        }
    }

    public void GetHit(int amout)
    {
        Debug.Log("Dano recebido!");
        if (isDie == true)  return;
        HP -= amout;
        
        if (HP > 0)
        {
            ChangeState(enemyState.FURY);
            animator.SetTrigger("GetHit");
        }
        else
        {
            StartCoroutine("Died");
        }
    }

    void stateManager()
    {
        switch (state)
        {
            case enemyState.ALERT:

                LookAt();

                break;


            case enemyState.FURY:
                LookAt();
                //print("Ativando furia");
                destination = _gameManager.player.position;
                agent.destination = destination;
                
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    Attack();
                }

                break;


            case enemyState.FOLLOW:
                LookAt();
                destination = _gameManager.player.position;
                agent.destination = destination;
                if(agent.remainingDistance <= agent.stoppingDistance)
                {
                    Attack();
                }

                break;
        }
    }


    void ChangeState(enemyState newState)
    {
        StopAllCoroutines();

        state = newState;   
        
        print(newState);

        switch (state)
        {
            case enemyState.IDLE:
                agent.stoppingDistance = 0.5f;
                destination = transform.position;
                agent.destination = destination;
                StartCoroutine("IDLE");

                break;


            case enemyState.ALERT:

                agent.stoppingDistance = 0.5f;
                destination = transform.position;
                agent.destination = destination;
                isAlert = true;
                //animator.SetTrigger("isAlert");
                StartCoroutine("ALERT");
                
                break;


            case enemyState.PATROL:
                agent.stoppingDistance = 0.5f;
                idWayPoint = Random.Range(0, _gameManager.giantsWayPoints.Length);
                print("id = " + idWayPoint);
                destination = _gameManager.giantsWayPoints[idWayPoint].position;
                agent.destination = destination;

                StartCoroutine("PATROL");

                break;


            case enemyState.FURY:
                destination = transform.position;
                agent.stoppingDistance = _gameManager.giantDistanceAttack;
                agent.destination = destination;
                
                break;


            case enemyState.FOLLOW:
                //isAttack = true;
                agent.stoppingDistance = _gameManager.giantDistanceAttack;
                StartCoroutine("FOLLOW");
                
                break;
        }
    }


    IEnumerator IDLE()
    {

        yield return new WaitForSeconds(_gameManager.giantIdleWaitTime);
        StayStill(50);

    }

    IEnumerator PATROL()
    {
        yield return new WaitUntil(() => agent.remainingDistance <= 0);

        StayStill(30);
    }

    IEnumerator ALERT()
    {
        yield return new WaitForSeconds(_gameManager.giantAlertTime);
        if (isPlayerVisible)
        {
            ChangeState(enemyState.FOLLOW);
            isAlert = false;
        }
        else
        {
            StayStill(10);
        }
    }

    IEnumerator ATTACK()
    {
        yield return new WaitForSeconds(_gameManager.giantAttackDelay);
        isAttack = false;
    }

    IEnumerator FOLLOW()
    {
        yield return new WaitUntil(() => !isPlayerVisible);
        
        print("perdi voce");

        yield return new WaitForSeconds(_gameManager.giantAlertTime);

        StayStill(50);

    }

    void StayStill(int yes)
    {
        if (Rand() < yes)
        {
            ChangeState(enemyState.IDLE);

        }
        else
        {
            ChangeState(enemyState.PATROL);
        }
    }

    int Rand()
    {
        int rand = Random.Range(0, 100);
        print(rand);
        return rand;
    }

    void Attack()
    {
        if (isAttack == false && isPlayerVisible == true)
        {
            isAttack = true;
            animator.SetTrigger("Attack");
        }
        StartCoroutine("ATTACK");
    }

    void AttackIsDone()
    {
        StartCoroutine("ATTACK");
    }

    void LookAt()
    {
        Vector3 lookDirection = (_gameManager.player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, _gameManager.giantLookAtSpeed * Time.deltaTime);
    }

}



