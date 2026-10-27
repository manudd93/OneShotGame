using System;
using UnityEngine;

[Serializable]
public class ShopEntry
{
    public ItemShopData item;
    public bool limitedStock;
    [Min(1)] public int stock = 1;

    // Valore a runtime: non viene salvato nell'asset né nella scena
    public int Remaining { get; private set; }
    public bool IsSoldOut => limitedStock && Remaining <= 0;

    public void Init() => Remaining = stock;

    public bool TryConsume()
    {
        if (IsSoldOut) return false;
        if (limitedStock) Remaining--;
        return true;
    }
}
