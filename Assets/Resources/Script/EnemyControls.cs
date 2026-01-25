using UnityEngine;

public class EnemyControls : MonoBehaviour
{
    public float attackRange;

    /// <summary>
    /// Searches for a GameObject with the tag "Player" within a 10-unit radius of the current transform.
    /// </summary>
    /// <returns>The first GameObject with the tag "Player" found within range, or null if none are found.</returns>
    public GameObject DetectedGameObject()
    {
        Collider[] collider = Physics.OverlapSphere(transform.position, 10f);
        for (int i = 0; i < collider.Length; i++)
        {
            if (collider[i].gameObject.tag == "Player")
            {
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
        return distance <= attackRange;
    }


    //GIZMO
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
