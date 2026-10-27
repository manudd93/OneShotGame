
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [Serializable]
    public class RoomInfo
    {
        [Tooltip("Nome univoco, ad esempio Main, Shop o Dungeon")]
        public string id;

        [Tooltip("Oggetto radice dell'ambiente")]
        public GameObject roomRoot;

        [Tooltip("Telecamera usata in questa stanza")]
        public Camera roomCamera;

        [Tooltip("Punto in cui posizionare il giocatore entrando")]
        public Transform playerSpawnPoint;
    }

    [Header("Stanze disponibili")]
    [SerializeField] private List<RoomInfo> rooms = new();

    [Header("Stanza iniziale")]
    [SerializeField] private string startingRoomId = "Main";

    [Header("Giocatore (opzionale)")]
    [SerializeField] private Transform player;

    [Header("Transizione")]
    [SerializeField] private CanvasGroup transitionPanel;
    [SerializeField, Min(0f)] private float fadeDuration = 0.4f;
    [SerializeField, Min(0f)] private float blackDuration = 0.1f;

    private RoomInfo currentRoom;
    private bool isTransitioning;

    private void Start()
    {
        if (transitionPanel == null)
        {
            Debug.LogError(
                "RoomManager: assegna il CanvasGroup della transizione."
            );
            enabled = false;
            return;
        }

        transitionPanel.alpha = 0f;
        transitionPanel.interactable = false;
        transitionPanel.blocksRaycasts = false;

        RoomInfo startingRoom = FindRoom(startingRoomId);

        if (startingRoom == null)
        {
            Debug.LogError(
                $"Stanza iniziale '{startingRoomId}' non trovata."
            );
            enabled = false;
            return;
        }

        ActivateRoom(startingRoom, false);
    }

    // Richiamabile dai pulsanti UI o da altri script.
    public void ChangeRoom(string roomId)
    {
        if (isTransitioning)
            return;

        RoomInfo targetRoom = FindRoom(roomId);

        if (targetRoom == null)
        {
            Debug.LogWarning($"Stanza '{roomId}' non trovata.");
            return;
        }

        if (targetRoom == currentRoom)
            return;

        if (targetRoom.roomRoot == null ||
            targetRoom.roomCamera == null)
        {
            Debug.LogError(
                $"La stanza '{roomId}' non ha Root o Camera assegnati."
            );
            return;
        }

        StartCoroutine(ChangeRoomRoutine(targetRoom));
    }

    private IEnumerator ChangeRoomRoutine(RoomInfo targetRoom)
    {
        isTransitioning = true;

        // 1. Oscura lo schermo.
        yield return Fade(0f, 1f);

        // 2. Cambia stanza mentre lo schermo è nero.
        ActivateRoom(targetRoom, true);

        // 3. Mantiene il nero per un breve istante.
        yield return new WaitForSecondsRealtime(blackDuration);

        // 4. Mostra la nuova stanza.
        yield return Fade(1f, 0f);

        isTransitioning = false;
    }

    private void ActivateRoom(RoomInfo targetRoom, bool movePlayer)
    {
        // Attiva la destinazione prima di spegnere la stanza precedente.
        // Questo evita di disattivare il Manager durante il cambio.
        targetRoom.roomRoot.SetActive(true);

        // Posiziona il giocatore nel punto di ingresso, se assegnato.
        if (movePlayer &&
            player != null &&
            targetRoom.playerSpawnPoint != null)
        {
            player.SetPositionAndRotation(
                targetRoom.playerSpawnPoint.position,
                targetRoom.playerSpawnPoint.rotation
            );
        }

        // Disattiva le altre stanze e le relative telecamere.
        foreach (RoomInfo room in rooms)
        {
            if (room == null)
                continue;

            bool isTarget = room == targetRoom;

            if (room.roomCamera != null)
                room.roomCamera.enabled = isTarget;

            if (room.roomRoot != null && !isTarget)
                room.roomRoot.SetActive(false);
        }

        // Garantisce che la telecamera di destinazione sia attiva.
        targetRoom.roomCamera.enabled = true;
        currentRoom = targetRoom;
    }

    private IEnumerator Fade(float from, float to)
    {
        transitionPanel.blocksRaycasts = true;

        if (fadeDuration <= 0f)
        {
            transitionPanel.alpha = to;
        }
        else
        {
            float elapsed = 0f;

            while (elapsed < fadeDuration)
            {
                elapsed += Time.unscaledDeltaTime;

                transitionPanel.alpha = Mathf.Lerp(
                    from,
                    to,
                    Mathf.Clamp01(elapsed / fadeDuration)
                );

                yield return null;
            }

            transitionPanel.alpha = to;
        }

        if (to <= 0f)
        {
            transitionPanel.blocksRaycasts = false;
        }
    }

    private RoomInfo FindRoom(string roomId)
    {
        return rooms.Find(
            room => room != null &&
                    room.id == roomId
        );
    }
}

