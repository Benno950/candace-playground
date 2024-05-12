using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.AI;

public class BallPitMonsterAI : MonoBehaviour
{

    NavMeshAgent navMeshAgentBallPitMonster;
    public Transform player; // Locates the player game object
    public DeathHandler deathHandlerScript;
    [HideInInspector] public bool playerDetected = false;
    [HideInInspector] public bool playerCaught = false;
    public Animator ballpitMonsterAnimations;

    
    private float wanderRadius;
    private float wanderTimer;



    [Header("Wander Settings")]
    public float minWanderRadius = 5f;
    public float maxWanderRadius = 10f;

    public float minWanderTimer = 3f;
    public float maxWanderTimer = 6f;

    [Header("Aggression Settings")]
    [Tooltip("If the player stays in the ball pit for set time the monster will become extremely aggressive")]
    public float timeToAggression = 5f;
    public float aggressiveSpeed = 20f;
    public float aggressiveAcceleration = 15f;
    public float aggressiveTurnSpeed = 250f;
    private float aggressionTimeRemaining;

    private float defaultSpeed;
    private float defaultAcceleration;
    private float defaultTurnSpeed;

    private Transform target;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = wanderTimer;
        navMeshAgentBallPitMonster = GetComponent<NavMeshAgent>();
        SetRandomWanderRadius();
        SetRandomWanderTimer();

        aggressionTimeRemaining = timeToAggression;

        defaultSpeed = navMeshAgentBallPitMonster.speed;
        defaultAcceleration = navMeshAgentBallPitMonster.acceleration;
        defaultTurnSpeed = navMeshAgentBallPitMonster.angularSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(aggressionTimeRemaining);
        if (!playerDetected)
        {
            Wander();
        }
        else
        {
            ChasePlayer();
        }
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

    public void ChasePlayer()
    {
        navMeshAgentBallPitMonster.SetDestination(player.position);
    }

    public void AggressionTimer()
    {
        aggressionTimeRemaining -= Time.deltaTime;

        // Check if the timer has reached zero
        if (aggressionTimeRemaining <= 0f)
        {
            IncreaseAggression();
        }
    }

    public void IncreaseAggression() 
    {
        Debug.LogWarning("Monster is angry Now");
        navMeshAgentBallPitMonster.speed = aggressiveSpeed;
        navMeshAgentBallPitMonster.acceleration = aggressiveAcceleration;
        navMeshAgentBallPitMonster.angularSpeed = aggressiveTurnSpeed;
        ballpitMonsterAnimations.SetBool("AngryMoveSpeed", true);
    }

    public void ResetAggression() 
    {
        Debug.LogWarning("Monster is calm again");
        navMeshAgentBallPitMonster.speed = defaultSpeed;
        navMeshAgentBallPitMonster.acceleration = defaultSpeed;
        navMeshAgentBallPitMonster.angularSpeed = defaultTurnSpeed;
        aggressionTimeRemaining = timeToAggression;
        ballpitMonsterAnimations.SetBool("AngryMoveSpeed", false);
    }

    private void Wander()
    {
        if (playerCaught == true) // Stop wandering
        {
            return;
        }

        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            navMeshAgentBallPitMonster.SetDestination(newPos);
            timer = 0;
            SetRandomWanderTimer(); // Set a new random wander timer after reaching the current destination
        }
    }

    public void PlayerCaught()
    {
        playerCaught = true;
        navMeshAgentBallPitMonster.speed = 0;
        ResetAggression();    
    }


    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player")) // Only player can trigger this
        Debug.LogWarning("You are caught!");
        deathHandlerScript.CaughtByBallPitMonster();
        PlayerCaught();
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

