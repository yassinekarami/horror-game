using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;
using Unity.VisualScripting;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/EnemyIsHitEvent")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "EnemyIsHitEvent", message: "[Enemy] was hit", category: "Events", id: "e0af55600b3133c1ca07ba06cf7cb297")]
public sealed partial class EnemyIsHitEvent : EventChannel<GameObject>
{
    private void OnEnable()
    {
        
    }
    public new void SendEventMessage(GameObject value)
    {
        
       if (value == null)
       {
            Debug.LogError("EnemyIsHitEvent : value is null");
            return;
       }
       
        base.SendEventMessage(value);   

        //if (value.gameObject.name.Equals(Enemy.Value.name))
        //{
        //     Debug.Log("EnemyIsHitEvent : " + value.name + " was hit");
        //     base.SendEventMessage(value);
        //}

    }
}

