using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "EnemyPatrolAction", story: "Check if [Target] is returned by [Controls] and update [State]", category: "Action", id: "2203d00b4485e206c7925cc0a244d61d")]
public partial class EnemyPatrolAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<EnemyControls> Controls;
    [SerializeReference] public BlackboardVariable<EnemyState> State;
    protected override Status OnUpdate()
    {
        Target.Value = Controls.Value.DetectedGameObject();
        Debug.Log("Patrol Action: " + (Target.Value == null ? "No Target" : "Target Acquired "+Target.Name));
        State.Value = Target.Value == null ? EnemyState.Patrol : EnemyState.Chase;
        return Target == null ? Status.Failure : Status.Success;
    }
}

