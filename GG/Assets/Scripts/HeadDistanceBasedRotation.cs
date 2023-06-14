using UnityEngine;
using UnityEngine.Animations.Rigging;
using System.Collections.Generic;

public class HeadDistanceBasedRotation : MonoBehaviour
{
    public Transform playerHeadTransform;
    public List<Transform> targetObjectTransforms = new List<Transform>();
    public float maxDistance = 10f;
    public float smoothSpeed = 5f;

    private MultiAimConstraint multiAimConstraint;
    private float currentWeight;

    private void Start()
    {
        multiAimConstraint = GetComponent<MultiAimConstraint>();
        currentWeight = multiAimConstraint.weight;
    }

    private void Update()
    {
        // Find the closest target object
        Transform closestTarget = GetClosestTarget();

        if (closestTarget != null)
        {
            // Calculate the distance between the player and the closest target object
            float distance = Vector3.Distance(playerHeadTransform.position, closestTarget.position);

            // Calculate the target weight based on the distance
            float targetWeight = distance <= maxDistance ? 1f : 0f;

            // Interpolate between the current weight and the target weight
            currentWeight = Mathf.Lerp(currentWeight, targetWeight, smoothSpeed * Time.deltaTime);

            // Update the weight of the Multi-Aim Constraint
            multiAimConstraint.weight = currentWeight;
        }
        else
        {
            // No target object found, set weight to 0
            currentWeight = 0f;
            multiAimConstraint.weight = 0f;
        }
    }

    private Transform GetClosestTarget()
    {
        Transform closestTarget = null;
        float closestDistance = Mathf.Infinity;

        // Iterate through all target objects and find the closest one
        foreach (Transform target in targetObjectTransforms)
        {
            float distance = Vector3.Distance(playerHeadTransform.position, target.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = target;
            }
        }

        return closestTarget;
    }
}




