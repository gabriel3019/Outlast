using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // Array de prefabs que se pueden instanciar en el pool
    public GameObject[] prefabs;

    // Listas de objetos agrupadas por tipo, usadas como pool
    List<GameObject>[] pools;

    void Awake()
    {
        // Inicializa el array de listas seg�n la cantidad de prefabs
        pools = new List<GameObject>[prefabs.Length];

        // Crea una lista vac�a para cada tipo de prefab
        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }

    // M�todo para obtener un objeto del pool
    public GameObject Get(int index)
    {
        GameObject select = null;

        // Busca un objeto inactivo en la lista del �ndice indicado
        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf) // Si encuentra uno inactivo, lo reutiliza
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        // Si no hay objetos inactivos disponibles, crea uno nuevo
        if (!select)
        {
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select); // Lo a�ade al pool
        }

        return select;
    }
}
