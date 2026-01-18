using NUnit.Framework.Constraints;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Transform _orientation;
    private Rigidbody _rigidbody;

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
    
    [Header("Ground")]
    [SerializeField] private float _playerHeight;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _groundDrag;

    [Header("Slide")]
    [SerializeField] private float _slideMultiplier;
    [SerializeField] private KeyCode _slideKey;
    private bool _isSliding;
    [SerializeField] private float _slideDrag;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.freezeRotation = true;
    }

    

    void Update()
    {
        SetInput();
        SetPlayerDrag();

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
     

        if (_isSliding)
        {
            _rigidbody.AddForce(_movementDirection.normalized * _movementSpeed * _slideMultiplier, ForceMode.Force);
        }
        else
        {
            _rigidbody.AddForce(_movementDirection.normalized * _movementSpeed, ForceMode.Force);
        }


    }

    private void SetPlayerJumping()
    {
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
        if (_isSliding)
        {
            _rigidbody.linearDamping = _slideDrag;
        }
        else
        {
            _rigidbody.linearDamping = _groundDrag;
        }
    }

}
