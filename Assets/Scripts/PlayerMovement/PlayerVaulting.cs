using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVaulting : MonoBehaviour
{
    // Start is called before the first frame update
//    private int vaultLayer;
    public Camera cam;
    private float playerHeight = 1.2f;
    private float playerRadius = 0.3f;

    public PlayerAnimationManager playerAnimationManager;

    [SerializeField] float vaultingSpeed = 0.5f;
    [SerializeField] LayerMask vaultLayer;
    [HideInInspector] public bool isVaulting = false;

    [HideInInspector] public bool isVaultingCooldown = false;
    public float vaultingCooldownDuration = 1f;
    private DeathHandler deathHandler;
    public GameObject vaultingTooltip;
    
    private StarterAssets.FirstPersonController firstPersonController;
    
    void Start()
    {
    //    vaultLayer = LayerMask.NameToLayer("VaultAble");
    //    vaultLayer = ~vaultLayer;
        vaultingTooltip.SetActive(false);
        firstPersonController = GetComponent<StarterAssets.FirstPersonController>();
        if (firstPersonController == null)
        {
            Debug.LogError("Error: Player vaulting script not present on player! No vaulting!");
        }

        deathHandler = GetComponent<DeathHandler>();
        if (deathHandler == null)
        {
            Debug.LogError("Error: Player death handler not found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vault();
        VaultingUIToolTip();
    }
    private void Vault()
    {
        // Check if vaulting is on cooldown
        if (isVaultingCooldown)
        {
            return; // Exit the function if vaulting is on cooldown
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out var firstHit, 1f, vaultLayer))
            {
                print("vaultable in front: " + firstHit.collider.gameObject.transform.parent.name);
                if (Physics.Raycast(firstHit.point + (cam.transform.forward * playerRadius) + (Vector3.up * 0.6f * playerHeight), Vector3.down, out var secondHit, playerHeight, ~0))
                {
                    print("found place to land");
                    StartCoroutine(VaultCooldown()); // Start the cooldown coroutine
                    StartCoroutine(LerpVault(secondHit.point, vaultingSpeed));
                    playerAnimationManager.VaultingAnimation();
                }
            }
        }
    }

    IEnumerator LerpVault(Vector3 targetPosition, float duration)
    {
        isVaulting = true;
        float time = 0;
        Vector3 startPosition = transform.position;

        while (time < duration)
        {
            DisableGrounded(); // Stops the player from jumping right after they climbed the ledge.
            float t = Mathf.SmoothStep(0f, 1f, time / duration); // Smoothing the linear vaulting arc with SmoothStep
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            time += Time.deltaTime;
            yield return null;
        }

        // Ensure final position is precisely the target position
        transform.position = targetPosition;
        isVaulting = false; 
    }

    private IEnumerator VaultCooldown()
    {
        isVaultingCooldown = true; // Set vaulting cooldown to true
        deathHandler.fallDamageEnable = false; // Adding invicibility frames so you dont die anymore while vaulting

        // Wait for the cooldown duration
        yield return new WaitForSeconds(vaultingCooldownDuration);

        isVaultingCooldown = false; // Reset vaulting cooldown
        deathHandler.fallDamageEnable = true;
    }

    private void DisableGrounded() 
    {
        print("Disabling Grounded");
        firstPersonController.Grounded = false;
    }

    private void VaultingUIToolTip() 
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out var firstHit, 1f, vaultLayer)) 
        {
            vaultingTooltip.SetActive(true);
        }
        else 
        {
            vaultingTooltip.SetActive(false);  
        }
    }
}