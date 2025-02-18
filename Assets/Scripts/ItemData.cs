using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptble Object/ItemData")]
// Define un ScriptableObject que contiene los datos de un �tem para su uso en el juego.
public class ItemData : ScriptableObject
{
    // Enum que define los tipos de �tem disponibles en el juego.
    public enum ItemType { Melee, Range, Glove, Shoe, Heal }

    [Header("# Main Info")]
    // Tipo de �tem (Melee, Range, etc.)
    public ItemType itemType;
    // Identificador �nico para el �tem.
    public int itemId;
    // Nombre del �tem.
    public string itemName;
    // Descripci�n del �tem, visible en el editor de Unity como un �rea de texto.
    [TextArea]
    public string itemDesc;
    // Icono del �tem, usado para representarlo visualmente en la interfaz de usuario.
    public Sprite itemIcon;

    [Header("# Level Data")]
    // Da�o base que inflige el �tem (solo relevante para armas).
    public float baseDamage;
    // Cantidad base de �tems en el inventario (por ejemplo, cu�ntos objetos hay por defecto).
    public int baseCount;
    // Arreglo de da�os adicionales que el �tem puede infligir en distintos niveles.
    public float[] damages;
    // Arreglo de cantidades adicionales (por ejemplo, para �tems de tipo 'Heal' podr�a ser la cantidad de salud restaurada en distintos niveles).
    public int[] counts;

    [Header("# Weapon")]
    // Prefabricado que representa el proyectil lanzado por el �tem (si es un �tem de tipo 'Range').
    public GameObject projectile;
    // Sprite que representa la mano que sostiene el �tem (relevante para �tems tipo 'Melee' o 'Glove').
    public Sprite hand;
}
