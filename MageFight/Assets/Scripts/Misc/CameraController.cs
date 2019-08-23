using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
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

    public AnimationCurve deathCurve;
    private float deathTimer;
    public float freezeDeathTime = 4f;
    
    private bool moving;
    private bool slowmo;

    private void Start()
    {
       camera = GetComponent<Camera>();
       startPos = transform.position;
       startSize = camera.orthographicSize;
    }
    private void Update()
    {
        if(slowmo)
        {
            deathTimer += Time.unscaledDeltaTime;
            Time.timeScale = deathCurve.Evaluate(deathTimer);
            if(deathTimer >= freezeDeathTime)
            {
                Time.timeScale = 1;
                deathTimer = 0;
            }
        }
        if(moving)
        {
            translationCurveTimer += Time.unscaledDeltaTime;
            endPos = new Vector3(target.position.x, target.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, endPos, translationCurve.Evaluate(translationCurveTimer / travelTransitionTime));
            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, endSize, translationCurve.Evaluate(translationCurveTimer / cameraSizeTransitionTime));
        }
        else
        {
            Vector2 positionsSum = Vector2.zero;
            foreach (Transform transform in playerList)
            {
                positionsSum += (Vector2)transform.position;
            }
            Vector3 positionAverage = positionsSum / playerList.Length;
            float playerDistance = Vector3.Distance(playerList[0].position, playerList[1].position);
            camera.orthographicSize = minCameraSize + playerDistance * 0.2f;
            camera.orthographicSize = Mathf.Clamp(camera.orthographicSize, minCameraSize, maxCameraSize);
            positionAverage.y += offsetYCamera;
            positionAverage.z = -10;
            if (camera.orthographicSize != maxCameraSize)
            {
                transform.position = positionAverage;
            }
        }
    }
    public void MoveTowards(Transform _target)
    {
        moving = true;
        slowmo = true;
        target = _target;
    }

    public void Reset()
    {
        moving = false;
        transform.position = startPos;
        camera.orthographicSize = startSize;
        translationCurveTimer = 0;
        Time.timeScale = 1;
        deathTimer = 0;
        slowmo = false;
    }
}
