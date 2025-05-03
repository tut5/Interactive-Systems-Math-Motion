using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Called by TrackingManager to update position
    public void SetPosition(Vector3 newPosition)
    {
        transform.localPosition += newPosition;
    }

    // Called by TrackingManager to update rotation
    public void SetRotation(Quaternion newRotation)
    {
        if (newRotation != Quaternion.identity)
            transform.localRotation = newRotation;
    }
}