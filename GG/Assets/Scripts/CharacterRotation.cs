using UnityEngine;

public class CharacterRotation : MonoBehaviour
{
    public Player player;

    public float speed = 2f;
    public float radius = 5f;
    public bool isClockwise = true;
    public Transform pivotPoint;
    public Transform otherCharacter;
    public GameObject rotatingObject;
    public Animator animator;

    private void Update()
    {
        if (pivotPoint == null || otherCharacter == null || rotatingObject == null || animator == null)
        {
            Debug.LogError("Missing references in CharacterMovement script.");
            return;
        }

        // Calculate the angle based on the direction
        float angle = isClockwise ? -speed : speed;
        angle *= Time.deltaTime;

        // Rotate the characters around the pivot point
        transform.RotateAround(pivotPoint.position, Vector3.up, angle);
        otherCharacter.RotateAround(pivotPoint.position, Vector3.up, angle);

        // Rotate the rotating object in the same speed and direction but on the same spot
        rotatingObject.transform.Rotate(Vector3.up, angle);

        // Set walking animation parameter based on movement
        bool isMoving = Mathf.Abs(angle) > 0.01f;
        animator.SetBool("IsWalking", isMoving);
    }

    public void StopMovement()
    {
        bool isMoving = false;
        animator.SetBool("IsWalking", false);
    }
}



