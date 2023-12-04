using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WispyWoods : EnemyBehavior
{
    [Header("Health Bar")]
    [SerializeField] Slider HealthBar;
    [SerializeField] GameObject HealthBarParent;

    [Header("Attacks")]
    [SerializeField] int onAttack;
    [SerializeField] float timeBetweenAttacks;
    [Header("AppleDrop")]
    [SerializeField] Vector2 AmountOfApplesToDrop;
    [SerializeField] Vector2 TimeBetweenApples;
    [SerializeField] GameObject ApplePrefab;
    [SerializeField] float AppleSpawnY;
    [SerializeField] Vector2 AppleSpawnX;
    [SerializeField]float AppleDropTimer;
    [SerializeField]int ApplesLeft;
    [Header("Breath")]
    [SerializeField] GameObject BreathPrefab;
    [SerializeField] float BreathInterval;
    [SerializeField] Transform BreathSpawnPoint;
    float BreathTimer;
    int AmmountOfBreaths;

    [Header("Sounds")]
    [SerializeField] AudioClip WispyWoodsFightMusic;

    Animator animator;
    float AttackDelay;
    float DieTimer;
    bool isDead;
    MusicManager musicManage;

    public override void EnemyStart()
    {
        HealthBar.value = Health;
        HealthBar.maxValue = Health;
        HealthBarParent.SetActive(false);
        animator = GetComponent<Animator>();
        musicManage = FindObjectOfType<MusicManager>();
    }

    public override void TakeDamage(float damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            DieTimer = 3;
            isDead = true;
        }

        HealthBar.value = Health;
    }

    public override void EnemyUpdate()
    {
        if (AttackDelay <= 0)
        {
            if (onAttack == 2)
            {
                if (BreathTimer > 0)
                {
                    BreathTimer -= Time.deltaTime;

                    if (BreathTimer <= 0)
                    {
                        animator.SetTrigger("breathAttack");
                        Instantiate(BreathPrefab, BreathSpawnPoint.position, BreathPrefab.transform.rotation);
                        BreathTimer = BreathInterval;
                        AmmountOfBreaths--;

                        if (AmmountOfBreaths == 0)
                        {
                            ChangeAttack();
                        }
                    }
                }
            }

            if (onAttack == 1)
            {
                if (AppleDropTimer > 0)
                {
                    AppleDropTimer -= Time.deltaTime;   

                    if (AppleDropTimer <= 0)
                    {
                        AppleDropTimer = Random.Range(TimeBetweenApples.x, TimeBetweenApples.y);

                        Vector3 RandomSpawnPosition = new Vector3(Random.Range(AppleSpawnX.x, AppleSpawnX.y), AppleSpawnY, 0);
                        Rigidbody2D appleRB = Instantiate(ApplePrefab, RandomSpawnPosition, ApplePrefab.transform.rotation).GetComponent<Rigidbody2D>();
                        appleRB.velocity -= Vector2.up * 4;

                        ApplesLeft--;
                        if (ApplesLeft == 0)
                        {
                            ChangeAttack();
                        }
                    }
                }
            }
        }
        else
        {
            AttackDelay -= Time.deltaTime;
        }

        if (isDead)
        {
            animator.SetBool("isDead", true);

            DieTimer -= Time.deltaTime;
            if (DieTimer <= 0)
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    public void StartBattle()
    {
        HealthBarParent.SetActive(true);
        onAttack = 2;
        AmmountOfBreaths = 2;
        BreathTimer = 0.1f;
        AttackDelay = 1;
        AppleDropTimer = 0.1f;
        animator.SetBool("hasStartedBattle", true);
        HealthBar.value = Health;
        HealthBar.maxValue = Health;
        musicManage.ChangeCurrentSong(WispyWoodsFightMusic);
    }

    void ChangeAttack()
    {
        AttackDelay = timeBetweenAttacks;

        if (onAttack == 1)
        {
            onAttack = 2;
            AmmountOfBreaths = 2;
        }
        else if (onAttack == 2)
        {            
            onAttack = 1;
            ApplesLeft = Random.Range(Mathf.RoundToInt(AmountOfApplesToDrop.x), Mathf.RoundToInt(AmountOfApplesToDrop.y));
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector3(AppleSpawnX.x, AppleSpawnY, 0), new Vector3(AppleSpawnX.y, AppleSpawnY, 0));
    }
}
