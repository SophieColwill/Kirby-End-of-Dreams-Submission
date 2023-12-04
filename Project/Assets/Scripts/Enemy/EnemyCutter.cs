using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCutter : MonoBehaviour
{
    bool isLookingRight;
    float BoomerangPower;
    Rigidbody2D controller;
    float Damage;

    public void StartThrow(bool lookingRight, float Power, float ReturnPower, float NewDamage)
    {
        controller = GetComponent<Rigidbody2D>();

        isLookingRight = lookingRight;
        BoomerangPower = ReturnPower;
        Damage = NewDamage;

        if (isLookingRight)
        {
            controller.velocity = new Vector2(Power, 0);
        }
        else
        {
            controller.velocity = new Vector2(-Power, 0);
        }
    }

    private void Update()
    {
        if (isLookingRight)
        {
            controller.velocity -= new Vector2(BoomerangPower * Time.deltaTime, 0);
        }
        else
        {
            controller.velocity += new Vector2(BoomerangPower * Time.deltaTime, 0);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        KirbyHealth target = collision.gameObject.GetComponent<KirbyHealth>();

        if (target != null)
        {
            target.SetHealth -= Damage;
        }

        Destroy(gameObject);
    }
}
