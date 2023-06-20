using UnityEngine;
using UnityEngine.Animations.Rigging;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using System;

public class HeadDistanceBasedRotation : MonoBehaviour
{
    public Transform playerHeadTransform;
    public List<Transform> targetObjectTransforms = new List<Transform>();
    public float maxDistance = 5f;
    public float smoothSpeed = 5f;

    private MultiAimConstraint multiAimConstraint;
    private float currentWeight;

    private void Start()
    {
        multiAimConstraint = GetComponent<MultiAimConstraint>();
        currentWeight = 0f;
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
            WeightedTransform weightedTransform = multiAimConstraint.data.sourceObjects.Where(x => x.transform == closestTarget).FirstOrDefault();
            int index = multiAimConstraint.data.sourceObjects.IndexOf(weightedTransform);

            var sources = multiAimConstraint.data.sourceObjects;

            for (int i = 0; i < sources.Count; i++)
                sources.SetWeight(i, 0f);

            sources.SetWeight(index, currentWeight);
            multiAimConstraint.data.sourceObjects = sources;
        }
        else
        {
            var sources = multiAimConstraint.data.sourceObjects;

            for(int i = 0; i < sources.Count; i++)
            {
              sources.SetWeight(i, 0f);
            }

            multiAimConstraint.data.sourceObjects = sources;
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

        float targetDistance = Vector3.Distance(playerHeadTransform.position, closestTarget.position);

        if (targetDistance <= maxDistance)
            return closestTarget;
        else
            return null;
    }
}




