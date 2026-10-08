using UnityEngine;

/// <summary>
/// Collega i 3 slot di equipaggiamento (Arma, Armatura, Accessorio) già
/// presenti nella tua UI all'EquipmentManager. Riusa lo stesso componente
/// InventorySlotUI che hai gia' messo sui GameObject slot.
/// Mettilo su un GameObject del Canvas (es. "EquipmentPanel").
/// </summary>
public class EquipmentUI : MonoBehaviour
{
    [Header("Riferimenti")]
    [SerializeField] private EquipmentManager equipmentManager;

    [Header("I 3 slot gia' creati nella UI")]
    [SerializeField] private InventorySlotUI weaponSlotUI;
    [SerializeField] private InventorySlotUI armorSlotUI;
    [SerializeField] private InventorySlotUI accessorySlotUI;

    private void OnEnable()
    {
        if (equipmentManager == null)
        {
            Debug.LogError("EquipmentUI: manca il riferimento a EquipmentManager!");
            return;
        }

        // Init con indice fittizio (0): qui non ci serve un indice numerico,
        // ma il click ci deve dire QUALE slot (Weapon/Armor/Accessory) e' stato premuto.
        weaponSlotUI.Init(0, _ => OnSlotClicked(EquipSlotType.Weapon));
        armorSlotUI.Init(0, _ => OnSlotClicked(EquipSlotType.Armor));
        accessorySlotUI.Init(0, _ => OnSlotClicked(EquipSlotType.Accessory));

        equipmentManager.OnEquipmentChanged += RefreshSlot;

        RefreshAll(); // stato iniziale
    }

    private void OnDisable()
    {
        if (equipmentManager == null) return;
        equipmentManager.OnEquipmentChanged -= RefreshSlot;
    }

    private void RefreshAll()
    {
        RefreshSlot(EquipSlotType.Weapon);
        RefreshSlot(EquipSlotType.Armor);
        RefreshSlot(EquipSlotType.Accessory);
    }

    private void RefreshSlot(EquipSlotType slot)
    {
        EquipmentData item = equipmentManager.GetEquipped(slot);
        // quantita' sempre 1/0: un equip o c'e' o non c'e', non si stacka
        int quantity = item != null ? 1 : 0;

        switch (slot)
        {
            case EquipSlotType.Weapon: weaponSlotUI.SetData(item, quantity); break;
            case EquipSlotType.Armor: armorSlotUI.SetData(item, quantity); break;
            case EquipSlotType.Accessory: accessorySlotUI.SetData(item, quantity); break;
        }
    }

    private void OnSlotClicked(EquipSlotType slot)
    {
        // Esempio: cliccando su uno slot equipaggiato, lo si toglie.
        // Personalizza questa logica come preferisci (es. apri un dettaglio invece di rimuovere).
        EquipmentData current = equipmentManager.GetEquipped(slot);
        if (current == null) return;

        Debug.Log($"Slot {slot} cliccato: {current.itemName}");
        // equipmentManager.Unequip(slot); // scommenta se vuoi che il click rimuova l'oggetto
    }
}
