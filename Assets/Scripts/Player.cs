using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // Vector de entrada para el movimiento del jugador
    public Vector2 inputVec;
    public float speed; // Velocidad de movimiento
    public Scanner scanner; // Detector de enemigos u objetos cercanos
    public Hand[] hands; // Referencias a las manos del jugador (pueden ser armas u objetos que sostiene)
    public RuntimeAnimatorController[] animCon; // Diferentes controladores de animación para el personaje

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;

    void Awake()
    {
        // Obtención de componentes
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        hands = GetComponentsInChildren<Hand>(true); // Obtiene todas las manos (incluidas las inactivas)
    }

    private void OnEnable()
    {
        // Ajusta la velocidad del jugador según la configuración del personaje
        speed *= Character.Speed;
        // Asigna la animación correspondiente según el personaje seleccionado
        anim.runtimeAnimatorController = animCon[GameManager.instance.playerId];
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        // Movimiento basado en teclas (comentado, ya que se usa el nuevo sistema de Input)
        //inputVec.x = Input.GetAxisRaw("Horizontal");
        //inputVec.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        // Calcula el desplazamiento basado en la entrada del jugador
        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }

    void LateUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        // Ajusta la animación de movimiento del personaje
        anim.SetFloat("Speed", inputVec.magnitude);

        // Voltea el sprite en función de la dirección en la que se mueve
        if (inputVec.x != 0)
        {
            spriter.flipX = inputVec.x < 0;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.instance.isLive)
            return;

        // Reduce la vida del jugador si está en contacto con un enemigo u objeto dañino
        GameManager.instance.health -= Time.deltaTime * 10;

        // Si la vida del jugador llega a 0, ejecuta la animación de muerte y termina el juego
        if (GameManager.instance.health < 0)
        {
            // Desactiva los hijos del jugador (probablemente armas u objetos visuales)
            for (int index = 2; index < transform.childCount; index++)
            {
                transform.GetChild(index).gameObject.SetActive(false);
            }

            anim.SetTrigger("Dead");
            GameManager.instance.GameOver();
        }
    }

    // Método del nuevo sistema de Input de Unity para obtener la dirección de movimiento
    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }
}
