using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToGround : MonoBehaviour
{
    void Start()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        {
            transform.position = new Vector3(transform.position.x, (transform.position.y + (transform.localScale.y/2)) - hit.distance, transform.position.z);
        }
    }
}
