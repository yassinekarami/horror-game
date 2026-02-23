using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "EnemyChaseAction", story: "[Agent] move to [target] and quit if [state] change", category: "Action", id: "d753676889be4528baea4517a7de2a8c")]
public partial class EnemyChaseAction : Action
{
    [SerializeReference] public BlackboardVariable<UnityEngine.AI.NavMeshAgent> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<EnemyState> State;

    protected override Status OnStart()
    {
        if (Agent != null && Target != null)
        {
            Agent.Value.SetDestination(Target.Value.transform.position);
        }
        return Status.Success;
    }


    protected override Status OnUpdate()
    {
        if (State.Value != EnemyState.Chase)
        {
            return Status.Failure;
        }
        return Status.Success;
    }
}

