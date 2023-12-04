using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoppyBros : EnemyBehavior
{
    [SerializeField] private Vector2 RandomThrowTime;
    [SerializeField] private Rigidbody2D BombOBJ;
    [SerializeField] Vector2 ThrowPower;
    [Range(-1, 1)]
    [SerializeField] int ThrowDirection;

    Animator animator;
    float Timer;

    public override void EnemyStart()
    {
        animator = GetComponent<Animator>();
    }

    public override void EnemyUpdate()
    {
        if (Timer > 0)
        {
            Timer -= Time.deltaTime;
        }
        else
        {
            animator.SetTrigger("attack");
            Timer = Random.Range(RandomThrowTime.x, RandomThrowTime.y);
            Rigidbody2D BombRefrence = Instantiate<Rigidbody2D>(BombOBJ, transform.position + (ThrowDirection * Vector3.right), BombOBJ.transform.rotation);
            BombRefrence.AddForce(new Vector2(ThrowDirection * ThrowPower.x, ThrowPower.y), ForceMode2D.Impulse);
        }
    }
}
