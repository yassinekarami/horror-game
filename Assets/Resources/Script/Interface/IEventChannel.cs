using UnityEngine;

public interface IEventChannel<T> {

    /// <summary>
    /// Broadcasts an event to all listeners subscribed to this channel
    /// </summary>
    /// <param name="data"></param>
    public void Broadcast(T data);
}
