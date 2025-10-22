using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum enemyState
{
    IDLE, ALERT, PATROL, FOLLOW, FURY
}


public class GameManager : MonoBehaviour
{

    public Transform player;

    [Header("GiantIA")]
    public float giantIdleWaitTime;
    public Transform[] giantsWayPoints;
    public float giantDistanceAttack = 1f;
    public float giantAlertTime = 2f;
    public float giantAttackDelay = 1f;
    public float giantLookAtSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
