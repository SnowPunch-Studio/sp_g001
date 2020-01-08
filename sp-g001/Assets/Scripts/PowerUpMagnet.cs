using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpMagnet : PowerUp
{
    bool isActive;
    HashSet<GameObject> foundGameObjects = new HashSet<GameObject>();
    float speed = 2f;
    bool m_Started;
    Vector3 sphereCenter;
    float sphereRadius;

    protected override void Start()
    {
        base.Start();
        m_Started = true;
    }

    protected override void PowerUpPayload()
    {
        base.PowerUpPayload();

        isActive = true;
        StartCoroutine(Finish());
    }

    private void Update()
    {
        if(isActive)
        {
            Debug.Log("Is Active");
            //sphereCenter = new Vector3(playerController.transform.position.x, playerController.transform.position.y, playerController.transform.position.z + 2);
            //sphereRadius = 3f;

            Collider[] hitColliders = Physics.OverlapSphere(sphereCenter, sphereRadius);
            foreach(Collider col in hitColliders)
            {
                if(col.gameObject.tag == "Coin")
                {
                    Debug.Log("Found Coin");
                    if (foundGameObjects.Add(col.gameObject))
                    {
                        Debug.Log("Coin is new");
                        float step = speed * Time.deltaTime;
                        col.gameObject.transform.position = Vector3.MoveTowards(col.gameObject.transform.position, playerController.gameObject.transform.position, step);
                    }

                    if(col.gameObject.transform.position == playerController.gameObject.transform.position)
                    {
                        Debug.Log("Coin made it to player");
                        // Increment player coin count
                        Destroy(col.gameObject);
                    }

                    Debug.Log("HashSet :: " + foundGameObjects);
                }
            }
        }
    }

    IEnumerator Finish()
    {
        yield return new WaitForSeconds(10);

        PowerUpHasExpired();
    }

    protected override void PowerUpHasExpired()
    {
        playerController.SetPowerUpState(PlayerController.PowerUpState.NONE);
        base.PowerUpHasExpired();
    }

    void OnDrawGizmos()
    {
        sphereCenter = new Vector3(playerController.transform.position.x, playerController.transform.position.y, playerController.transform.position.z + 2);
        sphereRadius = 3f;

        Gizmos.color = Color.cyan;
        if (m_Started && playerController != null)
            Gizmos.DrawWireSphere(sphereCenter, sphereRadius);
    }
}
