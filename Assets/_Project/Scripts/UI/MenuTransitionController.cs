using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuTransitionController : MonoBehaviour
{
    [Header("Configuración de Escena")]
    [SerializeField] private string targetSceneName = "Level01_Panico";

    [Header("Elementos de Transición")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private RectTransform characterRect;
    [SerializeField] private CanvasGroup buttonsCanvasGroup;

    [Header("Tiempos e Intensidad")]
    [SerializeField] private float transitionDuration = 1.2f;
    [SerializeField] private float characterExitDistance = 250f;

    [Header("Audio (Opcional)")]
    [SerializeField] private AudioSource menuMusicSource;

    private bool isTransitioning = false;

    public void StartGameTransition()
    {
        if (isTransitioning) return;
        StartCoroutine(TransitionRoutine());
    }

    private IEnumerator TransitionRoutine()
    {
        isTransitioning = true;

        // Desactivar interacción en los botones
        if (buttonsCanvasGroup != null)
        {
            buttonsCanvasGroup.interactable = false;
            buttonsCanvasGroup.blocksRaycasts = false;
        }

        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.blocksRaycasts = true;
        }

        Vector2 startCharPos = characterRect != null ? characterRect.anchoredPosition : Vector2.zero;
        Vector2 targetCharPos = startCharPos + new Vector2(characterExitDistance, 0f);

        float initialMusicVolume = menuMusicSource != null ? menuMusicSource.volume : 1f;
        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / transitionDuration);
            // Curva suave para la transición
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            // Fundido a negro
            if (fadeCanvasGroup != null)
            {
                fadeCanvasGroup.alpha = smoothT;
            }

            // Desvanecer botones
            if (buttonsCanvasGroup != null)
            {
                buttonsCanvasGroup.alpha = 1f - smoothT;
            }

            // Desplazar al personaje levemente hacia la derecha como si avanzara
            if (characterRect != null)
            {
                characterRect.anchoredPosition = Vector2.Lerp(startCharPos, targetCharPos, smoothT);
            }

            // Fade-out de la música de fondo
            if (menuMusicSource != null)
            {
                menuMusicSource.volume = Mathf.Lerp(initialMusicVolume, 0f, smoothT);
            }

            yield return null;
        }

        // Carga la escena de juego una vez la pantalla esté en negro absoluto
        SceneManager.LoadScene(targetSceneName);
    }
}