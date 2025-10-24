using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;
    private Transform cam;

    private bool isFloor;
    private bool isWalk;
    private bool isFlying;

    private float forcaY;
    private float wingSpeed = 1f;
    [SerializeField] private float maxFlyHeight;
    private float velocidade = 2f;

    [SerializeField] private Transform feetStella;
    [SerializeField] private LayerMask colisaoLayer;
    public int amoutDamage;



    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        cam = Camera.main.transform;
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movimento = new Vector3(horizontal, 0f, vertical);

        if( isFlying)
        {
            velocidade = 10f;
        } else
        {
            velocidade = 3f;
        }

        // Caminhada
        movimento = cam.TransformDirection(movimento); 
        movimento.y = 0; 
        controller.Move(movimento * Time.deltaTime * velocidade); 
        if (movimento != Vector3.zero) 
        { 
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movimento), Time.deltaTime * 10); 
            isWalk = true; 
        } else 
        { 
            isWalk = false; 
        } 
        animator.SetBool("isWalk", isWalk);


        // Verifica se esta no chão
        isFloor = Physics.CheckSphere(feetStella.position, 0.3f, colisaoLayer);
        animator.SetBool("isFloor", isFloor);

        // Pulo
        if (Input.GetKeyDown(KeyCode.Space) && isFloor)
        {
            forcaY = 5f;
            animator.SetTrigger("jump");
        }

        //Voo
        if (Input.GetKeyDown(KeyCode.F))
        {
            isFlying = !isFlying;
            animator.SetBool("isFlying", isFlying);
            if (isFlying)
            {
                forcaY = 5f;
            }
        }

        if (isFlying)
        {
            if (transform.position.y < maxFlyHeight)
            {
                forcaY = 3f;
            }
            else
            {
                forcaY = 0f;
                Vector3 pos = transform.position;
                pos.y = maxFlyHeight;
                transform.position = pos;
            }
            wingSpeed = 7f;
        }
        else
        {
            if (!isFloor)
            {
                forcaY += -9.81f * Time.deltaTime;
                wingSpeed = 5f;
            }
            else
            {
                wingSpeed = 1f;
            }
        }

        
        if (Input.GetButtonDown("Fire1"))
        {
            if (gameObject.GetComponent<WandCollector>().wandCollected)
            {
                animator.SetTrigger("attack");
            }
            
        }

        animator.SetFloat("wingSpeed", wingSpeed);

        controller.Move(new Vector3(0, forcaY, 0) * Time.deltaTime);
    }
}
