using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    public NavMeshAgent enemy;
    public Transform player;
    public Camera cam;
    Animator animator;

    public float detectionDistance = 25;
    public float minDistance = 4;
    private float lastUpdateTime = 0;
    void Awake() {
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastUpdateTime > 0.5)
        {
            Vector3 viewportPoint = cam.WorldToViewportPoint(transform.position);
            bool isInView = viewportPoint.z > 0 &&
                            viewportPoint.x >= 0 && viewportPoint.x <= 1 &&
                            viewportPoint.y >= 0 && viewportPoint.y <= 1;
            float distance = Vector3.Distance(player.transform.position, enemy.transform.position);
            Debug.Log("Distance " + distance);
            if (!isInView && distance < detectionDistance && distance > minDistance)
            {
                //enemy.isStopped = false;
                var ret = enemy.SetDestination(player.position);
                Debug.Log("setDestination ret" + ret);
                lastUpdateTime = Time.time;
                animator.SetBool("isMoving", true);
            }
            else
            {
                Debug.Log("resetpath");
                enemy.ResetPath();
                animator.SetBool("isMoving", false);
                //enemy.velocity = Vector3.zero;
                //enemy.isStopped = true;
            }
        }
    }
}
