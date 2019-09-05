using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOffScreenIndicator : MonoBehaviour {

    public bool activated = false;
    public Transform target;
    public float moveSpeed;
    public float screenHeight;
    public float screenWidth;
    public Transform arrow;
    public Vector2 offsetIndicator;

    private void Start()
    {
        screenHeight = 2f * Camera.main.orthographicSize;
        screenWidth = screenHeight * Camera.main.aspect;
    }

    // Update is called once per frame
    void Update () {
        if (activated)
        {
            FollowPlayer();
            ClampToScreen();
            CheckPlayerDead();
        }
	}

    private void CheckPlayerDead()
    {
        if (!target.GetComponent<PlayerBehavior>().isAlive)
        {
            SetActivated(false);
        }
    }

    private void FollowPlayer()
    {
        Vector3 dir = (target.position - transform.position).normalized;
        transform.position += dir * moveSpeed;
        float arrowAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        arrow.transform.rotation = Quaternion.Euler(0,0,arrowAngle);
    }

    private void ClampToScreen()
    {
        screenHeight = 2f * Camera.main.orthographicSize;
        screenWidth = screenHeight * Camera.main.aspect;
        Vector2 cameraPos = Camera.main.transform.position;
        Vector2 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -screenWidth / 2 + cameraPos.x + offsetIndicator.x, screenWidth / 2 + cameraPos.x - offsetIndicator.x);
        pos.y = Mathf.Clamp(pos.y, -screenHeight / 2 + cameraPos.y - offsetIndicator.y, screenHeight / 2 + cameraPos.y - offsetIndicator.y);
        transform.position = pos;
    }

    public void SetActivated(bool val)
    {
        activated = val;
        gameObject.SetActive(val);
    }

}
