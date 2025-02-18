using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;  // Daño que causa la bala
    public int per;       // Contador de impactos restantes (por ejemplo, cuántos enemigos puede afectar antes de desaparecer)

    Rigidbody2D rigid;   // Componente Rigidbody2D para controlar el movimiento de la bala

    void Awake()
    {
        // Inicializa el Rigidbody2D al inicio del juego
        rigid = GetComponent<Rigidbody2D>();
    }

    // Método que inicializa la bala con el daño, la cantidad de impactos y la dirección
    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage;  // Asigna el daño de la bala
        this.per = per;        // Asigna la cantidad de impactos restantes

        // Si la cantidad de impactos es positiva, la bala se mueve en la dirección proporcionada
        if (per >= 0)
        {
            rigid.velocity = dir * 15f;  // Establece la velocidad de la bala (multiplicada por 15 para mayor velocidad)
        }
    }

    // Detecta cuando la bala entra en contacto con otro objeto (como un enemigo)
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Si el objeto con el que colisiona no es un "Enemy" o el contador de impactos es -100, no hace nada
        if (!collision.CompareTag("Enemy") || per == -100)
            return;

        per--;  // Reduce el contador de impactos restantes

        // Si la bala ya no tiene impactos restantes, la detiene y la desactiva
        if (per < 0)
        {
            rigid.velocity = Vector2.zero;  // Detiene el movimiento de la bala
            gameObject.SetActive(false);    // Desactiva la bala
        }
    }

    // Detecta cuando la bala sale del área (por ejemplo, de la pantalla)
    void OnTriggerExit2D(Collider2D collision)
    {
        // Si el objeto con el que la bala ha salido no es un "Area" o el contador de impactos es -100, no hace nada
        if (!collision.CompareTag("Area") || per == -100)
            return;

        gameObject.SetActive(false);  // Desactiva la bala cuando sale de la zona
    }
}
