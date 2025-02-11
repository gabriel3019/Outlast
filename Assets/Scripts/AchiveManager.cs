using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class AchiveManager : MonoBehaviour
{
    public GameObject[] lockCharacter; // Array de personajes bloqueados
    public GameObject[] unlockCharacter; // Array de personajes desbloqueados
    public GameObject uiNotice; // UI para mostrar notificaciones de logros

    enum Achive { UnlockCharacter2, UnlockCharacter3 } // Enum con los logros disponibles
    Achive[] achives; // Array que almacena los logros
    WaitForSecondsRealtime wait; // Espera de tiempo en tiempo real para la notificación

    void Awake()
    {
        // Inicializa los logros y establece la espera de 5 segundos
        achives = (Achive[])Enum.GetValues(typeof(Achive));
        wait = new WaitForSecondsRealtime(5);

        // Si no existen datos guardados, se inicializan
        if (!PlayerPrefs.HasKey("MyData"))
        {
            Init();
        }
    }

    void Init()
    {
        // Crea un dato inicial en PlayerPrefs
        PlayerPrefs.SetInt("MyData", 1);

        // Establece todos los logros en 0 (bloqueados)
        foreach (Achive achive in achives)
        {
            PlayerPrefs.SetInt(achive.ToString(), 0);
        }
    }

    private void Start()
    {
        // Verifica qué personajes están desbloqueados al iniciar
        UnlockCharacter();
    }

    void UnlockCharacter()
    {
        // Activa o desactiva los personajes según su estado de desbloqueo
        for (int i = 0; i < lockCharacter.Length; i++)
        {
            string achiveName = achives[i].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;
            lockCharacter[i].SetActive(!isUnlock);
            unlockCharacter[i].SetActive(isUnlock);
        }
    }

    void LateUpdate()
    {
        // Revisa en cada frame si algún logro ha sido alcanzado
        foreach (Achive achive in achives)
        {
            CheckAchive(achive);
        }
    }

    void CheckAchive(Achive achive)
    {
        bool isAchived = false;

        // Verifica si se cumplen las condiciones para desbloquear un logro
        switch (achive)
        {
            case Achive.UnlockCharacter2:
                isAchived = GameManager.instance.kill >= 1000; // Se desbloquea al matar 1000 enemigos
                break;
            case Achive.UnlockCharacter3:
                isAchived = GameManager.instance.gameTime == GameManager.instance.maxGameTime; // Se desbloquea al llegar al tiempo máximo
                break;
        }

        // Si el logro se cumple y aún no ha sido desbloqueado, lo guarda y muestra una notificación
        if (isAchived && PlayerPrefs.GetInt(achive.ToString()) == 0)
        {
            PlayerPrefs.SetInt(achive.ToString(), 1);

            // Muestra la notificación correspondiente al logro desbloqueado
            for (int i = 0; i < uiNotice.transform.childCount; i++)
            {
                bool isActive = i == (int)achive;
                uiNotice.transform.GetChild(i).gameObject.SetActive(isActive);
            }
            StartCoroutine(NoticeRoutine());
        }
    }

    // Muestra la notificación del logro desbloqueado por 5 segundos
    IEnumerator NoticeRoutine()
    {
        uiNotice.SetActive(true);
        yield return wait;
        uiNotice.SetActive(false);
    }
}
