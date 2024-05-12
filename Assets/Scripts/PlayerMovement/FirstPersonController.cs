using UnityEngine;
using System.Collections;
using UnityEngine.UI;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	[RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM
	[RequireComponent(typeof(PlayerInput))]
#endif
	public class FirstPersonController : MonoBehaviour
	{
		[Header("Player")]
		[Tooltip("Move speed of the character in m/s")]
		public float MoveSpeed = 4.0f;
		[Tooltip("Sprint speed of the character in m/s")]
		public float SprintSpeed = 6.0f;
		[Tooltip("Rotation speed of the character")]
		public float RotationSpeed = 1.0f;
		[Tooltip("Acceleration and deceleration")]
		public float SpeedChangeRate = 10.0f;
		public bool movementEnabled = true;

		[Space(10)]
		[Tooltip("The height the player can jump")]
		public float JumpHeight = 1.2f;
		[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
		public float Gravity = -15.0f;

		[Space(10)]
		[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
		public float JumpTimeout = 0.1f;
		[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
		public float FallTimeout = 0.15f;

		[Header("Player Grounded")]
		[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
		public bool Grounded = true;
		[Tooltip("Useful for rough ground")]
		public float GroundedOffset = -0.14f;
		[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
		public float GroundedRadius = 0.5f;
		[Tooltip("What layers the character uses as ground")]
		public LayerMask GroundLayers;
		public LayerMask PushAble;

		[Header("Stamina System")]
		[Tooltip("A custom stamina system added to this script to handle vaulting")]

		// Custom Stamina System 
		public float MaxStamina = 2f;
		public float StaminaRecoveryRate = 1f;
		private float _currentStamina;

		private bool _isCooldown = false;
		private float _cooldownTimer = 0f;
		public float _cooldownDuration = 3f; // The time to wait before you can sprint again once your stamina depletes

		[Header("Custom Canvas Elements")]
		public Image staminaBar;
		public CanvasGroup staminaUI;

		[Header("Cinemachine")]
		[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
		public GameObject CinemachineCameraTarget;
		[Tooltip("How far in degrees can you move the camera up")]
		public float TopClamp = 90.0f;
		[Tooltip("How far in degrees can you move the camera down")]
		public float BottomClamp = -90.0f;


		// cinemachine
		[HideInInspector] public float _cinemachineTargetPitch;
		// Custom camera block system
		[HideInInspector] public bool disableLookInput = false;
		private float _cinemachineDefaultPitch; // Variable to store the default pitch

		// player
		[HideInInspector] public float _speed;
		private float _rotationVelocity;
		private float _terminalVelocity = 53.0f;

		// timeout deltatime
		private float _jumpTimeoutDelta;
		private float _fallTimeoutDelta;
		[HideInInspector] public float _verticalVelocity;

	
#if ENABLE_INPUT_SYSTEM
		private PlayerInput _playerInput;
#endif
		private CharacterController _controller;
		[HideInInspector] public StarterAssetsInputs _input;
		private GameObject _mainCamera;

		private const float _threshold = 0.01f;

		public PlayerAnimationManager playerAnimationManager;

		private bool IsCurrentDeviceMouse
		{
			get
			{
				#if ENABLE_INPUT_SYSTEM
				return _playerInput.currentControlScheme == "KeyboardMouse";
				#else
				return false;
				#endif
			}
		}

		private void Awake()
		{
			// get a reference to our main camera
			if (_mainCamera == null)
			{
				_mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
			}
		}

		private void Start()
		{
			_controller = GetComponent<CharacterController>();
			_input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM
			_playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif
			
			// reset our timeouts on start
			_jumpTimeoutDelta = JumpTimeout;
			_fallTimeoutDelta = FallTimeout;

			_currentStamina = MaxStamina; //Give the player a full stamina at the start instead of none.

			_cinemachineDefaultPitch = transform.localRotation.eulerAngles.x;
		}

		private void Update()
		{
			JumpAndGravity();
			GroundedCheck();
			Move();
			HandleSprinting();

			StaminaInterface();
			
			//Debug.Log(_currentStamina);

			//Debug.Log(_cooldownTimer);
		}

		private void LateUpdate()
		{
			CameraRotation();
		}

		private void GroundedCheck()
		{
			// set sphere position, with offset
			Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
			Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers + PushAble, QueryTriggerInteraction.Ignore); // Added PushAble to fix infinite fall
		}

		private void CameraRotation()
		{
			if (disableLookInput == true) 
			{
				return;
			}

			// if there is an input
			if (_input.look.sqrMagnitude >= _threshold)
			{
				//Don't multiply mouse input by Time.deltaTime
				float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
				
				_cinemachineTargetPitch += _input.look.y * RotationSpeed * deltaTimeMultiplier;
				_rotationVelocity = _input.look.x * RotationSpeed * deltaTimeMultiplier;

				// clamp our pitch rotation
				_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

				// Update Cinemachine camera target pitch
				CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);

				// rotate the player left and right
				transform.Rotate(Vector3.up * _rotationVelocity);
			}
		}

		private void Move()
		{
			if (movementEnabled == false)
			{
				return;
			}

			// set target speed based on move speed, sprint speed and if sprint is pressed
			float targetSpeed = canSprint() && _input.sprint ? SprintSpeed : MoveSpeed; // Fixed with help of Olaf.

			// a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

			// note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is no input, set the target speed to 0
			if (_input.move == Vector2.zero) targetSpeed = 0.0f;

			// a reference to the players current horizontal velocity
			float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
			

			float speedOffset = 0.1f;
			float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

			// accelerate or decelerate to target speed
			if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
			{
				// creates curved result rather than a linear one giving a more organic speed change
				// note T in Lerp is clamped, so we don't need to clamp our speed
				_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

				// round speed to 3 decimal places
				_speed = Mathf.Round(_speed * 1000f) / 1000f;
			}
			else
			{
				_speed = targetSpeed;
			}

			// normalise input direction
			Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

			// note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is a move input rotate player when the player is moving
			if (_input.move != Vector2.zero)
			{
				// move
				inputDirection = transform.right * _input.move.x + transform.forward * _input.move.y;

				playerAnimationManager.allowInspect = false;
				playerAnimationManager.MovingAnimation();
			}
			else
			{
				// No movement input, trigger a transition to idle or stopping animation
				 // Or whatever trigger you use for idle animation
				 playerAnimationManager.BackToIdle();
				 playerAnimationManager.allowInspect = true;
			}

			// move the player
			_controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
		}

		private void JumpAndGravity()
		{
			if (Grounded)
			{
				// reset the fall timeout timer
				_fallTimeoutDelta = FallTimeout;

				// stop our velocity dropping infinitely when grounded
				if (_verticalVelocity < 0.0f)
				{
					_verticalVelocity = -2f;
				}

				// Jump
				if (_input.jump && _jumpTimeoutDelta <= 0.0f)
				{
					playerAnimationManager.JumpAnimation();
					// the square root of H * -2 * G = how much velocity needed to reach desired height
					_verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
				}

				// jump timeout
				if (_jumpTimeoutDelta >= 0.0f)
				{
					_jumpTimeoutDelta -= Time.deltaTime;
				}
			}
			else
			{
				// reset the jump timeout timer
				_jumpTimeoutDelta = JumpTimeout;

				// fall timeout
				if (_fallTimeoutDelta >= 0.0f)
				{
					_fallTimeoutDelta -= Time.deltaTime;
				}

				// if we are not grounded, do not jump
				_input.jump = false;
			}

			// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
			if (_verticalVelocity < _terminalVelocity)
			{
				_verticalVelocity += Gravity * Time.deltaTime;
			}
		}

		private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
		{
			if (lfAngle < -360f) lfAngle += 360f;
			if (lfAngle > 360f) lfAngle -= 360f;
			return Mathf.Clamp(lfAngle, lfMin, lfMax);
		}

		private void OnDrawGizmosSelected()
		{
			Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
			Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

			if (Grounded) Gizmos.color = transparentGreen;
			else Gizmos.color = transparentRed;

			// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
			Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
		}

		private bool canSprint()
		{
			return !_isCooldown && _currentStamina > 0 && Grounded;
		}

		private void HandleSprinting()
		{

			// If currently on cooldown, do not handle sprinting
			if (_isCooldown)
			{
				// Reduce cooldown timer
				_cooldownTimer -= Time.deltaTime;
				// Check if cooldown is over
				if (_cooldownTimer <= 0)
				{
					// Reset cooldown state and allow regeneration
					_isCooldown = false;
				}
				
				// Exit method early if still on cooldown
				Debug.Log("Sprint Cooldown");
				return;
			}

			// If sprint input is pressed and there is enough stamina
			if (_input.sprint && _currentStamina > 0)
			{
				playerAnimationManager.SprintingAnimation();
				staminaUI.alpha = 1; // Turn on the stamina Bar
				_currentStamina -= Time.deltaTime * 2; // Times two to fix the stamina for now
				//StartCoroutine(SprintCooldownCoroutine());
			}
			else
			{	
				playerAnimationManager.StopSprintingAnimation();
				// Recover stamina
				if (_currentStamina < MaxStamina)
				{
					_currentStamina += StaminaRecoveryRate * Time.deltaTime;
				}
			}

			// If stamina is 0, disable sprinting and start cooldown
			if (_currentStamina <= 0)
			{
				_input.sprint = false;
				_isCooldown = true;
				_cooldownTimer = _cooldownDuration;
			}	
		}

		private void StaminaInterface() 
		{
			staminaBar.fillAmount = _currentStamina / MaxStamina; // Makes the bar move when your stamina runs out.

			if (_currentStamina > MaxStamina) 
			{
				staminaUI.alpha = 0; // Turn off the stamina Bar
			}
		}

		public void ResetCameraPitch()
		{
			Quaternion _cineMachineDefaultRotation = Quaternion.Euler(_cinemachineDefaultPitch, 0.0f, 0.0f);
			CinemachineCameraTarget.transform.localRotation = _cineMachineDefaultRotation;
		}

	}
}