using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _jumpHeight = 15f;
    [SerializeField] private float _gravity = 1f;

    private Vector3 _moveDirection = Vector3.zero;
    private Vector3 _velocity = Vector3.zero;
    private float _yVelocity;

    private bool _canDoubleJump = true;

    CharacterController _controller;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();    
    }

    void Start()
    {
        
    }

    void Update()
    {
        var xHorizontal = Input.GetAxis("Horizontal");
        _moveDirection.x = xHorizontal;
        _velocity = _moveDirection * _moveSpeed;

        if (_controller.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _yVelocity = _jumpHeight;
                _canDoubleJump = true;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (_canDoubleJump)
                {
                    _yVelocity = 0;
                    _yVelocity += _jumpHeight;
                    _canDoubleJump = false;
                }
            }
            _yVelocity -= _gravity;
        }

        _velocity.y = _yVelocity;
        _controller.Move(_velocity * Time.deltaTime);
    }
}
