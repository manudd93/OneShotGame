using System.Collections.Generic;
using UnityEngine;

public enum EquipSlotType
{
    Weapon,
    Armor,
    Accessory
}

/// <summary>
/// Una singola statistica dell'equipaggiamento, es: "Attacco" = 15
/// Puoi usare lo stesso nome (es. "Attacco") su più oggetti per poterli confrontare.
/// </summary>
[System.Serializable]
public struct StatValue
{
    public string statName;
    public float value;
}

/// <summary>
/// Oggetto equipaggiabile: arma, armatura o accessorio.
/// Crealo da: Create > Inventory > Equipment Item
/// </summary>
[CreateAssetMenu(fileName = "NuovoEquip", menuName = "Inventario/Equipaggiamento")]
public class EquipmentData : ItemMaking
{
    [Header("Equipaggiamento")]
    public EquipSlotType equipSlot;
    public List<StatValue> stats = new List<StatValue>();

    /// <summary>Ritorna il valore di una statistica per nome (0 se non presente).</summary>
    public float GetStat(string statName)
    {
        foreach (var s in stats)
            if (s.statName == statName) return s.value;
        return 0f;
    }
}
