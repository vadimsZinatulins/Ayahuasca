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
        [SerializeField] private CharacterController characterController;
        private Player _playerOwner;

        [Header("Solo Camera")]
        public Vector3 CameraOffset = new Vector3(0,10,2);
        private Camera PlayerCamera;
        
        [Header("Movement")]
        public float MoveSpeed = 2f;
        public float LerpInputTime = 0.5f;
        public float turnSmoothTime = 0.1f;
        public float gravity;
        private float fallVelocity;

        private Vector3 currentInput;
        private float turnSmoothVelocity;

        [Header("Ground Check")] 
        public Transform groundCheckPos;
        public float groundCheckRadius;
        private bool isGrounded;
        public LayerMask groundLayers;

        [Header("Jump Mechanic")] 
        public float jumpHeight;
        public float jumpCoyoteTime;
        private float _currentJumpCoyoteTime;
        public bool canJump;
        
        public void SetOwner(Player player)
        {
            _playerOwner = player;
        }

        public void SetInputs(Vector3 input)
        {
            if (characterController)
            {
                currentInput = Vector3.Lerp(currentInput, input, LerpInputTime*Time.deltaTime);
            }
        }
        private void Update()
        {
            GroundCheck();
            Movement();
            Gravity();
        }

        private void GroundCheck()
        {
            isGrounded = Physics.OverlapSphere(groundCheckPos.position, groundCheckRadius,groundLayers).Length>0;
            
            if ( isGrounded && _currentJumpCoyoteTime >= 0) //Just Grounded
            {
                _currentJumpCoyoteTime = 0;
                canJump = true;
            }else if (!isGrounded && _currentJumpCoyoteTime + Time.deltaTime < jumpCoyoteTime) //Falling but can jump
            {
                _currentJumpCoyoteTime += Time.deltaTime;
            }else if (!isGrounded && _currentJumpCoyoteTime + Time.deltaTime > jumpCoyoteTime)
            {
                _currentJumpCoyoteTime = jumpCoyoteTime;
            }
        }

        private void Movement()
        {
            //update camera
            PlayerCamera.transform.position = transform.position + CameraOffset;
            PlayerCamera.transform.LookAt(transform.position);
            
            if (!characterController.isGrounded && !isGrounded)
            {
                fallVelocity += gravity * Time.deltaTime;
            }
            else if(characterController.isGrounded && isGrounded)
            {
                fallVelocity = 0f;
            }

            Vector3 movementInput = new Vector3(currentInput.x, 0, currentInput.y);
            float targetAngle = Mathf.Atan2(-currentInput.x, -currentInput.y) * Mathf.Rad2Deg + PlayerCamera.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity,
                turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f,angle,0f);
            
            Debug.Log($"Current input: {currentInput}");
            characterController.Move(movementInput * MoveSpeed * Time.deltaTime);
            //Debug.Log(characterController.isGrounded);
        }

        private void Gravity()
        {
            characterController.Move(Vector3.down * fallVelocity * Time.deltaTime);
        }

        public void OnJump()
        {
            if (canJump && _currentJumpCoyoteTime<jumpCoyoteTime) //Pressed Jump
            {
                canJump = false;
                fallVelocity = -jumpHeight;
            }
        }

        public void SetCamera()
        {
            PlayerCamera = PlayersManager.Instance.CameraManager.GetSoloCamera();
        }
    }
}