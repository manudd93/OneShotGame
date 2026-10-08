using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Metti questo script su OGNI GameObject slot che hai già creato nella UI.
/// Nell'Inspector collega: l'Image dell'icona (obbligatoria), il Text/TMP
/// della quantità (opzionale) e il Button dello slot (di solito è sullo
/// stesso GameObject, quindi si prende automaticamente con GetComponent).
/// </summary>
[RequireComponent(typeof(Button))]
public class InventorySlotUI : MonoBehaviour
{
    [Header("Riferimenti UI (trascina qui i figli dello slot)")]
    [SerializeField] private Image iconImage;
    [SerializeField] private GameObject quantityLabelRoot; // l'oggetto che contiene il testo, per nasconderlo se vuoto
    [SerializeField] private Text quantityText;             // usa questo SE usi Text classico
    // [SerializeField] private TMPro.TextMeshProUGUI quantityTextTMP; // scommenta se usi TextMeshPro

    private Button button;
    private int slotIndex;
    private System.Action<int> onClickCallback;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(HandleClick);
    }

    /// <summary>
    /// Chiamato una volta dall'InventoryUI per dire a questo slot "sei lo slot numero X"
    /// e cosa fare quando viene cliccato.
    /// </summary>
    public void Init(int index, System.Action<int> onClick)
    {
        slotIndex = index;
        onClickCallback = onClick;
    }

    /// <summary>Aggiorna cosa mostra lo slot (icona + quantità).</summary>
    public void SetData(ItemMaking item, int quantity)
    {
        bool isEmpty = item == null || quantity <= 0;

        iconImage.enabled = !isEmpty;
        iconImage.sprite = isEmpty ? null : item.icon;

        if (quantityLabelRoot != null)
            quantityLabelRoot.SetActive(!isEmpty && quantity > 1);

        if (quantityText != null)
            quantityText.text = quantity > 1 ? quantity.ToString() : string.Empty;
    }

    private void HandleClick()
    {
        onClickCallback?.Invoke(slotIndex);
    }
}
