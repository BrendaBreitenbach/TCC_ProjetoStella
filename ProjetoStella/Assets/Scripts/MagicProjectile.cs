using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicProjectile : MonoBehaviour
{

    [Header("Attack Config")]
    public int amoutDamage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Chegou no inimigo");
        // Checa se o objeto está na layer "Enemy"
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            other.GetComponent<GiantIA>().GetHit(amoutDamage);
            Destroy(gameObject);
        }
    }

}
