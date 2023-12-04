using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedCopyAbility : EnemyBehavior
{
    [SerializeField] bool TurnAroundOnEdgeHit;
    [Header("Copy Star")]
    [SerializeField] float HorizontalSpeed;
    [SerializeField] float BounceHeight;
    [SerializeField] float DisapearTimer;

    [Header("Wall Check")]
    [SerializeField] LayerMask GroundMask;
    [SerializeField] float CheckWallEnd;
    [SerializeField] float CheckWallDistance;

    [Header("Ground Check")]
    [SerializeField] float CheckStartDistance;
    [SerializeField] float GroundCheckDistance;

    bool IsFacingRight;

    #region Getters
    float GetLookValue()
    {
        if (IsFacingRight)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }
    #endregion

    public override void TakeDamage(float damage)
    {
        Debug.Log("Dropped copy ability could not be hit");
    }

    public override void EnemyUpdate()
    {
        controller.velocity = new Vector2(GetLookValue() * HorizontalSpeed, controller.velocity.y);

        DisapearTimer -= Time.deltaTime;
        if (DisapearTimer <= 0)
        {
            Destroy(gameObject);
        }

        if (Physics2D.Raycast(GetGroundCheckStartPosition(), Vector2.down, CheckWallDistance, GroundMask))
        {
            controller.velocity = new Vector2 (controller.velocity.x, BounceHeight);
        }

        #region Turn Around At Wall

        if (Physics2D.Raycast(GetCheckWallPosition(true), Vector2.left, CheckWallDistance, GroundMask))
        {
            if (TurnAroundOnEdgeHit)
            {
                IsFacingRight = true;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        if (Physics2D.Raycast(GetCheckWallPosition(false), Vector2.right, CheckWallDistance, GroundMask))
        {
            if (TurnAroundOnEdgeHit)
            {
                IsFacingRight = false;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        #endregion
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
    
    Vector2 GetGroundCheckStartPosition()
    {
        return new Vector2(transform.position.x, transform.position.y - CheckStartDistance);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawSphere(GetCheckWallPosition(true), 0.05f);
        Gizmos.DrawSphere(GetCheckWallPosition(false), 0.05f);

        Gizmos.DrawLine(GetCheckWallPosition(true), GetCheckWallPosition(true) + (Vector2.left * CheckWallDistance));
        Gizmos.DrawLine(GetCheckWallPosition(false), GetCheckWallPosition(false) + (Vector2.right * CheckWallDistance));

        Gizmos.DrawSphere(GetGroundCheckStartPosition(), 0.05f);
        Gizmos.DrawLine(GetGroundCheckStartPosition(), GetGroundCheckStartPosition() + (Vector2.down * GroundCheckDistance));
    }
}
