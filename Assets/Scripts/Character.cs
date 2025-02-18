using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    // Ventaja personaje 1: Aumenta la velocidad del jugador si el playerId es 0
    public static float Speed
    {
        get { return GameManager.instance.playerId == 0 ? 1.1f : 1f; }
        // Si el playerId es 0, devuelve 1.1 (aumentando la velocidad), de lo contrario devuelve 1 (sin aumento de velocidad)
    }

    // Ventaja personaje 2: Aumenta la velocidad del arma si el playerId es 1
    public static float WeaponSpeed
    {
        get { return GameManager.instance.playerId == 1 ? 1.1f : 1f; }
        // Si el playerId es 1, devuelve 1.1 (aumentando la velocidad del arma), de lo contrario devuelve 1 (sin aumento de velocidad)
    }

    // Ventaja personaje 2: Aumenta la tasa de disparo del arma si el playerId es 1
    public static float WeaponRate
    {
        get { return GameManager.instance.playerId == 1 ? 0.9f : 1f; }
        // Si el playerId es 1, devuelve 0.9 (disminuyendo el tiempo de recarga del arma), de lo contrario devuelve 1 (sin cambios en la tasa de disparo)
    }

    // Ventaja personaje 3: Aumenta el daño si el playerId es 2
    public static float Damage
    {
        get { return GameManager.instance.playerId == 2 ? 1.2f : 1f; }
        // Si el playerId es 2, devuelve 1.2 (aumentando el daño), de lo contrario devuelve 1 (sin aumento de daño)
    }

    // Ventaja personaje 4: Aumenta el número de armas o habilidad de personaje si el playerId es 3
    public static int Count
    {
        get { return GameManager.instance.playerId == 3 ? 1 : 0; }
        // Si el playerId es 3, devuelve 1 (habilitando un efecto de personaje o habilidad extra), de lo contrario devuelve 0
    }
}
