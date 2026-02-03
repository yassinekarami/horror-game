using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "EnemyPlaySoundAction", story: "[Self] play [Patrol1] or [Patrol2]", category: "Action", id: "312eebf7126093cddd2b15aa8561dd02")]
public partial class EnemyPlaySoundAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<AudioClip> Patrol1;
    [SerializeReference] public BlackboardVariable<AudioClip> Patrol2;

    protected override Status OnStart()
    {
        AudioSource audioSource = Self.Value.GetComponent<AudioSource>();
        AudioClip clipToPlay = UnityEngine.Random.value < 0.5f ? Patrol1.Value : Patrol2.Value;
        audioSource.clip = clipToPlay;
        audioSource.Play();
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        AudioSource audioSource = Self.Value.GetComponent<AudioSource>();
        return audioSource.isPlaying ?  Status.Running : Status.Success;

    }

    protected override void OnEnd()
    {
    }
}

