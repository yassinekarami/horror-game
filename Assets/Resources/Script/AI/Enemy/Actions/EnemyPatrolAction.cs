using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using System.Collections.Generic;
using Random = UnityEngine.Random;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "EnemyPatrolAction", story: "[NavMesh] patrols and [audio] play [sounds] and wait when all [waypoints] has been visited", category: "Action", id: "2203d00b4485e206c7925cc0a244d61d")]
public partial class EnemyPatrolAction : Action
{
    [SerializeReference] public BlackboardVariable<UnityEngine.AI.NavMeshAgent> NavMesh;
    [SerializeReference] public BlackboardVariable<AudioSource> Audio;
    [SerializeReference] public BlackboardVariable<ScriptableObject> Sounds;
    [SerializeReference] public BlackboardVariable<List<GameObject>> Waypoints;
    protected override Status OnUpdate()
    {
  
        SoundsScriptableObject sound = (Sounds.Value as SoundsScriptableObject);
        sound.PlayRandomAudioClip(Audio.Value);

        bool patrol = Patrols();


        return patrol ? Status.Success : Status.Failure ;
    }

    private bool Patrols()
    {
        if (NavMesh != null && Waypoints.Value != null && Waypoints.Value.Count > 0)
        {
            if (NavMesh.Value.remainingDistance <= NavMesh.Value.stoppingDistance)
            {
                Debug.Log("Waypoints count " + Waypoints.Value.Count);
                List<GameObject> waypoints = Waypoints.Value;
                Vector3 currentNavMeshDestination = NavMesh.Value.destination;
                Vector3 newNavMeshDestination;
                do
                {
                    int randomIndex = Random.Range(0, waypoints.Count);
                    Debug.Log("Random index " + randomIndex);
                    newNavMeshDestination = waypoints[randomIndex].transform.position;
                }
                while (currentNavMeshDestination == newNavMeshDestination);

                NavMesh.Value.SetDestination(newNavMeshDestination);
                if (NavMesh.Value.remainingDistance <= NavMesh.Value.stoppingDistance)
                {
                    return true;
                }
            }
        }        
        return false;

    }
}

