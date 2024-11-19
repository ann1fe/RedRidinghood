using UnityEngine;
// In addition to WolfAI, it also creates a visual representation of wolf "behind a tree"
// Baby in this context is a billboard that is shown behind the closest tree to the wolf
public class WIPWolfAIWithBaby : MonoBehaviour
{
    public Camera cam; // Assign your camera in the Inspector or get it via code

    public float speedMultiplier = 1.0f;

    public Transform childTransform;

    private bool wasVisible;

    //Is triggering too often
    void Update()
    {
        bool isVisible = IsObjectVisibleAndNotOccluded(transform) || IsObjectVisibleAndNotOccluded(childTransform);

        if (isVisible)
        {
            if (!wasVisible)
            {
                Debug.Log("is visible");
                StopMovement();
                wasVisible = true;
            }
            // No need to call MoveCloserToCamera() when visible
        }
        else
        {
            wasVisible = false;
            Debug.Log("not visible");
            MoveCloserToCamera();
        }
    }
    

    bool IsObjectVisibleAndNotOccluded(Transform objTransform)
    {
        // First, check if the object is within the camera's view frustum
        Vector3 viewportPoint = cam.WorldToViewportPoint(objTransform.position);
        bool isInView = viewportPoint.z > 0 &&
                        viewportPoint.x >= 0 && viewportPoint.x <= 1 &&
                        viewportPoint.y >= 0 && viewportPoint.y <= 1;

        if (!isInView)
            return false;

        // Second, perform a raycast to check for occlusion
        Vector3 direction = (objTransform.position - cam.transform.position).normalized;
        float distance = Vector3.Distance(cam.transform.position, objTransform.position);

        Ray ray = new Ray(cam.transform.position, direction);

        // Use a LayerMask to ignore certain layers if needed
        int layerMask = ~0; // This includes all layers. Adjust as necessary.

        if (Physics.Raycast(ray, out RaycastHit hitInfo, distance, layerMask))
        {
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
        }

        // If the ray didn't hit anything, the object is not occluded (e.g., no colliders)
        return true;
    }

    void MoveCloserToCamera()
    {
        Vector3 direction = (cam.transform.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, cam.transform.position);
        if (distance < 1f)
        {
            //TODO kill player
            return;
        }

        // Calculate speed based on distance
        float speed = distance * speedMultiplier * Time.deltaTime;

        // Move the object toward the camera
        transform.position += direction * speed;
    }

     void StopMovement()
    {
        // Get the active terrain
        Terrain terrain = Terrain.activeTerrain;

        if (terrain == null)
        {
            Debug.LogError("No active terrain found.");
            return;
        }

        // Get all the tree instances on the terrain
        TreeInstance[] treeInstances = terrain.terrainData.treeInstances;
        Vector3 terrainPosition = terrain.transform.position;
        Vector3 terrainSize = terrain.terrainData.size;

        // Variables to keep track of the closest tree
        Vector3 closestTreePosition = Vector3.zero;
        float minDistance = Mathf.Infinity;
        bool foundTree = false;

        foreach (TreeInstance treeInstance in treeInstances)
        {
            // Convert the normalized position to world position
            Vector3 worldTreePosition = new Vector3(
                treeInstance.position.x * terrainSize.x,
                treeInstance.position.y * terrainSize.y,
                treeInstance.position.z * terrainSize.z
            ) + terrainPosition;

            // Create a temporary transform for visibility checks
            // Alternatively, you can create a single transform outside the loop and move it
            // For efficiency, avoid creating new objects in loops if possible

            // Check if the tree is outside of the camera's view
            if (IsPositionVisibleAndNotOccluded(worldTreePosition))
            {
                // The tree is visible, so we skip it
                continue;
            }

            // Tree is outside of view
            float distance = Vector3.Distance(transform.position, worldTreePosition);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestTreePosition = worldTreePosition;
                foundTree = true;
            }
        }

        if (foundTree)
        {
            // Teleport the childTransform to the closest tree's position
            childTransform.position = closestTreePosition;
            //TODO: actually moves the wolf, not the baby!
            AdjustHeight(childTransform);
            childTransform.LookAt(cam.transform);
        }
        else
        {
            Debug.LogWarning("No terrain trees outside of view found.");
        }
    }

    // Method to check if a position is visible and not occluded
    bool IsPositionVisibleAndNotOccluded(Vector3 position)
    {
        Vector3 viewportPoint = cam.WorldToViewportPoint(position);

        // Check if the position is within the camera's viewport
        if (viewportPoint.x >= 0 && viewportPoint.x <= 1 &&
            viewportPoint.y >= 0 && viewportPoint.y <= 1 &&
            viewportPoint.z > 0)
        {
            // The position is within the viewport, now check for occlusion
            RaycastHit hit;
            Vector3 direction = position - cam.transform.position;
            if (Physics.Raycast(cam.transform.position, direction, out hit))
            {
                // If the raycast hits something before reaching the position, it's occluded
                if (hit.point != position)
                {
                    // It's occluded
                    return false;
                }
            }
            // Not occluded
            return true;
        }
        else
        {
            // The position is outside the viewport
            return false;
        }
    }

    void AdjustHeight(Transform objTransform)
    {
        float yOffset = 5f;
        Terrain terrain = Terrain.activeTerrain;
        float x = objTransform.position.x;
        float z = objTransform.position.z;
        Vector3 position = new Vector3(x, 0f, z);
        float terrainHeight = terrain.SampleHeight(position) + terrain.transform.position.y;
        transform.position = new Vector3(x, terrainHeight + yOffset, z);
    }
}
