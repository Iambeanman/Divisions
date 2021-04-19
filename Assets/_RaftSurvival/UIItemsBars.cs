using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemsBars : MonoBehaviour
{

    public Item item;
    public RectTransform rectTransform;

    public Text scoreText;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Start()
    {
        scoreText.text = item.score.ToString();
    }
}
