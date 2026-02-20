using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/PlayerShotAtPositionEvent")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "PlayerShotAtPositionEvent", message: "Player shot at Position", category: "Events", id: "h4sea6qfvfljhsh1apsaqs17undlq7dz")]
public class PlayerShotAtPositionEvent : GenericScriptableObject<Vector3>
{

   

}
