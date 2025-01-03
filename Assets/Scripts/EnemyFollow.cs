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

    public float detectionDistance = 25;
    public float minDistance = 4;
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
            // Cast a ray from the player's position to the enemy
            Vector3 directionToEnemy = (enemy.transform.position - player.transform.position).normalized;

            // Check if the enemy is within ±30 degrees of the player's forward direction
            float angleToEnemy = Vector3.Angle(player.transform.forward, directionToEnemy);
            bool isWithinAngle = angleToEnemy <= 30f;

            Ray ray = new Ray(player.transform.position, directionToEnemy);
            RaycastHit hit;
            // Check if the ray hits the enemy and if there are no obstacles in between
            bool isEnemyVisible = Physics.Raycast(ray, out hit, detectionDistance) && hit.transform == enemy.transform;

            float distance = Vector3.Distance(player.transform.position, enemy.transform.position);
            // The enemy moves toward the player if it's not visible, within detection distance, and not too close or far
            if (!(isEnemyVisible && isWithinAngle) && distance < detectionDistance && distance > minDistance && !isFrozen)
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
