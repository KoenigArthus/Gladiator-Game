using UnityEngine;

public class CharacterRotation : MonoBehaviour
{
    public Player player;

    [SerializeField] public static float speed = 30f;
    public float radius = 5f;
    public bool isClockwise = true;
    public Transform pivotPoint;
    public Transform otherCharacter;
    public Animator animator;

    private void Update()
    {
        if (pivotPoint == null || otherCharacter == null || animator == null)
        {
            Debug.LogError("Missing references in CharacterMovement script.");
            return;
        }

        // Calculate the angle based on the direction
        float angle = isClockwise ? -speed : speed;
        angle *= Time.deltaTime;
        Debug.Log(angle);

        // Rotate the characters around the pivot point
        transform.RotateAround(pivotPoint.position, Vector3.up, angle);
        otherCharacter.RotateAround(pivotPoint.position, Vector3.up, angle);

        // Adjust the character's position relative to the pivot point
        transform.position = (transform.position - pivotPoint.position).normalized * radius + pivotPoint.position;



        // Set walking animation parameter based on movement
        //bool isMoving = Mathf.Abs(angle) > 0.01f;
        //animator.SetBool("IsWalking", isMoving);
    }

    public void StopMovement()
    {
        // Set the speed to zero to stop rotation
        speed = 0f;
        Debug.Log("Speed = 0");
        // Set the IsWalking parameter to false to stop the walking animation
        animator.SetBool("Walking", false);
        animator.SetBool("Idle", true);
    }

    /*public void PlayMovement()
    {
        speed = 30f;
        animator.SetBool("IsWalking", true);

    }*/

}