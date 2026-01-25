using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "EnemyChaseAction", story: "attack [Target] if is in [AttackRange] [EnemyState]", category: "Action", id: "d753676889be4528baea4517a7de2a8c")]
public partial class EnemyChaseAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<EnemyControls> AttackRange;
    protected override Status OnUpdate()
    {
        if (Target != null)
        {
            if (AttackRange.Value.IsInAttackRange(Target.Value))
            {
                // kill the player
                GameObject.Destroy(Target.Value);
                return Status.Success;
            }
        }
        return Status.Failure;
    }
}

