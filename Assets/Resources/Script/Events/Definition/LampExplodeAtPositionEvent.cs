using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/LampExplodeAtPositionEvent")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "LampExplodeAtPositionEvent", message: "Lamp explode at position", category: "Events", id: "aac7b3fa187deb667239dc0209046d88")]
public sealed partial class LampExplodeAtPositionEvent : GenericScriptableObject<Vector3>, IEventChannel<Vector3>
{

}



