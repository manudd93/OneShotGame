using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class TextMessage : MonoBehaviour
{
    public float pause = 0.1f;
    public float fastPause = 0.01f;       // velocità quando il tasto è premuto
    public KeyCode speedUpKey = KeyCode.Space;
    public TextMeshProUGUI DisplayMessage;
    public AudioClip textSound;

    private AudioSource audioSource;
    private Coroutine typingCoroutine;
    [SerializeField] GameManager gameManager;
    void Start()
    {
        if (DisplayMessage == null)
            DisplayMessage = GameObject.FindGameObjectWithTag("EventText").GetComponent<TextMeshProUGUI>();

        DisplayMessage.text = "";
        audioSource = GetComponent<AudioSource>();
    }

    public void ClearText()
    {
        StopTyping();
        DisplayMessage.text = "";
    }

    public void WriteText(string text)
    {
        StopTyping();
        DisplayMessage.text = text;
        DisplayMessage.maxVisibleCharacters = 0;
        typingCoroutine = StartCoroutine(TypeLetters(text.Length));
    }

    private void StopTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
    }

    IEnumerator TypeLetters(int length)
    {
        for (int i = 1; i <= length; i++)
        {
            DisplayMessage.maxVisibleCharacters = i;

            bool fast = gameManager.GetSpeedGame();

            // con la velocità alta il suono ogni 3 lettere, per evitare il "gracchiare"
            if (!fast || i % 3 == 0)
                audioSource.PlayOneShot(textSound);

            yield return new WaitForSeconds(fast ? fastPause : pause);
        }

        typingCoroutine = null;
    }
}