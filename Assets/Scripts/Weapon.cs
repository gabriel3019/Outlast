using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Propiedades del arma
    public int id; // Identificador del arma
    public int prefabId; // Identificador del prefab del proyectil
    public float damage; // Daño del arma
    public int count; // Número de proyectiles
    public float speed; // Velocidad de ataque o rotación

    float timer; // Temporizador para gestionar los disparos
    Player player; // Referencia al jugador

    void Awake()
    {
        // Obtiene la instancia del jugador desde el GameManager al iniciar el arma
        player = GameManager.instance.player;
    }

    void Update()
    {
        // Si el juego no está activo, no hacer nada
        if (!GameManager.instance.isLive)
            return;

        // Comportamiento del arma según su id
        switch (id)
        {
            case 0:
                // Rotación constante si el arma es del tipo 0 (por ejemplo, un arma giratoria)
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            default:
                // Manejo del temporizador para disparar a intervalos regulares
                timer += Time.deltaTime;
                if (timer > speed)
                {
                    timer = 0f;
                    Fire(); // Dispara un proyectil
                }
                break;
        }
    }

    // Mejora el arma aumentando el daño y la cantidad de proyectiles
    public void LevelUp(float damage, int count)
    {
        this.damage = damage * Character.Damage; // Aumenta el daño según el personaje
        this.count += count; // Incrementa el número de proyectiles

        if (id == 0)
            Batch(); // Si es un arma giratoria, actualiza los proyectiles

        // Notifica al jugador para aplicar la mejora
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    // Inicializa el arma con los datos del ítem
    public void Init(ItemData data)
    {
        // Configuración básica del arma
        name = "Weapon " + data.itemId;
        transform.parent = player.transform; // Asigna el arma al jugador
        transform.localPosition = Vector3.zero; // Posición inicial

        // Configuración de las propiedades del arma
        id = data.itemId;
        damage = data.baseDamage * Character.Damage;
        count = data.baseCount + Character.Count;

        // Asigna el prefab correspondiente al proyectil
        for (int i = 0; i < GameManager.instance.pool.prefabs.Length; i++)
        {
            if (data.projectile == GameManager.instance.pool.prefabs[i])
            {
                prefabId = i;
                break;
            }
        }

        // Configura la velocidad del arma según su tipo
        switch (id)
        {
            case 0:
                speed = 150 * Character.WeaponSpeed;
                Batch(); // Genera los proyectiles
                break;
            default:
                speed = 0.5f * Character.WeaponRate;
                break;
        }

        // Asigna el sprite del arma a la mano del jugador
        Hand hand = player.hands[(int)data.itemType];
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);

        // Notifica al jugador para aplicar la mejora
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    // Genera múltiples proyectiles alrededor del jugador
    void Batch()
    {
        for (int i = 0; i < count; i++)
        {
            Transform bullet;

            if (i < transform.childCount)
            {
                bullet = transform.GetChild(i);
            }
            else
            {
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                bullet.parent = transform;
            }

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            // Distribuye los proyectiles en un círculo
            Vector3 rotVec = Vector3.forward * 360 * i / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);

            bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero);
        }
    }

    // Dispara un proyectil hacia el objetivo más cercano
    void Fire()
    {
        if (!player.scanner.nearestTarget)
            return; // No dispara si no hay objetivo

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);

        bullet.GetComponent<Bullet>().Init(damage, count, dir);

        // Reproduce el sonido del disparo
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);
    }
}
