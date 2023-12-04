using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WispyWoodsBreath : MonoBehaviour
{
    [SerializeField] float Speed;
    [SerializeField] float Damage;

    private void Update()
    {
        transform.position -= Vector3.right * Speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        KirbyHealth target = collision.gameObject.GetComponent<KirbyHealth>();
        if (target != null)
        {
            target.SetHealth -= Damage;
        }
    }
}
