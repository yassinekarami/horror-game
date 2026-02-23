using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "EnemyIsDeadAction", story: "Handle [animator] and disable [BehaviorAgent] and [NavMeshAgent]", category: "Action", id: "32f7307b0c5e97d494d1de85e58c75a2")]
public partial class EnemyIsDeadAction : Action
{
    [SerializeReference] public BlackboardVariable<Animator> Animator;
    [SerializeReference] public BlackboardVariable<Behaviour> BehaviorAgent;
    [SerializeReference] public BlackboardVariable<NavMeshAgent> NavMeshAgent;
    protected override Status OnStart()
    {
        if (Animator.Value == null || BehaviorAgent.Value == null || NavMeshAgent.Value == null)
        {
            Debug.LogError("EnemyIsDeadAction : Animator / BehaviorAgent / NavMeshAgent is null");
            return Status.Failure;
        }
        Animator.Value.SetBool("Chase", false);
        Animator.Value.SetBool("Patrol", false);
        Animator.Value.SetTrigger("Dead");
  
        NavMeshAgent.Value.isStopped = true;
        NavMeshAgent.Value.enabled = false;
        BehaviorAgent.Value.enabled = false;
        return Status.Success;
    }

    
}

