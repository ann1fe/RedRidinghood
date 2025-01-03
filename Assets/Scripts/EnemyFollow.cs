using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Makes the Enemy chase down Player whenever player is nearby and not looking at the Enemy
/// </summary>
public class EnemyFollow : MonoBehaviour
{
    public NavMeshAgent enemy;
    public Transform player;
    public Camera cam;
    Animator animator;

    public float viewingAngle = 30f;
    public float maxDetectionDistance = 25;
    public float minDetectionDistance = 2;
    private float lastUpdateTime = 0;
    private bool isFrozen = false;
    void Awake() {
        animator = GetComponent<Animator>();
    }
    public void SetFrozen(bool frozen) {
        isFrozen = frozen;
    }

    void Update()
    {
        if (Time.time - lastUpdateTime > 0.5f)
        {
            Vector3 directionToEnemy = (enemy.transform.position - player.transform.position).normalized;

            // Check if the enemy is within ±30 degrees of the player's forward direction
            float angleToEnemy = Vector3.Angle(player.transform.forward, directionToEnemy);
            bool isWithinViewAngle = angleToEnemy <= viewingAngle;

            // Cast a ray from the player's position to the enemy
            Ray ray = new Ray(player.transform.position, directionToEnemy);
            RaycastHit hit;
            // Check if the ray hits the enemy (if there are no obstacles in between)
            bool isDetectable = Physics.Raycast(ray, out hit, maxDetectionDistance) && hit.transform == enemy.transform;

            // can see if it's in view angle and can be hit with raycast
            bool canSeeEnemy = isWithinViewAngle && isDetectable;

            // The enemy moves toward the player if it's not visible and not too close
            if (!canSeeEnemy && hit.distance > minDetectionDistance && !isFrozen)
            {
                enemy.SetDestination(player.position);
                animator.SetBool("isMoving", true);
            }
            else
            {
                enemy.ResetPath();
                animator.SetBool("isMoving", false);
            }

            lastUpdateTime = Time.time;
        }
    }
}
