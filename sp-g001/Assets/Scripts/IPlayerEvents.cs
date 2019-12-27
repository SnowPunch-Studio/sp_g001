using UnityEngine;
using UnityEngine.EventSystems;

public interface IPlayerEvents : IEventSystemHandler
{
    void OnPlayerCollide(ControllerColliderHit other);
    void OnPlayerReachedExit(GameObject exit);
}
