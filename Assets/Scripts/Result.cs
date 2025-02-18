using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Result : MonoBehaviour
{
    // Array de GameObjects para los t�tulos de victoria y derrota
    public GameObject[] titles;

    // M�todo para mostrar el t�tulo de derrota
    public void Lose()
    {
        titles[0].SetActive(true);
    }

    // M�todo para mostrar el t�tulo de victoria
    public void Win()
    {
        titles[1].SetActive(true);
    }
}
