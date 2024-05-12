using System;
using System.Collections;
using StarterAssets;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class DeathHandler : MonoBehaviour
{
    // Neccesary components.
    private StarterAssets.FirstPersonController firstPersonController;
    private PlayerVaulting playerVaultingScript;
    private Rigidbody rigidBodyRagdoll;
    private CharacterController characterController;
    private PlayerAnimationManager playerAnimationManager;
    //public PlayerInput _playerInput;
    public GameObject ballpitMonsterVisual;
    public GameObject candaceObject;
    public Transform candaceAttachment;
    public Transform testObject;

    private Animator ballpitMonsterAnimator;
    private float randomRotationY;

    private float torqueMagnitude = 0.01f; 

    // Death By Fall Speed 
    [Header("Lethal Fall Speed:")]
    [Tooltip("The player will die once they fall faster than this number.")]
    public float FatalFallSpeed = -10f; // At this speed in a vertical fall you die

    [Header("Game Over UI Elements")]
    public CanvasGroup blackScreen;
    public float fadeToBlackDuration = 1f;

    [Header("Debug:")]
    [Tooltip("Check this if you do not want to die from fall damage during play.")]
    public bool fallDamageEnable = true;
    private Quaternion defaultCameraRotation;


    void Start() 
    {
        firstPersonController = GetComponent<StarterAssets.FirstPersonController>();
        if (firstPersonController == null)
        {
            Debug.LogError("FirstPersonController.cs component not found on player.");
        }
        playerVaultingScript = GetComponent<PlayerVaulting>();
        if (playerVaultingScript == null)
        {
            Debug.LogError("PlayerVaulting.cs component not found on player.");
        }

        rigidBodyRagdoll = GetComponent<Rigidbody>();
        if (rigidBodyRagdoll == null)
        {
            Debug.LogError("Rigid body component not found on player.");
        }

        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogError("Character Controller component not found on player.");
        }

        playerAnimationManager = GetComponent<PlayerAnimationManager>();
        if (playerAnimationManager == null)
        {
            Debug.LogError("Player Animation Manager Script not found on player.");
        }
        ballpitMonsterAnimator = ballpitMonsterVisual.GetComponent<Animator>();
        if (ballpitMonsterVisual == null)
        {
            Debug.LogError("Ballpit Monster visual not found on player");
        }

        rigidBodyRagdoll.detectCollisions = false;
    }

    void Update()  
    {
        RtoRestart();
        FallDamageListener();
        KillBind();
    }

    // Fatal falling mechanics
    void FallDamageListener() 
    {
        if (fallDamageEnable == false || playerVaultingScript.isVaulting == true) 
        {
            return;
        }

        if (firstPersonController._verticalVelocity < FatalFallSpeed && firstPersonController.Grounded)
        {
            // Trigger death method if the player's fall speed exceeds the fatal threshold
            Debug.LogWarning("You fell too high.");
            RagdollPlayer();
            BlockAllMovement();
            FadeToBlack();
            DisableMouseInput();
        }
    }

    void KillBind() 
    {
        if (Input.GetKeyDown(KeyCode.T)) 
        {
            RagdollPlayer();
            BlockAllMovement();
            FadeToBlack();
            DisableMouseInput();
        }
    }

    // UI Things
    public void FadeToBlack()
    {
        playerAnimationManager.DeathAnimation();
        StartCoroutine(FadeCanvasGroup(blackScreen, blackScreen.alpha, 1f, fadeToBlackDuration));
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;
        
        while (elapsedTime < duration)
        {
            // Calculate the current alpha value based on the elapsed time and duration
            float currentAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);

            // Set the alpha value of the CanvasGroup
            canvasGroup.alpha = currentAlpha;

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Ensure the final alpha value is set correctly
        canvasGroup.alpha = endAlpha;
    }

    private void DisableStaminaUI() 
    {
        firstPersonController.staminaUI.alpha = 0;
    }

    public void BlockAllMovement()
    {
        firstPersonController.movementEnabled = false;     

        playerVaultingScript.enabled = false;
        DisableStaminaUI();
    }

    public void RagdollPlayer() 
    {
        rigidBodyRagdoll.detectCollisions = true;
        firstPersonController.Grounded = false;
        firstPersonController.enabled = false;
        characterController.enabled = false;
        rigidBodyRagdoll.isKinematic = false;

        Vector3 momentumDirection = characterController.velocity.normalized;

        rigidBodyRagdoll.velocity = characterController.velocity;
        rigidBodyRagdoll.AddTorque(transform.right * 30 * torqueMagnitude, ForceMode.Impulse);

        Debug.Log("Ragdolling Player");
    }

    public void CaughtByCandace() 
    {
        BlockAllMovement();
        DisableMouseInput();
        FadeToBlack();
        //RagdollPlayer();
        //ForceLookAt();

        // Set the parent of this game object to the target bone's transform
       // transform.SetParent(candaceAttachment, false);
        

    }

    private void ForceLookAt() 
    {
        // Set the LookAt target of the virtualCamera to the targetTransform

       // firstPersonController.CinemachineCameraTarget.LookAt = testObject.transform;
    }

    public void CaughtByBallPitMonster() 
    {
        BlockAllMovement();
        //DisableMouseInput();
        FadeToBlack();
        //RagdollPlayer();
        
        float randomRotationY = UnityEngine.Random.Range(-35f, 35f);
        ballpitMonsterVisual.transform.rotation = Quaternion.Euler(0f, randomRotationY, 0f);
        ballpitMonsterVisual.SetActive(true);
        ballpitMonsterAnimator.SetTrigger("Bite");
    }

    private void DisableMouseInput()
    {
        firstPersonController.disableLookInput = true;
        firstPersonController.ResetCameraPitch();
    }

    public void RtoRestart() 
    {
        if (Input.GetKeyDown(KeyCode.R)) 
        {
            ReloadScene();
        }
    }

    public void ReloadScene() 
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void FreezeAllTime()
    {
        Time.timeScale = 0;
    }
}
