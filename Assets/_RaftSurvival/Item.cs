using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    public bool isGrabed;

    public static List<Item> allItems = new List<Item>();

    private void Awake()
    {
        allItems.Add(this);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        allItems.Remove(this);
    }
}
