using UnityEngine;

public class TrampaColumna : MonoBehaviour
{
    [Header("Referencias Visuales")]
    public SpriteRenderer spriteRenderer;
    public Sprite[] fasesColumna;

    [Header("Configuracion de Distancia")]
    public float distanciaMaxima = 15f;
    public float distanciaMinima = 1.5f;

    [Header("Configuracion de Dano")]
    public float danoMaximoPorSegundo = 20f;

    private Transform transformJugador;
    private SaludJugador saludJugador;
    private int estadoActual = 0;

    private void Start()
    {
        GameObject jugador = GameObject.FindGameObjectWithTag("Player");
        if (jugador != null)
        {
            transformJugador = jugador.transform;
            saludJugador = jugador.GetComponent<SaludJugador>(); // Guardamos la referencia
        }
    }

    private void Update()
    {
        // Si el jugador no existe o murio, detenemos la logica
        if (transformJugador == null) return;

        // Calculamos la distancia matematica en tiempo real
        float distancia = Mathf.Abs(transform.position.x - transformJugador.position.x);

        // Verificamos si el jugador entro en el rango de vision
        if (distancia <= distanciaMaxima)
        {
            bool enSigilo = Input.GetKey(KeyCode.LeftControl);

            if (enSigilo)
            {
                ApagarTrampa();
            }
            else
            {
                CalcularEstadoPorDistancia(distancia);
                AplicarDanoGradual();
            }
        }
        else
        {
            ApagarTrampa(); // Se apaga si se aleja demasiado
        }
    }

    private void ApagarTrampa()
    {
        estadoActual = 0;
        spriteRenderer.sprite = fasesColumna[0];
    }

    private void CalcularEstadoPorDistancia(float distancia)
    {
        if (distancia <= distanciaMinima)
        {
            estadoActual = fasesColumna.Length - 1;
        }
        else
        {
            float rango = distanciaMaxima - distanciaMinima;
            float distanciaActual = distanciaMaxima - distancia;
            float porcentaje = distanciaActual / rango;

            estadoActual = Mathf.RoundToInt(porcentaje * (fasesColumna.Length - 1));
        }

        spriteRenderer.sprite = fasesColumna[estadoActual];
    }

    private void AplicarDanoGradual()
    {
        if (estadoActual > 0 && saludJugador != null)
        {
            float multiplicadorDano = (float)estadoActual / (fasesColumna.Length - 1);
            float danoAAplicar = danoMaximoPorSegundo * multiplicadorDano * Time.deltaTime;

            saludJugador.RecibirDano(danoAAplicar); // Aplicamos el daño directo al Cartógrafo
        }
    }
}