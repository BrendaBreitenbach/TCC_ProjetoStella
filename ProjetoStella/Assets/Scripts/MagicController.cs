using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicController : MonoBehaviour
{
    public Animator animator;
    public ParticleSystem magic;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // Verifica se o state atual tem a tag "Magia"
        if (stateInfo.IsTag("Magic"))
        {
            if (!magic.isPlaying)
                magic.Play();
        }
        else
        {
            if (magic.isPlaying)
                magic.Stop();
        }
    }
}
