using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public float score;
    public bool isGrabed;

    public static List<Item> allItems = new List<Item>();

    public UIItemsBars iItemsBar;

    private void Awake()
    {
        allItems.Add(this);
        score = Random.Range(1, 11);
    }

    void Start()
    {
        UIController.get.CreateItemBar(this);
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
       
    }

    private void OnDestroy()
    {
        allItems.Remove(this);
        if(iItemsBar != null)
            iItemsBar.gameObject.SetActive(false);
    }
}
