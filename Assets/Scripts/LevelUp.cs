using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    RectTransform rect; // Referencia al rect�ngulo del men� de subida de nivel
    Item[] items; // Lista de posibles mejoras para el jugador

    private void Awake()
    {
        // Obtiene la referencia del RectTransform del men� de subida de nivel
        rect = GetComponent<RectTransform>();

        // Obtiene todos los objetos de tipo Item dentro del men� (aunque est�n inactivos)
        items = GetComponentsInChildren<Item>(true);
    }

    // M�todo para mostrar el men� de subida de nivel
    public void Show()
    {
        Next(); // Selecciona los �tems disponibles para el jugador
        rect.localScale = Vector3.one; // Muestra el men� estableciendo su escala a 1 (visible)
        GameManager.instance.Stop(); // Pausa el juego
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp); // Reproduce sonido de subida de nivel
        AudioManager.instance.EffectBgm(true); // Modifica la m�sica de fondo
    }

    // M�todo para ocultar el men� de subida de nivel
    public void Hide()
    {
        rect.localScale = Vector3.zero; // Oculta el men� estableciendo su escala a 0 (invisible)
        GameManager.instance.Resume(); // Reanuda el juego
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select); // Reproduce sonido de selecci�n
        AudioManager.instance.EffectBgm(false); // Restaura la m�sica de fondo
    }

    // M�todo llamado cuando el jugador selecciona un �tem
    public void Select(int index)
    {
        items[index].OnClick(); // Activa la funcionalidad del �tem seleccionado
    }

    // M�todo que selecciona tres �tems aleatorios para mostrar al jugador
    void Next()
    {
        // Oculta todos los �tems antes de seleccionar los nuevos
        foreach (Item item in items)
        {
            item.gameObject.SetActive(false);
        }

        int[] ran = new int[3]; // Array para almacenar los �ndices de los �tems aleatorios

        // Bucle que selecciona tres �ndices distintos
        while (true)
        {
            ran[0] = Random.Range(0, items.Length);
            ran[1] = Random.Range(0, items.Length);
            ran[2] = Random.Range(0, items.Length);

            // Si los tres �ndices son diferentes, sale del bucle
            if (ran[0] != ran[1] && ran[1] != ran[2] && ran[0] != ran[2])
                break;
        }

        // Activa los �tems seleccionados aleatoriamente
        for (int i = 0; i < ran.Length; i++)
        {
            Item ranItem = items[ran[i]];

            // Si el �tem ha alcanzado su nivel m�ximo, muestra un �tem de reemplazo (�ndice 4)
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
