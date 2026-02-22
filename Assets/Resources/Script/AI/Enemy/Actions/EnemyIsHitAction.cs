using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "EnemyIsHitAction", story: "Decrease [Controls] health and update [State]", category: "Action", id: "4b30e24438c21499f7e95a4aaf86114c")]
public partial class EnemyIsHitAction : Action
{
    [SerializeReference] public BlackboardVariable<EnemyControls> Controls;
    [SerializeReference] public BlackboardVariable<EnemyState> State;
    protected override Status OnStart()
    {
        if (Controls.Value.IsDead())
        {
            Debug.LogError("EnemyIsHitAction : enemy is already dead");
            return Status.Failure;
        }
        Controls.Value.DecreaseHealth();
        State.Value = Controls.Value.IsDead() ? EnemyState.Dead : EnemyState.Trigger;
        return Status.Success;
    }

   
}

