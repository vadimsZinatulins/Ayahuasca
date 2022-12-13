using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace PlayerBehaviours
{
    /// <summary>
    /// This is the player physical character itself, this is gonna have all the mechanics components,
    /// such as movement, interaction, etc...
    /// </summary>
    public class PlayerCharacter : MonoBehaviour
    {
        
        //-------------------------------------------BEGIN-VARIABLES-------------------------------------------
        //----------------------------------------------PLAYER-------------------------------------------------
        /// <summary>
        /// Player owner of this character
        /// </summary>
        private Player _playerOwner;
        //-------------------------------------------CAMERA-(TEMP)-------------------------------------------
        [Header("Solo Camera")]
        //Change this later to the correct system, if needed. See camera manager to see the temporary implementation
        public Vector3 cameraOffset = new Vector3(0,10,2);
        private Camera _playerCamera;
        //-------------------------------------------MOVEMENT-------------------------------------------
        [Header("Movement")]
        /// <summary>
        /// Character controller for the movement
        /// </summary>
        [Tooltip("Character controller for the movement")]
        [SerializeField] private CharacterController characterController;
        
        /// <summary>
        /// Move speed of the character
        /// </summary>
        [Tooltip("Move speed of the character")]
        public float moveSpeed = 2f;
        
        /// <summary>
        /// Smoothness of the movement input
        /// </summary>
        [Tooltip("Smoothness of the movement input")]
        public float lerpMovement = 0.5f;
        
        /// <summary>
        /// Smoothness of the turning of the player
        /// </summary>
        [Tooltip("Smoothness of the turning of the player")]
        public float turnSmoothTime = 0.1f;
        
        /// <summary>
        /// Player gravity force amount
        /// </summary>
        [Tooltip("Player gravity force amount")]
        public float gravity;
        
        /// <summary>
        /// Current fall velocity amount
        /// </summary>
        [Tooltip("Current fall velocity amount")]
        private float _fallVelocity;

        /// <summary>
        /// Current movement input lerped
        /// </summary>
        [Tooltip("Current movement input lerped")]
        private Vector3 currentInput;
        
        /// <summary>
        /// Current turn smooth velocity
        /// </summary>
        [Tooltip("Current turn smooth velocity")]
        private float _turnSmoothVelocity;
        //-------------------------------------------GROUND-CHECK-------------------------------------------
        [Header("Ground Check")]
        /// <summary>
        /// Transform that stores the ground check location
        /// </summary>
        [Tooltip("Transform that stores the ground check location")]
        public Transform groundCheckPos;
        
        /// <summary>
        /// Radius for the ground check
        /// </summary>
        [Tooltip("Radius for the ground check")]
        public float groundCheckRadius;
        
        /// <summary>
        /// Holds if the player is grounded
        /// </summary>
        [Tooltip("Holds if the player is grounded")]
        private bool _isGrounded;
        
        /// <summary>
        /// Layers that trigger as ground
        /// </summary>
        [Tooltip("Layers that trigger as ground")]
        public LayerMask groundLayers;

        //-------------------------------------------JUMP-MECHANIC-------------------------------------------
        [Header("Jump Mechanic")]
        /// <summary>
        /// Enables or disables if the character can jump
        /// </summary>
        [Tooltip("Enables or disables if the character can jump")]
        public bool EnableJumpMechanic = true;
        
        /// <summary>
        /// How high the player jumps
        /// </summary>
        [Tooltip("How high the player jumps")]
        public float jumpHeight;
        
        /// <summary>
        /// The time offset for the ground check
        /// </summary>
        [Tooltip("The time offset for the ground check")]
        public float jumpCoyoteTime;
        
        /// <summary>
        /// Current coyote timer
        /// </summary>
        [Tooltip("Current coyote timer")]
        private float _currentJumpCoyoteTime;
        
        /// <summary>
        /// Jump cooldown max time
        /// </summary>
        [Tooltip("Jump cooldown max time")]
        public float jumpCooldownTime = 0.5f;
        
        /// <summary>
        /// Current jump cooldown timer
        /// </summary>
        [Tooltip("Current jump cooldown timer")]
        private float _currentJumpCooldownTime;
        
        /// <summary>
        /// Holds if the player can currently jump
        /// </summary>
        [Tooltip("Holds if the player can currently jump")]
        private bool canJump;

        /// <summary>
        /// Stores if the player already jumped
        /// </summary>
        [Tooltip("Stores if the player already jumped")]
        private bool didJump = false;
        
        //-------------------------------------------END-VARIABLES-------------------------------------------
        private void OnDrawGizmos()
        {
            if (groundCheckPos)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(groundCheckPos.position,groundCheckRadius);
            }
        }

        public void SetOwner(Player player)
        {
            _playerOwner = player;
        }

        public void SetMovementInput(Vector3 input)
        {
            currentInput = Vector3.Lerp(currentInput, input, lerpMovement*Time.deltaTime);
        }
        private void Update()
        {
            GroundCheck();
            Movement();
            Gravity();
        }

        private void GroundCheck()
        {
            _isGrounded = Physics.OverlapSphere(groundCheckPos.position, groundCheckRadius,groundLayers).Length>0;
            if (EnableJumpMechanic)
            {
                //Reduce jump cooldown timer only when on the floor
                if (_currentJumpCooldownTime > 0 && _isGrounded)
                {
                    _currentJumpCooldownTime = Mathf.Clamp(_currentJumpCooldownTime - Time.deltaTime, 0, jumpCooldownTime);
                }
                
                // Checks if the player can jump while in the ground
                if (_isGrounded && _currentJumpCooldownTime <= 0)
                {
                    //Add maybe delay time
                    _currentJumpCoyoteTime = 0;
                    canJump = true;
                    didJump = false;
                } // Checks if the player can jump if not in the ground, considering a offset time to be easier for the player
                else if (!_isGrounded && !didJump && _currentJumpCoyoteTime + Time.deltaTime < jumpCoyoteTime)
                {
                    canJump = true;
                } // Checks if the player has passed his time to jump, or did already jump
                else if ((canJump && _currentJumpCoyoteTime + Time.deltaTime > jumpCoyoteTime) || didJump)
                {
                    canJump = false;
                }
            }
        }

        private void Movement()
        {
            // Update camera
            _playerCamera.transform.position = transform.position + cameraOffset;
            _playerCamera.transform.LookAt(transform.position);
                
            //Converts the movement input, from vector2 to vector3
            Vector3 movementInput = new Vector3(currentInput.x, 0, currentInput.y);
            
            //Gets the desired rotation angle, based on the input
            float targetAngle = Mathf.Atan2(-currentInput.x, -currentInput.y) * Mathf.Rad2Deg + _playerCamera.transform.eulerAngles.y;
            
            //Calculates the new angle, based on the smootheness. The bigger the value, the slower it rotates
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f,angle,0f);
            
            //Moves the character based on the input
            characterController.Move(movementInput * moveSpeed * Time.deltaTime);
        }

        private void Gravity()
        {
            // Updates the fall velocity
            if (!characterController.isGrounded && !_isGrounded)
            {
                _fallVelocity += gravity * Time.deltaTime;
            }
            else if(characterController.isGrounded && _isGrounded)
            {
                _fallVelocity = 0f;
            }
            
            characterController.Move(Vector3.down * _fallVelocity * Time.deltaTime);
        }

        public void Jump()
        {
            if (EnableJumpMechanic)
            {
                if (canJump && _currentJumpCoyoteTime<jumpCoyoteTime) //Pressed Jump
                {
                    didJump = true;
                    _currentJumpCooldownTime = jumpCooldownTime;
                    _fallVelocity = -jumpHeight;
                }
            }
        }

        public void SetCamera()
        {
            _playerCamera = PlayersManager.Instance.CameraManager.GetSoloCamera();
        }
    }
}