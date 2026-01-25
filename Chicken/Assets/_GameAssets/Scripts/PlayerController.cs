
using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public event Action OnPlayerJumped;


    [Header("References")]
    [SerializeField] private Transform _orientation;
    private Rigidbody _rigidbody;
    [SerializeField] private StateController stateController;

    [Header("Movement")]
    private float _horizontalInput, _verticalInput;

    private Vector3 _movementDirection;

    [SerializeField] private float _movementSpeed;
    [SerializeField] private KeyCode _movementKey;
    

    [Header("Jump")]
    [SerializeField]private KeyCode _jumpKey;
    [SerializeField] private float _jumpForce;
    [SerializeField] private ForceMode _forceMode;
    [SerializeField] private bool _canJump;
    [SerializeField] private float _jumpCooldown;
    [SerializeField] private float airMultiplier;
    [SerializeField] private float airDrag;
    
    [Header("Ground")]
    [SerializeField] private float _playerHeight;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _groundDrag;

    [Header("Slide")]
    [SerializeField] private float _slideMultiplier;
    [SerializeField] private KeyCode _slideKey;
    private bool _isSliding;
    [SerializeField] private float _slideDrag;


    [Header("PowerUp")]
    private float startingMovementSpeed;
    private float startingJumpForce;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.freezeRotation = true;
        stateController = GetComponent<StateController>();
    }

    private void Start()
    {
        startingMovementSpeed = _movementSpeed;
        startingJumpForce = _jumpForce;
    }



    void Update()
    {
        SetInput();
        SetPlayerDrag();
        LimitPlayerSpeed();
        SetState();

    }

    private void FixedUpdate()
    {
        SetPlayerMovement();
    }


    private void SetInput()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(_slideKey))
        {
            _isSliding = true;
        }
        else if(Input.GetKeyUp(_slideKey))
        {
            _isSliding= false;
        }
        else if (Input.GetKeyDown(_movementKey))
        {
            _isSliding = false;
        }
        else if (Input.GetKey(_jumpKey) && _canJump && IsGrounded())
        {
            _canJump = false;
            SetPlayerJumping();
            Invoke(nameof(ResetJumping), _jumpCooldown);
        }
    }

    private void SetPlayerMovement()
    {
        _movementDirection = _orientation.transform.forward * _verticalInput + _orientation.transform.right * _horizontalInput;


        float forceMultiplier = stateController.GetCurrentState() switch
        {
            PlayerState.Move => 1f,
            PlayerState.Slide => _slideMultiplier,
            PlayerState.Jump =>airMultiplier,
            _ => 1f


        };

        _rigidbody.AddForce(_movementDirection.normalized * _movementSpeed * forceMultiplier, ForceMode.Force);
    }

    private void SetPlayerJumping()
    {
        OnPlayerJumped?.Invoke();
        _rigidbody.linearVelocity = new Vector3(_rigidbody.linearVelocity.x, 0f, _rigidbody.linearVelocity.z);
        _rigidbody.AddForce(transform.up *_jumpForce, _forceMode);
    }

    private void ResetJumping()
    {
        _canJump = true;
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, _playerHeight * 0.5f + 0.2f, _groundLayer);
    }

    private void SetPlayerDrag()
    {
        _rigidbody.linearDamping = stateController.GetCurrentState() switch
        {
            PlayerState.Move => _groundDrag,
            PlayerState.Slide => _slideDrag,
            PlayerState.Jump => airDrag,
            _ => _rigidbody.linearDamping


        };
    }

    private void LimitPlayerSpeed()
    {
        Vector3 _flatVelocity = new Vector3(_rigidbody.linearVelocity.x, 0f, _rigidbody.linearVelocity.z);

        if (_flatVelocity.magnitude > _movementSpeed)
        {
            Vector3 limitedVelocity = _flatVelocity.normalized * _movementSpeed;

            _rigidbody.linearVelocity = new Vector3(limitedVelocity.x, _rigidbody.linearVelocity.y, limitedVelocity.z);
        }
    }

    private void SetState()
    { 
        var movementDirection = GetMovementDirection();
        var isGrounded = IsGrounded();
        var isSliding = IsSliding();
        var currentState = stateController.GetCurrentState();


        var newState = currentState switch
        {
            _ when movementDirection == Vector3.zero && isGrounded && !isSliding => PlayerState.Idle,
            _ when movementDirection != Vector3.zero && isGrounded && !isSliding => PlayerState.Move,
            _ when movementDirection != Vector3.zero && isGrounded && isSliding => PlayerState.Slide,
            _ when movementDirection == Vector3.zero && isGrounded && isSliding => PlayerState.SlideIdle,
            _ when !_canJump && !isGrounded => PlayerState.Jump,
            _ =>currentState
        };


        if (newState != currentState)
        {
            stateController.ChangeState(newState);
        }


    }


    private Vector3 GetMovementDirection() 
    {
        return _movementDirection.normalized;
    }

    private bool IsSliding()
    {
        return _isSliding;
    }

    public void SetMovementSpeed(float speed, float duration)
    {
        _movementSpeed += speed;
        Invoke(nameof(ResetMovementSpeed), duration);
    }

    private void ResetMovementSpeed()
    {
        _movementSpeed = startingMovementSpeed;
    }

    public void SetJumpForece(float force, float duration)
    {
        _jumpForce += force;
        Invoke(nameof(ResetJumpForce), duration);
    }

    private void ResetJumpForce()
    {
        _jumpForce = startingJumpForce;
    }
}
