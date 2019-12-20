using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    enum State { MOVING, JUMPING, GAMEOVER };
	bool Shielded;

    private State state_;
    private CharacterController controller;
    private int coinCount;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float horizontalMoveSpeed;
    [SerializeField] private float forwardMoveSpeed;
    [SerializeField] private float jumpThrust;
    [SerializeField] private float gravity;
    //[SerializeField] private Text coinCounter;
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
        switch(this.state_)
        {
            case State.MOVING:
                if(col.gameObject.tag == "BoxObstacle")
                {
					if(Shielded) {
						Shielded = false;
					} else {
						Debug.Log("Calling ShowDeathSplash");
						ShowDeathSplash();
						this.state_ = State.GAMEOVER;
					}
                }
                if(col.gameObject.tag == "Finish")
                {
                    this.state_ = State.GAMEOVER;
                }
                if(col.gameObject.tag == "Coin")
                {
                    ++coinCount;
                    Destroy(col.gameObject);
                }
                break;
            case State.JUMPING:
                if(col.gameObject.tag == "Ground")
                {
                    this.state_ = State.MOVING;
                }
                if(col.gameObject.tag == "BoxObstacle")
                {
					if(Shielded) {
						Shielded = false;
					} else {
						Debug.Log("Calling ShowDeathSplash");
						ShowDeathSplash();
						this.state_ = State.GAMEOVER;
					}
                }
                if(col.gameObject.tag == "Finish")
                {
                    this.state_ = State.GAMEOVER;
                }
                if (col.gameObject.tag == "Coin")
                {
                    ++coinCount;
                    Destroy(col.gameObject);
                }
				//Shield
				if(col.gameObject.tag == "ShieldPowerup")
				{
					Shielded = true;
				}
				//Speed
				if(col.gameObject.tag == "SpeedPowerup")
				{
					moveSpeed *= 1.5;
					horizontalMoveSpeed *= 1.5;
					forwardMoveSpeed *= 1.5;
					jumpThrust *= 1.5;
					gravity *= 0.75;
				}
                break;
        }
    }

    void ShowDeathSplash()
    {
        Debug.Log("Showing splash");
        GameObject deathSplash = Instantiate(deathParticles) as GameObject;
        deathSplash.transform.position = transform.position;
        this.GetComponent<MeshRenderer>().enabled = false;
        //this.GetComponent<Behaviour>().enabled = false;
        this.GetComponent<TrailRenderer>().enabled = false;
        //Destroy(this.gameObject);
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
