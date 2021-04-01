using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;
    [SerializeField] Transform _pointA, _pointB;
    [SerializeField] Transform _pointsContainer;

    private bool _isCalledToPanel = false;

    private void Start()
    {
        _pointA.parent = _pointsContainer;
        _pointB.parent = _pointsContainer;
    }

    private void FixedUpdate()
    {
        var step = _speed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, 
            _isCalledToPanel ? _pointB.position : _pointA.position, step);
    }

    public void CallElevator()
    {
        _isCalledToPanel = !_isCalledToPanel;
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
