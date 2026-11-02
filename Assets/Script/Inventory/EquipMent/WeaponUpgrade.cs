using UnityEngine;

public class WeaponUpgrade : MonoBehaviour
{

    [SerializeField] Player player;
    [SerializeField] Wallet wallet;
    [SerializeField] GameManager gameManager;
    WeaponInstance weaponPresa;
    EquipmentData CurrentEquip;
    public int CurrentLevel;
    public void LoadStats()
    {
        weaponPresa = player.PrendiDati();
        CurrentEquip = player.PrendiDato();
        CurrentLevel = weaponPresa.Level;
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
        player.ReUpdateWeaponUpgraded(CurrentEquip);
        
    }
}
