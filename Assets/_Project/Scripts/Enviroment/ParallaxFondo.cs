using UnityEngine;

public class ParallaxFondo : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Arrastra tu cámara principal aquí (Main Camera)")]
    public Transform camaraPrincipal;

    [Tooltip("0 = Se mueve rápido con el jugador. 1 = Casi estático a la distancia")]
    [Range(0f, 1f)]
    public float multiplicadorParallax = 0.5f;

    private Vector3 ultimaPosicionCamara;

    private void Start()
    {
        // Si olvidas asignar la cámara manualmente, el código la busca por ti
        if (camaraPrincipal == null)
        {
            camaraPrincipal = Camera.main.transform;
        }
        ultimaPosicionCamara = camaraPrincipal.position;
    }

    private void LateUpdate()
    {
        // Calculamos cuánto se movió la cámara desde el frame anterior
        Vector3 movimientoCamara = camaraPrincipal.position - ultimaPosicionCamara;

        // Movemos la imagen multiplicando ese movimiento
        // En el eje Y reducimos un poco el efecto para que los saltos no mareen
        transform.position += new Vector3(movimientoCamara.x * multiplicadorParallax, movimientoCamara.y * (multiplicadorParallax * 0.5f), 0);

        ultimaPosicionCamara = camaraPrincipal.position;
    }
}