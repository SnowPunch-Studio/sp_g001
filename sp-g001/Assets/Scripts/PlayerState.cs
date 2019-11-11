using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    public static MoveState moving;
    public static JumpState jumping;
    protected Transform playerPosition;
    protected float moveSpeed;
    protected float jumpForce;
    protected float gravity;

    public virtual StateResponse HandleInput()
    {
        StateResponse res = new StateResponse();
        res.moveVector = this.playerPosition.position;
        return res;
    }

    public virtual StateResponse HandleCollision(ControllerColliderHit col)
    {
        Debug.Log("Collided in MoveState with " + col.gameObject.tag);
        return null;
    }

    public PlayerState init()
    {
        playerPosition = GameObject.FindWithTag("Player").transform;
        moveSpeed = 5;
        jumpForce = 20.0f;
        gravity = 38.0f;

        moving = new MoveState();
        jumping = new JumpState();
        return moving;
    }
}

public class MoveState : PlayerState
{
    public override StateResponse HandleInput()
    {
        StateResponse res = new StateResponse();
        Vector3 moveVector = this.playerPosition.position;

        if (Input.GetAxisRaw("Vertical") > 0)
        {
            moveVector.y = jumpForce;
            res.state = PlayerState.jumping;
        }

        res.moveVector = moveVector;
        return res;
    }

    /*public override StateResponse HandleCollision(ControllerColliderHit col)
    {
        Debug.Log("Collided in MoveState with " + col.gameObject.tag);
        return null;
    }*/
}

public class JumpState : PlayerState
{
    public override StateResponse HandleInput()
    {
        StateResponse res = new StateResponse();
        Vector3 moveVector = this.playerPosition.position; 

        if (Input.GetAxisRaw("Vertical") < 0)
        {
            moveVector.y = -1 * jumpForce;
        }

        res.moveVector = moveVector;
        return res;
    }

    public override StateResponse HandleCollision(ControllerColliderHit col)
    {
        Debug.Log("In JumpState collision");
        StateResponse res = new StateResponse();

        Debug.Log("Collision tag :: " + col.gameObject.tag);
        if (col.gameObject.tag == "Ground")
        {
            res.state = PlayerState.moving;
        }

        return res;
    }
}
