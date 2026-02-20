using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class EnemyControls : MonoBehaviour
{
    private float enemyHealth = 100f;

    [Header("Received event")]
    public LampExplodeAtPositionEvent lampExplodeAtPositionEvent;
    public PlayerShotAtPositionEvent playerShotAtPositionEvent;

    [Header("AI Behavior Graph variables")]
    [SerializeField] BehaviorGraphAgent graphAgent;
    [SerializeField] BlackboardVariable<NavMeshAgent> navMeshAgent;

    private void Start()
    {
        graphAgent = GetComponent<BehaviorGraphAgent>();

        if (graphAgent.GetVariable("NavMeshAgent", out navMeshAgent)) {
            lampExplodeAtPositionEvent.Event += OnLampExplode;
            playerShotAtPositionEvent.Event += OnPlayerShotAtPosition;

        }
   
    }

    private void OnDestroy()
    {
        if(navMeshAgent != null)
        {
            lampExplodeAtPositionEvent.Event -= OnLampExplode;
            playerShotAtPositionEvent.Event -= OnPlayerShotAtPosition;
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

    private void OnPlayerShotAtPosition(Vector3 position)
    {
        Debug.Log("Enemy received player shot at position event  " + position);
        navMeshAgent.Value.SetDestination(position);
    }

    /// <summary>
    /// Searches for a GameObject with the tag "Player" within a 10-unit radius of the current transform.
    /// </summary>
    /// <returns>The first GameObject with the tag "Player" found within range, or null if none are found.</returns>
    public GameObject DetectedGameObject()
    {
        Collider[] collider = Physics.OverlapSphere(transform.position, 100f);
        for (int i = 0; i < collider.Length; i++)
        {
            if (collider[i].gameObject.tag == "Player")
            {
                Debug.Log("Player Detected");
                return collider[i].gameObject;
            }
        }
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

    /// <summary>
    /// Reduces the enemy's health by 50 and logs the updated health value.
    /// </summary>
    public void DecreaseHealth()
    {
        enemyHealth -= 50f;
        Debug.Log("Enemy health decreased: " + enemyHealth);
    }

    /// <summary>
    /// Determines whether the enemy's health is zero or less.
    /// </summary>
    /// <returns>True if the enemy is dead; otherwise, false.</returns>
    public bool IsDead()
    {
        return enemyHealth <= 0f;
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
