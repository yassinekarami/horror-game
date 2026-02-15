using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "EnemyHearSoundAction", story: "[Controls] hear sound at [position]", category: "Action", id: "b107502d916bff83ec2f02abe1031970")]
public partial class EnemyHearSoundAction : Action
{
    [SerializeReference] public BlackboardVariable<EnemyControls> Controls;
    [SerializeReference] public BlackboardVariable<Transform> Position;
    protected override Status OnStart()
    {
        //Position.Value.position = Controls.Value.heardSoundPosition;
        //Debug.Log("onStart positionValue " + Position.Value.position);
        //Vector3 selfPosition = Controls.Value.transform.position;
        //Debug.Log("distance between enemy and heard sound "+Vector3.Distance(selfPosition, Position.Value.position));
        //if (Vector3.Distance(selfPosition, Position.Value.position) < 0.1f)
        //{
        //    return Status.Success;
        //}
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

