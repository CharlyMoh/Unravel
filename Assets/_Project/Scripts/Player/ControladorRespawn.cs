using UnityEngine;

public class ControladorRespawn : MonoBehaviour
{
    private Vector3 posicionRespawnActual;

    private void Start()
    {
        // Al iniciar la escena, tu posición inicial se convierte en tu primer respawn
        posicionRespawnActual = transform.position;
    }

    // Los checkpoints llamarán a esta función cuando pases sobre ellos
    public void FijarRespawn(Vector3 nuevaPosicion)
    {
        posicionRespawnActual = nuevaPosicion;
    }

    // Llama a esta función cuando el Cartógrafo muera o caiga al vacío
    public void Reaparecer()
    {
        transform.position = posicionRespawnActual;

        // Aquí en el futuro puedes reiniciar su vida, reproducir un sonido, etc.
    }
}