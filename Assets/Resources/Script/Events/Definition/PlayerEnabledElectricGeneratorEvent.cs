using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/PlayerEnabledElectricGeneratorEvent")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "PlayerEnabledElectricGeneratorEvent", 
    message: "Player enabled electric generator", category: "Events", id: "2hvd7iv3ourf7xgwlhgru5pku9qtdugc")]
public class PlayerEnabledElectricGeneratorEvent : GenericScriptableObject<GameObject>
{

}
