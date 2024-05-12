using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.AI;

public class CandaceAI : MonoBehaviour
{

    NavMeshAgent navMeshAgentCandace;
    public Transform player; // Locates the player game object
    public DeathHandler deathHandlerScript;
    private CandaceAnimations candaceAnimations;
    [HideInInspector] public bool playerDetected = false;
    [HideInInspector] public bool playerCaught = false;


    private float wanderRadius;
    private float wanderTimer;


    [Header("Wander Settings")]
    public float minWanderRadius = 5f;
    public float maxWanderRadius = 10f;

    public float minWanderTimer = 3f;
    public float maxWanderTimer = 6f;
    private float defaultMoveSpeed;

    private Transform target;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = wanderTimer;
        navMeshAgentCandace = GetComponent<NavMeshAgent>();
        candaceAnimations = GetComponent<CandaceAnimations>();

        SetRandomWanderRadius();
        SetRandomWanderTimer();
        candaceAnimations.Idle();

        defaultMoveSpeed = navMeshAgentCandace.speed; // Store the default speed set in the beginning of the level
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerDetected)
        {
            Wander();
        }
        else
        {
            ChasePlayer();
        }

        IdleCheck();
    }

    // Set a new random wander radius
    void SetRandomWanderRadius()
    {
        wanderRadius = Random.Range(minWanderRadius, maxWanderRadius);
    }

    // Set a new random wander timer
    void SetRandomWanderTimer()
    {
        wanderTimer = Random.Range(minWanderTimer, maxWanderTimer);
    }


    private void IdleCheck() 
    {
        if (navMeshAgentCandace.velocity.magnitude < 0.1f && !navMeshAgentCandace.pathPending)
        {
        //Debug.LogWarning("STOPP"); //desperate debug
        candaceAnimations.StopWander();
        candaceAnimations.Idle();
        return;
        }
    }

    public void ChasePlayer()
    {
        candaceAnimations.Chase();
        navMeshAgentCandace.speed = 4.5f;
        navMeshAgentCandace.SetDestination(player.position);
    }

    private void Wander()
    {
        if (playerCaught == true) // Stop wandering
        {
            return;
        }

        candaceAnimations.StopChase();
        candaceAnimations.Wander();

        timer += Time.deltaTime;
        navMeshAgentCandace.speed = defaultMoveSpeed;

        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            navMeshAgentCandace.SetDestination(newPos);
            timer = 0;
            SetRandomWanderTimer(); // Set a new random wander timer after reaching the current destination
        }
    }

    public void PlayerCaught()
    {
        playerCaught = true;
        candaceAnimations.Caught();
        navMeshAgentCandace.speed = 0; 
    }


    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}

