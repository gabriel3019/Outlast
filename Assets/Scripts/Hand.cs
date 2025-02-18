using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    // Variable que indica si la mano es la izquierda (true) o la derecha (false)
    public bool isLeft;
    // SpriteRenderer para la mano
    public SpriteRenderer spriter;

    // Referencia al SpriteRenderer del jugador (para verificar si está mirando hacia la izquierda o derecha)
    SpriteRenderer player;

    // Posiciones predefinidas para la mano derecha (normal y en reversa)
    Vector3 rightPos = new Vector3(0.35f, -0.15f, 0);
    Vector3 rightPosReverse = new Vector3(-0.15f, -0.15f, 0);

    // Rotaciones predefinidas para la mano izquierda (normal y en reversa)
    Quaternion leftRot = Quaternion.Euler(0, 0, -35);
    Quaternion leftRotReverse = Quaternion.Euler(0, 0, -135);

    // Método llamado al iniciar el objeto, antes de que se ejecute cualquier otro comportamiento
    private void Awake()
    {
        // Obtiene el SpriteRenderer del jugador, suponiendo que está en el segundo índice de los componentes del objeto padre
        player = GetComponentsInParent<SpriteRenderer>()[1];
    }

    // Método que se llama después de la actualización de cada fotograma
    private void LateUpdate()
    {
        // Verifica si el jugador está mirando hacia la izquierda (flipX es verdadero) o hacia la derecha (flipX es falso)
        bool isReverse = player.flipX;

        // Si la mano es izquierda
        if (isLeft)
        {
            // Ajusta la rotación de la mano izquierda dependiendo de la dirección del jugador
            transform.localRotation = isReverse ? leftRotReverse : leftRot;
            // Si el jugador está mirando hacia la izquierda, invierte el sprite de la mano
            spriter.flipY = isReverse;
            // Cambia el orden de los sprites (de qué capa se dibuja) según la dirección del jugador
            spriter.sortingOrder = isReverse ? 4 : 6;
        }
        else
        {
            // Si la mano no es izquierda (es derecha), ajusta la posición de la mano
            transform.localPosition = isReverse ? rightPosReverse : rightPos;
            // Si el jugador está mirando hacia la izquierda, invierte el sprite de la mano derecha
            spriter.flipX = isReverse;
            // Cambia el orden de los sprites (de qué capa se dibuja) según la dirección del jugador
            spriter.sortingOrder = isReverse ? 6 : 4;
        }
    }
}