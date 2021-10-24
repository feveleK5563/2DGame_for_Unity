using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBase : MonoBehaviour
{
    void Start()
    {
        rigid_body_ = GetComponent<Rigidbody2D>();
        animator_ = GetComponent<Animator>();
        velocity_ = new Vector2();

        Initialize();
    }

    void Update()
    {
        UpdateState();
        UpdateAction();

        transform.Translate(velocity_);
    }

    //-----------------------------------------------------

    // 初期化
    internal abstract void Initialize();

    // ステータス更新
    internal abstract void UpdateState();

    // 動作更新
    internal abstract void UpdateAction();

    //-----------------------------------------------------

    // 動力リセット
    internal void ResetVelocity()
    {
        velocity_.x = 0.0f;
        velocity_.y = 0.0f;
    }

    // 動力加算
    internal void AddVelocity(float x, float y)
    {
        velocity_ += new Vector2(x, y);
    }

    // 動力乗算
    internal void MultiVelocity(float x, float y)
    {
        velocity_ *= new Vector2(x, y);
    }

    // 動力取得
    public Vector2 GetVelocity()
    {
        return velocity_;
    }

    // 向き設定
    // False->Left, True->Right
    internal void SetDirection(bool l_or_r)
    {
        direction_LR_ = l_or_r;
    }

    // 向き取得
    // False->Left, True->Right
    public bool GetDirection()
    {
        return direction_LR_;
    }

    //-----------------------------------------------------

    internal Rigidbody2D rigid_body_ = null;
    internal Animator animator_ = null;

    private Vector2 velocity_;
    private bool direction_LR_ = true;
}
