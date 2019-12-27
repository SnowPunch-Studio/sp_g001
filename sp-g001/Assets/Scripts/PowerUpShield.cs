using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpShield : PowerUp, IPlayerEvents
{
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
            Destroy(other.gameObject);
            PowerUpHasExpired();
        }
    }

    void IPlayerEvents.OnPlayerReachedExit(GameObject exit)
    {

    }
}
