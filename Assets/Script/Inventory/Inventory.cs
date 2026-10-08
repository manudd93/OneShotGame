using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    [Header("Impostazioni")]
    [SerializeField] private int maxSlots = 8;

    [Header("Inventario")]
    [SerializeField] private List<ItemMaking> items = new List<ItemMaking>();

    public int MaxSlots => maxSlots;
    public List<ItemMaking> Items => items;

    // Controlla se c'è spazio
    public bool HasSpace()
    {
        return items.Count < maxSlots;
    }

    // Aggiunge un oggetto
    public bool AddItem(ItemMaking item)
    {
        if (item == null)
            return false;

        // Gli equipaggiamenti non vanno nell'inventario normale
        if (item.itemType == ItemType.Abilita ||
            item.itemType == ItemType.Accessorio ||
            item.itemType == ItemType.Arma)
        {
            Debug.LogWarning("Questo oggetto è un equipaggiamento e non dovrebbe essere aggiunto all'inventario normale.");
            return false;
        }

        if (!HasSpace())
        {
            Debug.Log("Inventario pieno!");
            return false;
        }

        items.Add(item);

        Debug.Log("Aggiunto all'inventario: " + item.itemName);

        return true;
    }

    // Rimuove un oggetto
    public bool RemoveItem(ItemMaking item)
    {
        if (item == null)
            return false;

        if (!items.Contains(item))
            return false;

        items.Remove(item);

        Debug.Log("Rimosso dall'inventario: " + item.itemName);

        return true;
    }

    // Controlla se possiedi un determinato oggetto
    public bool Contains(ItemMaking item)
    {
        return items.Contains(item);
    }

    // Svuota tutto
    public void ClearInventory()
    {
        items.Clear();
    }

    // Restituisce quanti slot sono occupati
    public int GetItemCount()
    {
        return items.Count;
    }
}

