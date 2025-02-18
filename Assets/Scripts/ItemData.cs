using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptble Object/ItemData")]
// Define un ScriptableObject que contiene los datos de un ítem para su uso en el juego.
public class ItemData : ScriptableObject
{
    // Enum que define los tipos de ítem disponibles en el juego.
    public enum ItemType { Melee, Range, Glove, Shoe, Heal }

    [Header("# Main Info")]
    // Tipo de ítem (Melee, Range, etc.)
    public ItemType itemType;
    // Identificador único para el ítem.
    public int itemId;
    // Nombre del ítem.
    public string itemName;
    // Descripción del ítem, visible en el editor de Unity como un área de texto.
    [TextArea]
    public string itemDesc;
    // Icono del ítem, usado para representarlo visualmente en la interfaz de usuario.
    public Sprite itemIcon;

    [Header("# Level Data")]
    // Daño base que inflige el ítem (solo relevante para armas).
    public float baseDamage;
    // Cantidad base de ítems en el inventario (por ejemplo, cuántos objetos hay por defecto).
    public int baseCount;
    // Arreglo de daños adicionales que el ítem puede infligir en distintos niveles.
    public float[] damages;
    // Arreglo de cantidades adicionales (por ejemplo, para ítems de tipo 'Heal' podría ser la cantidad de salud restaurada en distintos niveles).
    public int[] counts;

    [Header("# Weapon")]
    // Prefabricado que representa el proyectil lanzado por el ítem (si es un ítem de tipo 'Range').
    public GameObject projectile;
    // Sprite que representa la mano que sostiene el ítem (relevante para ítems tipo 'Melee' o 'Glove').
    public Sprite hand;
}
