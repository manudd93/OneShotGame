using UnityEngine;
using System.Collections;
public class SpriteChangeColor : MonoBehaviour
{

        [Header("Flash")]
        [SerializeField] private Color flashColor = Color.red;
        [SerializeField] private float flashDuration = 0.08f; // durata di ogni fase (colorato / normale)
        [SerializeField] private int flashCount = 2;

        [Header("Flash bianco pieno (opzionale)")]
        [Tooltip("Se assegnato, durante il flash lo sprite viene disegnato con questo materiale (silhouette a tinta unita)")]
        [SerializeField] private Material flashMaterial;

        private SpriteRenderer[] renderers;
        private Color[] originalColors;
        private Material[] originalMaterials;
        private Coroutine currentFlash;

        private void Awake()
        {
            // Prende lo sprite principale e anche quelli dei figli (utile se il personaggio ha più parti)
            renderers = GetComponentsInChildren<SpriteRenderer>();
            originalColors = new Color[renderers.Length];
            originalMaterials = new Material[renderers.Length];

            for (int i = 0; i < renderers.Length; i++)
            {
                originalColors[i] = renderers[i].color;
                originalMaterials[i] = renderers[i].sharedMaterial;
            }
        }

        // Flash con le impostazioni dell'Inspector
        public void Flash()
        {
            Flash(flashColor);
        }

        // Flash con un colore specifico (es. giallo per il critico)
        public void Flash(Color color)
        {
            if (currentFlash != null)
            {
                StopCoroutine(currentFlash);
                Restore();
            }
            currentFlash = StartCoroutine(FlashRoutine(color));
        }

        private IEnumerator FlashRoutine(Color color)
        {
            for (int n = 0; n < flashCount; n++)
            {
                Apply(color);
                yield return new WaitForSeconds(flashDuration);
                Restore();
                yield return new WaitForSeconds(flashDuration);
            }
            currentFlash = null;
        }

        private void Apply(Color color)
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                if (flashMaterial != null)
                    renderers[i].sharedMaterial = flashMaterial;

                renderers[i].color = color;
            }
        }

        private void Restore()
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].sharedMaterial = originalMaterials[i];
                renderers[i].color = originalColors[i];
            }
        }

        private void OnDisable()
        {
            // Evita che lo sprite resti colorato se l'oggetto viene disattivato durante il flash
            if (currentFlash != null)
            {
                StopCoroutine(currentFlash);
                currentFlash = null;
            }
            if (renderers != null) Restore();
        }
    }
