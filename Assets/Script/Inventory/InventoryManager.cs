using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rappresenta un singolo slot dell'inventario: un oggetto + quantità.
/// </summary>
[System.Serializable]
public class InventorySlot
{
    public ItemMaking item;
    public int quantity;

    public bool IsEmpty => item == null || quantity <= 0;

    public void Clear()
    {
        item = null;
        quantity = 0;
    }
}

/// <summary>
/// Gestisce un inventario fisso a 8 slot: aggiunta, rimozione,
/// scambio (swap) e sostituzione di oggetti.
/// Attaccalo a un GameObject persistente (es. il Player o un Manager).
/// </summary>
public class InventoryManager : MonoBehaviour
{
    [Header("Configurazione")]
    [SerializeField] private int slotCount = 8;

    [Header("Slot (visibili in Inspector per debug)")]
    [SerializeField] private InventorySlot[] slots;

    // Eventi utili per aggiornare la UI quando l'inventario cambia
    public event Action OnInventoryChanged;
    public event Action<int> OnSlotChanged; // passa l'indice dello slot modificato
   [SerializeField] GameManager gameManager;
    

    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
        slots = new InventorySlot[slotCount];
        for (int i = 0; i < slotCount; i++)
            slots[i] = new InventorySlot();
    }

    // ---------------------------------------------------------
    //  AGGIUNTA OGGETTI
    // ---------------------------------------------------------

    /// <summary>
    /// Prova ad aggiungere una quantità di un oggetto.
    /// Riempie prima gli stack esistenti, poi usa slot vuoti.
    /// Ritorna la quantità NON aggiunta (0 = tutto aggiunto con successo).
    /// </summary>
    public int AddItem(ItemMaking item, int amount = 1)
    {
        if (item == null || amount <= 0) return amount;

        // 1) Se stackabile, prova a riempire slot che contengono già l'oggetto
        if (item.isStackable)
        {
            for (int i = 0; i < slots.Length && amount > 0; i++)
            {
                if (slots[i].item == item && slots[i].quantity < item.maxStackSize)
                {
                    int spaceLeft = item.maxStackSize - slots[i].quantity;
                    int toAdd = Mathf.Min(spaceLeft, amount);

                    slots[i].quantity += toAdd;
                    amount -= toAdd;

                    OnSlotChanged?.Invoke(i);
                }
            }
        }

        // 2) Riempi slot vuoti con quello che resta
        for (int i = 0; i < slots.Length && amount > 0; i++)
        {
            if (slots[i].IsEmpty)
            {
                int toAdd = item.isStackable ? Mathf.Min(item.maxStackSize, amount) : 1;

                slots[i].item = item;
                slots[i].quantity = toAdd;
                amount -= toAdd;

                OnSlotChanged?.Invoke(i);

                // Se l'oggetto non è stackabile, ogni unità occupa uno slot a parte:
                // il ciclo continuerà finché ci sono slot vuoti e amount > 0
            }
        }

        OnInventoryChanged?.Invoke();
        return amount; // >0 se l'inventario era pieno e non tutto è entrato
    }

    /// <summary>
    /// Aggiunge un oggetto in uno SLOT SPECIFICO (invece di cercarlo automaticamente).
    /// Utile per hotbar, drag&drop, o quando vuoi decidere tu la posizione.
    /// Ritorna la quantità NON aggiunta (0 = tutto aggiunto con successo).
    /// </summary>
    public int AddItemToSlot(int slotIndex, ItemMaking item, int amount = 1)
    {
        if (!IsValidIndex(slotIndex) || item == null || amount <= 0) return amount;

        var slot = slots[slotIndex];

        // Slot vuoto: ci mettiamo l'oggetto direttamente
        if (slot.IsEmpty)
        {
            int toAdd = item.isStackable ? Mathf.Min(item.maxStackSize, amount) : 1;
            slot.item = item;
            slot.quantity = toAdd;
            amount -= toAdd;
        }
        // Slot con lo stesso oggetto stackabile: aggiungiamo allo stack
        else if (slot.item == item && item.isStackable && slot.quantity < item.maxStackSize)
        {
            int spaceLeft = item.maxStackSize - slot.quantity;
            int toAdd = Mathf.Min(spaceLeft, amount);
            slot.quantity += toAdd;
            amount -= toAdd;
        }
        // Slot occupato da un oggetto diverso (o pieno): non aggiungiamo nulla qui
        else
        {
            return amount; // il chiamante può decidere di fare uno swap, ad esempio
        }

        OnSlotChanged?.Invoke(slotIndex);
        OnInventoryChanged?.Invoke();
        return amount;
    }

    // ---------------------------------------------------------
    //  RIMOZIONE OGGETTI
    // ---------------------------------------------------------

    /// <summary>
    /// Rimuove una quantità di un oggetto specifico, cercandolo in tutti gli slot.
    /// Ritorna true se è riuscito a rimuoverne la quantità richiesta.
    /// </summary>
    public bool RemoveItem(ItemMaking item, int amount = 1)
    {
        if (item == null || amount <= 0) return false;

        // Verifica di avere abbastanza oggetti prima di modificare qualcosa
        if (GetTotalQuantity(item) < amount) return false;

        for (int i = 0; i < slots.Length && amount > 0; i++)
        {
            if (slots[i].item == item)
            {
                int toRemove = Mathf.Min(slots[i].quantity, amount);
                slots[i].quantity -= toRemove;
                amount -= toRemove;

                if (slots[i].quantity <= 0)
                    slots[i].Clear();

                OnSlotChanged?.Invoke(i);
            }
        }

        OnInventoryChanged?.Invoke();
        return true;
    }

    /// <summary>Svuota completamente uno slot specifico dato il suo indice.</summary>
    public void RemoveSlot(int slotIndex)
    {
        if (!IsValidIndex(slotIndex)) return;

        slots[slotIndex].Clear();
        OnSlotChanged?.Invoke(slotIndex);
        OnInventoryChanged?.Invoke();
    }

    // ---------------------------------------------------------
    //  SCAMBIO / SOSTITUZIONE
    // ---------------------------------------------------------

    /// <summary>
    /// Scambia il contenuto di due slot (utile per il drag & drop nella UI).
    /// </summary>
    public void SwapSlots(int indexA, int indexB)
    {
        if (!IsValidIndex(indexA) || !IsValidIndex(indexB) || indexA == indexB) return;

        (slots[indexA], slots[indexB]) = (slots[indexB], slots[indexA]);

        OnSlotChanged?.Invoke(indexA);
        OnSlotChanged?.Invoke(indexB);
        OnInventoryChanged?.Invoke();
    }

    /// <summary>
    /// Sostituisce direttamente il contenuto di uno slot con un nuovo oggetto,
    /// ritornando l'oggetto e la quantità che c'erano prima (utile per "droppare"
    /// il vecchio oggetto a terra quando lo si sostituisce).
    /// </summary>
    public (ItemMaking previousItem, int previousQuantity) SetSlot(int slotIndex, ItemMaking newItem, int newQuantity)
    {
        if (!IsValidIndex(slotIndex)) return (null, 0);

        var previousItem = slots[slotIndex].item;
        var previousQuantity = slots[slotIndex].quantity;

        slots[slotIndex].item = newItem;
        slots[slotIndex].quantity = newQuantity;

        OnSlotChanged?.Invoke(slotIndex);
        OnInventoryChanged?.Invoke();

        return (previousItem, previousQuantity);
    }

    // ---------------------------------------------------------
    //  USO OGGETTI (click sullo slot)
    // ---------------------------------------------------------

    /// <summary>
    /// Chiamato quando il giocatore clicca su uno slot: esegue la logica
    /// Use() dell'oggetto e, se consumeOnUse è true, ne rimuove 1.
    /// </summary>
    public void UseItem(int slotIndex)
    {
        if (!IsValidIndex(slotIndex)) return;

        var slot = slots[slotIndex];
        if (slot.IsEmpty) return;

        var item = slot.item; // salvato prima, perché RemoveItem potrebbe svuotare lo slot
                              //item.Use(gameObject); // gameObject = quello a cui è attaccato l'InventoryManager (es. il Player)
        gameManager.UseConsumable(item, slotIndex);
        // gameManager.UseConsumable(item);
        //if (item.onlyOnMap && gameManager.currentState == GameState.Idle)
        //{
        //    slot.quantity -= 1;
        //    if (slot.quantity <= 0) slot.Clear();

        //    OnSlotChanged?.Invoke(slotIndex);
        //    OnInventoryChanged?.Invoke();
        //    gameManager.UseConsumable(item);
        //    return;
        //}
        //if (item.consumeOnUse)
        //{
           
        //    slot.quantity -= 1;
        //    if (slot.quantity <= 0) slot.Clear();

        //    OnSlotChanged?.Invoke(slotIndex);
        //    OnInventoryChanged?.Invoke();
        //    gameManager.UseConsumable(item);
        //}
    }

    // ---------------------------------------------------------
    //  QUERY / UTILITY
    // ---------------------------------------------------------
    public void  UseItemConfirmed(int slotIndex)
    {
        if (!IsValidIndex(slotIndex)) return;

        var slot = slots[slotIndex];
        if (slot.IsEmpty) return;

        var item = slot.item;
        if (item.consumeOnUse)
        {

            slot.quantity -= 1;
            if (slot.quantity <= 0) slot.Clear();

            OnSlotChanged?.Invoke(slotIndex);
            OnInventoryChanged?.Invoke();
           
        }
    }
    public InventorySlot GetSlot(int index) => IsValidIndex(index) ? slots[index] : null;

    public IReadOnlyList<InventorySlot> GetAllSlots() => slots;

    public int GetTotalQuantity(ItemMaking item)
    {
        int total = 0;
        foreach (var slot in slots)
            if (slot.item == item) total += slot.quantity;
        return total;
    }

    public bool HasItem(ItemMaking item, int amount = 1) => GetTotalQuantity(item) >= amount;

    public bool IsFull()
    {
        foreach (var slot in slots)
            if (slot.IsEmpty) return false;
        return true;
    }

    public int GetFirstEmptySlotIndex()
    {
        for (int i = 0; i < slots.Length; i++)
            if (slots[i].IsEmpty) return i;
        return -1;
    }

    private bool IsValidIndex(int index) => index >= 0 && index < slots.Length;
}
