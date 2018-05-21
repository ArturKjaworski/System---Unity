using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Item item;

    [SerializeField] private Text amount;
    public int stack = 1;
    public Image icon;

    [SerializeField] private Sprite emptySlot;


    #region cursor detection
    public void OnPointerEnter(PointerEventData eventData)
    {
    Inventory.instance.active = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Inventory.instance.active = null;
    }
    #endregion

    #region add item
    public void addItem(Item nItem)
    {
        item = nItem;
        icon.sprite = item.icon;
        if (stack > 1)
            amount.text = stack.ToString();
    }

    public void addItem(int count)
    {
        stack += count;
        if (stack > 1)
            amount.text = stack.ToString();
    }
    #endregion

    #region delete item
    public void delItem()
    {
        if (stack == 1)
        {
            item = null;
            icon.sprite = emptySlot;
            amount.text = null;
        }
        else if (stack>1)
        {
            stack -= 1;

            if (stack == 1)
                amount.text = null;
            else
                amount.text = stack.ToString();
        }
    }

    public void eraseSlot()
    {
        item = null;
        stack = 1;
        icon.sprite = emptySlot;
        amount.text = null;
    }
    #endregion
}

