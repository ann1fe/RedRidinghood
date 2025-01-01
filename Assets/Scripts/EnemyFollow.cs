using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
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

    void Update() {
        if (Time.time - lastUpdateTime > 0.5)
        {
            // Transform enemy position into camera coordinate system
            Vector3 viewportPoint = cam.WorldToViewportPoint(enemy.transform.position);

            // Checks if the enemy is within the camera's view range
            bool isInView = viewportPoint.z > 0 &&
                            viewportPoint.x >= 0 && viewportPoint.x <= 1 &&
                            viewportPoint.y >= 0 && viewportPoint.y <= 1;

            float distance = Vector3.Distance(player.transform.position, enemy.transform.position);

            // enemy is moving towards the player if player is not looking at the enemy (isInView=false)
            // and if player is not too close and not too far
            if (!isInView && distance < detectionDistance && distance > minDistance && !isFrozen)
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
