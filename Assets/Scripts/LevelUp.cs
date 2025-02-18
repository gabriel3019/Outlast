using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    RectTransform rect; // Referencia al rectángulo del menú de subida de nivel
    Item[] items; // Lista de posibles mejoras para el jugador

    private void Awake()
    {
        // Obtiene la referencia del RectTransform del menú de subida de nivel
        rect = GetComponent<RectTransform>();

        // Obtiene todos los objetos de tipo Item dentro del menú (aunque estén inactivos)
        items = GetComponentsInChildren<Item>(true);
    }

    // Método para mostrar el menú de subida de nivel
    public void Show()
    {
        Next(); // Selecciona los ítems disponibles para el jugador
        rect.localScale = Vector3.one; // Muestra el menú estableciendo su escala a 1 (visible)
        GameManager.instance.Stop(); // Pausa el juego
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp); // Reproduce sonido de subida de nivel
        AudioManager.instance.EffectBgm(true); // Modifica la música de fondo
    }

    // Método para ocultar el menú de subida de nivel
    public void Hide()
    {
        rect.localScale = Vector3.zero; // Oculta el menú estableciendo su escala a 0 (invisible)
        GameManager.instance.Resume(); // Reanuda el juego
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select); // Reproduce sonido de selección
        AudioManager.instance.EffectBgm(false); // Restaura la música de fondo
    }

    // Método llamado cuando el jugador selecciona un ítem
    public void Select(int index)
    {
        items[index].OnClick(); // Activa la funcionalidad del ítem seleccionado
    }

    // Método que selecciona tres ítems aleatorios para mostrar al jugador
    void Next()
    {
        // Oculta todos los ítems antes de seleccionar los nuevos
        foreach (Item item in items)
        {
            item.gameObject.SetActive(false);
        }

        int[] ran = new int[3]; // Array para almacenar los índices de los ítems aleatorios

        // Bucle que selecciona tres índices distintos
        while (true)
        {
            ran[0] = Random.Range(0, items.Length);
            ran[1] = Random.Range(0, items.Length);
            ran[2] = Random.Range(0, items.Length);

            // Si los tres índices son diferentes, sale del bucle
            if (ran[0] != ran[1] && ran[1] != ran[2] && ran[0] != ran[2])
                break;
        }

        // Activa los ítems seleccionados aleatoriamente
        for (int i = 0; i < ran.Length; i++)
        {
            Item ranItem = items[ran[i]];

            // Si el ítem ha alcanzado su nivel máximo, muestra un ítem de reemplazo (índice 4)
            if (ranItem.level == ranItem.data.damages.Length)
            {
                items[4].gameObject.SetActive(true);
            }
            else
            {
                ranItem.gameObject.SetActive(true);
            }
        }
    }
}
