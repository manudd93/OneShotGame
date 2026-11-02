using UnityEngine;

[CreateAssetMenu(menuName = "Shop/Item")]
public class ItemShopData : ItemMaking
{
    [Header("Impostazione Shop")]
    public string displayName;
    public int price;
}

