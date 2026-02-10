using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Patrol1OrPatrol2IsPlayingCondition", story: "[Self] is playing [Patrol1] or [Patrol2]", category: "Conditions", id: "d8437b9f5dc3ac8bac4e25689877054e")]
public partial class Patrol1OrPatrol2IsPlayingCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<AudioClip> Patrol1;
    [SerializeReference] public BlackboardVariable<AudioClip> Patrol2;

    public override bool IsTrue()
    {
        Debug.Log(Self.Value.GetComponent<AudioSource>().isPlaying);
        AudioSource audioSource = Self.Value.GetComponent<AudioSource>();
        return audioSource.isPlaying; 
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
