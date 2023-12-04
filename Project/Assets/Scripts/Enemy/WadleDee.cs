using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class WadleDee : EnemyBehavior
{
    [SerializeField] LayerMask groundMask;
    [SerializeField] Vector2 CheckGroundEndPosition;
    [SerializeField] float CheckGroundEndLength;
    [SerializeField] bool TurnAroundAtGroundEnd;
    [SerializeField] float WadleDeeSpeed;

    [Header("Wall Check")]
    [SerializeField] float CheckWallEnd;
    [SerializeField] float CheckWallDistance;

    Animator animator;
    SpriteRenderer renderer;

    [Header("Other")]
    [SerializeField] bool IsFacingRight;
    public override void EnemyStart()
    {
        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
    }

    public override void UpdateAnimations()
    {
        renderer.flipX = !IsFacingRight;
        animator.SetFloat("speed", WadleDeeSpeed);
    }

    public override void EnemyUpdate()
    {
        if (IsFacingRight)
        {
            controller.velocity = new Vector2(WadleDeeSpeed, controller.velocity.y);
        }
        else
        {
            controller.velocity = new Vector2(-WadleDeeSpeed, controller.velocity.y);
        }

        if (TurnAroundAtGroundEnd)
        {
            if (!Physics2D.Raycast(GetCheckGroundEndPosition(true), Vector2.down, CheckGroundEndLength, groundMask))
            {
                IsFacingRight = true;
            }
            if (!Physics2D.Raycast(GetCheckGroundEndPosition(false), Vector2.down, CheckGroundEndLength, groundMask))
            {
                IsFacingRight = false;
            }
        }

        #region Turn Around At Wall

        if (Physics2D.Raycast(GetCheckWallPosition(true), Vector2.left, CheckWallDistance, groundMask))
        {
            IsFacingRight = true;
        }
        if (Physics2D.Raycast(GetCheckWallPosition(false), Vector2.right, CheckWallDistance, groundMask))
        {
            IsFacingRight = false;
        }

        #endregion
    }

    Vector2 GetCheckGroundEndPosition(bool Negate)
    {
        Vector2 Output = transform.position;
        Output += new Vector2(0, CheckGroundEndPosition.y);

        if (Negate)
        {
            Output -= new Vector2(CheckGroundEndPosition.x, 0);
        }
        else
        {
            Output += new Vector2(CheckGroundEndPosition.x, 0);
        }
        return Output;
    }

    Vector2 GetCheckWallPosition(bool Negate)
    {
        Vector2 Output = transform.position;

        if (Negate)
        {
            Output -= new Vector2(CheckWallEnd, 0);
        }
        else
        {
            Output += new Vector2(CheckWallEnd, 0);
        }
        return Output;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(GetCheckGroundEndPosition(true), 0.05f);
        Gizmos.DrawSphere(GetCheckGroundEndPosition(false), 0.05f);

        Gizmos.DrawLine(GetCheckGroundEndPosition(true), GetCheckGroundEndPosition(true) + (Vector2.down * CheckGroundEndLength));
        Gizmos.DrawLine(GetCheckGroundEndPosition(false), GetCheckGroundEndPosition(false) + (Vector2.down * CheckGroundEndLength));


        Gizmos.DrawSphere(GetCheckWallPosition(true), 0.05f);
        Gizmos.DrawSphere(GetCheckWallPosition(false), 0.05f);

        Gizmos.DrawLine(GetCheckWallPosition(true), GetCheckWallPosition(true) + (Vector2.left * CheckWallDistance));
        Gizmos.DrawLine(GetCheckWallPosition(false), GetCheckWallPosition(false) + (Vector2.right * CheckWallDistance));
    }
}
