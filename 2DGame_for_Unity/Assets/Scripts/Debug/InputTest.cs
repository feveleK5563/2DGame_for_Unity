using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputTest : MonoBehaviour
{
    void Start()
    {
    }

    void Update()
    {
        if (Gamepad.current == null)
        {
            // ゲームパッドが接続されてない
            return;
        }

        Gamepad pad = Gamepad.current;

        if (pad.buttonSouth.wasPressedThisFrame)
        {
            // Aボタン
            Debug.Log("Button_South(A or ×)");
        }
        else if (pad.buttonEast.wasPressedThisFrame)
        {
            // Bボタン
            Debug.Log("Button_East(B or ○)");
        }
        else if (pad.buttonWest.wasPressedThisFrame)
        {
            // Xボタン
            Debug.Log("Button_West(X or □)");
        }
        else if (pad.buttonNorth.wasPressedThisFrame)
        {
            // Yボタン
            Debug.Log("Button_North(Y or △)");
        }
        else if (pad.leftShoulder.wasPressedThisFrame)
        {
            // Lボタン
            Debug.Log("LeftShoulder(LB)");
        }
        else if (pad.rightShoulder.wasPressedThisFrame)
        {
            // Rボタン
            Debug.Log("RightShoulder(RB)");
        }
        else if (pad.selectButton.wasPressedThisFrame)
        {
            // Viewボタン
            Debug.Log("SelectButton(View)");
        }
        else if (pad.startButton.wasPressedThisFrame)
        {
            // Menuボタン
            Debug.Log("StartButton(Menu)");
        }
        else if (pad.leftStickButton.wasPressedThisFrame)
        {
            // Lスティックボタン（押し込み）
            Debug.Log("LeftStickButton");
        }
        else if (pad.rightStickButton.wasPressedThisFrame)
        {
            // Rスティックボタン（押し込み）
            Debug.Log("RightStickButton");
        }

        {
            Vector2 lsv = pad.leftStick.ReadValue();
            if (lsv.magnitude > 0.0f)
            {
                // Lスティック
                Debug.Log("L stick:" + lsv.x + "," + lsv.y);
            }
        }
        {
            Vector2 rsv = pad.rightStick.ReadValue();
            if (rsv.magnitude > 0.0f)
            {
                // Rスティック
                Debug.Log("R stick:" + rsv.x + "," + rsv.y);
            }
        }
        {
            // Dパッド（十字ボタン）
            float dpx = pad.dpad.x.ReadValue();
            float dpy = pad.dpad.y.ReadValue();
            if ((dpx != 0) || (dpy != 0))
            {
                Debug.Log("D-Pad:" + dpx + "," + dpy);
            }
        }
        {
            // Lトリガー
            float ltrv = pad.leftTrigger.ReadValue();
            if (ltrv > 0)
            {
                Debug.Log("LeftTrigger:" + ltrv);
            }
        }
        {
            // Rトリガー
            float rtrv = pad.rightTrigger.ReadValue();
            if (rtrv > 0)
            {
                Debug.Log("RightTrigger:" + rtrv);
            }
        }
    }
}
