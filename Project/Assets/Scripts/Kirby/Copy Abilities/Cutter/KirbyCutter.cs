using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class KirbyCutter : MonoBehaviour
{
    [SerializeField] float Damage;

    bool isLookingRight;
    float BoomerangPower;
    Rigidbody2D controller;

    public void StartThrow(bool lookingRight, float Power, float ReturnPower)
    {
        controller = GetComponent<Rigidbody2D>();

        isLookingRight = lookingRight;
        BoomerangPower = ReturnPower;

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
        EnemyBehavior target = collision.gameObject.GetComponent<EnemyBehavior>();

        if (target != null) 
        {
            target.TakeDamage(Damage);
        }

        Destroy(gameObject);
    }
}
