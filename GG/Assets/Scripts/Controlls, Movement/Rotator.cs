using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float fadingSpeed;
    public float minSpeed;
    public float currentSpeed;
    public float maxSpeed;
    public float dampingSpeed;

    private void Update()
    {
        // Clamp the currentSpeed between minSpeed and maxSpeed
        currentSpeed = Mathf.Clamp(currentSpeed, minSpeed, maxSpeed);

        // Rotate the object around its Y-axis at the specified rotation speed
        transform.Rotate(Vector3.up, currentSpeed * Time.deltaTime);
    }

    public void StopMovement()
    {
        LeanTween.value(gameObject, currentSpeed, 0f, fadingSpeed)
        .setOnUpdate((float value) => { currentSpeed = value; });
    }

    public void StartMovement()
    {
        LeanTween.value(gameObject, currentSpeed, maxSpeed, fadingSpeed)
        .setOnUpdate((float value) => { currentSpeed = value; });
    }


}
