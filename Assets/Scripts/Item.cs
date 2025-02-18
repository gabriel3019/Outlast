using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    // Referencia a los datos del ítem, definidos en el ScriptableObject 'ItemData'.
    public ItemData data;
    // Nivel actual del ítem.
    public int level;
    // Referencia a la clase Weapon, solo se utiliza si el ítem es de tipo 'Melee' o 'Range'.
    public Weapon weapon;
    // Referencia a la clase Gear, solo se utiliza si el ítem es de tipo 'Glove' o 'Shoe'.
    public Gear gear;

    // Variables privadas para almacenar componentes de la interfaz de usuario.
    Image icon;
    Text textLevel;
    Text textName;
    Text textDesc;

    // Inicializa los componentes de la UI cuando se carga el objeto.
    void Awake()
    {
        // Obtiene el icono del ítem (el segundo componente Image hijo del objeto).
        icon = GetComponentsInChildren<Image>()[1];
        // Asigna el icono del ítem desde el ScriptableObject 'data'.
        icon.sprite = data.itemIcon;

        // Obtiene todos los componentes Text hijos y los asigna a las variables correspondientes.
        Text[] texts = GetComponentsInChildren<Text>();
        textLevel = texts[0];
        textName = texts[1];
        textDesc = texts[2];
        // Asigna el nombre del ítem al componente Text.
        textName.text = data.itemName;
    }

    // Método llamado cuando el objeto se habilita o activa.
    // Actualiza los textos de nivel y descripción en función del nivel del ítem.
    void OnEnable()
    {
        // Actualiza el nivel del ítem mostrando "Lv. x".
        textLevel.text = "Lv." + (level + 1);

        // Dependiendo del tipo de ítem, ajusta la descripción del ítem.
        switch (data.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                // Para armas, muestra el daño y la cantidad (por ejemplo, de munición) en la descripción.
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100, data.counts[level]);
                break;
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                // Para guantes o zapatos, muestra la tasa de daño en la descripción.
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100);
                break;
            default:
                // Para otros tipos de ítems, solo muestra la descripción original.
                textDesc.text = string.Format(data.itemDesc);
                break;
        }
    }

    // Método que se llama cuando el ítem es clickeado.
    // Incrementa el nivel del ítem y aplica los cambios correspondientes.
    public void OnClick()
    {
        // Dependiendo del tipo de ítem, realiza la acción correspondiente.
        switch (data.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                // Si el ítem es de tipo 'Melee' o 'Range', crea o mejora el arma asociada.
                if (level == 0)
                {
                    // Si es el primer nivel, crea un nuevo objeto de arma y lo inicializa.
                    GameObject newWeapon = new GameObject();
                    weapon = newWeapon.AddComponent<Weapon>();
                    weapon.Init(data);
                }
                else
                {
                    // Si no es el primer nivel, mejora el arma existente con el nuevo daño y cantidad.
                    float nextDamage = data.baseDamage;
                    int nextCount = 0;

                    nextDamage += data.baseDamage * data.damages[level];
                    nextCount += data.counts[level];

                    // Llama al método LevelUp de la clase Weapon para mejorar el arma.
                    weapon.LevelUp(nextDamage, nextCount);
                }

                // Incrementa el nivel del ítem.
                level++;
                break;
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                // Si el ítem es de tipo 'Glove' o 'Shoe', crea o mejora el equipo asociado.
                if (level == 0)
                {
                    // Si es el primer nivel, crea un nuevo objeto de equipo y lo inicializa.
                    GameObject newGear = new GameObject();
                    gear = newGear.AddComponent<Gear>();
                    gear.Init(data);
                }
                else
                {
                    // Si no es el primer nivel, mejora el equipo existente con la nueva tasa.
                    float nextRate = data.damages[level];
                    gear.LevelUp(nextRate);
                }

                // Incrementa el nivel del ítem.
                level++;
                break;
            case ItemData.ItemType.Heal:
                // Si el ítem es de tipo 'Heal', restaura la salud del jugador al máximo.
                GameManager.instance.health = GameManager.instance.maxHealth;
                break;
        }

        // Si el nivel del ítem ha alcanzado el máximo, desactiva el botón (no se puede mejorar más).
        if (level == data.damages.Length)
        {
            GetComponent<Button>().interactable = false;
        }
    }
}
