using UnityEngine;

public class WolfAI : MonoBehaviour
{
    public Camera cam;
    public float speedMultiplier = 0.5f;
    public float turnSpeed = 5f; // How quickly the wolf changes direction after hitting something

    private Rigidbody rb;
    private Vector3 currentDirection; // Current movement direction
    private bool isAvoiding = false; // Whether the wolf is actively avoiding an obstacle

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        // Initialize the wolf's direction towards the player
        currentDirection = Vector3.zero;
    }

    void Update()
    {
        if (IsObjectVisibleAndNotOccluded(transform))
        {
            StopMovement();
        }
        else
        {
            MoveCloserToCamera();
        }
    }

    bool IsObjectVisibleAndNotOccluded(Transform objTransform)
    {
        Vector3 viewportPoint = cam.WorldToViewportPoint(objTransform.position);
        bool isInView = viewportPoint.z > 0 &&
                        viewportPoint.x >= 0 && viewportPoint.x <= 1 &&
                        viewportPoint.y >= 0 && viewportPoint.y <= 1;

        if (!isInView)
            return false;

        Vector3 direction = (objTransform.position - cam.transform.position).normalized;
        float distance = Vector3.Distance(cam.transform.position, objTransform.position);

        Ray ray = new Ray(cam.transform.position, direction);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, distance))
        {
            Debug.DrawRay(ray.origin, ray.direction * hitInfo.distance, Color.green);
            return hitInfo.transform == objTransform;
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * distance, Color.magenta);
        }

        return true;
    }

    void MoveCloserToCamera()
    {
        if (isAvoiding) return; // Skip movement if avoiding an obstacle

        Vector3 direction = (cam.transform.position - transform.position).normalized;
        float speed = speedMultiplier * Time.deltaTime;

        currentDirection = direction;

        // Move the wolf
        rb.MovePosition(transform.position + direction * speed);

        // Rotate the wolf to face the camera
        if (direction != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, turnSpeed * Time.deltaTime);
        }
    }

    void StopMovement()
    {
        rb.velocity = Vector3.zero;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the wolf collided with an obstacle
        if (collision.gameObject.CompareTag("Ground"))
        {
            return;
        }
        // Calculate a new direction to avoid the obstacle
        Vector3 collisionNormal = collision.contacts[0].normal;
        currentDirection = Vector3.Reflect(currentDirection, collisionNormal);

        // Start avoiding the obstacle
        isAvoiding = true;

        // Temporarily stop avoiding after a short delay
        Invoke(nameof(ResetAvoidance), 1f);
    }

    void ResetAvoidance()
    {
        isAvoiding = false;
    }
}