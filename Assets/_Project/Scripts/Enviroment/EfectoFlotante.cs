using UnityEngine;

public class EfectoFlotante : MonoBehaviour
{
    [Header("Configuracion de Flotacion")]
    [Tooltip("Que tan rapido sube y baja")]
    public float velocidad = 1.5f;
    [Tooltip("Que tan alto y bajo llega")]
    public float amplitud = 0.1f;

    private void Update()
    {
        // Calculamos la diferencia de la onda senoidal entre este frame y el anterior
        float ondaActual = Mathf.Sin(Time.time * velocidad) * amplitud;
        float ondaAnterior = Mathf.Sin((Time.time - Time.deltaTime) * velocidad) * amplitud;
        float diferencia = ondaActual - ondaAnterior;

        // Le sumamos esa pequeña diferencia al eje Y actual
        transform.position += new Vector3(0, diferencia, 0);
    }
}