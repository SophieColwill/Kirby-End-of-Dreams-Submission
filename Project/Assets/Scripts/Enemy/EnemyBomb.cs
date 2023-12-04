using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomb : MonoBehaviour
{
    [SerializeField] float ExplosionRadius = 1;
    [SerializeField] float TimeTillExplode = 1;
    [SerializeField] float ExplosionDamage = 50;
    [SerializeField] GameObject ExplosionEffect;
    [SerializeField] bool ShowExplosionRadius;
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
            Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, ExplosionRadius);

            foreach (Collider2D target in targets)
            {
                KirbyHealth havior = target.GetComponent<KirbyHealth>();
                if (havior != null)
                {
                    havior.SetHealth -= ExplosionDamage;
                }
            }

            Explode();
        }
    }

    void Explode()
    {
        Collider2D[] targetsInRange = Physics2D.OverlapCircleAll(transform.position, ExplosionRadius);

        foreach (Collider2D item in targetsInRange)
        {
            KirbyHealth target = item.GetComponent<KirbyHealth>();
            if (target != null)
            {
                target.SetHealth -= ExplosionDamage;
            }
        }

        Destroy(Instantiate(ExplosionEffect, transform.position, ExplosionEffect.transform.rotation), 0.25f);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Kirby")
        {
            Explode();
        }
    }

    private void OnDrawGizmos()
    {
        if (ShowExplosionRadius)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, ExplosionRadius);
        }
    }
}
