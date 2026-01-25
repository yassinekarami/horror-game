using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "EnemyPatrol", story: "Update [enemyDetection] and assign [Target]", category: "Action", id: "2203d00b4485e206c7925cc0a244d61d")]
public partial class EnemyPatrolAction : Action
{
    [SerializeReference] public BlackboardVariable<EnemyControls> EnemyDetection;
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    protected override Status OnUpdate()
    {
        Target.Value = EnemyDetection.Value.DetectedGameObject();
        return Target == null ? Status.Failure : Status.Success;
    }
}

