using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterWispyWoodsBossfightArena : MonoBehaviour
{
    [SerializeField] WispyWoods BossRefrence;
    [SerializeField] CameraManager camManager;
    [SerializeField] GameObject BossFightWall;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        KirbyMovment target = collision.GetComponent<KirbyMovment>();
        if (target != null)
        {
            BossRefrence.StartBattle();
            camManager.ChangeCurrentCamera("bossfight");
            BossFightWall.SetActive(true);
        }
    }
}
