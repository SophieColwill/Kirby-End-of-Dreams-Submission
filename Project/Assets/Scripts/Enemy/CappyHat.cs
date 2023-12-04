using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CappyHat : EnemyBehavior
{
    [SerializeField] float ThrowSpeed;

    public override void EnemyStart()
    {
        controller.velocity = new Vector2(0, ThrowSpeed);
    }

    public override void OnEnemyCollide(Collision2D collision)
    {
        Cappy target = collision.gameObject.GetComponent<Cappy>();
        if (target != null)
        {
            target.RegainHat();
            Destroy(gameObject);
        }
    }
}
