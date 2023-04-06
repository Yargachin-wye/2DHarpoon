using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    private bool lineRendererSpeedUp = false;

    private bool startFly = false;
    private bool stopFly = false;
    [Range(0.01f, 4)] [SerializeField] private float StartWaveSize = 2;
    float waveSize = 0;
    [SerializeField] [Range(1, 50)] private float ropeProgressionSpeed = 1;

    public AnimationCurve ropeAnimationCurve;
    public AnimationCurve ropeProgressionCurve;

    int numSections = 0;
    float moveTime = 0;

    private void Start()
    {
        waveSize = StartWaveSize;
    }
    private void Update()
    {
        if (startFly)
        {
            moveTime += Time.deltaTime;
            if (waveSize > 0)
            {
                waveSize -= Time.deltaTime;
            }
            else
            {
                waveSize = 0;
            }
            DrawRopeWaves();
        }
        else if (stopFly)
        {
            RenderingRope();
        }
    }
    void DrawRopeWaves()
    {
        int j = _lineRenderer.positionCount - 1;
        for (int i = 0; i < _lineRenderer.positionCount; i++)
        {
            float delta = (float)i / ((float)_lineRenderer.positionCount - 1f);
            Vector2 offset =
                Vector2.Perpendicular
                (transform.GetChild(transform.childCount - 1).position - transform.GetChild(0).position).normalized * ropeAnimationCurve.Evaluate(delta) * waveSize;
            Vector2 targetPosition = Vector2.Lerp(transform.GetChild(transform.childCount - 1).position, transform.GetChild(0).position, delta) + offset;
            Vector2 currentPosition = Vector2.Lerp(transform.GetChild(transform.childCount - 1).position, targetPosition, ropeProgressionCurve.Evaluate(moveTime) * ropeProgressionSpeed);

            _lineRenderer.SetPosition(j, currentPosition);

            j--;
        }
    }
    private void RenderingRope()
    {
        for (int i = 0; i < numSections; i++)
        {
            Vector3 pose = new Vector3(transform.GetChild(i).position.x, transform.GetChild(i).position.y, 0);
            if (!lineRendererSpeedUp && i < numSections-2)
            {
                _lineRenderer.SetPosition(i, Vector3.MoveTowards(_lineRenderer.GetPosition(i), pose, Time.deltaTime * 20));
            }
            else
            {
                _lineRenderer.SetPosition(i, pose);
            }
        }
    }
    public void SetCount(int num)
    {
        numSections = num;
        _lineRenderer.positionCount = num * 4;
    }
    public void StartFly()
    {
        startFly = true;
    }
    public void StopFly()
    {
        int ost = _lineRenderer.positionCount % 4;

        int i = 0; ;
        for (; i < numSections/* - 1*/;)
        {
            Vector2 v = _lineRenderer.GetPosition(i * 4);
            _lineRenderer.SetPosition(i, v);
            i++;
        }
        if (numSections - 1 >= 0)
            _lineRenderer.positionCount = numSections;
        else
            _lineRenderer.positionCount = 0;
        startFly = false;
        stopFly = true;

        StartCoroutine(TimerRender());

        DebugUi.instance.LogText(
            "transform = " + transform.childCount +
            "; num = " + numSections +
            "; lineR = " + _lineRenderer.positionCount);
    }

    IEnumerator TimerRender()
    {
        yield return new WaitForSeconds(0.2f);
        lineRendererSpeedUp = true;
    }
}
