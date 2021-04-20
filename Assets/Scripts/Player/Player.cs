using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float _moveSpeed = 10f;
    [SerializeField] float _jumpHeight = 15f;
    [SerializeField] float _gravity = 1f;
    [SerializeField] private float _pushPower = 2f;

    [Header("Attributes")] 
    [SerializeField] int _maxLives = 3;

    [Header("References")] 
    [SerializeField] Transform _levelStartingPosition;

    private int _coins;
    private int _currentLives;

    private Vector3 _moveDirection = Vector3.zero;
    private Vector3 _velocity = Vector3.zero;
    private Vector3 _wallSurfaceNormal = Vector3.zero;
    private float _yVelocity;

    private bool _canDoubleJump = true;
    private bool _canWallJump = false;

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
        

        if (_controller.isGrounded)
        {
            _canWallJump = true;
            _moveDirection.x = xHorizontal;
            _velocity = _moveDirection * _moveSpeed;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _yVelocity = _jumpHeight;
                _canDoubleJump = true;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) && !_canWallJump)
            {
                if (_canDoubleJump)
                {
                    _yVelocity = 0;
                    _yVelocity += _jumpHeight;
                    _canDoubleJump = false;
                }
            }

            if (Input.GetKeyDown(KeyCode.Space) && _canWallJump)
            {
                _yVelocity = 0;
                _yVelocity = _jumpHeight;
                _velocity = _wallSurfaceNormal * _moveSpeed * 2;
            }

            _yVelocity -= _gravity;
        }

        _velocity.y = _yVelocity;
        _controller.Move(_velocity * Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {

        if(hit != null)
        {
            if (!_controller.isGrounded && hit.transform.tag == "Wall")
            {
                Debug.DrawRay(hit.normal, hit.normal, Color.blue);
                _wallSurfaceNormal = hit.normal;
                _canWallJump = true;
            }

            if (hit.collider.CompareTag("Movable Object"))
            {
                var _movableObject = hit.collider.attachedRigidbody;
                if (_movableObject.isKinematic == true)
                    return;

                if (hit.moveDirection.y < -0.3f)
                    return;

                if (Input.GetKey(KeyCode.E))
                {
                    var pushDir = new Vector3(hit.moveDirection.x, 0);
                    _movableObject.velocity = pushDir * _pushPower;
                }
            }
        }
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

    public int CoinsCollected()
    {
        return _coins;
    }
}
