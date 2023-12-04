using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatforms : MonoBehaviour
{
    [SerializeField] GameObject ObjectOnUpOnly;
    [SerializeField] KirbyMovment moveScript;

    void Update()
    {
        ObjectOnUpOnly.SetActive(moveScript.controller.velocity.y <= 0);
    }
}
