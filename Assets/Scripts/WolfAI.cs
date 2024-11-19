using UnityEngine;

public class WolfAI : MonoBehaviour
{
    public Camera cam;

    public float speedMultiplier = 0.5f;

    void Update()
    {
        if (IsObjectVisibleAndNotOccluded(transform))
        {
            // Debug.Log("is visible");
            StopMovement();
        }
        else
        {
            // Debug.Log("not visible");
            MoveCloserToCamera();
        }
    }
    

    bool IsObjectVisibleAndNotOccluded(Transform objTransform)
    {
        // Check if the object is within the camera's view
        Vector3 viewportPoint = cam.WorldToViewportPoint(objTransform.position);
        bool isInView = viewportPoint.z > 0 &&
                        viewportPoint.x >= 0 && viewportPoint.x <= 1 &&
                        viewportPoint.y >= 0 && viewportPoint.y <= 1;

        if (!isInView)
            return false;

        // Perform a raycast to check for occlusion
        Vector3 direction = (objTransform.position - cam.transform.position).normalized;
        float distance = Vector3.Distance(cam.transform.position, objTransform.position);

        Ray ray = new Ray(cam.transform.position, direction);

      
        
        //int layerMask = ~0; 

        if (Physics.Raycast(ray, out RaycastHit hitInfo, distance))
        {
            Debug.DrawRay(ray.origin,ray.direction * hitInfo.distance, Color.green);  
            if (hitInfo.transform == objTransform)
            {

                // The ray hit the object directly; it's not occluded
                return true;
            }
            else
            {
            
                // The ray hit another object before reaching the target object; it's occluded
                return false;
            }
        } else {
            Debug.DrawRay(ray.origin,ray.direction * distance, Color.magenta);  

        }

        // If the ray didn't hit anything, the object is not occluded 
        return true;
    }

    void MoveCloserToCamera()
    {
        Vector3 direction = (cam.transform.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, cam.transform.position);
        if (distance < 2f)
        {
            GameManager.Instance.GameOver();
            return;
        }

        // Calculate speed based on distance
        float speed = distance * speedMultiplier * Time.deltaTime;

        // Move the object toward the camera
        transform.position += direction * speed;
    }

     void StopMovement()
    {
        // Do nothing
    }
}
