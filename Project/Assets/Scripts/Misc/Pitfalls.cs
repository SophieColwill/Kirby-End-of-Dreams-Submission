using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pitfalls : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        KirbyHealth target = other.GetComponent<KirbyHealth>();
        if (target != null)
        {
            target.SetHealth = 0;
        }
    }
}
