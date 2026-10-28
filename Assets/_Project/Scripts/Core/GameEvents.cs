using System;
using UnityEngine;

namespace Unravel.Core
{
    /// <summary>
    /// Canal global de eventos desacoplados para Unravel.
    /// Permite que los sensores, el audio y la UI reaccionen sin referencias directas al jugador.
    /// </summary>
    public static class GameEvents
    {
        // Evento de emisión acústica: pasa el punto de origen en el mundo y el radio en metros
        public static event Action<Vector2, float> OnNoiseEmitted;

        public static void EmitNoise(Vector2 origin, float radius)
        {
            OnNoiseEmitted?.Invoke(origin, radius);
        }

        // Evento para el cambio dimensional del Nivel 3 (true = Color / tangible, false = Gris / intangible)
        public static event Action<bool> OnDimensionChanged;

        public static void ChangeDimension(bool isColor)
        {
            OnDimensionChanged?.Invoke(isColor);
        }

        // Evento de registro de daño/sacrificio moral
        public static event Action<int> OnDamageAbsorbed;

        public static void AbsorbDamage(int amount)
        {
            OnDamageAbsorbed?.Invoke(amount);
        }
    }
}