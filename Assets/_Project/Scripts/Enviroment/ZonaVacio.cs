using UnityEngine;

public class ZonaVacio : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si el objeto que cae y atraviesa esta zona es el Cartógrafo
        if (collision.CompareTag("Player"))
        {
            // Buscamos su cerebro de respawn
            ControladorRespawn respawnDelJugador = collision.GetComponent<ControladorRespawn>();

            if (respawnDelJugador != null)
            {
                // Lo teletransportamos al último checkpoint que tocó
                respawnDelJugador.Reaparecer();
            }
        }
    }
}