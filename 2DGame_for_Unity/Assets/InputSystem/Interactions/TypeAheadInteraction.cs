using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif

// 先行入力するやつ
#if UNITY_EDITOR
[InitializeOnLoad]
#endif
public class TypeAheadInteraction : IInputInteraction
{
#if UNITY_EDITOR
    static TypeAheadInteraction()
    {
        Initialize();
    }
#endif

    [RuntimeInitializeOnLoadMethod]
    static void Initialize()
    {
        InputSystem.RegisterInteraction<TypeAheadInteraction>();
    }

    [Tooltip("先行入力時間")]
    public float PrecedeTime = 1f;
    [Tooltip("ボタンを押したと判断するしきい値")]
    public float PressPoint = 0.5f;

    // 時間計測
    private float count_time_ = 0f;

    public void Process(ref InputInteractionContext context)
    {
        // PrecedeTime 秒だけ "入力した" 状態を維持する
        if (context.timerHasExpired)
        {
            count_time_ -= Time.deltaTime;
            Debug.Log("TIME : " + count_time_);
            if (count_time_ > 0f)
            {
                context.Started();
                context.PerformedAndStayPerformed();
                context.SetTimeout(Time.deltaTime);
            }
            else
            {
                count_time_ = 0f;
                context.Canceled();
                return;
            }
        }

        switch (context.phase)
        {
            case InputActionPhase.Waiting:
                if (context.ControlIsActuated(PressPoint))
                {
                    // 入力を感知 + 計測開始
                    if (count_time_ == 0f)
                    {
                        context.Started();
                        context.PerformedAndStayPerformed();
                    }
                    context.SetTimeout(Time.deltaTime);
                    count_time_ = PrecedeTime;
                }
                break;

            case InputActionPhase.Performed:
                break;
        }
    }

    public void Reset()
    {
    }
}
