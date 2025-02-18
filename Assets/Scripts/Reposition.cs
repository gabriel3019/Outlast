using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    // Referencia al componente Collider2D del objeto
    Collider2D coll;

    void Awake()
    {
        // Obtiene el Collider2D del objeto al iniciar
        coll = GetComponent<Collider2D>();
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        // Si el objeto que sale del área no es la zona límite, no hacer nada
        if (!collision.CompareTag("Area"))
            return;

        // Posición del jugador y del objeto actual
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;

        switch (transform.tag)
        {
            case "Ground":
                // Calcula la dirección en la que se moverá el terreno al salir del área
                float diffX = playerPos.x - myPos.x;
                float diffY = playerPos.y - myPos.y;
                float dirX = diffX < 0 ? -1 : 1;
                float dirY = diffY < 0 ? -1 : 1;
                diffX = Mathf.Abs(diffX);
                diffY = Mathf.Abs(diffY);

                // Mueve el terreno en la dirección correspondiente para reposicionarlo
                if (diffX > diffY)
                {
                    transform.Translate(Vector3.right * dirX * 40);
                }
                else if (diffX < diffY)
                {
                    transform.Translate(Vector3.up * dirY * 40);
                }
                break;

            case "Enemy":
                // Si el collider está activo, reposiciona al enemigo cerca del jugador
                if (coll.enabled)
                {
                    Vector3 dist = playerPos - myPos;
                    Vector3 ran = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
                    transform.Translate(ran + dist * 2);
                }
                break;
        }
    }
}
