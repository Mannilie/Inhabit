using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour
{
    public float speed = 10f;
    public float radialDistance = 2f;
    public Transform target;
    
    // Update is called once per frame
    void Update()
    {
        if (transform.parent != target)
        {
            transform.SetParent(null);

            Vector3 direction = target.position - transform.position;
            transform.position += direction.normalized * speed * Time.deltaTime;

            float distance = Vector3.Distance(target.position, transform.position);
            if (distance < radialDistance)
            {
                transform.SetParent(target);
            }
        }
    }
}
