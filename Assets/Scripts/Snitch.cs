using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snitch : MonoBehaviour
{

    private float maxVelocity = 20;
    private float maxAccel = 40;
    public float delta = .01f;

    private Vector3 turn;
    

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddRelativeForce(RandomForwardDirection() * maxAccel);

        turn = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 1);
        turn = turn.normalized;
    }

    // Update is called once per frame
    void Update()
    {
        //look where you're going.
        transform.localRotation = Quaternion.LookRotation(rb.velocity);

        //nudge the turning direction both horizontally and vertically.
        turn.x = Nudge(turn.x);
        turn.y = Nudge(turn.y);
        rb.AddRelativeForce(turn.normalized * maxAccel);

        //Capping velocity
        //https://answers.unity.com/questions/683158/how-to-limit-speed-of-a-rigidbody.html
        if (rb.velocity.magnitude > maxVelocity)
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);

    }

    private Vector3 RandomForwardDirection()
    {
        Vector3 forward = Vector3.forward * Random.Range(0f, 1f);
        Vector3 turn = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.forward) * Vector3.right;
        Vector3 direction = forward + turn;
        return direction.normalized;
    }

    private float Nudge(float val)
    {
        if (val >= 0) val += (Random.Range(-.5f, 1f) > val) ? delta : -delta;
        else val += (Random.Range(-1f, .5f) < val) ? -delta: delta;
        val *= (Random.Range(0f, 1f) < .01f) ? -1 : 1;
        return val;
    }
}
