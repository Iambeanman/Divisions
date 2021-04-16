using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{

    public Transform target;

    public LineRenderer lr;
    private Vector3 currentGrapplePosition;
    public int quality;
    public float waveCount;
    public float waveHeight;
    public AnimationCurve affectCurve;

    public float ropeSpeed;

    public bool shotDone;

    public float maxDistToAnim;

    public bool moveBack;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 0;
        target = null;
    }

    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    private void LateUpdate()
    {
        if(shotDone)
        DrawRope();
    }

    void DrawRope()
    {
        if (target == null || moveBack)
        {
            currentGrapplePosition = transform.position;

            lr.positionCount = 2;
            lr.SetPosition(0, Player.get.transform.position);
            lr.SetPosition(1, target.transform.position);

            return;
        }

        if (lr.positionCount == 0)
        {
            lr.positionCount = quality;
        }

        var grapplePoint = target.position;
        var tipPosition = transform.position;
        var up = Quaternion.LookRotation((grapplePoint - tipPosition).normalized) * Vector3.right;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * ropeSpeed);

        float distance = Vector3.Distance(transform.position, target.position);

        for (var i = 0; i < quality; i++)
        {
            var delta = i / (float)quality;
            Vector3 offset = Vector3.zero;
            if (distance < maxDistToAnim)
                offset = up * waveHeight * Mathf.Sin(delta * waveCount * Mathf.PI) * affectCurve.Evaluate(delta);

            lr.SetPosition(i, Vector3.Lerp(tipPosition, currentGrapplePosition, delta) + offset);
        }


    }


}
