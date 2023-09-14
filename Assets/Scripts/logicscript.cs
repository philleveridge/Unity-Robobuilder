using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class logicscript : MonoBehaviour
{
    private float timer = 0;

    /*                              0    1    2    3   4    5    6   7   8    9    10  11  12  13   14   15  */
    private int[] HunoBasicPose = { 125, 179, 199, 88, 108, 126, 72, 49, 163, 141, 51, 47, 49, 199, 205, 205 };

    [System.Serializable]
    public struct Joint
    {
        public string inputAxis;
        public GameObject robotPart;
    }
    public Joint[] joints;



    // Start is called before the first frame update
    void Start()
    {
        for (int i=0; i<joints.Length; i++)
        {
            GameObject servo = joints[i].robotPart;
            ServoScript x = servo.GetComponent<ServoScript>();
            Debug.Log("Connecting ... " + x.ServoID);
            x.setpos(HunoBasicPose[x.ServoID]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 2.0)
        {
            timer = 0.0f;
        }
        else
        {
            timer = timer + Time.deltaTime;
        }
    }

    // CONTROL

    public void StopAllJointRotations()
    {
        for (int i = 0; i < joints.Length; i++)
        {
            GameObject robotPart = joints[i].robotPart;
            UpdateRotationState(RotationDirection.None, robotPart);
        }
    }

    public void RotateJoint(int jointIndex, RotationDirection direction)
    {
        StopAllJointRotations();
        Joint joint = joints[jointIndex];
        UpdateRotationState(direction, joint.robotPart);
    }

    // HELPERS

    static void UpdateRotationState(RotationDirection direction, GameObject robotPart)
    {
        ServoScript servo = robotPart.GetComponent<ServoScript>();
        servo.rotationState = direction;
    }

}
