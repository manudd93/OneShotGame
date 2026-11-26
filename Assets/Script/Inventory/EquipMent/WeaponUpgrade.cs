using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WeaponUpgrade : MonoBehaviour
{

    [SerializeField] Player player;
    [SerializeField] Wallet wallet;
    [SerializeField] GameManager gameManager;
    WeaponInstance weaponPresa;
    EquipmentData CurrentEquip;
    [SerializeField] Image iconWeapon;
    [SerializeField] TextMeshProUGUI nameWeapon;
    [SerializeField] TextMeshProUGUI costUpgrade;
    [SerializeField] TextMeshProUGUI statsChange;

    public int CurrentLevel;
    public void LoadStats()
    {
        weaponPresa = player.PrendiDati();
        CurrentEquip = player.PrendiDato();
        CurrentLevel = weaponPresa.Level;
        nameWeapon.text = CurrentEquip.itemName + " + " + weaponPresa.GetCurrentLevel(); ;
        iconWeapon.sprite = CurrentEquip.icon;
        costUpgrade.text = weaponPresa.GetCurrentCost().ToString();
    }
     void Refresh()
    {
        CurrentLevel = weaponPresa.Level;
    }
    public void OnEnable()
    {
        LoadStats();
    }

    public void UpgradeThis()
    {
        weaponPresa.TryUpgrade(ref wallet.Gold,wallet);
        Refresh();
        wallet.RefreshGold();
        player.ReUpdateWeaponUpgraded(CurrentEquip);
        LoadStats();
        
    }
}
