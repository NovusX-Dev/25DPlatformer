using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;
    [SerializeField] Transform _pointsContainer;
    [SerializeField] Transform _pointA, _pointB;

    private Vector3 _currentTarget;

    void Start()
    {
        _pointA.parent = _pointsContainer;
        _pointB.parent = _pointsContainer;
    }

    void FixedUpdate()
    {
        var step = _speed * Time.deltaTime;

        if (transform.position == _pointA.position)
        {
            _currentTarget = _pointB.position;
        }
        else if (transform.position == _pointB.position)
        {
            _currentTarget = _pointA.position;
        }

        transform.position = Vector3.MoveTowards(transform.position, _currentTarget, step);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent = this.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent = null;
        }
    }
}
