using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KirbyBeam : MonoBehaviour
{
    [SerializeField] float AttackDamage;
    List<EnemyBehavior> behaviors = new List<EnemyBehavior>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyBehavior target = collision.GetComponent<EnemyBehavior>();
        Debug.Log(collision.gameObject.name);

        if (target != null)
        {

            bool hasTargetBeenHitBefore = false;

            foreach (EnemyBehavior behavior in behaviors)
            {
                if (behavior == target)
                {
                    hasTargetBeenHitBefore = true;
                    break;
                }
            }

            if (!hasTargetBeenHitBefore)
            {
                target.TakeDamage(AttackDamage);

                Debug.Log("Kirby has hit " + target.gameObject.name + " with a Beam Attack");
                behaviors.Add(target);
            }
        }
    }
}
