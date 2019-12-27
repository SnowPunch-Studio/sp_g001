using UnityEngine;
using UnityEngine.EventSystems;

public class PowerUp : MonoBehaviour
{
    public string powerUpName;
    public string powerUpExplanation;
    public string powerUpQuote;
    [Tooltip("Tick true for power ups that are instant use")]
    public bool expiresImmediately;
    public GameObject specialEffect;
    public AudioClip soundEffect;

    protected PlayerController playerController;
    protected SpriteRenderer spriteRenderer;

    protected enum PowerUpState
    {
        InAttractMode,
        IsCollected,
        IsExpiring
    }

    protected PowerUpState powerUpState;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void Start()
    {
        powerUpState = PowerUpState.InAttractMode;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        PowerUpCollected(other.gameObject);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        PowerUpCollected(other.gameObject);
    }

    protected virtual void PowerUpCollected(GameObject gameObjectCollectingPowerUp)
    {
        if(gameObjectCollectingPowerUp.tag != "Player")
        {
            return;
        }

        if(powerUpState == PowerUpState.IsCollected || powerUpState == PowerUpState.IsExpiring)
        {
            return;
        }

        powerUpState = PowerUpState.IsCollected;
        playerController = gameObjectCollectingPowerUp.GetComponent<PlayerController>();

        gameObject.transform.parent = playerController.gameObject.transform;
        gameObject.transform.position = playerController.gameObject.transform.position;

        PowerUpEffects();

        PowerUpPayload();

        foreach(GameObject go in EventSystemListeners.main.listeners)
        {
            ExecuteEvents.Execute<IPowerUpEvents> (go, null, (x, y) => x.OnPowerUpCollected(this, playerController));
        }

        //spriteRenderer.enabled = false;
    }

    protected virtual void PowerUpEffects()
    {
        if(specialEffect != null)
        {
            Instantiate(specialEffect, transform.position, transform.rotation, transform);
        }

        if(soundEffect != null)
        {
            //MainGameController.main.PlaySound(soundEffect);
        }
    }

    protected virtual void PowerUpPayload()
    {
        Debug.Log("Power Up collected, issuing payload for: " + gameObject.name);

        if(expiresImmediately)
        {
            PowerUpHasExpired();
        }
    }

    protected virtual void PowerUpHasExpired()
    {
        if(powerUpState == PowerUpState.IsExpiring)
        {
            return;
        }

        powerUpState = PowerUpState.IsExpiring;

        foreach(GameObject go in EventSystemListeners.main.listeners)
        {
            ExecuteEvents.Execute<IPowerUpEvents>(go, null, (x, y) => x.OnPowerUpExpired(this, playerController));
        }

        Debug.Log("Power Up has expired, removing after a delay for: " + gameObject.name);
        DestroySelfAfterDelay();
    }

    protected virtual void DestroySelfAfterDelay()
    {
        Destroy(gameObject);
    }
}
