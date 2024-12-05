using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class bloodSprayEffect : MonoBehaviour
{
    private Vector3 target;
    private float fallSpeed = 0.01f;
    private float acceleration = 0.0f;
    private float inertia = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        target = transform.position - new Vector3(0, 5, 0);
        // Move the object towards the target point
        transform.position = Vector3.MoveTowards(transform.position, target, fallSpeed*acceleration);

        inertia -= 0.34f;

        transform.localScale = transform.localScale + new Vector3(0.0f, 0.1f, 0.1f);

        if (inertia >= 0.0f)
        {
            transform.localScale = transform.localScale + new Vector3(0.1f, 0.0f, 0.0f);


        }
        if (inertia < 0.0f)
        {
            transform.localScale = transform.localScale - new Vector3(0.01f, 0.08f, 0.08f);
            acceleration += 1.5f;

        }
        if (transform.position.y < -1)
        {
            Debug.Log("Below -1");
            Destroy(gameObject);
        }
    }


}
