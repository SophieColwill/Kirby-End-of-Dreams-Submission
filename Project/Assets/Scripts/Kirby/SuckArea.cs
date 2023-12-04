using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuckArea : MonoBehaviour
{
    [SerializeField] private List<Collider2D> ObjectsInRange = new List<Collider2D>();

    public List<Collider2D> GetObjectInRange
    {
        get { return ObjectsInRange; }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ObjectsInRange.Add(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ObjectsInRange.Remove(collision);
    }
}
