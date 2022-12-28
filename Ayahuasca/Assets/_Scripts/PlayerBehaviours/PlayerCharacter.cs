using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Behaviours.Interfaces;
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
        /* Player owner of this character */
        private Player _playerOwner;

        //-------------------------------------------CAMERA-(TEMP)-------------------------------------------
        [Header("Solo Camera")]
        //Change this later to the correct system, if needed. See camera manager to see the temporary implementation
        public Vector3 cameraOffset = new Vector3(0, 10, 2);

        private Camera _playerCamera;

        //-------------------------------------------MOVEMENT-------------------------------------------
        [Header("Movement")]
        /* Character controller for the movement */
        [Tooltip("Character controller for the movement")]
        [SerializeField]
        private CharacterController characterController;

        /* Move speed of the character */
        [Tooltip("Move speed of the character")]
        public float moveSpeed = 2f;

        /* Smoothness of the movement input */
        [Tooltip("Smoothness of the movement input")]
        public float lerpMovement = 0.5f;

        /* Smoothness of the turning of the player */
        [Tooltip("Smoothness of the turning of the player")]
        public float turnSmoothTime = 0.1f;

        /* Player gravity force amount */
        [Tooltip("Player gravity force amount")]
        public float gravity;

        /* Current fall velocity amount */
        [Tooltip("Current fall velocity amount")]
        private float _fallVelocity;

        /* Current movement input lerped */
        [Tooltip("Current movement input lerped")]
        private Vector3 currentInput;

        /* Current turn smooth velocity */
        [Tooltip("Current turn smooth velocity")]
        private float _turnSmoothVelocity;

        //-------------------------------------------GROUND-CHECK-------------------------------------------
        [Header("Ground Check")]
        /* Transform that stores the ground check location */
        [Tooltip("Transform that stores the ground check location")]
        public Transform groundCheckPos;

        /* Radius for the ground check */
        [Tooltip("Radius for the ground check")]
        public float groundCheckRadius;

        /* Holds if the player is grounded */
        [Tooltip("Holds if the player is grounded")]
        private bool _isGrounded;
        
        private float waterGroundDifference = 0;
        public float maxWaterGroundDistance = 0.5f;

        /* Layers that trigger as ground */
        [Tooltip("Layers that trigger as ground")]
        public LayerMask groundLayers;

        public LayerMask waterLayers;

        //-------------------------------------------JUMP-MECHANIC-------------------------------------------
        [Header("Jump Mechanic")]
        /* Layers that trigger as ground */
        [Tooltip("Enables or disables if the character can jump")]
        public bool EnableJumpMechanic = true;

        /* How high the player jumps */
        [Tooltip("How high the player jumps")] public float jumpHeight;

        /* The time offset for the ground check */
        [Tooltip("The time offset for the ground check")]
        public float jumpCoyoteTime;

        /* Current coyote timer */
        [Tooltip("Current coyote timer")] private float _currentJumpCoyoteTime;

        /* Jump cooldown max time */
        [Tooltip("Jump cooldown max time")] public float jumpCooldownTime = 0.5f;

        /* Current jump cooldown timer */
        [Tooltip("Current jump cooldown timer")]
        private float _currentJumpCooldownTime;

        /* Holds if the player can currently jump */
        [Tooltip("Holds if the player can currently jump")]
        private bool canJump;

        /* Stores if the player already jumped */
        [Tooltip("Stores if the player already jumped")]
        private bool didJump = false;

        //-------------------------------------------INTERACT-MECHANIC-------------------------------------------
        [Header("Interact System")]
        /* Radius of interaction */
        [Tooltip("Radius of interaction")]
        public float interactRadius;

        /* Interact layers */
        [Tooltip("Interact layers")] public LayerMask interactLayers;

        /* Current close interactables */
        private List<GameObject> currentInteractables = new List<GameObject>();

        /* Current close riddables */
        private List<GameObject> currentRidables = new List<GameObject>();

        //-------------------------------------------RIDING-MECHANIC-------------------------------------------

        /* Which player thing is the player riding */
        private PlayerRiding _currentRidingType;

        private GameObject currentRiddable;
        [SerializeField] private List<Collider> collidersToDeativateOnRide;

        //------------------------------------------BOAT-MECHANIC-------------------------------------------
        [Header("BOAT")]
        public float boatPushForce = 5f;
        public float rowingSideForce = 0.2f;
        public float rowingFowardForce = 3f;
        //-------------------------------------------END-VARIABLES-------------------------------------------
        private void OnDrawGizmos()
        {
            if (groundCheckPos)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(groundCheckPos.position, groundCheckRadius);
            }
        }

        public void SetOwner(Player player)
        {
            _playerOwner = player;
        }

        public Player GetOwner()
        {
            return _playerOwner;
        }

        public PlayerRiding GetRidingType()
        {
            return _currentRidingType;
        }

        public void SetMovementInput(Vector3 input)
        {
            currentInput = Vector3.Lerp(currentInput, input, lerpMovement * Time.deltaTime);
        }

        private void Update()
        {
            switch (_currentRidingType)
            {
                case PlayerRiding.NONE:
                    NormalChecks();
                    NormalMovement();
                    NormalGravity();
                    break;
                case PlayerRiding.BOAT:
                
                    break;
            }
        }

        // When the player is not controlling anything
        #region Normal Functions

        private void NormalChecks()
        {
            GroundCheck();
            InteractCheck();
        }

        private void InteractCheck()
        {
            Vector3 positionCheck = transform.position;
            Collider[] InteractColliders = Physics.OverlapSphere(positionCheck, interactRadius, interactLayers);
            var OrderedInteractions =
                InteractColliders.OrderBy(d => (d.transform.position - transform.position).sqrMagnitude);
            currentInteractables = new List<GameObject>();
            currentRidables = new List<GameObject>();
            foreach (var interactableGO in OrderedInteractions)
            {
                if (interactableGO.TryGetComponent(out IInteractable interactable))
                {
                    currentInteractables.Add(interactableGO.gameObject);
                }

                if (interactableGO.TryGetComponent(out IRiddable riddable))
                {
                    currentRidables.Add(interactableGO.gameObject);
                }
            }

            if (currentInteractables.Count > 0)
            {
                string interactText = currentInteractables[0].GetComponent<IInteractable>().GetInteractText();
                Debug.Log(interactText);
            }

            if (currentRidables.Count > 0)
            {
                if (currentRiddable == null)
                {
                    string rideText = currentRidables[0].GetComponent<IRiddable>().GetRideText();
                    Debug.Log(rideText);
                }
            }
        }

        private bool CheckNextStep(Vector3 position)
        {
            RaycastHit groundHit;
            if (Physics.Raycast(position, -transform.up * 2f, out groundHit, 2, groundLayers))
            {
                RaycastHit waterHit;
                if (Physics.Raycast(position, -transform.up * 2f, out waterHit, 2, waterLayers))
                {
                    Debug.LogWarning("Checking difference");
                    waterGroundDifference = (groundHit.point - waterHit.point).magnitude;
                    if (waterGroundDifference <= maxWaterGroundDistance)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return false;
        }
        private void GroundCheck()
        {
            _isGrounded = Physics.OverlapSphere(groundCheckPos.position, groundCheckRadius, groundLayers).Length > 0;
            if (EnableJumpMechanic)
            {
                //Reduce jump cooldown timer only when on the floor
                if (_currentJumpCooldownTime > 0 && _isGrounded)
                {
                    _currentJumpCooldownTime =
                        Mathf.Clamp(_currentJumpCooldownTime - Time.deltaTime, 0, jumpCooldownTime);
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

        private void NormalMovement()
        {
            // Update camera
            // _playerCamera.transform.position = transform.position + cameraOffset;
            // _playerCamera.transform.LookAt(transform.position);

            //Converts the movement input, from vector2 to vector3
            Vector3 movementInput = new Vector3(currentInput.x, 0, currentInput.y);

            //Gets the desired rotation angle, based on the input
            float targetAngle = Mathf.Atan2(-currentInput.x, -currentInput.y) * Mathf.Rad2Deg +
                                _playerCamera.transform.eulerAngles.y;

            //Calculates the new angle, based on the smootheness. The bigger the value, the slower it rotates
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity,
                turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            //Moves the character based on the input
            if (CheckNextStep(transform.position + movementInput * moveSpeed * Time.deltaTime))
            {
                characterController.Move(movementInput * moveSpeed * Time.deltaTime);
            }
        }

        private void NormalGravity()
        {
            // Updates the fall velocity
            if (!characterController.isGrounded && !_isGrounded)
            {
                _fallVelocity += gravity * Time.deltaTime;
            }
            else if (characterController.isGrounded && _isGrounded)
            {
                _fallVelocity = 0f;
            }

            characterController.Move(Vector3.down * _fallVelocity * Time.deltaTime);
        }

        #endregion
        

        public void ActiveColliders(bool isEnable)
        {
            foreach (var collider in collidersToDeativateOnRide)
            {
                collider.enabled = isEnable;
            }
        }
        public void SetCamera()
        {
            _playerCamera = PlayersManager.Instance.CameraManager.GetSoloCamera();
            if (SplitScreenManager.Instance != null)
            {
                SplitScreenManager.Instance.SetupPlayerCamera(gameObject, _playerCamera);
            }
            
        }
        
        public void OnInteract()
        {
            switch (_currentRidingType)
            {
                case PlayerRiding.NONE:
                    if (currentInteractables.Count > 0)
                    {
                        currentInteractables[0].GetComponent<IInteractable>().Interact(transform);
                    }

                    if (_currentRidingType == PlayerRiding.NONE && currentRiddable == null)
                    {
                        if (currentRidables.Count > 0)
                        {
                            currentRidables[0].GetComponent<IRiddable>().StartRiding(gameObject, ref _currentRidingType);
                            if (_currentRidingType != PlayerRiding.NONE)
                            {
                                currentRiddable = currentRidables[0];
                                ActiveColliders(false);
                            }

                        }
                    }
                    break;
                case PlayerRiding.BOAT:
                    if (_currentRidingType == PlayerRiding.BOAT && currentRiddable != null)
                    {
                        if (currentRiddable.GetComponent<IRiddable>().StopRiding(gameObject))
                        {
                            currentRiddable = null;
                            _currentRidingType = PlayerRiding.NONE;
                        }
                    }
                    break;
            }
        }
        
        public bool OnStopRiding()
        {
            if (currentRiddable != null)
            {
                if (CheckNextStep(transform.position))
                {
                    currentRiddable = null;
                    _currentRidingType = PlayerRiding.NONE;
                    ActiveColliders(true);

                    return true;
                }
            }

            return false;
        }
        
        public void OnJump()
        {
            if (EnableJumpMechanic)
            {
                if (canJump && _currentJumpCoyoteTime < jumpCoyoteTime) //Pressed Jump
                {
                    didJump = true;
                    _currentJumpCooldownTime = jumpCooldownTime;
                    _fallVelocity = -jumpHeight;
                }
            }
        }
        
        public void OnAction1()
        {
            switch (_currentRidingType)
            {
                case PlayerRiding.NONE:
                    foreach (var riddable in currentRidables)
                    {
                        if (riddable.TryGetComponent(out Boat boat))
                        {
                            Vector3 dir = (boat.transform.position - transform.position).normalized;
                            RaycastHit hit;
                            if (Physics.Raycast(transform.position, dir, out hit, 1, interactLayers))
                            {
                                if (hit.collider.gameObject == boat.gameObject)
                                {
                                    boat.Push(dir*boatPushForce,hit.point-dir);
                                }
                            }
                            
                        }
                    }
                    break;
                case PlayerRiding.BOAT:
                    if (currentRiddable != null)
                    {
                        if (currentRiddable.TryGetComponent(out Boat boat))
                        {
                            boat.Row(gameObject, rowingSideForce, rowingFowardForce);
                        }
                    }
                    break;
            }
        }
        public void OnAction2()
        {
            switch (_currentRidingType)
            {
                case PlayerRiding.BOAT:
                    if (currentRiddable != null)
                    {
                        if (currentRiddable.TryGetComponent(out Boat boat))
                        {
                            // Switch side rowing
                        }
                    }
                    break;
            }
        }
    }
}

public enum PlayerRiding
{
    NONE,
    BOAT
}