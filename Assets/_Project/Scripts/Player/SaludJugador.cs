using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SaludJugador : MonoBehaviour
{
    [Header("Atributos de Vida")]
    public float saludMaxima = 100f;
    public float saludActual;

    [Header("Interfaz de Usuario")]
    public Image imagenBarraSalud;

    [Header("Efectos Visuales")]
    private SpriteRenderer spriteRenderer;
    private Color colorOriginal;

    [Header("Reinicio por Muerte")]
    [Tooltip("Arrastra aquí el punto de inicio del nivel")]
    public Transform puntoInicioNivel;

    private void Start()
    {
        saludActual = saludMaxima;

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            colorOriginal = spriteRenderer.color;
        }

        ActualizarBarra();
    }

    public void RecibirDano(float cantidad)
    {
        saludActual -= cantidad;
        saludActual = Mathf.Clamp(saludActual, 0, saludMaxima);

        ActualizarBarra();

        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(DestelloDano());
        }

        if (saludActual <= 0)
        {
            Morir();
        }
    }

    // Método para curar al 100%
    public void RestaurarSaludCompleta()
    {
        saludActual = saludMaxima;
        ActualizarBarra();
    }

    private void ActualizarBarra()
    {
        if (imagenBarraSalud != null)
        {
            imagenBarraSalud.fillAmount = saludActual / saludMaxima;
        }
    }

    private IEnumerator DestelloDano()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.15f);
            spriteRenderer.color = colorOriginal;
        }
    }

    private void Morir()
    {
        Debug.Log("Vida en 0. Regresando al inicio...");

        // 1. Mover al jugador al principio del nivel
        if (puntoInicioNivel != null)
        {
            transform.position = puntoInicioNivel.position;
        }
        else
        {
            Debug.LogWarning("Falta asignar el Punto de Inicio en el Inspector.");
        }

        // 2. Llenar la barra de sangre nuevamente
        RestaurarSaludCompleta();
    }
}