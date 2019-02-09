using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class Broom : MonoBehaviour
{

    protected float maxVelocity = 20;
    protected float maxAccel = 20;
    protected int teamNum = -1;
    protected bool falling;

    protected Rigidbody rb;
    protected TeamManager Team;
    protected Transform snitch;
    protected Transform score;

    // Start is called before the first frame update
    virtual protected void Start()
    {
        rb = GetComponent<Rigidbody>();
        Team = transform.parent.GetComponent<TeamManager>();
        teamNum = Team.Team;
        score = Team.score;
        snitch = Team.snitch;
        maxVelocity = Team.MaxVelocity;
        maxAccel = Team.MaxAcceleration;
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            if (r.transform == this.transform) r.material.color = Color.clear;
            else r.material.color = (teamNum == 0) ? Color.red : Color.green;
        }
        falling = false;
        transform.position = new Vector3(((teamNum == 0) ? -40 : 40), 10, 0);
    }

    // Update is called once per frame
    virtual protected void Update()
    {
        if (!falling) Chase();
        else Fall();
    }

    virtual protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Snitch")
        {
            try
            {
                Team.Score();
            }
            catch (NullReferenceException)
            {
                /* Another bug where spread brooms are calling this method even though it's overridden.*/
            }
        }
        if (collision.gameObject.tag == "Player")
        {
            Broom other = collision.gameObject.GetComponent<SpreadBroom>();
            if (other == null)
            {
                other = collision.gameObject.GetComponent<Broom>();
            }
            try
            {
                if (other.teamNum != teamNum)
                {
                    if (falling || Random.Range(0f, 1f) < Team.TackleProbability)
                    {
                        other.Hit();
                    }
                }
            }
            catch (NullReferenceException)
            {
                /* Wierd bug where collision triggers on new broom before start, so no team is set yet. Ignoring.*/
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

    public virtual void Chase()
    {
        transform.LookAt(snitch);
        rb.AddRelativeForce(Vector3.forward * maxAccel);
        //Capping velocity
        //https://answers.unity.com/questions/683158/how-to-limit-speed-of-a-rigidbody.html
        if (rb.velocity.magnitude > maxVelocity)
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
    }

    protected void Fall()
    {
        rb.AddForce(Vector3.down * maxAccel);
        //not capping velocity because it's funnier.
    }

    public void Hit()
    {
        falling = true;
        GetComponent<Renderer>().material.color = new Color(0, 0, 1, .2f);
    }

    public int GetTeam() { return teamNum;  }
}
