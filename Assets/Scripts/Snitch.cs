using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snitch : MonoBehaviour
{

    private float maxVelocity = 10;
    private float maxAccel = 20;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 d = RandomForwardDirection() * maxAccel;
        rb.AddRelativeForce(d);
        transform.localRotation = Quaternion.LookRotation(rb.velocity);

        //Capping velocity
        //https://answers.unity.com/questions/683158/how-to-limit-speed-of-a-rigidbody.html
        if (rb.velocity.magnitude > maxVelocity)
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);

        Debug.Log(rb.velocity.magnitude);
    }

    private Vector3 RandomForwardDirection()
    {
        Vector3 forward = Vector3.forward * Random.Range(0f, 1f);
        Vector3 turn = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.forward) * Vector3.right;
        Vector3 direction = forward + turn;
        return direction.normalized;
    }
}
