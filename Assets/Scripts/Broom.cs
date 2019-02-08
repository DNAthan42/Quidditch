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

    private bool falling;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            if (r.transform == this.transform) continue;
            r.material.color = (team == 0) ? Color.red : Color.green;
        }
        falling = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!falling) Chase();
        else Fall();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Snitch")
        {
            score.GetChild(team).GetChild(1).GetComponent<UnityEngine.UI.Text>().text = (++points[team]).ToString();
        }
        if (collision.gameObject.tag == "Player")
        {
            Broom other = collision.gameObject.GetComponent<Broom>();
            if (other.team != team)
            {
                if (falling || Random.Range(0f, 1f) < .5)
                {
                    other.Hit();
                }
            }
        }
        if (collision.gameObject.tag == "Ground" && falling)
        {
            falling = false;
            GameObject child = Instantiate(this.gameObject, this.transform.parent);
            child.transform.position = new Vector3(((child.GetComponent<Broom>().team == 0) ? -40 : 40), 10, 0);
            Destroy(this.gameObject);
        }
    }

    private void Chase()
    {
        transform.LookAt(snitch);
        rb.AddRelativeForce(Vector3.forward * maxAccel);
        //Capping velocity
        //https://answers.unity.com/questions/683158/how-to-limit-speed-of-a-rigidbody.html
        if (rb.velocity.magnitude > maxVelocity)
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
    }

    private void Fall()
    {
        rb.AddForce(Vector3.down * maxAccel);
        //not capping velocity because it's funnier.
    }

    public void Hit()
    {
        falling = true;
    }
}
