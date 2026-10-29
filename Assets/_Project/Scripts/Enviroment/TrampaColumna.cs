using UnityEngine;

public class TrampaColumna : MonoBehaviour
{
    [Header("Referencias Visuales")]
    public SpriteRenderer spriteRenderer;
    public Sprite[] fasesColumna; // Aqui guardaremos los 7 sprites

    [Header("Configuracion de Distancia")]
    public float distanciaMaxima = 15f; // Distancia donde empieza a abrirse
    public float distanciaMinima = 1.5f; // Distancia donde la boca esta totalmente abierta

    [Header("Configuracion de Dano")]
    public float danoMaximoPorSegundo = 20f; // Dano cuando esta en fase maxima (fase 6)

    private Transform transformJugador;
    private int estadoActual = 0; // Va del 0 (cerrada) al 6 (abierta total)
    private bool jugadorEnRango = false;

    private void Update()
    {
        // Solo calculamos si el jugador esta dentro de la gran caja verde
        if (jugadorEnRango && transformJugador != null)
        {
            // Detectamos si el jugador esta usando la mecanica de sigilo.
            // Nota: Input.GetKey lee la tecla directamente. Si ya tienes una variable 
            // como "estaEnSigilo" en tu PlayerController, es mejor llamarla desde alli.
            bool enSigilo = Input.GetKey(KeyCode.LeftControl);

            if (enSigilo)
            {
                // Si va en sigilo, la trampa "no lo escucha". Se apaga inmediatamente.
                estadoActual = 0;
                spriteRenderer.sprite = fasesColumna[0];
            }
            else
            {
                // Si camina normal o suelta el Control, la trampa reacciona a la distancia
                CalcularEstadoPorDistancia();
                AplicarDanoGradual();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            transformJugador = collision.transform;
            jugadorEnRango = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // El jugador huyo: apagar la trampa
            jugadorEnRango = false;
            transformJugador = null;
            estadoActual = 0;
            spriteRenderer.sprite = fasesColumna[0]; // Regresar al reposo
        }
    }

    private void CalcularEstadoPorDistancia()
    {
        // 1. A que distancia esta el Cartografo?
        float distancia = Vector2.Distance(transform.position, transformJugador.position);

        // 2. Definir en que estado debe estar segun la distancia
        if (distancia >= distanciaMaxima)
        {
            estadoActual = 0;
        }
        else if (distancia <= distanciaMinima)
        {
            // Si esta muy cerca, estado maximo (indice 6 en un arreglo de 7 sprites)
            estadoActual = fasesColumna.Length - 1;
        }
        else
        {
            // Calcular el porcentaje de acercamiento (1.0 = pegado, 0.0 = lejos)
            float rango = distanciaMaxima - distanciaMinima;
            float distanciaActual = distanciaMaxima - distancia;
            float porcentaje = distanciaActual / rango;

            // Mapear ese porcentaje a los 7 sprites disponibles
            estadoActual = Mathf.RoundToInt(porcentaje * (fasesColumna.Length - 1));
        }

        // 3. Cambiar la imagen directamente sin usar Animator
        spriteRenderer.sprite = fasesColumna[estadoActual];
    }

    private void AplicarDanoGradual()
    {
        // Si la boca esta minimamente abierta, empieza a hacer dano
        if (estadoActual > 0)
        {
            // Multiplicador: Fase 3 hace el 50% de dano, Fase 6 hace el 100% de dano
            float multiplicadorDano = (float)estadoActual / (fasesColumna.Length - 1);
            float danoAAplicar = danoMaximoPorSegundo * multiplicadorDano * Time.deltaTime;

            // Aqui enviaras el dano a la vida de tu jugador en el futuro
            Debug.Log($"Boca Fase {estadoActual} - Dano aplicado: {danoAAplicar}");
        }
    }
}