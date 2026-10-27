using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Finestra di conferma equipaggiamento: mostra il nuovo oggetto e lo
/// confronta con quello attualmente equipaggiato nello stesso slot.
/// Mettilo su un pannello del Canvas, inizialmente disattivato (SetActive(false)).
/// </summary>
public class EquipConfirmPopupUI : MonoBehaviour
{
    [Header("Riferimenti")]
    [SerializeField] private GameObject panelRoot;       // il pannello da mostrare/nascondere
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI statsComparisonText;    // qui scriviamo il confronto riga per riga
    [SerializeField] private Image RarityImage;
    [SerializeField] private TextMeshProUGUI rarityText;
    [SerializeField] private Button equipButton;
    [SerializeField] private Button cancelButton;

    [Header("Colori confronto")]
    [SerializeField] private Color betterColor = new Color(0.3f, 0.8f, 0.3f); // verde
    [SerializeField] private Color worseColor = new Color(0.8f, 0.3f, 0.3f);  // rosso
    [SerializeField] private Color neutralColor = Color.white;

    private EquipmentManager equipmentManager;
    private InventoryManager inventoryManager; // per rimettere il vecchio oggetto nell'inventario
    private EquipmentData pendingItem;
    [SerializeField] GameManager gameManager;

    private void Awake()
    {
        equipButton.onClick.AddListener(ConfirmEquip);
        cancelButton.onClick.AddListener(Close);
        if (panelRoot != null) panelRoot.SetActive(false);
    }

    /// <summary>
    /// Da chiamare quando droppa un oggetto equipaggiabile e vuoi chiedere
    /// al giocatore se equipaggiarlo subito. Es: al termine della battaglia.
    /// </summary>
    public void Show(EquipmentData newItem, EquipmentManager equipmentManager, InventoryManager inventoryManager)
    {
        this.pendingItem = newItem;
        this.equipmentManager = equipmentManager;
        this.inventoryManager = inventoryManager;

        EquipmentData current = equipmentManager.GetEquipped(newItem.equipSlot);

        // Info oggetto
        itemNameText.text = newItem.itemName;
        descriptionText.text = newItem.description;
        iconImage.sprite = newItem.icon;
        iconImage.enabled = newItem.icon != null;
        CalculateRarity(newItem.rarità);

        // Confronto statistiche
        statsComparisonText.text = BuildComparisonText(newItem, current);

        panelRoot.SetActive(true);
    }

    public void CalculateRarity(Rarity rarità)
    {
        switch (rarità)
        {
            case Rarity.Indecente:
                RarityImage.color = Color.black;
                rarityText.text = "Indecente";
                break;
            case Rarity.Scarso:
                RarityImage.color = Color.grey;
                rarityText.text = "Scarso";
                break;

            case Rarity.Normale:
                RarityImage.color = Color.floralWhite;
                rarityText.text = "Normale";
                break;

            case Rarity.Super:

                RarityImage.color = Color.blue;
                rarityText.text = "Super";
                break;
            case Rarity.Raro:
                RarityImage.color = Color.red;
                rarityText.text = "Raro";
                break;
            case Rarity.Ricercato:
                RarityImage.color = Color.green;
                rarityText.text = "Ricercato";
                break;
            case Rarity.Epico:
                RarityImage.color = Color.purple;
                rarityText.text = "Epico";
                break;

            case Rarity.Leggendario:
                RarityImage.color = Color.gold;
                rarityText.text = "Leggendario";
                break;
        }
    }

    private string BuildComparisonText(EquipmentData newItem, EquipmentData current)
    {
        // Raccogliamo tutti i nomi di statistiche presenti sull'uno e/o sull'altro oggetto
        var statNames = new List<string>();
        foreach (var s in newItem.stats)
            if (!statNames.Contains(s.statName)) statNames.Add(s.statName);
        if (current != null)
            foreach (var s in current.stats)
                if (!statNames.Contains(s.statName)) statNames.Add(s.statName);

        if (statNames.Count == 0)
            return "Nessuna statistica";

        var sb = new System.Text.StringBuilder();

        foreach (var statName in statNames)
        {
            float newValue = newItem.GetStat(statName);
            float currentValue = current != null ? current.GetStat(statName) : 0f;
            float diff = newValue - currentValue;

            Color lineColor = neutralColor;
            string diffText = "";

            if (current != null) // solo se c'e' qualcosa da confrontare
            {
                if (diff > 0) { lineColor = betterColor; diffText = $" (+{diff:0.#})"; }
                else if (diff < 0) { lineColor = worseColor; diffText = $" ({diff:0.#})"; }
            }

            string hexColor = ColorUtility.ToHtmlStringRGB(lineColor);
            sb.AppendLine($"{statName}: {currentValue:0.#} -> <color=#{hexColor}>{newValue:0.#}{diffText}</color>");
        }

        return sb.ToString();
    }

    private void ConfirmEquip()
    {
        if (pendingItem == null) return;

        EquipmentData previous = equipmentManager.Equip(pendingItem);

        // Se c'era gia' qualcosa equipaggiato in quello slot, torna nell'inventario a 8 sloti
        if (pendingItem.equipSlot==EquipSlotType.Skill)
        {
            gameManager.SetSkill(pendingItem);
        }
        if (previous != null && inventoryManager != null)
        {
            inventoryManager.AddItem(previous, 1);
        }

        Close();
    }

    private void Close()
    {
        pendingItem = null;
        panelRoot.SetActive(false);
    }
}
