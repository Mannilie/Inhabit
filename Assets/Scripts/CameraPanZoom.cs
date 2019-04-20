using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPanZoom : MonoBehaviour
{
    public Camera attachedCamera;
    public float zoomSensitivity = 1f;
    public float minSize = 1f, maxSize = 5f;
    public Vector2 limitSize = new Vector2(20, 20);
    private Vector3 currMousePos, prevMousePos;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, limitSize);
    }
    private void Update()
    {
        // Get screen mouse position
        currMousePos = Input.mousePosition;
        // Get input scroll axis
        float inputScroll = Input.GetAxis("Mouse ScrollWheel");
        if (inputScroll != 0)
        {
            // Zoom to mouse world coordinate by input scroll
            Zoom(currMousePos, zoomSensitivity * inputScroll);
        }
    }
    private void LateUpdate()
    {
        // Unlock the cursor before panning (because Pan will hide it)
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // If Middle mouse is down
        if (Input.GetMouseButton(2))
        {
            // Pan the camera based on previous and current mouse positions
            Pan(prevMousePos, currMousePos);
        }

        // Calculate dimensions of camera
        float height = attachedCamera.orthographicSize * 2.0f;
        float width = height * attachedCamera.aspect;
        Vector2 size = new Vector2(width, height);
        // Filter position to stay within defined bounds
        attachedCamera.transform.position = Restrict(attachedCamera.transform.position, size);

        // Record last mouse position
        prevMousePos = Input.mousePosition;
    }
    private Vector3 Restrict(Vector3 position, Vector2 size)
    {
        // Create bounds using limitsize and incoming position
        Bounds limit = new Bounds(transform.position, limitSize);
        Bounds point = new Bounds(position, size);

        // Filter the position
        if (point.max.x > limit.max.x) position.x = limit.max.x - point.extents.x;
        if (point.min.x < limit.min.x) position.x = limit.min.x + point.extents.x;
        if (point.max.y > limit.max.y) position.y = limit.max.y - point.extents.y;
        if (point.min.y < limit.min.y) position.y = limit.min.y + point.extents.y;
        
        // Return modified position
        return position;
    }

    public void Pan(Vector3 prevScreenPos, Vector3 currScreenPos)
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        Vector3 prevWorldPos = attachedCamera.ScreenToWorldPoint(prevScreenPos);
        Vector3 currWorldPos = attachedCamera.ScreenToWorldPoint(currScreenPos);
        // Get movement as a vector2 to cancel out Z
        Vector2 delta = prevWorldPos - currWorldPos;
        // Translate camera
        attachedCamera.transform.Translate(delta * 1f);
    }
    public void Zoom(Vector3 screenPoint, float amount)
    {
        // Record the old size
        float oldSize = attachedCamera.orthographicSize;
        // Calculate new size based on min and max limits
        float newSize = Mathf.Clamp(oldSize - amount, minSize, maxSize);
        // Get difference between old and new size
        float difference = oldSize - newSize;
        // Get a percentage of zoom based on old size and difference
        float percentage = 1.0f / oldSize * difference;
        // Get world point from screen point
        Vector3 worldPoint = attachedCamera.ScreenToWorldPoint(screenPoint);
        // Get direction to target point
        Vector3 direction = worldPoint - attachedCamera.transform.position;
        // Move camera percentage of the way to target
        attachedCamera.transform.position += direction * percentage;
        // Apply zoom to camera
        attachedCamera.orthographicSize = newSize;
    }
}
