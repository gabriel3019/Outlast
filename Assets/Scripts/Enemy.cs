using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

public class Enemy : MonoBehaviour
{
    // Variables p�blicas que pueden ser ajustadas en el inspector
    public float speed; // Velocidad de movimiento del enemigo
    public float health; // Salud actual del enemigo
    public float maxHealth; // Salud m�xima del enemigo
    public RuntimeAnimatorController[] animCon; // Controladores de animaci�n para diferentes tipos de enemigos
    public Rigidbody2D target; // Referencia al jugador (target) a seguir

    // Estado del enemigo (si est� vivo o muerto)
    bool isLive = true;

    // Componentes del enemigo
    Rigidbody2D rigid; // Componente Rigidbody2D para el movimiento
    Collider2D coll; // Collider2D para detectar colisiones
    Animator anim; // Componente Animator para las animaciones
    SpriteRenderer spriter; // Componente SpriteRenderer para manipular el sprite
    WaitForFixedUpdate wait; // Para esperar la siguiente actualizaci�n fija, utilizado en el retroceso

    // M�todo de inicializaci�n (se llama una vez cuando el objeto es creado)
    void Awake()
    {
        // Obtener referencias a los componentes del enemigo
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate(); // Inicializar el objeto de espera para FixedUpdate
    }

    // M�todo llamado en cada actualizaci�n f�sica (movimiento del enemigo)
    void FixedUpdate()
    {
        // Si el juego no est� en marcha o el enemigo no est� vivo, no hacer nada
        if (!GameManager.instance.isLive)
            return;

        // Si el enemigo est� muerto o est� en la animaci�n de ser golpeado, no hacer nada
        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;

        // Calcular la direcci�n hacia el jugador (target)
        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime; // Movimiento normalizado seg�n la velocidad

        // Mover al enemigo en la direcci�n calculada
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero; // Detener la velocidad de Rigidbody2D
    }

    // M�todo que ajusta la direcci�n del sprite para que se vea mirando hacia el jugador
    void LateUpdate()
    {
        // Si el juego no est� en marcha o el enemigo no est� vivo, no hacer nada
        if (!GameManager.instance.isLive)
            return;

        // Si el enemigo no est� vivo, no hacer nada
        if (!isLive)
            return;

        // Si la posici�n del jugador est� a la izquierda del enemigo, voltear el sprite
        spriter.flipX = target.position.x < rigid.position.x;
    }

    // M�todo que se llama cuando un enemigo es reciclado (se activa nuevamente)
    private void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>(); // Obtener la referencia al jugador
        isLive = true; // El enemigo est� vivo
        coll.enabled = true; // Habilitar el collider
        rigid.simulated = true; // Habilitar simulaci�n f�sica en Rigidbody2D
        spriter.sortingOrder = 2; // Establecer el orden de renderizado del sprite
        anim.SetBool("Dead", false); // Asegurarse de que la animaci�n de muerte no est� activa
        health = maxHealth; // Restaurar la salud m�xima
    }

    // Inicializar el enemigo con datos espec�ficos
    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType]; // Establecer el controlador de animaci�n seg�n el tipo de sprite
        speed = data.speed; // Establecer la velocidad
        maxHealth = data.health; // Establecer la salud m�xima
        health = data.health; // Establecer la salud actual
    }

    // M�todo que maneja las colisiones con las armas (en este caso, balas)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si la colisi�n no es con una bala o si el enemigo no est� vivo, no hacer nada
        if (!collision.CompareTag("Bullet") || !isLive)
            return;

        // Reducir la salud del enemigo con el da�o de la bala
        health -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack()); // Iniciar el retroceso (knockback)

        // Si el enemigo a�n tiene salud, reproducir la animaci�n de "golpeado"
        if (health > 0)
        {
            anim.SetTrigger("Hit"); // Activar la animaci�n de golpeado
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit); // Reproducir el sonido de golpe
        }
        else
        {
            // El enemigo ha muerto
            isLive = false;
            coll.enabled = false; // Deshabilitar el collider para evitar m�s colisiones
            rigid.simulated = false; // Detener la simulaci�n f�sica del enemigo
            spriter.sortingOrder = 1; // Cambiar el orden de renderizado del sprite (lo pone detr�s de otros objetos)
            anim.SetBool("Dead", true); // Activar la animaci�n de muerte

            // Si el juego sigue en marcha, sumar un "kill" y experiencia
            if (GameManager.instance.isLive)
            {
                GameManager.instance.kill++;
                GameManager.instance.GetExp(); // Aumentar la experiencia del jugador
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead); // Reproducir el sonido de muerte
            }
        }
    }

    // M�todo para aplicar retroceso cuando el enemigo es golpeado
    IEnumerator KnockBack()
    {
        yield return wait; // Espera hasta la siguiente actualizaci�n f�sica
        Vector3 playerPos = GameManager.instance.player.transform.position; // Obtener la posici�n del jugador
        Vector3 dirVec = transform.position - playerPos; // Calcular la direcci�n del retroceso
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse); // Aplicar la fuerza de retroceso
    }

    // M�todo que se llama cuando el enemigo muere
    void Dead()
    {
        gameObject.SetActive(false); // Desactivar el objeto del enemigo (lo "destruye")
    }
}
