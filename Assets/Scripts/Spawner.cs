using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Puntos de generación de enemigos
    public Transform[] spawnPoint;
    // Datos de los enemigos a spawnear según el nivel
    public SpawnData[] spawnData;
    // Duración de cada nivel
    public float leveltime;

    int level; // Nivel actual del juego
    float timer; // Temporizador para el spawn de enemigos

    private void Awake()
    {
        // Obtiene todos los puntos de spawn hijos de este objeto
        spawnPoint = GetComponentsInChildren<Transform>();
        // Calcula el tiempo de nivel dividiendo el tiempo máximo del juego por la cantidad de tipos de spawn
        leveltime = GameManager.instance.maxGameTime / spawnData.Length;
    }

    void Update()
    {
        // Si el juego no está activo, no hacer nada
        if (!GameManager.instance.isLive)
            return;

        // Actualiza el temporizador
        timer += Time.deltaTime;
        // Calcula el nivel actual basado en el tiempo de juego
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / leveltime), spawnData.Length - 1);

        // Si ha pasado el tiempo necesario, genera un enemigo
        if (timer > spawnData[level].spawnTime)
        {
            timer = 0;
            Spawn();
        }
    }

    // Método para generar un enemigo en un punto aleatorio
    void Spawn()
    {
        // Obtiene un enemigo del pool de objetos
        GameObject enemy = GameManager.instance.pool.Get(0);
        // Asigna una posición aleatoria dentro de los puntos de spawn (evitando el índice 0 que es el padre)
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        // Inicializa el enemigo con los datos del nivel actual
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
    }
}

// Clase serializable para definir los datos de spawn de los enemigos
[System.Serializable]
public class SpawnData
{
    public float spawnTime; // Tiempo entre spawns de enemigos
    public int spriteType; // Tipo de sprite del enemigo
    public int health; // Vida del enemigo
    public float speed; // Velocidad del enemigo
}
