using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    // Referencia al RectTransform del objeto para poder manipular su posición en la UI
    RectTransform rect;

    // Método que se ejecuta al inicializar el objeto
    void Awake()
    {
        // Obtiene el RectTransform del objeto al que este script está asignado
        rect = GetComponent<RectTransform>();
    }

    // Método que se ejecuta en cada actualización física (FixedUpdate se usa para actualizar el movimiento de objetos)
    void FixedUpdate()
    {
        // Actualiza la posición del RectTransform para que siga la posición del jugador
        // Convierte la posición mundial del jugador en la cámara a coordenadas de pantalla (UI)
        rect.position = Camera.main.WorldToScreenPoint(GameManager.instance.player.transform.position);
    }
}
