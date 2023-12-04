using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SirKible : EnemyBehavior
{
    [SerializeField] private Vector2 RandomThrowTime;
    [SerializeField] private EnemyCutter CutterOBJ;
    [SerializeField] float ThrowPower;
    [SerializeField] float ReturnPower;
    [SerializeField] float CutterDamage;
    [SerializeField] bool isLookingRight;

    Animator animator;
    float Timer;

    public override void EnemyStart()
    {
        animator = GetComponent<Animator>();
        Timer = Random.Range(RandomThrowTime.x, RandomThrowTime.y);
    }

    public override void EnemyUpdate()
    {
        controller.velocity = Vector3.zero;

        if (Timer > 0)
        {
            Timer -= Time.deltaTime;
        }
        else
        {
            Timer = Random.Range(RandomThrowTime.x, RandomThrowTime.y);

            Vector3 StartThrowPosition = transform.position;

            if (isLookingRight)
            {
                StartThrowPosition += new Vector3(1f, 0, 0) * 2f;
            }
            else
            {
                StartThrowPosition -= new Vector3(1, 0, 0) * 2f;
            }

            EnemyCutter CutterRefrence = Instantiate<EnemyCutter>(CutterOBJ, StartThrowPosition, CutterOBJ.transform.rotation);

            CutterRefrence.StartThrow(isLookingRight, ThrowPower, ReturnPower, CutterDamage);
            animator.SetTrigger("Attack");
        }
    }

    public override void InhaleUpdate()
    {
        base.InhaleUpdate();
    }
}
