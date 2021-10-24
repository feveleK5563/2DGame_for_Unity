using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputType : int
{
    Button_A = 0,
    Button_B = 1,
    Button_X = 2,
    Button_Y = 3,
    Button_LB = 4,
    Button_RB = 5,
    Button_View = 6,
    Button_Menu = 7,
    Button_LStick = 8,
    Button_RStick = 9,
    ButtonNum,
}


// UnityのInputをなんか使いやすくしたい系ラッパー
public class InputSystem : MonoBehaviour
{
    public float InputRecordTime;   // 入力の保存時間

    private static readonly string[] ButtonType =
    {
        "Button_A",         // 0
        "Button_B",         // 1
        "Button_X",         // 2
        "Button_Y",         // 3
        "Button_LB",        // 4
        "Button_RB",        // 5
        "Button_View",      // 6
        "Button_Menu",      // 7
        "Button_LStick",    // 8
        "Button_RStick",    // 9
    };

    private struct ButtonData
    {
        public bool is_push;            // 入力状態
        public float state_time;        // 状態の継続時間
        public float input_record_time; // 入力の記録時間
    }
    private ButtonData[] data_;

    //-----------------------------------------------------

    void Start()
    {
        data_ = new ButtonData[(int)InputType.ButtonNum];
        ClearButtonData();
    }

    void Update()
    {
        // 入力状態を記録
        for (int i = 0; i < (int)InputType.ButtonNum; ++i)
        {
            // 入力状態継続の時間計測
            bool is_push = Input.GetButtonDown(ButtonType[i]);
            if (data_[i].is_push != is_push)
            {
                data_[i].state_time = 0.0f;
            }
            else
            {
                data_[i].state_time += Time.deltaTime;
            }
            data_[i].is_push = is_push;

            // 入力の記録保持判定
            if (is_push)
            {
                data_[i].input_record_time = InputRecordTime;
            }
            else if (data_[i].input_record_time > 0.0f)
            {
                data_[i].input_record_time -= Time.deltaTime;
                if (data_[i].input_record_time <= 0.0f)
                {
                    data_[i].input_record_time = 0.0f;
                }
            }
        }
    }

    //-----------------------------------------------------

    // ボタン入力情報をクリア
    void ClearButtonData()
    {
        for (int i = 0; i < (int)InputType.ButtonNum; ++i)
        {
            data_[i].is_push = false;
            data_[i].state_time = 0.0f;
            data_[i].input_record_time = 0.0f;
        }
    }

    //-----------------------------------------------------

    // 押した瞬間か
    public bool IsDown(InputType type)
    {
        return data_[(int)type].is_push &&
               data_[(int)type].state_time == 0.0f;
    }

    // 押しているか
    public bool IsPush(InputType type)
    {
        return data_[(int)type].is_push;
    }

    // 離した瞬間か
    public bool IsUp(InputType type)
    {
        return !data_[(int)type].is_push &&
               data_[(int)type].state_time == 0.0f;
    }

    // 離しているか
    public bool IsRelease(InputType type)
    {
        return !data_[(int)type].is_push;
    }

    // 押したか（先行入力取得）
    public bool IsPushRecord(InputType type)
    {
        return data_[(int)type].input_record_time > 0.0f;
    }

    // 状態の継続時間取得
    public float GetStateTime(InputType type)
    {
        return data_[(int)type].state_time;
    }
}
