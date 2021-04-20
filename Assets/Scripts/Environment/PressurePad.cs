using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePad : MonoBehaviour
{

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Movable Object"))
        {
            var distance = Vector3.Distance(transform.position, other.transform.position);

            if (distance < 0.1f)
            {
                GetComponentInChildren<Renderer>().material.color = Color.blue;
                other.GetComponent<Rigidbody>().isKinematic = true;
                Destroy(this);
            }
        }
    }
}
