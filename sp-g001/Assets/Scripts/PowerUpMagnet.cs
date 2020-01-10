using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpMagnet : PowerUp
{
    bool isActive;
    HashSet<GameObject> foundGameObjects = new HashSet<GameObject>();
    float speed = 15f;
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
        if(isActive && playerController != null)
        {
            Collider[] hitColliders = Physics.OverlapSphere(sphereCenter, sphereRadius);
            foreach(Collider col in hitColliders)
            {
                if(col.gameObject.tag == "Coin")
                {
                    float step = speed * Time.deltaTime;
                    Vector3 target = new Vector3(playerController.gameObject.transform.position.x, playerController.gameObject.transform.position.y, playerController.gameObject.transform.position.z + 3);
                    col.gameObject.transform.position = Vector3.MoveTowards(col.gameObject.transform.position, playerController.gameObject.transform.position, step);
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
        if (m_Started && playerController != null)
        {
            sphereCenter = new Vector3(playerController.transform.position.x, playerController.transform.position.y, playerController.transform.position.z + 2);
            sphereRadius = 3f;

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(sphereCenter, sphereRadius);
        }
    }
}
