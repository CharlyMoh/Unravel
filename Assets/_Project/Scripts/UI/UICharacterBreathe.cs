using UnityEngine;

public class UICharacterBreathe : MonoBehaviour
{
    [Header("Configuración de Respiración")]
    [Tooltip("Amplitud del movimiento vertical en píxeles")]
    [SerializeField] private float breatheAmplitude = 3.5f;

    [Tooltip("Velocidad de respiración (ciclos por segundo)")]
    [SerializeField] private float breatheFrequency = 1.1f;

    private RectTransform rectTransform;
    private Vector2 baseAnchoredPos;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        baseAnchoredPos = rectTransform.anchoredPosition;
    }

    void Update()
    {
        float verticalOffset = Mathf.Sin(Time.time * breatheFrequency) * breatheAmplitude;
        rectTransform.anchoredPosition = new Vector2(baseAnchoredPos.x, baseAnchoredPos.y + verticalOffset);
    }
}