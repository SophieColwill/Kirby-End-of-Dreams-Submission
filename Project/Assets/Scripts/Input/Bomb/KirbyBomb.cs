using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KirbyBomb : MonoBehaviour
{
    [SerializeField] float ExplosionRadius = 1;
    [SerializeField] float TimeTillExplode = 1;
    [SerializeField] float ExplosionDamage = 50;
    [SerializeField] GameObject ExplosionEffect;

    float Timer = 1;

    // Start is called before the first frame update
    void Start()
    {
        Timer = TimeTillExplode;
    }

    // Update is called once per frame
    void Update()
    {
        Timer -= Time.deltaTime;

        if (Timer <= 0)
        {
            Explode();
        }
    }

    void Explode()
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, ExplosionRadius);

        foreach (Collider2D target in targets)
        {
            EnemyBehavior havior = target.GetComponent<EnemyBehavior>();
            if (havior != null)
            {
                havior.TakeDamage(ExplosionDamage);
            }
        }

        Destroy(Instantiate(ExplosionEffect, transform.position, ExplosionEffect.transform.rotation), 0.25f);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyBehavior target = collision.gameObject.GetComponent<EnemyBehavior>();

        if (target != null)
        {
            Explode();
        }
    }
}
