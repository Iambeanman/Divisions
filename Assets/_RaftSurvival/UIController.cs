using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{


    public List<UIItemsBars> itemsBars = new List<UIItemsBars>();
    public RectTransform targetCanvas;
    public Vector2 globalOffset;

    public UIItemsBars itemBarPrefab;

    public static UIController get;

    public RectTransform playerPanel;
    public Text healthPoint;

    private void Awake()
    {
        get = this;
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
        UpdateItemBarsPositions();
        UpdatePlayerPanelPosition();
    }

    void UpdateItemBarsPositions()
    {
        foreach (var itemBar in itemsBars)
        {
            if (itemBar == null || itemBar.item == null) continue;

            Vector2 targetScreenPos = Camera.main.WorldToViewportPoint(itemBar.item.transform.position);

            Vector2 screenPosition = new Vector2(((targetScreenPos.x * targetCanvas.sizeDelta.x) - (targetCanvas.sizeDelta.x * 0.5f)),
                ((targetScreenPos.y * targetCanvas.sizeDelta.y) - (targetCanvas.sizeDelta.y * 0.5f)));

            itemBar.rectTransform.anchoredPosition = Vector2.Lerp(itemBar.rectTransform.anchoredPosition, screenPosition, Time.deltaTime * 100 * 2);
        }
    }

    void UpdatePlayerPanelPosition()
    {
        Vector2 targetScreenPos = Camera.main.WorldToViewportPoint(Player.get.transform.position);

        Vector2 screenPosition = new Vector2(((targetScreenPos.x * targetCanvas.sizeDelta.x) - (targetCanvas.sizeDelta.x * 0.5f)),
            ((targetScreenPos.y * targetCanvas.sizeDelta.y) - (targetCanvas.sizeDelta.y * 0.5f)));

        playerPanel.anchoredPosition = Vector2.Lerp(playerPanel.anchoredPosition, screenPosition + globalOffset, Time.deltaTime * 100 * 2);
    }

    public void CreateItemBar(Item item)
    {
        var createdBar = Instantiate(itemBarPrefab, transform);
        createdBar.gameObject.SetActive(true);
        createdBar.transform.SetAsFirstSibling();
        createdBar.item = item;
        item.iItemsBar = createdBar;
        itemsBars.Add(createdBar);
    }
}
