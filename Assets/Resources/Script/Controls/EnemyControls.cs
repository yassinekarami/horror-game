using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class EnemyControls : MonoBehaviour
{
    public EnemyIsHitEvent enemyIsHitEvent;
    public LampExplodeAtPositionEvent lampExplodeAtPositionEvent;

    [SerializeField] private BehaviorGraphAgent graphAgent;
    private BlackboardVariable<NavMeshAgent> navMeshAgent;

    private void Start()
    {
        graphAgent = GetComponent<BehaviorGraphAgent>();

        if (graphAgent.GetVariable("NavMeshAgent", out navMeshAgent)) {
            lampExplodeAtPositionEvent.Event += OnLampExplode;
        }
        enemyIsHitEvent.Event += EnemyIsHitEvent;
   
    }

    private void OnDestroy()
    {
        enemyIsHitEvent.Event -= EnemyIsHitEvent;
        if(navMeshAgent != null)
        {
            lampExplodeAtPositionEvent.Event -= OnLampExplode;
        }
 
    }
    private void EnemyIsHitEvent(GameObject value0)
    {
        Debug.Log("enemy is hit event triggered - Test_Event " + value0.name + " hit by ");
    }

    private void OnLampExplode(Vector3 position)
    {
        Debug.Log("Enemy received lamp explode event  " + position);
        navMeshAgent.Value.SetDestination(position);

    }

    /// <summary>
    /// Searches for a GameObject with the tag "Player" within a 10-unit radius of the current transform.
    /// </summary>
    /// <returns>The first GameObject with the tag "Player" found within range, or null if none are found.</returns>
    public GameObject DetectedGameObject()
    {
        //Collider[] collider = Physics.OverlapSphere(transform.position, detectTargetRange);
        //for (int i = 0; i < collider.Length; i++)
        //{
        //    if (collider[i].gameObject.tag == "Player")
        //    {   Debug.Log("Player Detected");
        //        return collider[i].gameObject;
        //    }
        //}
        return null;
    }

    /// <summary>
    /// check if the target is in attack range
    /// </summary>
    /// <param name="target"></param>
    /// <returns>true is the target is in attack range false otherwise</returns>
    public bool IsInAttackRange(GameObject target)
    {
        float distance = Vector3.Distance(transform.position, target.transform.position);
        //  return distance <= attackRange;
        return false;
    }


    //GIZMO
    private void OnDrawGizmos()
    {
 //       Gizmos.color = Color.red;
 //       Gizmos.DrawWireSphere(transform.position, attackRange);

 //       Gizmos.color = Color.green;
 //       Gizmos.DrawWireSphere(transform.position, detectTargetRange);
    }
}
