using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

public class Enemy : MonoBehaviour
{
    // Variables públicas que pueden ser ajustadas en el inspector
    public float speed; // Velocidad de movimiento del enemigo
    public float health; // Salud actual del enemigo
    public float maxHealth; // Salud máxima del enemigo
    public RuntimeAnimatorController[] animCon; // Controladores de animación para diferentes tipos de enemigos
    public Rigidbody2D target; // Referencia al jugador (target) a seguir

    // Estado del enemigo (si está vivo o muerto)
    bool isLive = true;

    // Componentes del enemigo
    Rigidbody2D rigid; // Componente Rigidbody2D para el movimiento
    Collider2D coll; // Collider2D para detectar colisiones
    Animator anim; // Componente Animator para las animaciones
    SpriteRenderer spriter; // Componente SpriteRenderer para manipular el sprite
    WaitForFixedUpdate wait; // Para esperar la siguiente actualización fija, utilizado en el retroceso

    // Método de inicialización (se llama una vez cuando el objeto es creado)
    void Awake()
    {
        // Obtener referencias a los componentes del enemigo
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate(); // Inicializar el objeto de espera para FixedUpdate
    }

    // Método llamado en cada actualización física (movimiento del enemigo)
    void FixedUpdate()
    {
        // Si el juego no está en marcha o el enemigo no está vivo, no hacer nada
        if (!GameManager.instance.isLive)
            return;

        // Si el enemigo está muerto o está en la animación de ser golpeado, no hacer nada
        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;

        // Calcular la dirección hacia el jugador (target)
        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime; // Movimiento normalizado según la velocidad

        // Mover al enemigo en la dirección calculada
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero; // Detener la velocidad de Rigidbody2D
    }

    // Método que ajusta la dirección del sprite para que se vea mirando hacia el jugador
    void LateUpdate()
    {
        // Si el juego no está en marcha o el enemigo no está vivo, no hacer nada
        if (!GameManager.instance.isLive)
            return;

        // Si el enemigo no está vivo, no hacer nada
        if (!isLive)
            return;

        // Si la posición del jugador está a la izquierda del enemigo, voltear el sprite
        spriter.flipX = target.position.x < rigid.position.x;
    }

    // Método que se llama cuando un enemigo es reciclado (se activa nuevamente)
    private void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>(); // Obtener la referencia al jugador
        isLive = true; // El enemigo está vivo
        coll.enabled = true; // Habilitar el collider
        rigid.simulated = true; // Habilitar simulación física en Rigidbody2D
        spriter.sortingOrder = 2; // Establecer el orden de renderizado del sprite
        anim.SetBool("Dead", false); // Asegurarse de que la animación de muerte no está activa
        health = maxHealth; // Restaurar la salud máxima
    }

    // Inicializar el enemigo con datos específicos
    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType]; // Establecer el controlador de animación según el tipo de sprite
        speed = data.speed; // Establecer la velocidad
        maxHealth = data.health; // Establecer la salud máxima
        health = data.health; // Establecer la salud actual
    }

    // Método que maneja las colisiones con las armas (en este caso, balas)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si la colisión no es con una bala o si el enemigo no está vivo, no hacer nada
        if (!collision.CompareTag("Bullet") || !isLive)
            return;

        // Reducir la salud del enemigo con el daño de la bala
        health -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack()); // Iniciar el retroceso (knockback)

        // Si el enemigo aún tiene salud, reproducir la animación de "golpeado"
        if (health > 0)
        {
            anim.SetTrigger("Hit"); // Activar la animación de golpeado
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit); // Reproducir el sonido de golpe
        }
        else
        {
            // El enemigo ha muerto
            isLive = false;
            coll.enabled = false; // Deshabilitar el collider para evitar más colisiones
            rigid.simulated = false; // Detener la simulación física del enemigo
            spriter.sortingOrder = 1; // Cambiar el orden de renderizado del sprite (lo pone detrás de otros objetos)
            anim.SetBool("Dead", true); // Activar la animación de muerte

            // Si el juego sigue en marcha, sumar un "kill" y experiencia
            if (GameManager.instance.isLive)
            {
                GameManager.instance.kill++;
                GameManager.instance.GetExp(); // Aumentar la experiencia del jugador
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead); // Reproducir el sonido de muerte
            }
        }
    }

    // Método para aplicar retroceso cuando el enemigo es golpeado
    IEnumerator KnockBack()
    {
        yield return wait; // Espera hasta la siguiente actualización física
        Vector3 playerPos = GameManager.instance.player.transform.position; // Obtener la posición del jugador
        Vector3 dirVec = transform.position - playerPos; // Calcular la dirección del retroceso
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse); // Aplicar la fuerza de retroceso
    }

    // Método que se llama cuando el enemigo muere
    void Dead()
    {
        gameObject.SetActive(false); // Desactivar el objeto del enemigo (lo "destruye")
    }
}
