
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public int HealtGrow, DamageGrow, MoveSpeedGrow;
    public int Level;
    public int expMax;
    public int Exp;
    public int SkillPoint;
    [SerializeField] TextMeshProUGUI textLv;
    [SerializeField] TextMeshProUGUI textCurrExp;
    [SerializeField] TextMeshProUGUI textMaxexp;
    [SerializeField] TextMeshProUGUI textPointSkill;
    public GameObject LevelUpPanel;
    public Image ExpBar;
    float maxBar = 100f;
    Player player;
   [SerializeField] MacroStatsUiPopUp macroStatsUiPopUp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        textLv = GameObject.FindGameObjectWithTag("LevelHero").GetComponent<TextMeshProUGUI>();
        textCurrExp = GameObject.FindGameObjectWithTag("CurrentExpText").GetComponent<TextMeshProUGUI>();
        textMaxexp = GameObject.FindGameObjectWithTag("CurrentMaxExpText").GetComponent<TextMeshProUGUI>();
        ExpBar = GameObject.FindGameObjectWithTag("CurrentExpBar").GetComponent<Image>();
        player = GetComponent<Player>();
        maxBar = expMax;
        UpdateText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LevelUp(int experience)
    {
        Level += 1;
        
        Exp = 0;
        expMax += 25;
        ExpBar.fillAmount = 0;
        maxBar = expMax;
        player.playerLevelupGrow(DamageGrow, HealtGrow, MoveSpeedGrow, 0);
        UpdateText();
        AddSkillPoint();
        if (experience > expMax)
        {
            LevelUp(experience);
        }
        LevelUpPanel.gameObject.SetActive(true);
        macroStatsUiPopUp.SetStats(SkillPoint);
        textPointSkill.text = SkillPoint.ToString();
        
       
        
    }
    void UpdateText()
    {
        textLv.text = Level.ToString();
        textCurrExp.text = Exp.ToString();
        textMaxexp.text = expMax.ToString();
    }
    public void AddExperience(int experience)
    {
        Exp += experience;
        ExpBar.fillAmount = Exp / maxBar;
        if (Exp >= expMax)
        {
            LevelUp(experience);
           
        }
    }

    public void AddSkillPoint()
    {
        SkillPoint += 1;
    }
}
