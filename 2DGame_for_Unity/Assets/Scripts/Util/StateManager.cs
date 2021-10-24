using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ステータス変更関数
public delegate bool StateCondition();

// ステータスを設定したり取得したりするやつ
class StateManager<T>
{
    private T state_;                   // ステータス
    private float state_time_ = 0.0f;   // ステータス継続時間

    //-----------------------------------------------------

    // 更新処理
    public void Update()
    {
        state_time_ += Time.deltaTime;
    }

    //-----------------------------------------------------

    // 普通にステータス設定
    public void ChangeState(T state)
    {
        if (EqualityComparer<T>.Default.Equals(state_, state))
        {
            return;
        }

        state_ = state;
        state_time_ = 0.0f;
        return;
    }

    // 渡したステータス変更関数を上から実行
    public bool ChangeStateCondition(
        StateCondition[] conditions, int index = 0)
    {
        if (conditions[index]())
        {
            return true;
        }
        if (conditions.Length == ++index)
        {
            return false;
        }
        return ChangeStateCondition(conditions, index);
    }

    // ステータス取得
    public T GetState()
    {
        return state_;
    }

    // ステータスの時間取得
    public float GetStateTime()
    {
        return state_time_;
    }
}