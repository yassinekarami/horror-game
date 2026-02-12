using UnityEngine;

public interface IEventChannel {

    /// <summary>
    /// Broadcasts an event to all listeners subscribed to this channel
    /// passing the position of the event as a parameter.
    /// </summary>
    /// <param name="transform"></param>
    public void Broadcast(Vector3 position);
}
