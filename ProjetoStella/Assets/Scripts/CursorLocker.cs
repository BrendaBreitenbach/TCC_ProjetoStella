using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorLocker : MonoBehaviour
{

    [SerializeField] private bool lockOnStart = true;

    // Start is called before the first frame update
    void Start()
    {
        if (lockOnStart)
            LockCursor();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            UnlockCursor();

        // Exemplo: L para travar de novo
        if (Input.GetKeyDown(KeyCode.L))
            LockCursor();
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;                   
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;  
        Cursor.visible = true;                   
    }
}
