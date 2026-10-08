using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gestisce i 3 slot fissi di equipaggiamento del personaggio.
/// Mettilo sullo stesso GameObject dell'InventoryManager (es. il Player).
/// </summary>
public class EquipmentManager : MonoBehaviour
{
    private Dictionary<EquipSlotType, EquipmentData> equipped = new Dictionary<EquipSlotType, EquipmentData>();
   [SerializeField] Player player;

    // Passa lo slot cambiato, cosi la UI puo aggiornarsi
    public event Action<EquipSlotType> OnEquipmentChanged;

    public EquipmentData GetEquipped(EquipSlotType slot)
    {
        return equipped.TryGetValue(slot, out var item) ? item : null;
    }

    /// <summary>
    /// Equipaggia un nuovo oggetto nel suo slot. Ritorna l'oggetto
    /// precedentemente equipaggiato (null se lo slot era vuoto), cosi
    /// puoi rimetterlo nell'inventario a 8 slot.
    /// </summary>
    public EquipmentData Equip(EquipmentData newItem)
    {
        if (newItem == null) return null;

        EquipmentData previous = GetEquipped(newItem.equipSlot);
        equipped[newItem.equipSlot] = newItem;

        OnEquipmentChanged?.Invoke(newItem.equipSlot);
        
        player.UpdateStatsEquip(newItem);
        return previous;
    }

    /// <summary>Toglie l'oggetto equipaggiato in quello slot (torna nell'inventario a parte).</summary>
    public EquipmentData Unequip(EquipSlotType slot)
    {
        EquipmentData previous = GetEquipped(slot);
        equipped.Remove(slot);

        OnEquipmentChanged?.Invoke(slot);
        return previous;
    }
}
