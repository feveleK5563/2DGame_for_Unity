using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTest : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        // Button
        // Positive Button : ~
        // Gravity      : 1000
        // Dead         : 0.001
        // Sensitivity  : 1000
        // Type         : Key or Mouse Button
        if (Input.GetButton("Button_A"))
        {
            // joystick button 0
            Debug.Log("Button_A");
        }
        else if (Input.GetButton("Button_B"))
        {
            // joystick button 1
            Debug.Log("Button_B");
        }
        else if (Input.GetButton("Button_X"))
        {
            // joystick button 2
            Debug.Log("Button_X");
        }
        else if (Input.GetButton("Button_Y"))
        {
            // joystick button 3
            Debug.Log("Button_Y");
        }
        else if (Input.GetButton("Button_LB"))
        {
            // joystick button 4
            Debug.Log("Button_LB");
        }
        else if (Input.GetButton("Button_RB"))
        {
            // joystick button 5
            Debug.Log("Button_RB");
        }
        else if (Input.GetButton("Button_View"))
        {
            // joystick button 6
            Debug.Log("Button_View");
        }
        else if (Input.GetButton("Button_Menu"))
        {
            // joystick button 7
            Debug.Log("Button_Menu");
        }
        else if (Input.GetButton("Button_LStick"))
        {
            // joystick button 8
            Debug.Log("Button_LStick");
        }
        else if (Input.GetButton("Button_RStick"))
        {
            // joystick button 9
            Debug.Log("Button_RStick");
        }

        // Axis
        // Gravity      : 0
        // Dead         : 0.2
        // Sensitivity  : 1
        // Type         : Joystick Axis
        {
            // Axis(x) : X axis
            // Axis(y) : Y axis
            float lsx = Input.GetAxis("Axis_LStick_X");
            float lsy = Input.GetAxis("Axis_LStick_Y");
            if ((lsx != 0) || (lsy != 0))
            {
                Debug.Log("L stick:" + lsx + "," + lsy);
            }
        }
        {
            // Axis(x) : 4th axis (Joysticks)
            // Axis(y) : 5th axis (Joysticks)
            float rsx = Input.GetAxis("Axis_RStick_X");
            float rsy = Input.GetAxis("Axis_RStick_Y");
            if ((rsx != 0) || (rsy != 0))
            {
                Debug.Log("R stick:" + rsx + "," + rsy);
            }
        }
        {
            // Axis(x) : 6th axis (Joysticks)
            // Axis(y) : 7th axis (Joysticks)
            float dpx = Input.GetAxis("Axis_DPad_X");
            float dpy = Input.GetAxis("Axis_DPad_Y");
            if ((dpx != 0) || (dpy != 0))
            {
                Debug.Log("D Pad:" + dpx + "," + dpy);
            }
        }
        {
            // Axis : 3rd axis (Joysticks and Scrollwheel)
            // Gravity      : 3
            // Dead         : 0.1
            // Sensitivity  : 3
            float tri = Input.GetAxis("Axis_LRTrigger");
            if (tri > 0)
            {
                Debug.Log("L trigger:" + tri);
            }
            else if (tri < 0)
            {
                Debug.Log("R trigger:" + tri);
            }
        }
    }
}
