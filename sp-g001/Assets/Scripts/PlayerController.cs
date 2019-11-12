using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    enum State { MOVING, JUMPING, GAMEOVER };

    private State state_;
    private CharacterController controller;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpThrust;
    [SerializeField] private float gravity;
    private float verticalVelocity;

    void Start()
    {
        this.moveSpeed = 12.0f;
        this.gravity = 38.0f;
        this.jumpThrust = 15.0f;
        this.verticalVelocity = 0.0f;

        this.controller = gameObject.GetComponent<CharacterController>();
        this.state_ = State.MOVING;
    }

    void Update()
    {
        Vector3 moveVector = transform.position;

        switch(this.state_)
        {
            case State.GAMEOVER:
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                #else
                    Application.Quit();
                #endif
                break;
            case State.MOVING:
                if (Input.GetKeyDown("w"))
                {
                    verticalVelocity = this.jumpThrust;
                    this.state_ = State.JUMPING;
                }
                break;
            case State.JUMPING:
                if (Input.GetKeyDown("s"))
                {
                    verticalVelocity = -this.jumpThrust;
                }
                break;
        }

        moveVector.x = Input.GetAxisRaw("Horizontal") * moveSpeed;
        moveVector.z = moveSpeed;
        verticalVelocity -= gravity * Time.deltaTime;
        moveVector.y = verticalVelocity;

        this.controller.Move(moveVector * Time.deltaTime);
    }

    void OnControllerColliderHit(ControllerColliderHit col)
    {
        switch(this.state_)
        {
            case State.MOVING:
                if(col.gameObject.tag == "BoxObstacle")
                {
                    this.state_ = State.GAMEOVER;
                }
                if(col.gameObject.tag == "Finish")
                {
                    this.state_ = State.GAMEOVER;
                }
                break;
            case State.JUMPING:
                if(col.gameObject.tag == "Ground")
                {
                    this.state_ = State.MOVING;
                }
                if(col.gameObject.tag == "BoxObstacle")
                {
                    this.state_ = State.GAMEOVER;
                }
                if(col.gameObject.tag == "Finish")
                {
                    this.state_ = State.GAMEOVER;
                }
                break;
        }
    }
}
