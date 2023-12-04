using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBeam : MonoBehaviour
{
    bool CanHurt = true;
    float Damage;
    float ActiveTime;

    public void OnStart(float InputedDamage, float InputedTime)
    {
        Damage = InputedDamage;
        ActiveTime = InputedTime;
    }

    private void Update()
    {
        ActiveTime -= Time.deltaTime;
        if (ActiveTime <= 0 )
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CanHurt)
        {
            KirbyHealth target = collision.gameObject.GetComponent<KirbyHealth>();
            if (target != null)
            {
                target.SetHealth -= Damage;
                CanHurt = false;
            }
        }
    }
}
