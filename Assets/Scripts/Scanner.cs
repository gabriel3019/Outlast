using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    // Radio de escaneo para detectar objetivos
    public float scanRange;
    // Capa que define qu� objetos pueden ser detectados
    public LayerMask targetLayer;
    // Lista de objetivos detectados dentro del rango de escaneo
    public RaycastHit2D[] targets;
    // Referencia al objetivo m�s cercano
    public Transform nearestTarget;

    void FixedUpdate()
    {
        // Realiza un escaneo circular en la posici�n actual del objeto
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
        // Determina cu�l es el objetivo m�s cercano
        nearestTarget = GetNearest();
    }

    // M�todo para encontrar el objetivo m�s cercano de la lista de detectados
    Transform GetNearest()
    {
        Transform result = null;
        float diff = 100; // Valor inicial alto para asegurar la comparaci�n

        foreach (RaycastHit2D target in targets)
        {
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;
            float curDiff = Vector3.Distance(myPos, targetPos);

            // Si la distancia es menor que la diferencia actual, actualiza el objetivo m�s cercano
            if (curDiff < diff)
            {
                diff = curDiff;
                result = target.transform;
            }
        }

        return result; // Retorna el objetivo m�s cercano encontrado
    }
}
