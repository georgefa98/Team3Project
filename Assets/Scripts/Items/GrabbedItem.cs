using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrabbedItem : MonoBehaviour
{
    RectTransform rectTransform;
    Image panel;
    Image icon;
    Text stackAmount;

    Sprite emptyCellSprite;
    Color emptyCellColor;

    Item item;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        panel = GetComponent<Image>();
        icon = transform.GetChild(0).GetComponent<Image>();
        stackAmount = transform.GetChild(1).GetComponent<Text>();

        emptyCellSprite = icon.sprite;
        emptyCellColor = icon.color;

        AllowClickThrough();
    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.anchoredPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }

    public Item CurrentItem {
        get {
            return item;
        }
        set {
            item = value;
            if(item != null) {
                icon.sprite = item.itemInfo.icon;
                icon.color = Color.white;
                stackAmount.text = "" + item.stackAmount;
            } else {
                icon.sprite = emptyCellSprite;
                icon.color = emptyCellColor;
                stackAmount.text = "";
            }
        }
    }

    public void AllowClickThrough() {
        panel.raycastTarget = false;
        icon.raycastTarget = false;
        stackAmount.raycastTarget = false;
    }

    public void Hide() {
        panel.enabled = false;
        icon.enabled = false;
        stackAmount.enabled = false;
    }

    public void Show() {
        panel.enabled = true;
        icon.enabled = true;
        stackAmount.enabled = true;
    }
}
