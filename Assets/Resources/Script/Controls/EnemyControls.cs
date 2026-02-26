using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class EnemyControls : MonoBehaviour
{
    [Header("Enemy attributes")]
    [SerializeField] private float enemyHealth = 100f;
    [SerializeField] private float hearSoundRange = 2f;

    [Header("Received event")]
    public LampExplodeAtPositionEvent lampExplodeAtPositionEvent;
    public PlayerShotAtPositionEvent playerShotAtPositionEvent;

    [Header("AI Behavior Graph variables")]
    [SerializeField] BehaviorGraphAgent graphAgent;
    [SerializeField] BlackboardVariable<NavMeshAgent> navMeshAgent;
    [SerializeField] BlackboardVariable<EnemyState> enemyState;
    [SerializeField] BlackboardVariable<float> NavMeshAgentSpeed;
    [SerializeField] BlackboardVariable<float> ChaseRange;

    private void Start()
    {
        graphAgent = GetComponent<BehaviorGraphAgent>();
        if (graphAgent.GetVariable("State", out enemyState) && graphAgent.GetVariable("NavMeshAgentSpeed", out NavMeshAgentSpeed))
        {
            enemyState.OnValueChanged += OnStateValueChanged;
        }
        if (graphAgent.GetVariable("NavMeshAgent", out navMeshAgent)) {
            lampExplodeAtPositionEvent.Event += OnLampExplode;
            playerShotAtPositionEvent.Event += OnPlayerShotAtPosition;
        }
    }

    private void OnDestroy()
    {
        if (enemyState != null)
        {
            enemyState.OnValueChanged -= OnStateValueChanged;
        }
        if (navMeshAgent != null)
        {
            lampExplodeAtPositionEvent.Event -= OnLampExplode;
            playerShotAtPositionEvent.Event -= OnPlayerShotAtPosition;
        }
    }
    /// <summary>
    /// Handles the event when a lamp explodes by logging the event and setting the agent's destination to the explosion
    /// position.
    /// </summary>
    /// <param name="position">The world position where the lamp exploded.</param>

    private void OnLampExplode(Vector3 eventPosition)
    {
        Debug.Log("Enemy received lamp explode event " + eventPosition);
        Debug.Log(Vector3.Distance(transform.position, eventPosition));
        if (Vector3.Distance(transform.position, eventPosition) <= hearSoundRange)
        {
            hearSoundRange += 0.5f;
            navMeshAgent.Value.SetDestination(eventPosition);
        }
    }

    /// <summary>
    /// Handles the event when the player shoots at a position by logging the event and setting the agent's destination to the
    /// </summary>
    /// <param name="position">The world position where the player shot.</param>
    private void OnPlayerShotAtPosition(Vector3 eventPosition)
    {
        Debug.Log("Enemy received player shot at position event  " + eventPosition);
        if (Vector3.Distance(transform.position, eventPosition) <= hearSoundRange)
        {
            enemyState.Value = EnemyState.Trigger;
        }
    }

    /// <summary>
    /// Change the navMeshAgent speed according to the enemy state
    /// </summary>
    private void OnStateValueChanged()
    {
       switch (enemyState.Value)
       {
            case EnemyState.Idle:
                NavMeshAgentSpeed.Value = 0f;
                break;
            case EnemyState.Patrol:
                NavMeshAgentSpeed.Value = 3.5f;
                break;
            case EnemyState.Chase:
                NavMeshAgentSpeed.Value = 0f;
                break;
            case EnemyState.Attack:
                NavMeshAgentSpeed.Value = 0f;
                break;
       }
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
    /// 
    /// </summary>
    public void ChangeCurrentStateToHitState()
    {
        enemyState.Value = EnemyState.Hit;
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ChaseRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, hearSoundRange);
    }
}
