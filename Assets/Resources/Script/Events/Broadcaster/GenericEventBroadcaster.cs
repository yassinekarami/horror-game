using Unity.VisualScripting;
using UnityEngine;

public class GenericEventBroadcaster<T> : MonoBehaviour, IEventChannel<T>
{
    public GenericScriptableObject<T> genericScriptableObject;

    public void Broadcast(T data)
    {
        Debug.Log("Generic event broadcasted - Test_Event");
        genericScriptableObject.Broadcast(data);
    }
}
