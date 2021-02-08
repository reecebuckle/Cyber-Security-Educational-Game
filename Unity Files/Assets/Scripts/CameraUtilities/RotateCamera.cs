using UnityEngine;

/*
 * Utility class used to rotate camera's orientation of map by 90 degrees
 */

namespace CameraUtilities
{
    public class RotateCamera : MonoBehaviour
    {
        public void RotateClockwise() => transform.Rotate(Vector3.up, -90, Space.Self);

        public void RotateAntiClockwise() => transform.Rotate(Vector3.up, 90, Space.Self);
    }
}