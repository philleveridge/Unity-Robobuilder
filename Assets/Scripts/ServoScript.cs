using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GridBrushBase;

public enum RotationDirection { None = 0, Positive = 1, Negative = -1 };

public class ServoScript : MonoBehaviour
{
    // Start is called before the first frame update
    public int ServoPosition; 
    public int ServoID;
    public ArticulationBody servo;

    private float ServoAngle;

    public RotationDirection rotationState = RotationDirection.None;
    public float speed = 10.0f;

    void Start()
    {
        Debug.Log("Servo "+ServoID+ " start");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if ( Input.GetKeyDown(KeyCode.Space))

        setpos(ServoPosition);

        float cpar = CurrentPrimaryAxisRotation();

        if (ServoAngle != cpar)
        {
            if (ServoAngle> cpar ) 
            {
                rotationState = RotationDirection.Positive;
            }
            else
            {
                rotationState = RotationDirection.Negative;
            }
        }
        else
        {
            rotationState = RotationDirection.None;
        }

        if (rotationState != RotationDirection.None)
        {
            float rotationChange = (float)rotationState * speed * Time.fixedDeltaTime;

            if (Math.Abs(cpar - ServoAngle) < rotationChange)
            {
                RotateTo(ServoAngle);
            }
            else 
            { 
                float rotationGoal = cpar + rotationChange;
                RotateTo(rotationGoal);
            }
        }
    }

    // MOVEMENT HELPERS

    float CurrentPrimaryAxisRotation()
    {
        float currentRotationRads = servo.jointPosition[0];
        float currentRotation = Mathf.Rad2Deg * currentRotationRads;
        return currentRotation;
    }

    void RotateTo(float primaryAxisRotation)
    {
        var drive = servo.xDrive;
        drive.target = primaryAxisRotation;
        servo.xDrive = drive;
    }

    public void setpos(int n)
    {
        const float UnitAngle = 269.0f / 255.0f;

        if (n < 0) n = 0;
        if (n > 255) n = 255;
        ServoPosition = n;
        float angle = (n - 128) * UnitAngle;
        ServoAngle = angle;
        Debug.Log("Servo pos  ("+n+")="+ServoAngle);
    }
}
