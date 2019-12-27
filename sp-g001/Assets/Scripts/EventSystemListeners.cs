using System.Collections.Generic;
using UnityEngine;

public class EventSystemListeners : MonoBehaviour
{
    public static EventSystemListeners main;

    [Tooltip("Listeners that want to know about messages.  By default any object with tag = Listener is included, but you can add more here, or add at runtime with method EventSystemListeners.main.AddListener()")]
    public List<GameObject> listeners;

    private void Awake()
    {
        if(main == null)
        {
            main = this;
        }
        else
        {
            Debug.LogWarning("EventSystemListeners re-creation attempted, destroying the new one");
            Destroy(gameObject);
        }
    }

    public void Initialize()
    {
        if(listeners == null)
        {
            listeners = new List<GameObject>();
        }

        GameObject[] gos = GameObject.FindGameObjectsWithTag("Listener");
        listeners.AddRange(gos);
        Debug.Log("Listeners :: " + listeners);
    }

    public void AddListener(GameObject go)
    {
        if(!listeners.Contains(go))
        {
            listeners.Add(go);
        }
    }

    private void Update()
    {

    }
}
