using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private static CameraManager instance;
    public static CameraManager Get() { return instance; }
    private void Awake()
    {
        if(!instance)
            instance = this;
        else
            Destroy(gameObject);
    }
    public Camera mainCamera;
    public Camera leftCamera;
    public Camera rightCamera;
    public List<GameObject> playerList;
    private float timer = 0;
    public float presTime = 5f;
    public float deathCamTime = 5f;
    private bool onPlayerPresentation = false;
    private bool onDeathCam = false;

    private void Update()
    {
        CheckTimerOnPlayerPresentation();
        CheckTimerOnDeathCam();
    }

    public void ActivateCamerasPlayerPresentation()
    {
        mainCamera.gameObject.SetActive(false);
        leftCamera.gameObject.SetActive(true);
        rightCamera.gameObject.SetActive(true);
        onPlayerPresentation = true;
    }
    public void ActivateCamerasGameplay()
    {
        mainCamera.gameObject.SetActive(true);
        leftCamera.gameObject.SetActive(false);
        rightCamera.gameObject.SetActive(false);
    }
    public void ActivateDeathcam()
    {
        mainCamera.GetComponent<CameraController>().MoveTowards();
        onDeathCam = true;
    }

    private void CheckTimerOnPlayerPresentation()
    {
        if(onPlayerPresentation)
        {
            Vector3 lCamPos = playerList[0].transform.position;
            lCamPos.z = -1;
            Vector3 rCamPos = playerList[1].transform.position;
            rCamPos.z = -1;
            leftCamera.gameObject.transform.position = lCamPos;
            rightCamera.gameObject.transform.position = rCamPos;
            if(timer >= presTime)
            {
                timer = 0;
                onPlayerPresentation = false;
                GameplayManager.Get().SendEvent(GameplayManager.Events.PlayerPresentationEnd);
            }
            else
                timer += Time.deltaTime;
        }
    }
    public void CheckTimerOnDeathCam()
    {
        if(onDeathCam)
        {
            if(timer < deathCamTime)
                timer += Time.deltaTime;
            else
            {
                timer = 0;
                onDeathCam = false;
                GameplayManager.Get().SendEvent(GameplayManager.Events.DeathCameraEnd);
            }
        }
    }
}
