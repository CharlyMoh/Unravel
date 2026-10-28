using System.Collections;
using UnityEngine;

public class PlataformaInestable : MonoBehaviour
{
    [Header("Configuración de Tiempos")]
    public float tiempoParaDesaparecer = 0.5f; // Tiempo que tarda en caerse al pisarla
    public float tiempoParaReaparecer = 3f;    // Tiempo que tarda en volver

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private bool seEstaDesmoronando = false;

    private void Start()
    {
        // El código busca sus propios componentes al iniciar
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Solo se activa si es el Cartógrafo y la plataforma está intacta
        if (collision.gameObject.CompareTag("Player") && !seEstaDesmoronando)
        {
            // Verificamos que el jugador esté tocando la plataforma desde arriba
            foreach (ContactPoint2D contacto in collision.contacts)
            {
                // contacto.normal.y < -0.5f significa que la fuerza viene de arriba hacia abajo
                if (contacto.normal.y < -0.5f)
                {
                    StartCoroutine(SecuenciaDesmoronamiento());
                    break;
                }
            }
        }
    }

    private IEnumerator SecuenciaDesmoronamiento()
    {
        seEstaDesmoronando = true;

        // 1. El jugador pisa y empieza a temblar/desestabilizarse
        yield return new WaitForSeconds(tiempoParaDesaparecer);

        // 2. La plataforma se "cae" (se vuelve invisible e intocable)
        spriteRenderer.enabled = false;
        boxCollider.enabled = false;

        // 3. El tiempo de castigo antes de regresar
        yield return new WaitForSeconds(tiempoParaReaparecer);

        // 4. La plataforma reaparece para volver a intentarlo
        spriteRenderer.enabled = true;
        boxCollider.enabled = true;
        seEstaDesmoronando = false;
    }
}