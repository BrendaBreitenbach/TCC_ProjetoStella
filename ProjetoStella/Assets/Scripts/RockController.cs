using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RockDelay());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator RockDelay()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        yield return new WaitForSeconds(0.5f);

        rb.isKinematic = true;
        rb.useGravity = false;
    }
}
