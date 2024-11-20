using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    public NavMeshAgent enemy;
    public Transform Player;
    public Camera cam;
    private float lastUpdateTime = 0;
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

            if (!isInView)
            {
                //enemy.isStopped = false;
                var ret = enemy.SetDestination(Player.position);
                Debug.Log("setDestination ret" + ret);
                lastUpdateTime = Time.time;
            }
            else
            {
                Debug.Log("resetpath");
                enemy.ResetPath();
                //enemy.velocity = Vector3.zero;
                //enemy.isStopped = true;
            }
        }
    }
}
