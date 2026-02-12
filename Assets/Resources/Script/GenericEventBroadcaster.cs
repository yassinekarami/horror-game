using Unity.VisualScripting;
using UnityEngine;

public class GenericEventBroadcaster : MonoBehaviour, IEventChannel
{
    public LampExplodeAtPositionEvent eventChannel;

    public void Broadcast(Vector3 position)
    {
        Debug.Log("Generic event broadcasted - Test_Event");
        eventChannel.Broadcast(position);

    }
}
