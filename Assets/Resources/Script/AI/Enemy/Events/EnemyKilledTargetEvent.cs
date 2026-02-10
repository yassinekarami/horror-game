using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/EnemyKilledTargetEvent")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "EnemyKilledTargetEvent", message: "Enemy killed [Target]", category: "Events", id: "42859d79f3f88e0fbb6be033edf4ef1a")]
public sealed partial class EnemyKilledTargetEvent : EventChannel<GameObject>
{
    internal void CreateEventHandler(GameObject gameObject)
    {
        throw new NotImplementedException();
    }

    internal void RegisterListener(System.Action value)
    {
        Debug.Log("RegisterListener");
      
    }
}

