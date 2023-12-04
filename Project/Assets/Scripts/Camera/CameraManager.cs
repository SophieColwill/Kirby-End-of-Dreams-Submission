using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] List<VirutalCameraRefrence> cameras;

    public void ChangeCurrentCamera(string newCamID)
    {
        foreach (VirutalCameraRefrence cam in cameras)
        {
            cam.Cam.SetActive(cam.ID == newCamID);
        }
    }
}

[System.Serializable]
public class VirutalCameraRefrence
{
    public GameObject Cam;
    public string ID;
}