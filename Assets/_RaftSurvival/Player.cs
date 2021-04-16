using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public Hook hook;
    public Rope rope;
    public float throwSmooth = 0.5f;
    public float maxDrag = 100;

    public Vector3 throwPosition;
    public Vector3 startPoint;
    public Vector3 movePoint;

    LineRenderer trajectoryRender;
    Camera mainCamera;

    public float dist;
    public int trajectoryResolution;

    public static Player get;
    public bool drawTrajectory;
    private void Awake()
    {
        trajectoryRender = GetComponent<LineRenderer>();
        rope.transform.position = transform.position;
        mainCamera = Camera.main;
        get = this;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        Aim();
        ShowTrajectory(hook.transform.position, throwPosition);
    }

    public float trajectorySmooth;

    void Aim()
    {
        if (hook.hookState != Hook.EHookState.Idle) return;

        if (Input.GetMouseButtonDown(0))
        {
            startPoint = Input.mousePosition;
            rope.target = hook.transform;
            drawTrajectory = true;
        }

        if (Input.GetMouseButton(0))
        {
            dist = Vector3.Distance(startPoint, movePoint);
            movePoint = Input.mousePosition;

            trajectoryResolution = (int)Mathf.Clamp((float)(dist / trajectorySmooth),0f,40);

            // for reverse
            throwPosition = startPoint - movePoint;
            //throwPosition =  movePoint - startPoint;
            throwPosition.z = throwPosition.y;

            throwPosition.y = dist;
            throwPosition *= throwSmooth;

            throwPosition = Vector3.ClampMagnitude(throwPosition, maxDrag);
        }

        if (Input.GetMouseButtonUp(0))
        {
            hook.ChangeState(Hook.EHookState.Fly);
            hook.body.AddForce(throwPosition, ForceMode.VelocityChange);
            rope.shotDone = true;


            trajectoryRender.positionCount = 0;
            drawTrajectory = false;
        }
    }

    private void LateUpdate()
    {
      
    }

    void ShowTrajectory(Vector3 origin,Vector3 speed)
    {
        if (!drawTrajectory)
        {
            trajectoryRender.positionCount = 0;
            return;
        }

        Vector3[] points = new Vector3[trajectoryResolution];
        trajectoryRender.positionCount = points.Length;

        for (int i = 0; i < points.Length; i++)
        {
            float time = i * 0.1f;

            points[i] = origin + speed * time + Physics.gravity * time * time / 2f;
        }

        trajectoryRender.SetPositions(points);
    }
}
