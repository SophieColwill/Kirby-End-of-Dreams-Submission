using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] protected float Health;
    [SerializeField] float EnemyDamage;
    protected Rigidbody2D controller;

    [SerializeField] bool CanHurtKirby = true;
    [SerializeField] bool DestroyOnHit = false;
    public CopyAbilitiy ThisCopyAbilotyToGive;
    [SerializeField] float Size = 1;
    [SerializeField] bool isBoss = false;

    private Vector2 InhaleSpeed;

    private void Start()
    {
        controller = GetComponent<Rigidbody2D>();
        EnemyStart();
    }

    private void Update()
    {
        UpdateAnimations();

        if (!isBoss)
        {
            if (InhaleSpeed.magnitude > 0)
            {
                InhaleUpdate();
            }
            else
            {
                EnemyUpdate();
            }
        }
        else
        {
            EnemyUpdate();
        }
    }

    public virtual void EnemyUpdate()
    {

    }

    public virtual void InhaleUpdate()
    {
        controller.velocity = InhaleSpeed;
    }

    public void KirbyInhale(int LookingRight)
    {
        InhaleSpeed += new Vector2(0.05f * -LookingRight, 0);
    }

    public void KirbyStopInhale()
    {
        InhaleSpeed = Vector2.zero;
        controller.velocity = Vector2.zero;
    }

    public virtual void OnEnemyCollide(Collision2D collision)
    {

    }

    public virtual void EnemyStart()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isBoss)
        {
            OnEnemyCollide(collision);
            KirbyCopyAbilities player = collision.gameObject.GetComponent<KirbyCopyAbilities>();
            if (player != null)
            {
                if (player.GetIsInhaling())
                {
                    player.AddInhaledObject(ThisCopyAbilotyToGive, Size);
                    Destroy(gameObject);
                }
                else
                {
                    if (CanHurtKirby)
                    {
                        player.GetComponent<KirbyHealth>().SetHealth -= EnemyDamage;
                        if (DestroyOnHit)
                        {
                            Destroy(gameObject);
                        }
                    }
                }
            }
        }
    }

    public virtual void UpdateAnimations()
    {

    }

    public virtual void TakeDamage(float damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
