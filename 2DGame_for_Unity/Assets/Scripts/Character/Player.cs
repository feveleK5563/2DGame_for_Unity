using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Player : CharacterBase
{
    // InputSystemを持ってるゲームオブジェクトを指定してくれ～～
    public GameObject gMainSystem;

    public float RunSpeed;  // 走る速度
    public float JumpForce; // ジャンプ力

    public enum State : int
    {
        Non         = 0,    // なし
        Idle        = 1,    // 待機
        Run         = 2,    // 走る
        Jump_Idle   = 3,    // ジャンプ_待機派生
        Jump_Run    = 4,    // ジャンプ_走る派生
        Fall_Idle   = 5,    // 落下_待機派生
        Fall_Run    = 6,    // 落下_走る派生
        Landing     = 7,    // 着地
        Squat       = 8,    // しゃがみ
        Squat_End   = 9,    // しゃがみから立ちへ
        Attack1     = 10,   // 攻撃_1段階
        Attack2     = 11,   // 攻撃_2段階
    }

    private InputSystem input = null;

    private StateManager<State> state_;
    private Vector2 input_axis_;

    private bool is_ground_ = false;

    //-----------------------------------------------------

    // 初期化
    internal override void Initialize()
    {
        input = gMainSystem.GetComponent<InputSystem>();

        SetDirection(true);
        animator_.SetBool("DirectionLR", true);

        state_ = new StateManager<State>();
        state_.ChangeState(State.Idle);
        animator_.SetInteger("State", (int)state_.GetState());
    }

    // ステータス更新
    internal override void UpdateState()
    {
        state_.Update();

        input_axis_.x = Input.GetAxis("Axis_DPad_X");
        input_axis_.y = Input.GetAxis("Axis_DPad_Y");

        State state = state_.GetState();
        float state_time = state_.GetStateTime();

        switch (state)
        {
            // 待機
            case State.Idle:
                state_.ChangeStateCondition(
                    new StateCondition[] {
                        ChangeState_Attack,
                        ChangeState_Jump,
                        ChangeState_Squat,
                        ChangeState_Run,
                        ChangeState_Fall,
                        ChangeState_Idle,
                    });
                break;

            // 走る
            case State.Run:
                state_.ChangeStateCondition(
                    new StateCondition[] {
                        ChangeState_Attack,
                        ChangeState_Jump,
                        ChangeState_Idle,
                        ChangeState_Squat,
                        ChangeState_Fall,
                        ChangeState_Run,
                    });
                break;

            // ジャンプ
            case State.Jump_Idle:
            case State.Jump_Run:
                ChangeState_Landing();
                break;

            // 落下
            case State.Fall_Idle:
            case State.Fall_Run:
                ChangeState_Landing();
                break;

            // しゃがみ
            case State.Squat:
                ChangeState_Squat();
                break;

            // 攻撃
            case State.Attack1:
            case State.Attack2:
                if (state_time >= 0.2f)
                {
                    bool is_change = false;
                    is_change = state_.ChangeStateCondition(
                        new StateCondition[] {
                            ChangeState_Attack,
                            ChangeState_Jump,
                            ChangeState_Squat,
                    });
                    if (!is_change && state_time >= 0.3f)
                    {
                        state_.ChangeStateCondition(
                            new StateCondition[] {
                                ChangeState_Idle,
                                ChangeState_Run,
                        });
                    }
                }
                break;

            // 着地
            // しゃがみから立ちへ
            case State.Landing:
            case State.Squat_End:
                if (state_time >= 0.05f)
                {
                    ChangeState_Idle();
                }
                state_.ChangeStateCondition(
                    new StateCondition[] {
                            ChangeState_Jump,
                            ChangeState_Run,
                            ChangeState_Attack,
                });
                break;

            default:
                break;
        }

        animator_.SetBool("DirectionLR", GetDirection());
        animator_.SetInteger("State", (int)state);
    }

    // 動作更新
    internal override void UpdateAction()
    {
        switch (state_.GetState())
        {
            // 走る
            case State.Run:
                AddVelocity((input_axis_.x > 0.0f) ? RunSpeed :
                            (input_axis_.x < 0.0f) ? -RunSpeed :
                            0.0f, 0.0f);
                break;

            // ジャンプ
            case State.Jump_Idle:
            case State.Jump_Run:
                AddVelocity((input_axis_.x > 0.0f) ? RunSpeed :
                            (input_axis_.x < 0.0f) ? -RunSpeed :
                            0.0f, 0.0f);
                if (is_ground_)
                {
                    rigid_body_.AddForce(Vector2.up * JumpForce);
                    is_ground_ = false;
                }
                break;

            // 落下
            case State.Fall_Idle:
            case State.Fall_Run:
                AddVelocity((input_axis_.x > 0.0f) ? RunSpeed :
                            (input_axis_.x < 0.0f) ? -RunSpeed :
                            0.0f, 0.0f);
                break;

            // 着地
            case State.Landing:
                AddVelocity((input_axis_.x > 0.0f) ? RunSpeed :
                            (input_axis_.x < 0.0f) ? -RunSpeed :
                            0.0f, 0.0f);
                break;

            // しゃがみ
            case State.Squat:
                MultiVelocity(0.9f, 1.0f);
                break;

            // 攻撃
            case State.Attack1:
            case State.Attack2:
                MultiVelocity(0.9f, 1.0f);
                break;

            default:
                break;
        }
    }

    //-----------------------------------------------------

    // 向きの更新
    void UpdateDirection()
    {
        if (input_axis_.x > 0.0f)
        {
            SetDirection(true);
        }
        else if (input_axis_.x < 0.0f)
        {
            SetDirection(false);
        }
    }

    //-----------------------------------------------------

    // 待機遷移
    bool ChangeState_Idle()
    {
        if (input_axis_.x == 0.0f)
        {
            ResetVelocity();
            state_.ChangeState(State.Idle);
            return true;
        }
        return false;
    }

    // 走る遷移
    bool ChangeState_Run()
    {
        ResetVelocity();
        if (input_axis_.x != 0.0f)
        {
            UpdateDirection();
            state_.ChangeState(State.Run);
            return true;
        }
        return false;
    }

    // ジャンプ遷移
    bool ChangeState_Jump()
    {
        ResetVelocity();
        if (is_ground_ &&
            input.IsDown(InputType.Button_A) ||
            input.IsPushRecord(InputType.Button_A))
        {
            UpdateDirection();
            state_.ChangeState(
                input_axis_.x == 0.0f
                ? State.Jump_Idle
                : State.Jump_Run);
            return true;
        }
        return false;
    }

    // 落下遷移
    bool ChangeState_Fall()
    {
        ResetVelocity();
        if (!is_ground_)
        {
            state_.ChangeState(
                input_axis_.x == 0.0f
                ? State.Fall_Idle
                : State.Fall_Run);
            return true;
        }
        return false;
    }

    // 着地遷移
    bool ChangeState_Landing()
    {
        ResetVelocity();
        if (is_ground_)
        {
            state_.ChangeState(State.Landing);
            return true;
        }
        return false;
    }

    // しゃがみ+しゃがみ戻り遷移
    bool ChangeState_Squat()
    {
        if (is_ground_ &&
            input_axis_.y < -0.5f)
        {
            // しゃがみへ遷移
            state_.ChangeState(State.Squat);
            return true;
        }
        else if (state_.GetState() == State.Squat)
        {
            // しゃがみから立ち遷移
            state_.ChangeState(State.Squat_End);
        }
        return false;
    }

    // 攻撃遷移
    bool ChangeState_Attack()
    {
        if (input.IsDown(InputType.Button_X) ||
            input.IsPushRecord(InputType.Button_X))
        {
            UpdateDirection();
            state_.ChangeState(
                state_.GetState() != State.Attack1
                ? State.Attack1
                : State.Attack2);
            return true;
        }
        return false;
    }

    //-----------------------------------------------------

    // 常時接触判定
    void OnCollisionStay2D(Collision2D other)
    {
        if (rigid_body_.velocity.y <= 0 &&
            other.gameObject.tag == "Ground")
        {
            // 着地判定
            is_ground_ = true;
        }
    }

    // 非接触判定
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            // 地面から離れる
            is_ground_ = false;
        }
    }
}
