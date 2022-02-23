using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// ボタン長押しで連射扱いにするInteractionクラス
#if UNITY_EDITOR
[InitializeOnLoad]
#endif
public class LongPressedRapidfireInteraction : IInputInteraction
{
#if UNITY_EDITOR
    static LongPressedRapidfireInteraction()
    {
        Initialize();
    }
#endif

    [RuntimeInitializeOnLoadMethod]
    static void Initialize()
    {
        InputSystem.RegisterInteraction<LongPressedRapidfireInteraction>();
    }

    [Tooltip("連射と判断する間隔")]
    public float Duration = 0.2f;
    [Tooltip("ボタンを押したと判断するしきい値")]
    public float PressPoint = 0.5f;


    public void Process(ref InputInteractionContext context)
    {
        //連射判断タイミングの処理
        if (context.timerHasExpired)
        {
            context.Canceled();
            if (context.ControlIsActuated(PressPoint))
            {
                context.Started();
                context.Performed();
                context.SetTimeout(Duration);
            }
            return;
        }

        switch (context.phase)
        {
            case InputActionPhase.Waiting:
                //入力受付状態になった時
                if (context.ControlIsActuated(PressPoint))
                {
                    //実行状態にする
                    context.Started();
                    context.Performed();
                    //連射の振る舞いをするタイミング
                    context.SetTimeout(Duration);
                }
                break;

            case InputActionPhase.Performed:

                if (!context.ControlIsActuated(PressPoint))
                {
                    //ボタンが離れたらキャンセルにする。
                    context.Canceled();
                    return;
                }

                break;
        }
    }

    public void Reset()
    {

    }
}
