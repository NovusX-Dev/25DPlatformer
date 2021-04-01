using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player _player = other.GetComponent<Player>();
            if (_player != null)
            {
                _player.DamagePlayer(1);
                _player.RespawnPlayer();
            }
        }
    }
}
