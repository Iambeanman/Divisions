using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{

    public Item grabedItem;

    public EHookState hookState;

    Vector3 moveDir;
    public Rigidbody body;

    public LayerMask layerMask;

    public float distanceToStopMove;
    public float distanceToPlayer;

    public Vector3 startPosition;
    public Vector3 startRotation;
    public float comebackMoveSpeed;


    public enum EHookState
    {
        Idle,
        Aim,
        Fly,
        Comeback
    }

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        startPosition = transform.localPosition;
        startRotation = transform.localEulerAngles;
        body.isKinematic = true;
        body.useGravity = false;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StateLogic();
    }

    public void ChangeState(EHookState newState)
    {
        if (hookState == newState) return;
        hookState = newState;

        switch (hookState)
        {
            case EHookState.Idle:
                if(grabedItem != null)
                {
                    Player.get.raft.PickUpItem(grabedItem.score);
                    Destroy(grabedItem.gameObject);
                    grabedItem = null;
                }
             
                Player.get.rope.moveBack = false;
                transform.localPosition = startPosition;
                transform.localRotation = Quaternion.Euler(startRotation);
                body.velocity = Vector3.zero;
               
                Player.get.rope.lr.positionCount = 0;

                body.isKinematic = true;
                body.useGravity = false;

                break;
            case EHookState.Aim:
                break;
            case EHookState.Fly:
                body.isKinematic = false;
                body.useGravity = true;
                break;
            case EHookState.Comeback:

                if (grabedItem != null)
                {
                    grabedItem.GetComponent<Rigidbody>().isKinematic = true;
                    grabedItem.GetComponent<Rigidbody>().useGravity = false;
                    grabedItem.GetComponent<WaterFloating>().floatingEnabled = false;
                }

                body.useGravity = false;
                body.velocity = Vector3.zero;
                moveDir = Player.get.transform.position - transform.position;
                Player.get.rope.moveBack = true;
                break;
        }
    }

    void StateLogic()
    {
        switch (hookState)
        {
            case EHookState.Idle:
                break;
            case EHookState.Aim:
                break;
            case EHookState.Fly:

                transform.rotation = Quaternion.LookRotation(body.velocity);
                break;
            case EHookState.Comeback:

                Vector3 dir = transform.position - Player.get.transform.position;
                transform.rotation = Quaternion.LookRotation(dir.normalized);

                distanceToPlayer = Vector3.Distance(transform.position, Player.get.transform.position);

                if (distanceToPlayer > distanceToStopMove)
                    transform.position = Vector3.MoveTowards(transform.position, Player.get.transform.position, Time.deltaTime * comebackMoveSpeed);
                else ChangeState(EHookState.Idle);

              

                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision == null) return;

        if (hookState != EHookState.Fly) return;


        var castColliders = Physics.OverlapSphere(transform.position, 2 , layerMask);

        if(castColliders.Length > 0)
        {
            foreach (var col in castColliders)
            {
                var item = col.GetComponent<Item>();
                if (item != null)
                {
                    grabedItem = col.GetComponent<Item>();
                    transform.position = col.transform.position;
                    item.GetComponent<Collider>().enabled = false;
                    item.transform.SetParent(this.transform);
                    item.isGrabed = true;
                    break;
                }
            }
        }
       
        ChangeState(EHookState.Comeback);
    }
}
