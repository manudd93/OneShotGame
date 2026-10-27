using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ShopSlot : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] TMP_Text nameText, priceText, stockText;
    [SerializeField] Button buyButton;
    [SerializeField] GameObject soldOutOverlay; // opzionale

    ShopEntry entry;

    public void Setup(ShopEntry entry, Action<ShopEntry, ShopSlot> onBuy)
    {
        this.entry = entry;
        icon.sprite = entry.item.icon;
        nameText.text = entry.item.displayName;
        priceText.text = entry.item.price.ToString();
        buyButton.onClick.AddListener(() => onBuy(entry, this));
        Refresh();
    }

    public void Refresh()
    {
        bool soldOut = entry.IsSoldOut;
        buyButton.interactable = !soldOut;
        if (soldOutOverlay) soldOutOverlay.SetActive(soldOut);

        if (stockText)
        {
            stockText.gameObject.SetActive(entry.limitedStock);
            stockText.text = soldOut ? "Esaurito" : $"Rimasti: {entry.Remaining}";
        }
    }
}