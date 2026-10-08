using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextMessage : MonoBehaviour
{
    public float pause = 0.1f;
     string message;
    public TextMeshProUGUI DisplayMessage;
    AudioSource audioSource;
    public AudioClip textSound;
    void Start()
    {
        DisplayMessage= GameObject.FindGameObjectWithTag("EventText").GetComponent<TextMeshProUGUI>();
        message = DisplayMessage.text;
        DisplayMessage.text = ""; // Clear the GUI text
        audioSource = GetComponent<AudioSource>();


    }

    public void ClearText()
    {
        message = "";
        DisplayMessage.text = "";
    }
    public  void WriteText(string Text)
    {
        message = Text;
        StartCoroutine(TypeLetters());
    }
    IEnumerator TypeLetters()
    {
        // Iterate over each letter
        foreach (char letter in message.ToCharArray())
        {
            DisplayMessage.text += letter; // Add a single character to the GUI text
            audioSource.PlayOneShot(textSound);

            yield return new WaitForSeconds(pause);
        }
    }
    // Start is called before the first frame update
   
}
