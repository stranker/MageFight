using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    private static CameraController instance;
    public static CameraController Get() { return instance; }
    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);
    }

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
    public List<Transform> playerList = new List<Transform>();
    private new Camera camera;

    public float minCameraSize = 8f;
    public float maxCameraSize = 14f;
    public float maxXPos = 3;
    public float maxYPositionPositive = 3;
    public float maxYPositionNegative = 3;

    public AnimationCurve slowmoCurve;
    private float slowmoTimer;
    public float slowmoTotalTime = 4f;

    public bool shaking = false;
    public float shakeOffset = 0.1f;
    public float shakeTime = 0.5f;
    public float defaultSpellShakeTime = 0.5f;
    private float shakeTimer = 0;

    public float playerDistanceZoomFactor = 0.5f;

    private void Start()
    {
        foreach (var player in GameManager.Instance.activeWizardList)
        {
            playerList.Add(player.transform);
        }
        camera = GetComponent<Camera>();
        startPos = transform.position;
        startSize = camera.orthographicSize;
    }

    public void CheckCameraState()
    {
        if (playerList.Count > 0)
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
    }

    private void NormalMovement()
    {
        Vector2 positionsSum = playerList[0].position + playerList[1].position;
        Vector3 positionAverage = positionsSum / playerList.Count;
        float playerDistance = Vector3.Distance(playerList[0].position, playerList[1].position);
        camera.orthographicSize = minCameraSize + playerDistance * playerDistanceZoomFactor;
        camera.orthographicSize = Mathf.Clamp(camera.orthographicSize, minCameraSize, maxCameraSize);
        positionAverage.x = Mathf.Clamp(positionAverage.x, -maxXPos, maxXPos);
        positionAverage.y = Mathf.Clamp(positionAverage.y, -maxYPositionNegative, maxYPositionPositive);
        positionAverage.z = -10;
        transform.position = positionAverage;
    }

    private void SlowmotionMovement()
    {
        slowmoTimer += Time.unscaledDeltaTime * 0.5f;
        Time.timeScale = slowmoCurve.Evaluate(slowmoTimer);
        if (slowmoTimer >= slowmoTotalTime)
        {
            Time.timeScale = 1;
            slowmoTimer = 0;
            cameraState = CameraStates.Normal;
        }
        translationCurveTimer += Time.unscaledDeltaTime;
        endPos = target != null ? new Vector3(target.position.x, target.position.y, transform.position.z) : new Vector3(0,0,-10);
        transform.position = Vector3.Lerp(transform.position, endPos, translationCurve.Evaluate(translationCurveTimer / travelTransitionTime));
        camera.orthographicSize = target != null ? Mathf.Lerp(camera.orthographicSize, endSize, translationCurve.Evaluate(translationCurveTimer / cameraSizeTransitionTime)) : camera.orthographicSize;
    }

    private void Update()
    {
        CheckCameraState();
        CheckCameraShaking();
    }

    private void CheckCameraShaking()
    {
        if (shaking)
        {
            float xOffset = UnityEngine.Random.Range(-shakeOffset, shakeOffset);
            float yOffset = UnityEngine.Random.Range(-shakeOffset, shakeOffset);
            Vector3 newPos = new Vector2(xOffset, yOffset);
            transform.position += newPos;
            shakeTimer += Time.deltaTime;
            if (shakeTimer >= shakeTime)
            {
                shaking = false;
                shakeTimer = 0;
            }
        }
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
        slowmoTimer = 0;
    }

    public void CameraShake(float amount, float time)
    {
        shakeTime = time;
        shakeOffset = amount;
        shaking = true;
    }

    public void SpellShake(float amount)
    {
        if (amount != 0)
        {
            shakeTime = defaultSpellShakeTime;
            shakeOffset = amount;
            shaking = true;
        }
    }

}
