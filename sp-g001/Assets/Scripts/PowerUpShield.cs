using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpShield : PowerUp, IPlayerEvents
{
    bool m_Started;
    Vector3 boxScale;
    Vector3 boxCenter;

    protected override void Start()
    {
        base.Start();
        m_Started = true;
    }

    protected override void PowerUpPayload()
    {
        base.PowerUpPayload();
        playerController.SetPowerUpState(PlayerController.PowerUpState.INVINCIBLE);
    }

    protected override void PowerUpHasExpired()
    {
        playerController.SetPowerUpState(PlayerController.PowerUpState.NONE);
        base.PowerUpHasExpired();
    }

    void IPlayerEvents.OnPlayerCollide(ControllerColliderHit other)
    {
        if(powerUpState != PowerUpState.IsCollected)
        {
            return;
        }

        if(other.gameObject.tag == "BoxObstacle")
        {
            Collider[] hitColliders = Physics.OverlapBox(boxCenter, boxScale, Quaternion.identity);

            foreach(Collider col in hitColliders)
            {
                if(col.gameObject.tag == "BoxObstacle")
                {
                    Destroy(col.gameObject);
                }
            }

            //Destroy(other.gameObject);
            PowerUpHasExpired();
        }
    }

    void OnDrawGizmos()
    {
        boxCenter = new Vector3(playerController.transform.position.x, playerController.transform.position.y, playerController.transform.position.z + 4);
        boxScale = playerController.transform.localScale * 6;
        boxScale.z *= 2;

        Gizmos.color = Color.cyan;
        if(m_Started && playerController != null)
            Gizmos.DrawWireCube(boxCenter, boxScale);
    }

    void IPlayerEvents.OnPlayerReachedExit(GameObject exit)
    {

    }
}
