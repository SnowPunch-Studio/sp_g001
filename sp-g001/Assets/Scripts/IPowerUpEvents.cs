using UnityEngine.EventSystems;

public interface IPowerUpEvents : IEventSystemHandler
{
    void OnPowerUpCollected(PowerUp powerUp, PlayerController player);
    void OnPowerUpExpired(PowerUp powerUp, PlayerController player);
}
