using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform[] waypoints;
    public float reachThreshold = 1.0f;
    public float detectionRadius = 20f;
    public int maxHP = 100;
    public int currentHP;

    private NavMeshAgent agent;
    private Animator animator;
    private GameObject target;
    private int currentWaypointIndex = 0;
    private bool isDead = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        currentHP = maxHP;

        if (waypoints.Length > 0)
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        else
            Debug.LogWarning("No waypoints set for EnemyAI.");
    }

    void Update()
    {
        if (isDead) return;

        // Check for death
        if (currentHP <= 0)
        {
            Die();
            return;
        }

        // Detect and chase player
        FindPlayerTarget();

        if (target != null)
        {
            agent.SetDestination(target.transform.position);
        }
        else
        {
            PatrolWaypoints();
        }

        // Update walk animation
        bool isWalking = agent.velocity.magnitude > 0.1f;
        animator.SetBool("IsWalking", isWalking);
    }

    void PatrolWaypoints()
    {
        if (waypoints.Length == 0) return;

        if (!agent.pathPending && agent.remainingDistance < reachThreshold)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    void FindPlayerTarget()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        float closestDistance = detectionRadius;
        GameObject closest = null;

        foreach (GameObject player in players)
        {
            float dist = Vector3.Distance(transform.position, player.transform.position);
            if (dist < closestDistance)
            {
                closest = player;
                closestDistance = dist;
            }
        }

        target = closest;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHP -= damage;

        if (currentHP <= 0)
            Die();
    }

    private void Die()
{
    isDead = true;
    agent.isStopped = true;
    animator.SetBool("IsDead", true);

    // Optional: disable collider to prevent further interaction
    Collider col = GetComponent<Collider>();
    if (col != null) col.enabled = false;

    // Destroy this GameObject after 5 seconds
    Invoke(nameof(DestroySelf), 1.5f);
}

private void DestroySelf()
{
    Destroy(gameObject);
}

}
