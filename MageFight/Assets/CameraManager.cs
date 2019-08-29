using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera mainCamera;
    public Camera leftCamera;
    public Camera rightCamera;
    public List<GameObject> playerList;
    private float timer = 0;
    private float time = 5;
	void Start ()
    {
        mainCamera.gameObject.SetActive(false);
        leftCamera.gameObject.SetActive(true);
        rightCamera.gameObject.SetActive(true);
    }

    private void Update()
    {
        Vector3 lCamPos = playerList[0].transform.position;
        lCamPos.z = -1;
        Vector3 rCamPos = playerList[1].transform.position;
        rCamPos.z = -1;
        leftCamera.gameObject.transform.position = lCamPos;
        rightCamera.gameObject.transform.position = rCamPos;
        if(timer >= time)
        {
            mainCamera.gameObject.SetActive(true);
            leftCamera.gameObject.SetActive(false);
            rightCamera.gameObject.SetActive(false);
        }
        else
            timer += Time.deltaTime;
    }
}
