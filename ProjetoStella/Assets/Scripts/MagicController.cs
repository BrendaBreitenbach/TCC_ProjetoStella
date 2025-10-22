using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MagicController : MonoBehaviour
{

    public Animator animator;

    [Header("Magic Config")]
    public GameObject magicParticlePrefab;
    public Transform spawPointParticle;
    public GameObject magicAttackPrefab;
    public Transform spawnPointAttack;

    public float speed;
    public Transform playerTransform;

    public bool isWand;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        isWand = gameObject.GetComponent<WandCollector>().wandCollected = true;
    }

    public void magicParticle()
    {
        if (isWand)
        {
            //Debug.Log("Magia acionada!");
            GameObject particlePrefab = Instantiate(magicParticlePrefab, spawPointParticle.position, spawPointParticle.rotation);
            Destroy(particlePrefab, 2f);
        }
        
    }

    public void magicAttack()
    {
        if (isWand)
        {
            Vector3 spawnPosition = spawPointParticle.position;

            // Direção frente do player
            //Vector3 direction = playerTransform.forward;

            // Direção frente da camera
            //Vector3 direction = Camera.main.transform.forward;

            // Direção frente camera + player
            Vector3 direction = Vector3.Lerp(playerTransform.forward, Camera.main.transform.forward, 0.7f);
            direction.Normalize();

            GameObject magicPrefab = Instantiate(magicAttackPrefab, spawnPosition, Quaternion.LookRotation(direction));

            Rigidbody rb = magicPrefab.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.velocity = direction * speed;
            }
            Destroy(magicPrefab, 5f);
        }
        
    }


}
