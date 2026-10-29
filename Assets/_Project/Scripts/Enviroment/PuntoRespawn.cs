using UnityEngine;

public class PuntoRespawn : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si el jugador atraviesa este punto invisible
        if (collision.CompareTag("Player"))
        {
            // Busca su script de respawn y le entrega la posición exacta de este checkpoint
            ControladorRespawn respawnDelJugador = collision.GetComponent<ControladorRespawn>();

            if (respawnDelJugador != null)
            {
                respawnDelJugador.FijarRespawn(transform.position);
            }
        }
    }
}