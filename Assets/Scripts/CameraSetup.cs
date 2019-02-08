using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetup : MonoBehaviour
{
    public Transform snitch;
    private bool action;
    private Vector3 InitialPosition;
    private Vector3 InitialAngle;

    // Start is called before the first frame update
    void Start()
    {
        action = false;
        InitialAngle = transform.eulerAngles;
        InitialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(snitch);
        if (Input.GetKeyDown(KeyCode.Space)) SwapCam();
    }

    private void SwapCam()
    {
        if (action)
        {
            transform.parent = null;
            transform.eulerAngles = InitialAngle;
            transform.position = InitialPosition;
            action = false;
        }
        else
        {
            transform.parent = snitch;
            transform.localEulerAngles = new Vector3(0, 180, 0);
            transform.localPosition = Vector3.zero;
            action = true;
        }
    }
}
