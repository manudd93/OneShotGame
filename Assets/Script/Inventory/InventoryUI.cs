using UnityEngine;

/// <summary>
/// Collega gli 8 GameObject slot già presenti nella tua UI all'InventoryManager.
/// Mettilo su un GameObject qualsiasi del Canvas (es. il pannello "InventoryPanel").
/// </summary>
public class InventoryUI : MonoBehaviour
{
    [Header("Riferimenti")]
    [SerializeField] private InventoryManager inventoryManager;

    [Header("I tuoi 8 slot già creati nella UI (trascinali qui in ordine)")]
    [SerializeField] private InventorySlotUI[] slotsUI; // deve avere 8 elementi

    private void OnEnable()
    {
        if (inventoryManager == null)
        {
            Debug.LogError("InventoryUI: manca il riferimento a InventoryManager!");
            return;
        }

        // Collega ogni slot UI al suo indice + al click handler
        for (int i = 0; i < slotsUI.Length; i++)
        {
            slotsUI[i].Init(i, OnSlotClicked);
        }

        // Aggiorna la UI ogni volta che l'inventario cambia
        inventoryManager.OnInventoryChanged += RefreshAllSlots;
        inventoryManager.OnSlotChanged += RefreshSlot;

        RefreshAllSlots(); // stato iniziale
    }

    private void OnDisable()
    {
        if (inventoryManager == null) return;

        inventoryManager.OnInventoryChanged -= RefreshAllSlots;
        inventoryManager.OnSlotChanged -= RefreshSlot;
    }

    private void OnSlotClicked(int index)
    {
        // Cliccando sullo slot, "usiamo" l'oggetto (vedi ItemData.Use / consumeOnUse)
        inventoryManager.UseItem(index);
    }

    private void RefreshAllSlots()
    {
        for (int i = 0; i < slotsUI.Length; i++)
            RefreshSlot(i);
    }

    private void RefreshSlot(int index)
    {
        if (index < 0 || index >= slotsUI.Length) return;

        var slot = inventoryManager.GetSlot(index);
        slotsUI[index].SetData(slot?.item, slot?.quantity ?? 0);
    }
}
