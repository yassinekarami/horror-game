using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Target is visible", story: "[Target] is visible by [Self]", category: "Conditions", id: "ac6caf99a28385296a6b6cfe0ff18f5f")]
public partial class TargetIsVisibleCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<GameObject> Self;

    public override bool IsTrue()
    {
        Ray ray = new Ray(Self.Value.transform.position, Target.Value.transform.position - Self.Value.transform.position);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {   Debug.Log(hitInfo.collider.gameObject.name);
            if (hitInfo.collider.gameObject != Target.Value)
            {
                return false;
            }
        }
        return true;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
