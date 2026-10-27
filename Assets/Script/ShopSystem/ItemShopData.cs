using UnityEngine;

[CreateAssetMenu(menuName = "Shop/Item")]
public class ItemShopData : ScriptableObject
{
    public string id;
    public string displayName;
    [TextArea] public string description;
    public Sprite icon;
    public int price;
}

