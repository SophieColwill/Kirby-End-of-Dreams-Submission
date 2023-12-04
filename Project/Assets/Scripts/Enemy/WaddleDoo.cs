using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class WaddleDoo : EnemyBehavior
{
    [SerializeField] LayerMask groundMask;
    [SerializeField] Vector2 CheckGroundEndPosition;
    [SerializeField] float CheckGroundEndLength;
    [SerializeField] bool TurnAroundAtGroundEnd;
    [SerializeField] float WadleDooSpeed;

    [Header("Wall Check")]
    [SerializeField] float CheckWallEnd;
    [SerializeField] float CheckWallDistance;

    [Header("Attacking")]
    [SerializeField] EnemyBeam BeamAttackOBJ_Right;
    [SerializeField] EnemyBeam BeamAttackOBJ_Left;
    [SerializeField] Vector2 RandomAttackIntervals;
    [SerializeField] float BeamAttackDamage;
    [SerializeField] float BeamAttackTime;
    [SerializeField] Vector2 SpawnDistanceFromWaddleDoo = new Vector2(4.33f, 1.73f);

    [SerializeField]float TimeTillNextAttack;
    float DisableMovementValue;
    bool IsFacingRight;

    Animator animator;
    SpriteRenderer renderer;
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

    public override void EnemyStart()
    {
        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
        TimeTillNextAttack = Random.Range(RandomAttackIntervals.x, RandomAttackIntervals.y);
    }

    public override void UpdateAnimations()
    {
        renderer.flipX = !IsFacingRight;
        animator.SetFloat("speed", WadleDooSpeed);
        animator.SetBool("attack", TimeTillNextAttack <= 0);
    }

    public override void EnemyUpdate()
    {
        if (DisableMovementValue <= 0)
        {
            if (IsFacingRight)
            {
                controller.velocity = new Vector2(WadleDooSpeed, controller.velocity.y);
            }
            else
            {
                controller.velocity = new Vector2(-WadleDooSpeed, controller.velocity.y);
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
        else
        {
            DisableMovementValue -= Time.deltaTime;
            if (DisableMovementValue <= 0)
            {
                TimeTillNextAttack = Random.Range(RandomAttackIntervals.x, RandomAttackIntervals.y);
            }
        }

        if (TimeTillNextAttack > 0)
        {
            TimeTillNextAttack -= Time.deltaTime;

            if (TimeTillNextAttack <= 0)
            {
                Attack();
                DisableMovementValue = BeamAttackTime;
            }
        }
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

    void Attack()
    {
        if (IsFacingRight)
        {
            Vector3 spawnPosition = new Vector3(transform.position.x + (-GetLookValue() * SpawnDistanceFromWaddleDoo.y), transform.position.y, transform.position.z);
            EnemyBeam BeamOBJ = Instantiate<EnemyBeam>(BeamAttackOBJ_Right, spawnPosition, BeamAttackOBJ_Right.transform.rotation);

            BeamOBJ.OnStart(BeamAttackDamage, BeamAttackTime);
            Destroy(BeamOBJ.gameObject, BeamAttackTime);
        }
        else
        {
            Vector3 spawnPosition = new Vector3(transform.position.x + (-GetLookValue() * SpawnDistanceFromWaddleDoo.x), transform.position.y, transform.position.z);
            EnemyBeam BeamOBJ = Instantiate<EnemyBeam>(BeamAttackOBJ_Left, spawnPosition, BeamAttackOBJ_Left.transform.rotation);

            BeamOBJ.OnStart(BeamAttackDamage, BeamAttackTime);
            Destroy(BeamOBJ.gameObject, BeamAttackTime);
        }
    }
}
