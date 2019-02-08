using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    //Team Settings
    public float MaxAcceleration;
    public float MaxVelocity;
    public float TackleProbability;
    public int Team;
    private int Points;

    //Required GameObjects
    public Transform score;
    public Transform snitch;

    public void Score()
    {
        score.GetChild(Team).GetChild(1).GetComponent<UnityEngine.UI.Text>().text = (++Points).ToString();
    }
}
