using UnityEngine;

public class PlataformaAscendente : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float velocidad = 2f;
    public float limiteSuperior = 5f;  // Altura (Y) donde desaparece
    public float limiteInferior = -5f; // Altura (Y) donde reaparece

    private void Update()
    {
        // 1. Mover la plataforma hacia arriba constantemente
        transform.Translate(Vector3.up * velocidad * Time.deltaTime);

        // 2. Evaluar si llegó al límite superior
        if (transform.position.y >= limiteSuperior)
        {
            // 3. Teletransportarla al fondo manteniendo su posición X original
            transform.position = new Vector3(transform.position.x, limiteInferior, transform.position.z);
        }
    }

    // --- LÓGICA DE FRICCIÓN (Crucial para que el jugador no se caiga) ---
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Emparenta al jugador con la plataforma para que suban juntos
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Libera al jugador cuando salta de la plataforma
            collision.transform.SetParent(null);
        }
    }
}