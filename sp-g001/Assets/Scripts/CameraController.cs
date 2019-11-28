using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    private Transform lookAt;
    private Vector3 startOffset;
    private Vector3 moveVector;

    public void InitCamera()
    {
        Debug.Log("Initializing camera");
        lookAt = GameObject.FindGameObjectWithTag("Player").transform;
        Debug.Log(lookAt);
        startOffset = transform.position - lookAt.position;
    }

    void LateUpdate()
    {
        if(lookAt != null)
        {
            moveVector = lookAt.position + startOffset;

            moveVector.y = Mathf.Clamp(moveVector.y, -1, 10);

            transform.position = moveVector;
        }
    }
}