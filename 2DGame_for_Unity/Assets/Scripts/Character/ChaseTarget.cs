using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseTarget : CharacterBase
{
    public CharacterBase TargetChara;
    public Vector2 MoveRate;
    public Vector2 MovePosOffset;

    public bool IsLoopScroll = false;
    public float LoopLine;

    // 初期化
    internal override void Initialize()
    {
    }

    // ステータス更新
    internal override void UpdateState()
    {
    }

    // 動作更新
    internal override void UpdateAction()
    {
        Vector2 velocity = new Vector2();
        if (IsLoopScroll)
        {
            velocity.x = TargetChara.GetVelocity().x;
            if (LoopLine > 0.0f)
            {
                float pos_x = transform.position.x;
                float dist = TargetChara.transform.position.x - transform.position.x;
                float current_quotient = dist / LoopLine;
                if (current_quotient > 1.0f)
                {
                    pos_x += LoopLine;
                }
                else if (current_quotient < 0.0f)
                {
                    pos_x -= LoopLine;
                }
                transform.position = new Vector2(pos_x, transform.position.y);
            }
        }
        else
        {
            velocity.x = TargetChara.transform.position.x - transform.position.x;
            if (TargetChara.GetDirection())
            {
                velocity.x += MovePosOffset.x;
            }
            else
            {
                velocity.x -= MovePosOffset.x;
            }
        }

        velocity.y = TargetChara.transform.position.y - transform.position.y;
        velocity.y += MovePosOffset.y;
        velocity *= MoveRate;

        ResetVelocity();
        AddVelocity(velocity.x, velocity.y);
    }    
}
