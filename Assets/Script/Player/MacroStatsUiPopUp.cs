using UnityEngine;
using TMPro;
public class MacroStatsUiPopUp : MonoBehaviour
{
    public int MaxPoint;
    public int currPoint;
    int CurrPointForza;
    int currPointVitalità;
    int currPointMente;
    int currPointVelocità;
    [SerializeField] PlayerMacroStats playerMacroStats;
    [SerializeField] TextMeshProUGUI ForzaPointText;
    [SerializeField] TextMeshProUGUI VitalitaPointText;
    [SerializeField] TextMeshProUGUI MentePointText;
    [SerializeField] TextMeshProUGUI VelocitaPointText;
    [SerializeField] TextMeshProUGUI CurrPointText;
    
    public void SetStats(int NewPoint)
    {
        MaxPoint = NewPoint;
    }
    private void OnEnable()
    {
        UpdateText();
        currPoint = MaxPoint;
        
    }
    void UpdateText()
    {
        CurrPointText.text = MaxPoint.ToString();
    }
     public void AddPoint(int index)
    {
        switch (index)
        {
            
        

        
            case 1:
                if (MaxPoint > 0)
                {
                    
                    MaxPoint--;
                    CurrPointForza += 1;
                    ForzaPointText.text = CurrPointForza.ToString();
                    UpdateText();
                }
                break;
            case 2:
                if (MaxPoint > 0)
                {

                    MaxPoint--;
                    currPointVitalità += 1;
                    VitalitaPointText.text = currPointVitalità.ToString();
                    UpdateText();
                }
                break;
            case 3:
                if (MaxPoint > 0)
                {

                    MaxPoint--;
                    currPointMente += 1;
                    MentePointText.text = currPointMente.ToString();
                    UpdateText();
                }
                break;
            case 4:
                if (MaxPoint > 0)
                {

                    MaxPoint--;
                    currPointVelocità += 1;
                    VelocitaPointText.text = currPointVelocità.ToString();
                    UpdateText();
                }
                break;
        }
    }

    public void RemovePoint(int index)
    {
        switch (index)
        {


            case 1:
                if (MaxPoint < currPoint)
                {
                    if (CurrPointForza <=0)
                    {
                        
                        CurrPointForza = 0;
                        return;
                    }
                    else
                    {
                        MaxPoint++;

                        CurrPointForza -= 1;
                        ForzaPointText.text = CurrPointForza.ToString();
                        UpdateText();
                    }
                   
                }
                break;
            case 2:
                if (MaxPoint < currPoint)
                {
                    if (currPointVitalità <= 0)
                    {

                        currPointVitalità = 0;
                        return;
                    }
                    else
                    {
                        MaxPoint++;

                        currPointVitalità -= 1;
                        VitalitaPointText.text = currPointVitalità.ToString();
                        UpdateText();
                    }
                }
                break;
            case 3:
                if (MaxPoint < currPoint)
                {

                    if (currPointMente <= 0)
                    {

                        currPointMente = 0;
                        return;
                    }
                    else
                    {
                        MaxPoint++;
                        currPointMente -= 1;
                        MentePointText.text = currPointMente.ToString();
                        UpdateText();
                    }
                   
                }
                break;
            case 4:
                if (MaxPoint < currPoint)
                {
                    if (currPointVelocità <= 0)
                    {

                        currPointVelocità = 0;
                        return;
                    }
                    MaxPoint++;

                    currPointVelocità -= 1;
                    VelocitaPointText.text = currPointVelocità.ToString();
                    UpdateText();
                }
                break;
        }

        
    }
    public void ConfirmStats()
    {
        if (MaxPoint > 0)
        {
            Debug.Log("utilizza i tuoi punti");
            return;
        }
        playerMacroStats.SetNewStats(this.CurrPointForza, this.currPointVitalità, this.currPointMente, this.currPointVelocità);
        this.gameObject.SetActive(false);
        MaxPoint = 0;
        CurrPointForza = 0;
        currPointVitalità = 0;
        currPointMente = 0;
        currPointVelocità = 0;
        
    }
}
