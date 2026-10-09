using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Player : MonoBehaviour
{
    //MicroStats
    public int Healt,MaxHealt, MinDamage,MaxDamage,Critico,Difesa,Abilita,Oneshot,Rapidita;
    public Image MaxHealtBar,CurrentHealtBar;
    public  string HeroName;
    float maxBar = 100f;
    float maxBarExp = 100f;
    public int baseDamageMin, baseDamageMax;
    public int baseDefense;
    public int baseHealt;
    public int baseAbilita;
    public int baseSpeed;
    [SerializeField] TextMeshProUGUI ForzaText;
    [SerializeField] TextMeshProUGUI VitalitaText;
    [SerializeField] TextMeshProUGUI VelocitaText;
    [SerializeField] TextMeshProUGUI CriticoText;
    [SerializeField] TextMeshProUGUI OneShotText;
    [SerializeField] TextMeshProUGUI MenteText;
    public TextMeshProUGUI HeroNameText,MaxHealtText,CurrentHealtText,DamageMinText,DamageMaxText,AbilitaText,RapiditaText,DifesaText;
    //Macro stats
    public int forza, vitalita, velocita, mente;
    TextMessage textMessage;
    public bool isDie;
    GameManager gameManager;
    public Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        Inizializza();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Inizializza()
    {
        
        ForzaText = GameObject.FindGameObjectWithTag("ForzaText").GetComponent<TextMeshProUGUI>();
        VitalitaText = GameObject.FindGameObjectWithTag("VitalitaText").GetComponent<TextMeshProUGUI>();
        VelocitaText = GameObject.FindGameObjectWithTag("VelocitaText").GetComponent<TextMeshProUGUI>();
        CriticoText= GameObject.FindGameObjectWithTag("CriticoText").GetComponent<TextMeshProUGUI>();
        OneShotText= GameObject.FindGameObjectWithTag("OneShotText").GetComponent<TextMeshProUGUI>();
        MenteText= GameObject.FindGameObjectWithTag("MenteText").GetComponent<TextMeshProUGUI>();
        HeroNameText= GameObject.FindGameObjectWithTag("Hero Name").GetComponent<TextMeshProUGUI>();
        MaxHealtText= GameObject.FindGameObjectWithTag("MaxHealt").GetComponent<TextMeshProUGUI>();
        CurrentHealtText = GameObject.FindGameObjectWithTag("CurrentHealt").GetComponent<TextMeshProUGUI>();
       // MaxHealtBar = GameObject.FindGameObjectWithTag("MaxHealtBar").GetComponent<Image>();
        CurrentHealtBar = GameObject.FindGameObjectWithTag("CurrentHealtBar").GetComponent<Image>();
        DamageMinText= GameObject.FindGameObjectWithTag("damagemin").GetComponent<TextMeshProUGUI>();
        DamageMaxText = GameObject.FindGameObjectWithTag("damageMax").GetComponent<TextMeshProUGUI>();
        RapiditaText= GameObject.FindGameObjectWithTag("RapiditaText").GetComponent<TextMeshProUGUI>();
        DifesaText = GameObject.FindGameObjectWithTag("DifesaText").GetComponent<TextMeshProUGUI>();
        AbilitaText= GameObject.FindGameObjectWithTag("AbilitaText").GetComponent<TextMeshProUGUI>();
        //textMessage = GameObject.FindObjectOfType<TextMessage>();
        textMessage = Object.FindAnyObjectByType<TextMessage>();
        
        ForzaText.text = forza.ToString();
        VitalitaText.text =vitalita.ToString();
        VelocitaText.text = velocita.ToString();
         CriticoText.text = Critico.ToString();
        OneShotText.text = Oneshot.ToString();
        DifesaText.text = Difesa.ToString();
        MenteText.text = mente.ToString();
        HeroNameText.text = HeroName;
        MaxHealtText.text = MaxHealt.ToString();
        CurrentHealtText.text = Healt.ToString();
        DamageMaxText.text = MaxDamage.ToString();
        DamageMinText.text = MinDamage.ToString();
        RapiditaText.text = Rapidita.ToString();
        AbilitaText.text = Abilita.ToString();
        gameManager = GameObject.FindAnyObjectByType<GameManager>();
        baseDamageMin = MinDamage;
        baseDamageMax = MaxDamage;
        baseDefense = Difesa;
        baseHealt = MaxHealt;
        baseAbilita = Abilita;
        baseSpeed = Rapidita;
        // textMessage.WriteText(HeroName + " inizia la sua avventura...");
       // MaxHealtBar.fillAmount = MaxHealt;


    }

    public void LoseHealt(int healt)
    {

    }

    public void RecoverHealt(int amount)
    {
        Healt += amount;
        if (Healt > MaxHealt) Healt = MaxHealt;
        UpdateText();
    }

    public bool CheckPlayerFullLife()
    {
        if (Healt ==MaxHealt)
        {
            return true;
        }
        return false;
    }
    public bool CheckPlayerDie()
    {
        if (Healt <= 0)
        {
            return true;
        }

        return false;
    }

    public void TakeDamage(int damage) {

        int calculateDamage= CalculateDamage(damage);
        Healt -= calculateDamage;
        CurrentHealtBar.fillAmount = Healt / maxBar;
        gameManager.WriteMessage(gameManager.NumeroNemici[gameManager.SelectedEnemy].EnemyName + " infligge " + calculateDamage);
        if (Healt <= 0)
        {
            Healt = 0;
            isDie = true;
        }
        if (CurrentHealtText != null)
        {
            UpdateText();
           

        }
       
        
    }


    public void playerLevelupGrow(int damageGrow,int healtGrow,int speed,int defense)
    {
        baseDamageMin += damageGrow;
        baseDamageMax += damageGrow;
        MinDamage += damageGrow;
        MaxDamage += damageGrow;
        MaxHealt += healtGrow;
        RecoverHealt(20);
        Rapidita += speed;
        Difesa += defense;
        maxBar = MaxHealt;
        UpdateText();
        
        
    }
    
    public void UpdateStatsEquip(EquipmentData newEquip)
    {
        this.MaxDamage = baseDamageMax + newEquip.damage;
        this.MinDamage = baseDamageMin + newEquip.damage;
        this.Difesa += newEquip.defense;
        this.Oneshot += newEquip.OneShot;
        this.forza += newEquip.Forza;
        this.vitalita += newEquip.Vitalità;
        this.velocita += newEquip.Velocità;
        this.mente += newEquip.Mente;
        this.Critico += newEquip.critico;
        this.Rapidita += newEquip.Rapidità;
        this.MaxHealt += newEquip.maxHealt;
        UpdateText();
    }
    
    public void UpdateText()
    {
        ForzaText.text = forza.ToString();
        VitalitaText.text = vitalita.ToString();
        MenteText.text = mente.ToString();
        VelocitaText.text = velocita.ToString();
        CurrentHealtText.text = Healt.ToString();
        MaxHealtText.text = MaxHealt.ToString();
        DamageMinText.text = MinDamage.ToString();
        DamageMaxText.text = MaxDamage.ToString();
        RapiditaText.text = Rapidita.ToString();
        DifesaText.text = Difesa.ToString();
        maxBar = MaxHealt;
        CurrentHealtBar.fillAmount = Healt / maxBar;
      
        
        AbilitaText.text = Abilita.ToString();
        OneShotText.text = Oneshot.ToString();
        CriticoText.text = Critico.ToString();


    }

    int CalculateDamage(int damage)
    {

        float dannoFinale = damage * (100f / (100f + this.Difesa * 2f));
        dannoFinale -= this.Difesa * 0.25f;

        return Mathf.Max(1, Mathf.RoundToInt(dannoFinale));

    }

   
   

    
}
   

