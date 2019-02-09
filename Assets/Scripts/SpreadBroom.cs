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
        rb = GetComponent<Rigidbody>();
        spreadTeam = transform.parent.GetComponent<SpreadTeam>();
        Team = spreadTeam;
        teamNum = spreadTeam.Team;
        score = spreadTeam.score;
        snitch = spreadTeam.snitch;
        maxVelocity = spreadTeam.MaxVelocity;
        maxAccel = spreadTeam.MaxAcceleration;
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            if (r.transform == this.transform) r.material.color = Color.clear;
            else r.material.color = (Team.Team == 0) ? Color.red : Color.green;
        }
        falling = false;
        transform.position = new Vector3(((Team.Team == 0) ? -40 : 40), 10, 0);

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
            int otherTeam = -1;
            try
            {
                otherTeam = other.gameObject.GetComponent<SpreadBroom>().GetTeam();
            }
            catch (NullReferenceException)
            {
                otherTeam = other.gameObject.GetComponent<Broom>().GetTeam();
            }

            if (otherTeam == teamNum)
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

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }

    protected override void Update()
    {
        if (falling) Fall();
        else if (snitchSensed) Chase();
        else if (enemySensed) Attack();
        else if (teamSensed) Spread();
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
