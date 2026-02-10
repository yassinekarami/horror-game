using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/EnemyIsHitEvent")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "EnemyIsHitEvent", message: "[Enemy] was hit", category: "Events", id: "e0af55600b3133c1ca07ba06cf7cb297")]
public sealed partial class EnemyIsHitEvent : EventChannel<GameObject> { }

