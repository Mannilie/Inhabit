using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public enum State
    {
        Inhabiting,
        Seeking
    }

    public float movementSpeed = 10f;
    [Header("Inhabit")]
    public bool invertDirection = false;
    public float targetDistance = 2f;
    public float distanceSpeed = 10f;

    public Transform target;
    public State currentState = State.Seeking;

    private float distance = 0f;
    private Vector3 direction;
    
    // Update is called once per frame
    protected virtual void Update()
    {
        if (!target)
            return;

        if (currentState == State.Seeking)
        {
            // Get direction to target
            direction = (target.position - transform.position).normalized;
            // Move fly
            transform.position += direction * movementSpeed * Time.deltaTime;
            distance = Vector3.Distance(target.position, transform.position);
            // Really close to the planet
            if (distance < targetDistance)
            {
                currentState = State.Inhabiting;
            }
        }

        if (currentState == State.Inhabiting)
        {
            distance = Mathf.Lerp(distance, targetDistance, distanceSpeed * Time.deltaTime);

            direction = Quaternion.AngleAxis(invertDirection ? -movementSpeed : movementSpeed, Vector3.forward) * direction;
            transform.position = target.position - direction * distance;
        }
    }

    public void Seek(Transform target)
    {
        if (this.target != target)
        {
            this.target = target;
            currentState = State.Seeking;
        }
    }
}
