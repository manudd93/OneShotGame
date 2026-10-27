using UnityEngine;


using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{
    [SerializeField] Wallet wallet;
    [SerializeField] Inventory inventory;
    [SerializeField] List<ShopEntry> entries = new(); // si compila nell'Inspector
    [SerializeField] ShopSlot slotPrefab;
    [SerializeField] Transform content;

    void Start()
    {
        foreach (var entry in entries)
        {
            if (entry.item == null) continue;
            entry.Init();
            var slot = Instantiate(slotPrefab, content);
            slot.Setup(entry, Buy);
        }
    }

    void Buy(ShopEntry entry, ShopSlot slot)
    {
        if (entry.IsSoldOut) return;

        // Prima controllo i soldi, poi scalo lo stock
        if (!wallet.TrySpend(entry.item.price))
        {
            Debug.Log("Monete insufficienti!");
            return;
        }

        entry.TryConsume();
        //inventory.Add(entry.item);
        Debug.Log("Comprato" + entry.item.displayName);
        slot.Refresh();
    }
}