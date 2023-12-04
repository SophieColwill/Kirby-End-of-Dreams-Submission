using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class KirbyHealth : MonoBehaviour
{
    [SerializeField] float InvicibilityTime;
    [SerializeField] float Timer;

    [SerializeField] float MaxHealth;
    float Health;
    [SerializeField] Slider HealthBar;

    [SerializeField] int DropCopyAbilityChance = 4;

    KirbyCopyAbilities copyAbilityManager;

    private void Start()
    {
        Health = MaxHealth;
        HealthBar.maxValue = MaxHealth;
        HealthBar.value = MaxHealth;
        copyAbilityManager = GetComponent<KirbyCopyAbilities>();
    }

    private void Update()
    {
        if (Timer > 0)
        {
            Timer -= Time.deltaTime;
        }
    }

    public float SetHealth 
    {
        get { return Health; }
        set
        {
            if (Timer <= 0)
            {
                Health = value;
                HealthBar.value = value;
                Timer = InvicibilityTime;

                if (Health <= 0)
                {
                    SceneManager.LoadScene("Green Greens");
                }
                else
                {
                    int DropChance = Random.Range(1, DropCopyAbilityChance + 1);
                    if (DropChance == 1)
                    {
                        copyAbilityManager.DropCopyAbility();
                    }
                }
            }
        } 
    }
}
