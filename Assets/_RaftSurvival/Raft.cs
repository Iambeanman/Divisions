using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raft : MonoBehaviour
{

    public float health;

    public float sizeMultiplier = 1.03f;

    void Start()
    {
        UIController.get.healthPoint.text = health.ToString();
    }
   
    void Update()
    {
        
    }

    public void PickUpItem(float itemScore)
    {
        if (itemScore <= health)
        {
            health += itemScore;
            transform.localScale *= sizeMultiplier;
        }
        else health /=  2;

        UIController.get.healthPoint.text = health.ToString();
    }
}
