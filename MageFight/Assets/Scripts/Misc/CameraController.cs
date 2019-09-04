using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public enum CameraStates {Normal, Slowmo};
    public CameraStates cameraState = CameraStates.Normal;
    public AnimationCurve translationCurve;
    private float translationCurveTimer;
    public float travelTransitionTime = 2f;
    public float cameraSizeTransitionTime = 2f;
    private Vector3 startPos;
    private Vector3 endPos;
    public Transform target;
    private float startSize;
    public float endSize = 3.0f;
    public Transform[] playerList;
    private new Camera camera;
    public float offsetYCamera = 2f;
    public float minCameraSize = 8f;
    public float maxCameraSize = 14f;
    public float maxXPos = 3;
    public float maxYPos = 3;

    public AnimationCurve deathCurve;
    private float deathTimer;
    public float freezeDeathTime = 4f;

    private void Start()
    {
       camera = GetComponent<Camera>();
       startPos = transform.position;
       startSize = camera.orthographicSize;
    }

    public void CheckCameraState()
    {
        switch (cameraState)
        {
            case CameraStates.Normal:
                NormalMovement();
                break;
            case CameraStates.Slowmo:
                SlowmotionMovement();
                break;
            default:
                break;
        }
    }

    private void NormalMovement()
    {
        Vector2 positionsSum = playerList[0].position + playerList[1].position;
        Vector3 positionAverage = positionsSum / playerList.Length;
        float playerDistance = Vector3.Distance(playerList[0].position, playerList[1].position);
        camera.orthographicSize = minCameraSize + playerDistance * 0.2f;
        camera.orthographicSize = Mathf.Clamp(camera.orthographicSize, minCameraSize, maxCameraSize);
        positionAverage.x = Mathf.Clamp(positionAverage.x, -maxXPos, maxXPos);
        positionAverage.y = Mathf.Clamp(positionAverage.y, -maxYPos, maxYPos);
        positionAverage.z = -10;
        transform.position = positionAverage;
    }

    private void SlowmotionMovement()
    {
        deathTimer += Time.unscaledDeltaTime;
        Time.timeScale = deathCurve.Evaluate(deathTimer);
        if (deathTimer >= freezeDeathTime)
        {
            Time.timeScale = 1;
            deathTimer = 0;
            cameraState = CameraStates.Normal;
        }
        translationCurveTimer += Time.unscaledDeltaTime;
        endPos = new Vector3(target.position.x, target.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, endPos, translationCurve.Evaluate(translationCurveTimer / travelTransitionTime));
        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, endSize, translationCurve.Evaluate(translationCurveTimer / cameraSizeTransitionTime));
    }

    private void Update()
    {
        CheckCameraState();
    }

    public void MoveTowards()
    {
        cameraState = CameraStates.Slowmo;
    }

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    public void Reset()
    {
        cameraState = CameraStates.Normal;
        transform.position = startPos;
        camera.orthographicSize = startSize;
        translationCurveTimer = 0;
        Time.timeScale = 1;
        deathTimer = 0;
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawCube(transform.position, new Vector3(maxXPos, maxYPos, 0));
    }

}
