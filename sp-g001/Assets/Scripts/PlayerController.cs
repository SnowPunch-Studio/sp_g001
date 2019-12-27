using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public enum State { MOVING, JUMPING, GAMEOVER };
    public enum PowerUpState { NONE, INVINCIBLE };
    public bool Shielded;

    private State state_;
    private PowerUpState powerUpState; 
    private CharacterController controller;
    private int coinCount;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float horizontalMoveSpeed;
    [SerializeField] private float forwardMoveSpeed;
    [SerializeField] private float jumpThrust;
    [SerializeField] private float gravity;
    private float verticalVelocity;

    public GameObject deathParticles;

    void Start()
    {
        this.coinCount = 0;
        this.verticalVelocity = 0.0f;
        this.controller = gameObject.GetComponent<CharacterController>();
        this.state_ = State.MOVING;
		Shielded = false;
    }

    void Update()
    {
        Vector3 moveVector = transform.position;

        switch(this.state_)
        {
            case State.GAMEOVER:
                StartCoroutine(HandleGameOver());
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

        if(this.state_ != State.GAMEOVER)
        {
            moveVector.x = Input.GetAxisRaw("Horizontal") * horizontalMoveSpeed;
            moveVector.z = forwardMoveSpeed;
            verticalVelocity -= (gravity + jumpThrust) * Time.deltaTime;
            moveVector.y = verticalVelocity;

            this.controller.Move(moveVector * Time.deltaTime);
            //coinCounter.text = coinCount.ToString();
        }
    }

    void OnControllerColliderHit(ControllerColliderHit col)
    {
        if (powerUpState == PowerUpState.INVINCIBLE)
        {
            if (col.gameObject.tag == "Finish")
            {
                this.state_ = State.GAMEOVER;
            }
            if (col.gameObject.tag == "Coin")
            {
                ++coinCount;
                Destroy(col.gameObject);
            }
            if(col.gameObject.tag != "Ground")
            {
                Debug.Log("Collided while invincible. Sending message");
                SendCollisionMessages(col);
            }
        }
        else
        {
            switch (this.state_)
            {
                case State.MOVING:
                    if (col.gameObject.tag == "BoxObstacle")
                    {
                        if (Shielded)
                        {
                            Destroy(col.gameObject);
                            Shielded = false;
                        }
                        else
                        {
                            Debug.Log("Calling ShowDeathSplash");
                            ShowDeathSplash();
                            this.state_ = State.GAMEOVER;
                        }
                    }
                    if (col.gameObject.tag == "Finish")
                    {
                        this.state_ = State.GAMEOVER;
                    }
                    if (col.gameObject.tag == "Coin")
                    {
                        ++coinCount;
                        Destroy(col.gameObject);
                    }
                    //Shield
                    /*if (col.gameObject.tag == "ShieldPowerup")
                    {
                        Debug.Log("Hit the shield");
                        Shielded = true;
                        Destroy(col.gameObject);
                    }
                    //Speed
                    if (col.gameObject.tag == "SpeedPowerup")
                    {
                        Destroy(col.gameObject);
                        //moveSpeed *= 1.5f;
                        horizontalMoveSpeed *= 1.5f;
                        forwardMoveSpeed *= 1.5f;
                        //jumpThrust *= 1.5f;
                        //gravity *= 0.75f;
                    }*/
                    break;
                case State.JUMPING:
                    if (col.gameObject.tag == "Ground")
                    {
                        this.state_ = State.MOVING;
                    }
                    if (col.gameObject.tag == "BoxObstacle")
                    {
                        if (Shielded)
                        {
                            Destroy(col.gameObject);
                            Shielded = false;
                        }
                        else
                        {
                            Debug.Log("Calling ShowDeathSplash");
                            ShowDeathSplash();
                            this.state_ = State.GAMEOVER;
                        }
                    }
                    if (col.gameObject.tag == "Finish")
                    {
                        this.state_ = State.GAMEOVER;
                    }
                    if (col.gameObject.tag == "Coin")
                    {
                        ++coinCount;
                        Destroy(col.gameObject);
                    }
                    //Shield
                    /*if(col.gameObject.tag == "ShieldPowerup")
                    {
                        Debug.Log("Hit the shield");
                        Shielded = true;
                        Destroy(col.gameObject);
                    }
                    //Speed
                    if(col.gameObject.tag == "SpeedPowerup")
                    {
                        Destroy(col.gameObject);
                        //moveSpeed *= 1.5f;
                        horizontalMoveSpeed *= 1.5f;
                        forwardMoveSpeed *= 1.5f;
                        jumpThrust *= 1.5f;
                        gravity *= 0.75f;
                    }*/
                    break;
            }
        }
    }

    public void SendCollisionMessages(ControllerColliderHit col)
    {
        foreach(GameObject go in EventSystemListeners.main.listeners)
        {
            ExecuteEvents.Execute<IPlayerEvents>(go, null, (x, y) => x.OnPlayerCollide(col));
        }
    }

    public void SetState(State newState)
    {
        this.state_ = newState;
    }

    public void SetPowerUpState(PowerUpState newState)
    {
        this.powerUpState = newState;
    }

    public void BoostMoveSpeed(float multiplier)
    {
        this.forwardMoveSpeed *= multiplier;
    }

    void ShowDeathSplash()
    {
        Debug.Log("Showing splash");
        GameObject deathSplash = Instantiate(deathParticles) as GameObject;
        deathSplash.transform.position = transform.position;
        this.GetComponent<MeshRenderer>().enabled = false;
        this.GetComponent<TrailRenderer>().enabled = false;
    }

    IEnumerator HandleGameOver()
    {
        yield return new WaitForSeconds(1);
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
