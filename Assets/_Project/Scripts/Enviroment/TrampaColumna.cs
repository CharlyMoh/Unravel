using UnityEngine;

public class TrampaColumna : MonoBehaviour
{
    [Header("Referencias Visuales")]
    public SpriteRenderer spriteRenderer;
    public Sprite[] fasesColumna; // Aquí guardaremos los 7 sprites

    [Header("Configuración de Distancia")]
    public float distanciaMaxima = 15f; // Distancia donde empieza a abrirse
    public float distanciaMinima = 1.5f; // Distancia donde la boca está totalmente abierta

    [Header("Configuración de Daño")]
    public float dañoMaximoPorSegundo = 20f; // Daño cuando está en fase máxima (fase 6)

    private Transform transformJugador;
    private int estadoActual = 0; // Va del 0 (cerrada) al 6 (abierta total)
    private bool jugadorEnRango = false;

    private void Update()
    {
        // Solo calculamos si el jugador está dentro de la gran caja verde
        if (jugadorEnRango && transformJugador != null)
        {
            // Detectamos si el jugador está usando la mecánica de sigilo.
            // Nota: Input.GetKey lee la tecla directamente. Si ya tienes una variable 
            // como "estaEnSigilo" en tu PlayerController, es mejor llamarla desde allí.
            bool enSigilo = Input.GetKey(KeyCode.LeftControl);

            if (enSigilo)
            {
                // Si va en sigilo, la trampa "no lo escucha". Se apaga inmediatamente.
                estadoActual = 0;
                spriteRenderer.sprite = fasesColumna[0];
            }
            else
            {
                // Si camina normal o suelta el Shift, la trampa reacciona a la distancia
                CalcularEstadoPorDistancia();
                AplicarDañoGradual();
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
            // El jugador huyó: apagar la trampa
            jugadorEnRango = false;
            transformJugador = null;
            estadoActual = 0;
            spriteRenderer.sprite = fasesColumna[0]; // Regresar al reposo
        }
    }

    private void CalcularEstadoPorDistancia()
    {
        // 1. ¿A qué distancia está el Cartógrafo?
        float distancia = Vector2.Distance(transform.position, transformJugador.position);

        // 2. Definir en qué estado debe estar según la distancia
        if (distancia >= distanciaMaxima)
        {
            estadoActual = 0;
        }
        else if (distancia <= distanciaMinima)
        {
            // Si está muy cerca, estado máximo (índice 6 en un arreglo de 7 sprites)
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

    private void AplicarDañoGradual()
    {
        // Si la boca está minimamente abierta, empieza a hacer daño
        if (estadoActual > 0)
        {
            // Multiplicador: Fase 3 hace el 50% de daño, Fase 6 hace el 100% de daño
            float multiplicadorDaño = (float)estadoActual / (fasesColumna.Length - 1);
            float dañoAAplicar = dañoMaximoPorSegundo * multiplicadorDaño * Time.deltaTime;

            // Aquí enviarás el daño a la vida de tu jugador en el futuro
            Debug.Log($"Boca Fase {estadoActual} - Daño aplicado: {dañoAAplicar}");
        }
    }
}