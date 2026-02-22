using System;
using Unity.Behavior;
using Unity.VisualScripting;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Target is visible", story: "[Target] is visible by [Self]", category: "Conditions", id: "ac6caf99a28385296a6b6cfe0ff18f5f")]
public partial class TargetIsVisibleCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<GameObject> Self;


    public override bool IsTrue()
    {
     //   Debug.Log("Target "+ Target.Value.transform.position);
     //   Debug.Log("Self " + Self.Value.transform.position);
        bool result = false;
        if (Target.Value == null || Self.Value == null)
        {
            result = false;
        }
        else
        {
            Ray ray = new Ray(Self.Value.transform.position, Target.Value.transform.position - Self.Value.transform.position);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 8))
            {
                if (hitInfo.collider.gameObject != Target.Value)
                {
                    result = false;
                }
                else
                {
                    Debug.Log("detected gameObject " + hitInfo.collider.gameObject.name);
                    result = true;
                }
            }
        }

    //    Debug.Log("TargetIsVisibleCondition result: " + result);    
        return result;
    }
    public void OnDrawGizmos()
    {
        
    }
    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
