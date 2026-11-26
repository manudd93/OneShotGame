using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GenerateNPC : MonoBehaviour
{
    public int ID;
    public string Nome;
    public bool MaleGender;
    public bool hasCorrupt;
    public bool takeItem;
    public int ItemId;
    public GameObject SfxNpc;
    public string Message;
    public List<GameObject> MaleSprite = new List<GameObject>();
    public List<GameObject> FemaleSprite = new List<GameObject>();
    public List<string> NormalMessage = new List<string>();
    public List<string> ItemMessage = new List<string>();
    public List<string> CorruptMessage = new List<string>();
    [SerializeField] TextMessage textMessage;
    [SerializeField] GameManager gameManager;



    public void ExecuteNpc()
    {
        textMessage.WriteText(Message);
        gameManager.DoneNPC();
        
    }
    public void Generate()
    {
        Nome = GetName();
        HadItem();
        hasCorrupt = HasCorrupt();
        LoadSprite();
        LoadMessage();
       
    }

    void HadItem()
    {
        int random = UnityEngine.Random.Range(0, 100);
        if (random < 10)
        {
            takeItem = true;
        }
        else
        {
            takeItem = false;
        }
    }
    void LoadMessage()
    {
        if (hasCorrupt)
        {
            int random= UnityEngine.Random.Range(0, CorruptMessage.Count);
            Message = Nome + ":" + CorruptMessage[random];
            return;
        }
        if (takeItem)
        {
            int random = UnityEngine.Random.Range(0, ItemMessage.Count);
            Message = Nome + ":" + ItemMessage[random];
            return;
        }
        if(hasCorrupt==false && takeItem == false)
        {
            int random = UnityEngine.Random.Range(0, NormalMessage.Count);
            Message = Nome+":"+ NormalMessage[random];
        }
        
    }
    void LoadSprite()
    {
        if (MaleGender)
        {
            int Random= UnityEngine.Random.Range(0, MaleSprite.Count);
            SfxNpc = MaleSprite[Random];
        }
        else
        {
            int Random = UnityEngine.Random.Range(0, FemaleSprite.Count);
            SfxNpc = FemaleSprite[Random];
        }
    }
    bool HasCorrupt()
    {

        int random= UnityEngine.Random.Range(0, 100);
        if (random < 10)
        {
            takeItem = false;
            return true;
        }
        if (random > 10)
        {
            return false;
        }

        return false;
    }

    string GetName()
    {
        int RandomGender = UnityEngine.Random.Range(0,100);
        Debug.Log(RandomGender);
        string[] Malename = { "Francesco", "Federico ", "Nicola ", "Leonardo ", "Marco ", "Emanuele", "Giuseppe", "Lorenzo ", "Luca ", "Andrea ", "Pietro ", "Simone ", "Sergio ", "Tommaso", "Davide ", "Giulio", "Gabriele ", "Alessandro", "Stefano ", "Roberto" };
        string[] FemaleName = { "Anna", "Aurora", "Margherita", "Sofia", "Emma", "Sara", "Chiara", "Ludovica", "Beatrice", "Giulia", "Alice", "Camilla", "Elena", "Elisa", "Martina", "Ginevra", "Laura", "Gigliola", "Vittoria", "Valentina" };
        if (RandomGender <= 50)
        {
            int RandomNameMale = UnityEngine.Random.Range(0, Malename.Length);
            MaleGender = true;
            return Malename[RandomNameMale];
          
        }
        if (RandomGender > 50)
        {
            int RandomFemale= UnityEngine.Random.Range(0, FemaleName.Length);
            MaleGender = false;
            return FemaleName[RandomFemale];
        }

        return "";
    }
}
