using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantStep : MonoBehaviour
{
    private CameraShake cameraShake;

    void Start()
    {
        cameraShake = Camera.main.GetComponent<CameraShake>();
    }

    
    void Update()
    {
     
    }
    public void TriggerShake()
    {
        if (cameraShake != null)
        {
            // Calcula a distância entre inimigo e câmera
            float distance = Vector3.Distance(Camera.main.transform.position, transform.position);
            float intensity = Mathf.Clamp(1 / distance, 0.05f, 0.2f);

            // Inicia o tremor
            StartCoroutine(cameraShake.Shake(0.3f, intensity));
        }
    }
}
