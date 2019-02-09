using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpreadBroom : Broom
{
    SpreadTeam spreadTeam;
    Transform sensor;

    bool snitchSensed;
    bool teamSensed;
    bool enemySensed;
    Transform enemy;
    Vector3 spreadDirection;

    protected override void Start()
    {
        base.Start();
        spreadTeam = transform.parent.GetComponent<SpreadTeam>();
        sensor = Instantiate(spreadTeam.Sensor, this.transform);
        sensor.localScale = new Vector3(spreadTeam.radius, spreadTeam.radius, spreadTeam.radius);
    }

    void FixedUpdate()
    {
        snitchSensed = false;
        teamSensed = false;
        enemySensed = false;
        enemy = null;
        spreadDirection = new Vector3(0, 0, 0);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Snitch")
        {
            snitchSensed = true;
        }
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponent<SpreadBroom>().Team.Team == Team.Team)
            {
                teamSensed = true;
                spreadDirection += ((other.transform.position - this.transform.position) / spreadTeam.radius) * -1;
            }
            else
            {
                enemySensed = true;
                enemy = other.transform;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Snitch")
        {
            Team.Score();
        }
        if (collision.gameObject.tag == "Ground" && falling)
        {
            falling = false;
            GameObject child = Instantiate(this.gameObject, this.transform.parent);
            child.name = this.name;
            Destroy(this.gameObject);
        }
        if (collision.gameObject.tag == "Player")
        {
            try
            {
                Broom other = collision.gameObject.GetComponent<Broom>();
                if (other.GetTeam().Team != Team.Team)
                {
                    if (falling || Random.Range(0f, 1f) < Team.TackleProbability)
                    {
                        other.Hit();
                    }
                }
            }
            catch (NullReferenceException)
            {
                SpreadBroom other = collision.gameObject.GetComponent<SpreadBroom>();
                if (other.Team.Team != Team.Team)
                {
                    if (falling || Random.Range(0f, 1f) < Team.TackleProbability)
                    {
                        other.Hit();
                    }
                }
            }
        }
    }

    protected override void Update()
    {
        if (falling) Fall();
        else if (snitchSensed) Chase();
        else if (teamSensed) Spread();
        else if (enemySensed) Attack();
        else Chase();
        //Capping velocity
        //https://answers.unity.com/questions/683158/how-to-limit-speed-of-a-rigidbody.html
        if (rb.velocity.magnitude > maxVelocity)
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
    }

    protected void Spread()
    {
        rb.AddForce(spreadDirection.normalized * maxAccel);
        transform.localRotation = Quaternion.LookRotation(spreadDirection.normalized);

    }

    protected void Attack()
    {
        transform.LookAt(enemy);
        rb.AddRelativeForce(Vector3.forward * maxAccel);
    }

}
