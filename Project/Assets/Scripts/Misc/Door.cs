using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private CameraManager manager;
    [SerializeField] private Vector3 DoorExitPoint;
    [SerializeField] private string ActivateCameraID;
    public void EnterDoor(KirbyMovment moveRef)
    {
        moveRef.transform.position = DoorExitPoint;
        manager.ChangeCurrentCamera(ActivateCameraID);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(DoorExitPoint, 0.1f);
    }
}
