using UnityEngine;

public class RotationLock : MonoBehaviour
{
    [SerializeField] private Quaternion initialRotation;

    private void Awake()
    {
        // Store the initial rotation of the GameObject
        initialRotation = transform.rotation;
    }

    private void LateUpdate()
    {
        // Reset the rotation of the GameObject to the initial rotation
        transform.rotation = initialRotation;
    }
}