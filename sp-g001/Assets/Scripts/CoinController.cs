using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    void Start()
    {
        transform.localEulerAngles = new Vector3(90, 0, 0);
    }

    void Update()
    {
        transform.Rotate(0.0f, 0.0f, 90.0f * Time.deltaTime, Space.Self);
    }
}
