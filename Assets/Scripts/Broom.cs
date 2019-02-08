using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Broom : MonoBehaviour
{
    Rigidbody rb;
    public Transform snitch;

    float maxVelocity = 20;
    float maxAccel = 20;

    public int team = 0;
    public Transform score;

    private static int[] points = { 0, 0 };

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            if (r.transform == this.transform) continue;
            r.material.color = (team == 0) ? Color.red : Color.green;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(snitch);
        rb.AddRelativeForce(Vector3.forward * maxAccel);
        //Capping velocity
        //https://answers.unity.com/questions/683158/how-to-limit-speed-of-a-rigidbody.html
        if (rb.velocity.magnitude > maxVelocity)
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Snitch")
        {
            score.GetChild(team).GetChild(1).GetComponent<UnityEngine.UI.Text>().text = (++points[team]).ToString();
        }
        if (collision.gameObject.tag == "Player" && collision.gameObject.GetComponent<Broom>().team != team)
        {
            if (Random.Range(0f, 1f) < .5)
            {
                GameObject other = Instantiate(collision.gameObject, collision.transform.parent);
                Vector3 spawn = new Vector3(((other.GetComponent<Broom>().team == 0) ? -40 : 40), 10, 0);
                other.transform.position = spawn;
                Destroy(collision.gameObject);
            }
        }
    }
}
