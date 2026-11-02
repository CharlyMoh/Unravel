using UnityEngine;

public class MenuParallax : MonoBehaviour
{
    [Header("Referencias de Capas")]
    [SerializeField] private RectTransform characterRect;
    [SerializeField] private RectTransform backgroundRect;

    [Header("Intensidad de Movimiento")]
    [SerializeField] private float characterStrength = 14f;
    [SerializeField] private float backgroundStrength = 6f;
    [SerializeField] private float smoothTime = 4f;

    private Vector2 initialCharacterPos;
    private Vector2 initialBackgroundPos;

    void Start()
    {
        if (characterRect != null) initialCharacterPos = characterRect.anchoredPosition;
        if (backgroundRect != null) initialBackgroundPos = backgroundRect.anchoredPosition;
    }

    void Update()
    {
        // Vector normalizado de -1 a 1 respecto al centro de pantalla
        Vector2 mouseOffset = new Vector2(
            (Input.mousePosition.x / Screen.width) - 0.5f,
            (Input.mousePosition.y / Screen.height) - 0.5f
        );

        if (characterRect != null)
        {
            Vector2 targetChar = initialCharacterPos + (mouseOffset * characterStrength);
            characterRect.anchoredPosition = Vector2.Lerp(characterRect.anchoredPosition, targetChar, Time.deltaTime * smoothTime);
        }

        if (backgroundRect != null)
        {
            // El fondo se desplaza en dirección contraria para acentuar la profundidad
            Vector2 targetBg = initialBackgroundPos - (mouseOffset * backgroundStrength);
            backgroundRect.anchoredPosition = Vector2.Lerp(backgroundRect.anchoredPosition, targetBg, Time.deltaTime * smoothTime);
        }
    }
}