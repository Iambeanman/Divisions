using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WaterFloating : MonoBehaviour
{
    internal Rigidbody m_body;
    [Header("Floating info")]
    public bool floatingEnabled = true;
    [Header("Object info")]
    public float objectVerticalSize = 0.4f;

    [Header("Underwater")]
    public float maxDeepDownVelocity = 15f;
    public float maxVerticalVelocity = 0.2f;
    public float maxAngularVelocity = 1.5f;

    [Header("Water Info")]
    public LayerMask waterMask;
    public float waterDensity = 1f;
    public float waterSpeed = 0f;
    //Вспомогательные значения, информация о контакте с водой
    #region DATA_VALUES
    internal int pointsInWater = 0;
    internal List<bool> waterPoints = new List<bool>();
    internal List<float> pointsDeepValue = new List<float>();
    #endregion
    
    public List<Transform> floatingPoints = new List<Transform>();

    public bool inWater;

    public virtual void Awake() {
        m_body = GetComponent<Rigidbody>();
    }

    public virtual void Start() 
    {
        waterMask = LayerMask.GetMask("Water");
    }

    public virtual void FixedUpdate() 
    {
        //Плаваем на воде
        FloatOnWater();
        
        //Стабилизация скачков в воде
        VerticalVelocityLimit();
    }

    void FloatOnWater()
    {
        if(floatingEnabled == false) return;

        waterPoints.Clear();
        pointsInWater = 0;
        foreach (Transform p in floatingPoints)
        {
            Vector3 force = GetWaterForceAtPosition(p.position, out bool isInWater);
            //Debug.Log("Force : " + force.ToString("f2"));
            waterPoints.Add(isInWater);
            if (isInWater) pointsInWater++;
            m_body.AddForceAtPosition(force, p.position, ForceMode.Acceleration);
        }

        for (int i = 0; i < waterPoints.Count; i++)
        {
            if (waterPoints[i] == true) inWater = true;
            else inWater = false;
        }
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < waterPoints.Count; i++)
        {
            Gizmos.color = Color.red;
            if (waterPoints[i] == true) Gizmos.color = Color.green;

            Gizmos.DrawSphere(floatingPoints[i].position, 0.3f);
        }
        if (Application.isPlaying == false)
        {
            foreach (var fp in floatingPoints)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(fp.transform.position, 0.3f);
            }
        }
    }

        public virtual void VerticalVelocityLimit()
        {
            //Скорость погружения лодки
            if (m_body.velocity.y < -maxDeepDownVelocity && pointsInWater == waterPoints.Count)
            {
                Vector3 fixedVelocity = m_body.velocity;
                fixedVelocity.y = -maxDeepDownVelocity;
                m_body.velocity = fixedVelocity;
            }

            if (m_body.velocity.y > maxVerticalVelocity && (pointsInWater >= 2 || floatingPoints.Count >= 2))
            {
                Vector3 fixedVelocity = m_body.velocity;
                fixedVelocity.y = maxVerticalVelocity;
                m_body.velocity = fixedVelocity;
                //Debug.Log($"Boat stability fixed: {m_body.velocity:f2}");
            }
            if (m_body.angularVelocity.sqrMagnitude > maxAngularVelocity * maxAngularVelocity && (pointsInWater >= 2 || waterPoints.Count < 2))
            {
                m_body.angularVelocity = m_body.angularVelocity.normalized * maxAngularVelocity;
            }
        }

    WaterCollision collisionData;
    float rayAddY = 4f;
    public Vector3 GetWaterForceAtPosition(Vector3 point, out bool inWater)
    {
        //Насколько глубоко погрузилась чтока в воду
        pointsDeepValue.Clear();
        pointsDeepValue.Add(0);
        //Закон архимеда FA=ρVg
        bool isRaycast = Physics.Raycast(point + Vector3.up * rayAddY, Vector3.down, out RaycastHit hit, 15f + rayAddY, waterMask, QueryTriggerInteraction.Ignore);
        float Vpoint = 0f;
        inWater = false;
        float pDeep = hit.point.y - point.y;
        //Означает что лодка сильно под водой, но такого быть не должно по сути
        if (isRaycast == false) Vpoint = 0f;
        else
        {
            pointsDeepValue[pointsDeepValue.Count - 1] = pDeep;
            //Нижняя точка лодки назодится над водой 
            if (pDeep <= 0) return Vector3.up * 0f;

            Vpoint = waterDensity * (pDeep / objectVerticalSize) * -Physics.gravity.y;
        }
        inWater = true;

        collisionData.contactPoint = hit.point;
        collisionData.deep = pDeep;

        //Message
        SendMessage("OnWaterCollide", collisionData , SendMessageOptions.DontRequireReceiver );

        return Vector3.up * (Vpoint / floatingPoints.Count) * 0.9f;
    }
}
public struct WaterCollision
{
    public Vector3 contactPoint;
    public float deep;
}