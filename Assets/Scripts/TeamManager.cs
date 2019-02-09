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
    public int TeamSize;
    private int Points;

    //Required GameObjects
    public Transform score;
    public Transform snitch;
    public GameObject Broom;

    void Start()
    {
        for (int i = 0; i < TeamSize; i++)
        {
            GameObject player = Instantiate(Broom, this.transform);
            player.name = $"{Team}-Player {i}";
        }
    }

    public void Score()
    {
        score.GetChild(Team).GetChild(1).GetComponent<UnityEngine.UI.Text>().text = (++Points).ToString();
    }
}
