using UnityEngine;

public class AttackController : MonoBehaviour
{

    public Transform targetToAttack;

    public Material idleStateMaterial;
    public Material followStateMaterial;
    public Material attackStateMaterial;
    public float unitDamage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && targetToAttack == null)
        {
            targetToAttack = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") && targetToAttack != null)
        {
            targetToAttack = null;
        }
    }

    public void SetIdleMaterial()
    {
        GetComponent<Renderer>().material = idleStateMaterial;
    }

    public void SetFollowMaterial()
    {
        GetComponent<Renderer>().material = followStateMaterial;
    }

    public void SetAttackMaterial()
    {
        GetComponent<Renderer>().material = attackStateMaterial;
    }

    private void OnDrawGizmos()
    {
        // Follow Distance / Area
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 10f * 0.2f);

        // Attack Distance / Area
                Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1f);

        // Stop Attack Distance / Area
                Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 1.2f);

        if (targetToAttack != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position + new Vector3(1, 0, 1), targetToAttack.position);
        }
    }
}
