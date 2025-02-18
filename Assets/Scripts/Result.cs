using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Result : MonoBehaviour
{
    // Array de GameObjects para los títulos de victoria y derrota
    public GameObject[] titles;

    // Método para mostrar el título de derrota
    public void Lose()
    {
        titles[0].SetActive(true);
    }

    // Método para mostrar el título de victoria
    public void Win()
    {
        titles[1].SetActive(true);
    }
}
