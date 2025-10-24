using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandCollector : MonoBehaviour
{

    public Transform mao;
    private Animator animator;
    public bool wandCollected;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "VarinhaMagica")
        {
            collision.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            collision.gameObject.GetComponent<BoxCollider>().enabled = false;
            collision.transform.position = mao.position;
            collision.transform.rotation = mao.rotation;

            collision.transform.SetParent(mao);
            wandCollected = true;
            animator.SetLayerWeight(2, 1);
           
        }
         
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
