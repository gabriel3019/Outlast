using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    // Referencia a los datos del �tem, definidos en el ScriptableObject 'ItemData'.
    public ItemData data;
    // Nivel actual del �tem.
    public int level;
    // Referencia a la clase Weapon, solo se utiliza si el �tem es de tipo 'Melee' o 'Range'.
    public Weapon weapon;
    // Referencia a la clase Gear, solo se utiliza si el �tem es de tipo 'Glove' o 'Shoe'.
    public Gear gear;

    // Variables privadas para almacenar componentes de la interfaz de usuario.
    Image icon;
    Text textLevel;
    Text textName;
    Text textDesc;

    // Inicializa los componentes de la UI cuando se carga el objeto.
    void Awake()
    {
        // Obtiene el icono del �tem (el segundo componente Image hijo del objeto).
        icon = GetComponentsInChildren<Image>()[1];
        // Asigna el icono del �tem desde el ScriptableObject 'data'.
        icon.sprite = data.itemIcon;

        // Obtiene todos los componentes Text hijos y los asigna a las variables correspondientes.
        Text[] texts = GetComponentsInChildren<Text>();
        textLevel = texts[0];
        textName = texts[1];
        textDesc = texts[2];
        // Asigna el nombre del �tem al componente Text.
        textName.text = data.itemName;
    }

    // M�todo llamado cuando el objeto se habilita o activa.
    // Actualiza los textos de nivel y descripci�n en funci�n del nivel del �tem.
    void OnEnable()
    {
        // Actualiza el nivel del �tem mostrando "Lv. x".
        textLevel.text = "Lv." + (level + 1);

        // Dependiendo del tipo de �tem, ajusta la descripci�n del �tem.
        switch (data.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                // Para armas, muestra el da�o y la cantidad (por ejemplo, de munici�n) en la descripci�n.
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100, data.counts[level]);
                break;
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                // Para guantes o zapatos, muestra la tasa de da�o en la descripci�n.
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100);
                break;
            default:
                // Para otros tipos de �tems, solo muestra la descripci�n original.
                textDesc.text = string.Format(data.itemDesc);
                break;
        }
    }

    // M�todo que se llama cuando el �tem es clickeado.
    // Incrementa el nivel del �tem y aplica los cambios correspondientes.
    public void OnClick()
    {
        // Dependiendo del tipo de �tem, realiza la acci�n correspondiente.
        switch (data.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                // Si el �tem es de tipo 'Melee' o 'Range', crea o mejora el arma asociada.
                if (level == 0)
                {
                    // Si es el primer nivel, crea un nuevo objeto de arma y lo inicializa.
                    GameObject newWeapon = new GameObject();
                    weapon = newWeapon.AddComponent<Weapon>();
                    weapon.Init(data);
                }
                else
                {
                    // Si no es el primer nivel, mejora el arma existente con el nuevo da�o y cantidad.
                    float nextDamage = data.baseDamage;
                    int nextCount = 0;

                    nextDamage += data.baseDamage * data.damages[level];
                    nextCount += data.counts[level];

                    // Llama al m�todo LevelUp de la clase Weapon para mejorar el arma.
                    weapon.LevelUp(nextDamage, nextCount);
                }

                // Incrementa el nivel del �tem.
                level++;
                break;
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                // Si el �tem es de tipo 'Glove' o 'Shoe', crea o mejora el equipo asociado.
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

                // Incrementa el nivel del �tem.
                level++;
                break;
            case ItemData.ItemType.Heal:
                // Si el �tem es de tipo 'Heal', restaura la salud del jugador al m�ximo.
                GameManager.instance.health = GameManager.instance.maxHealth;
                break;
        }

        // Si el nivel del �tem ha alcanzado el m�ximo, desactiva el bot�n (no se puede mejorar m�s).
        if (level == data.damages.Length)
        {
            GetComponent<Button>().interactable = false;
        }
    }
}
