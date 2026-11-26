using UnityEngine;

public class WeaponInstance 
{
    public EquipmentData data;
    public int Level;
    
    public WeaponLevel Current => data.levels[Level];

    public bool CanUpgrade => Level <= data.levels.Length;

    public WeaponInstance(EquipmentData data, int level = 0)
    {
        this.data = data;
        this.Level = level;
    }
    public int GetCurrentLevel()
    {
        return Level;
    }

    public int GetCurrentCost()
    {
        return data.levels[Level].UpgradeCost;
    }
    public bool TryUpgrade(ref int Gold, Wallet wallet)
    {
        if (!CanUpgrade) return false;
        
        int costo = data.levels[Level + 1].UpgradeCost;
        if (Gold < costo) return false;
        Gold -= costo;
        Level++;
        wallet.RefreshGold();
        
        return true;
    }
}
