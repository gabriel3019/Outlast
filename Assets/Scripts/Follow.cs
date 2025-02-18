using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    // Referencia al RectTransform del objeto para poder manipular su posici�n en la UI
    RectTransform rect;

    // M�todo que se ejecuta al inicializar el objeto
    void Awake()
    {
        // Obtiene el RectTransform del objeto al que este script est� asignado
        rect = GetComponent<RectTransform>();
    }

    // M�todo que se ejecuta en cada actualizaci�n f�sica (FixedUpdate se usa para actualizar el movimiento de objetos)
    void FixedUpdate()
    {
        // Actualiza la posici�n del RectTransform para que siga la posici�n del jugador
        // Convierte la posici�n mundial del jugador en la c�mara a coordenadas de pantalla (UI)
        rect.position = Camera.main.WorldToScreenPoint(GameManager.instance.player.transform.position);
    }
}
