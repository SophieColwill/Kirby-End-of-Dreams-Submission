using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitOutStar : MonoBehaviour
{
    [SerializeField] float Damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player")
        {
            EnemyBehavior target = collision.gameObject.GetComponent<EnemyBehavior>();

            if (target != null)
            {
                target.TakeDamage(Damage);
            }

            Debug.Log(collision.gameObject.name);
            Destroy(gameObject);
        }
    }
}
