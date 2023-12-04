using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cappy : EnemyBehavior
{
    [SerializeField] CappyHat HatOBJ;
    [SerializeField] Vector2 TimeBetweenAttacks;
    [SerializeField] Transform CappyHatSpawnPoint;

    bool HasHat = true;
    [SerializeField] float timeTillThrowHat;
    [SerializeField] Animator CappyAnimator;


    public override void EnemyStart()
    {
        timeTillThrowHat = Random.Range(TimeBetweenAttacks.x, TimeBetweenAttacks.y);
    }

    public override void EnemyUpdate()
    {
        if (timeTillThrowHat > 0)
        {
            timeTillThrowHat -= Time.deltaTime;
        }
        else
        {
            if (HasHat)
            {
                Instantiate(HatOBJ, CappyHatSpawnPoint.position, HatOBJ.transform.rotation);
                timeTillThrowHat = Random.Range(TimeBetweenAttacks.x, TimeBetweenAttacks.y);
                HasHat = false;
            }
        }
    }

    public void RegainHat()
    {
        HasHat = true;
    }

    public override void UpdateAnimations()
    {
        CappyAnimator.SetBool("hasHat", HasHat);
    }
}
