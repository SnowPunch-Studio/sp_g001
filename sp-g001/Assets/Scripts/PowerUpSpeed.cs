using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpeed : PowerUp
{
    public float speedMultiplier = 1.5f;

    protected override void PowerUpPayload()
    {
        playerController.BoostMoveSpeed(speedMultiplier);
        base.PowerUpPayload();
    }
}
