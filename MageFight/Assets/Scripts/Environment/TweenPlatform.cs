using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenPlatform : MonoBehaviour {

    public Vector3 initialScale;
    public Vector3 finalScale;
    public float minTweenTime;
    public float maxTweenTime;
    private float timer;
    private float tweenTime;
    private bool isTweening = false;
    public AnimationCurve tweenCurve;

    private void Start()
    {
        finalScale = transform.localScale;
        transform.localScale = initialScale;
        tweenTime = Random.Range(minTweenTime, maxTweenTime);
    }

    private void Update()
    {
        if (isTweening)
        {
            timer += Time.deltaTime;
            transform.localScale = new Vector3(tweenCurve.Evaluate(timer / tweenTime) * finalScale.x, tweenCurve.Evaluate(timer / tweenTime) * finalScale.y);
            if (timer > tweenTime)
            {
                transform.localScale = finalScale;
                timer = 0;
                isTweening = false;
            }
        }
    }

    public void TweenScale()
    {
        isTweening = true;
    }

}
