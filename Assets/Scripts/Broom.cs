using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Broom : MonoBehaviour
{

    private float maxVelocity = 20;
    private float maxAccel = 20;
    private bool falling;

    private Rigidbody rb;
    private TeamManager Team;
    private Transform snitch;
    private Transform score;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Team = transform.parent.GetComponent<TeamManager>();
        score = Team.score;
        snitch = Team.snitch;
        maxVelocity = Team.MaxVelocity;
        maxAccel = Team.MaxAcceleration;
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            if (r.transform == this.transform) r.material.color = Color.clear;
            else r.material.color = (Team.Team == 0) ? Color.red : Color.green;
        }
        falling = false;
        transform.position = new Vector3(((Team.Team == 0) ? -40 : 40), 10, 0);
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
            Team.Score();
        }
        if (collision.gameObject.tag == "Player")
        {
            Broom other = collision.gameObject.GetComponent<Broom>();
            if (other.Team.Team != Team.Team)
            {
                if (falling || Random.Range(0f, 1f) < Team.TackleProbability)
                {
                    other.Hit();
                }
            }
        }
        if (collision.gameObject.tag == "Ground" && falling)
        {
            falling = false;
            GameObject child = Instantiate(this.gameObject, this.transform.parent);
            child.name = this.name;
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
        GetComponent<Renderer>().material.color = new Color(0, 0, 1, .2f);
    }
}
