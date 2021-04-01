using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorPanel : MonoBehaviour
{
    [SerializeField] int _requiredCoins = 10;
    [SerializeField] MeshRenderer _panelLight;
    [SerializeField] private Elevator _elevator;
    

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player _player = other.GetComponentInParent<Player>();
            if (_player != null)
            {
                if (Input.GetKeyDown(KeyCode.E) && _player.CoinsCollected() >= _requiredCoins)
                {
                    _panelLight.material.color = _panelLight.material.color == Color.green ? Color.red : Color.green;
                    _elevator.CallElevator();
                }
            }
        }
    }
}
