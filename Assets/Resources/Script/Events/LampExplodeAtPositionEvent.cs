using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/LampExplodeAtPositionEvent")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "LampExplodeAtPositionEvent", message: "Lamp explode at position", category: "Events", id: "aac7b3fa187deb667239dc0209046d88")]
public sealed partial class LampExplodeAtPositionEvent : EventChannel<Vector3>, IEventChannel
{
    /// <summary>
    /// Broadcasts an event message at the specified position.
    /// </summary>
    /// <param name="position">The position where the event message is broadcast.</param>
    public void Broadcast(Vector3 position)
    {
        this.SendEventMessage(position);
    }
}

