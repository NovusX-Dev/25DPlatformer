using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float _moveSpeed = 10f;
    [SerializeField] float _jumpHeight = 15f;
    [SerializeField] float _gravity = 1f;

    [Header("Attributes")] 
    [SerializeField] int _maxLives = 3;

    [Header("References")] 
    [SerializeField] Transform _levelStartingPosition;

    private int _coins;
    private int _currentLives;

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
        if (_levelStartingPosition == null)
        {
            Debug.LogError("Level Starting Position is not Set!!!");
            Time.timeScale = 0;
        }
        
        UIManager.Instance.UpdateCoinsText(_coins);
        _currentLives = _maxLives;
        UIManager.Instance.UpdateLivesText(_currentLives);

        transform.position = _levelStartingPosition.position;
    }

    void Update()
    {
        CalculateMovment();

        //for debugging only
        if (Input.GetKeyDown(KeyCode.O))
        {
            transform.position = _levelStartingPosition.position;
        }
    }

    private void CalculateMovment()
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

    public void AddCoins(int amount)
    {
        _coins += amount;
        UIManager.Instance.UpdateCoinsText(_coins);
    }

    public void DamagePlayer(int amount)
    {
        _currentLives -= amount;
        UIManager.Instance.UpdateLivesText(_currentLives);

        if (_currentLives < 1)
        {
            _currentLives = 0;
            UIManager.Instance.UpdateLivesText(_currentLives);
            StartCoroutine(PlayerDeathRoutine());
        }
    }

    IEnumerator PlayerDeathRoutine()
    {
        Time.timeScale = 0.5f;
        _controller.enabled = false;
        yield return new WaitForSeconds(2f);
        LevelManager.Instance.RestartLevel();
    }

    public void RespawnPlayer()
    {
        _controller.enabled = false;
        transform.position = _levelStartingPosition.position;
        StartCoroutine(RestartControllerRoutine());
    }

    IEnumerator RestartControllerRoutine()
    {
        yield return new WaitForEndOfFrame();
        _controller.enabled = true;
    }
}
