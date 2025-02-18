using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    // Tipo de equipo (por ejemplo, Guante, Zapato, etc.)
    public ItemData.ItemType type;
    // Tasa de incremento asociada al equipo
    public float rate;

    // Método que inicializa el equipo con los datos del ItemData
    public void Init(ItemData data)
    {
        // Asigna un nombre al equipo basado en el ID del artículo
        name = "Gear " + data.itemId;
        // Establece el equipo como hijo del jugador en la jerarquía
        transform.parent = GameManager.instance.player.transform;
        // Establece la posición local del equipo a (0, 0, 0) relativo al jugador
        transform.localPosition = Vector3.zero;

        // Establece las propiedades del equipo a partir de los datos del artículo
        type = data.itemType;
        rate = data.damages[0];
        // Aplica las modificaciones del equipo
        ApplyGear();
    }

    // Método que se llama cuando el equipo sube de nivel, actualizando la tasa de incremento
    public void LevelUp(float rate)
    {
        this.rate = rate;
        // Aplica las modificaciones del equipo después de mejorar su tasa
        ApplyGear();
    }

    // Método que aplica los efectos del equipo en función de su tipo
    void ApplyGear()
    {
        // Aplica los efectos específicos según el tipo de equipo
        switch (type)
        {
            case ItemData.ItemType.Glove:
                // Si es un guante, aumenta la tasa de daño
                RateUp();
                break;
            case ItemData.ItemType.Shoe:
                // Si es un zapato, aumenta la velocidad del jugador
                SpeedUp();
                break;
        }
    }

    // Método que aumenta la tasa de daño de las armas del jugador
    void RateUp()
    {
        // Obtiene todas las armas del jugador
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        // Itera sobre todas las armas y ajusta sus propiedades en función de la tasa
        foreach (Weapon weapon in weapons)
        {
            switch (weapon.id)
            {
                case 0:
                    // Si el arma tiene ID 0 (probablemente la principal), aumenta su velocidad en función de la tasa
                    float speed = 150 * Character.WeaponSpeed;
                    weapon.speed = 150 + (150 * rate);
                    break;
                default:
                    // Para otras armas, ajusta su velocidad en función de la tasa
                    speed = 0.5f * Character.WeaponRate;
                    weapon.speed = speed * (1f - rate);
                    break;
            }
        }
    }

    // Método que aumenta la velocidad del jugador
    void SpeedUp()
    {
        // Calcula la velocidad base del jugador
        float speed = 3 * Character.Speed;
        // Ajusta la velocidad total del jugador en función de la tasa de aumento del equipo
        GameManager.instance.player.speed = speed + speed * rate;
    }
}
