using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Instancia est�tica del GameManager para acceso global
    public static GameManager instance;

    [Header("# Game Control")]
    // Bandera que indica si el juego est� en curso
    public bool isLive;
    // Tiempo transcurrido en el juego
    public float gameTime;
    // Tiempo m�ximo permitido para la partida
    public float maxGameTime = 2 * 10f;

    [Header("# Player Info")]
    // Identificador del jugador (por ejemplo, si hay dos jugadores)
    public int playerId;
    // Salud actual del jugador
    public float health;
    // Salud m�xima del jugador
    public float maxHealth = 100;
    // Nivel del jugador
    public int level;
    // N�mero de enemigos eliminados por el jugador
    public int kill;
    // Experiencia del jugador
    public int exp;
    // Umbrales de experiencia necesarios para subir de nivel
    public int[] nextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600 };

    [Header("# Game Object")]
    // Referencia al PoolManager para la gesti�n de objetos del juego
    public PoolManager pool;
    // Referencia al jugador
    public Player player;
    // UI para mostrar la interfaz de subir de nivel
    public LevelUp uiLevelUp;
    // UI para mostrar el resultado final (Victoria o Derrota)
    public Result uiResult;
    // UI del joystick para controlar al jugador
    public Transform uiJoy;
    // Objeto para limpiar enemigos al finalizar el juego
    public GameObject enemyCleaner;
    // Gestor del r�cord de puntuaci�n m�s alta
    public HighScoreManager highScoreManager;
    // UI que muestra el r�cord de puntuaci�n m�s alta
    public GameObject uiRecord;

    // M�todo que se ejecuta al iniciar el juego
    void Awake()
    {
        // Inicializa la instancia del GameManager
        instance = this;
        // Establece la tasa de fotogramas del juego a 60 FPS
        Application.targetFrameRate = 60;
        // Busca el HighScoreManager en la escena
        highScoreManager = FindObjectOfType<HighScoreManager>();
    }

    // M�todo que inicia el juego con un id de jugador
    public void GameStart(int id)
    {
        playerId = id;
        health = maxHealth; // Establece la salud del jugador al m�ximo

        // Activa al jugador y configura la interfaz de nivel
        player.gameObject.SetActive(true);
        uiLevelUp.Select(playerId % 2);
        uiRecord.SetActive(false); // Oculta el UI del r�cord

        // Reanuda el juego y reproduce m�sica de fondo
        Resume();

        // Reproduce los efectos de sonido correspondientes
        AudioManager.instance.PlayBgm(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
    }

    // M�todo que se llama al terminar el juego con derrota
    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    // Rutina de la derrota, espera un momento y muestra la pantalla de resultado
    IEnumerator GameOverRoutine()
    {
        isLive = false; // Marca que el juego ha terminado

        yield return new WaitForSeconds(0.5f); // Espera medio segundo

        // Activa la pantalla de resultado y muestra la derrota
        uiResult.gameObject.SetActive(true);
        uiResult.Lose();
        Stop(); // Detiene el juego

        // Guarda la puntuaci�n m�s alta
        highScoreManager.SaveHighScore(kill);

        // Detiene la m�sica y reproduce el efecto de derrota
        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);
    }

    // M�todo que se llama al terminar el juego con victoria
    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());
    }

    // Rutina de la victoria, limpia los enemigos y muestra la pantalla de resultado
    IEnumerator GameVictoryRoutine()
    {
        isLive = false;
        enemyCleaner.SetActive(true); // Activa el limpiador de enemigos
        yield return new WaitForSeconds(0.5f);

        // Activa la pantalla de resultado y muestra la victoria
        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        Stop(); // Detiene el juego

        // Guarda la puntuaci�n m�s alta
        highScoreManager.SaveHighScore(kill);

        // Detiene la m�sica y reproduce el efecto de victoria
        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);
    }

    // M�todo que recarga la escena actual para intentar de nuevo
    public void GameRetry()
    {
        SceneManager.LoadScene(0); // Recarga la escena del �ndice 0 (principal)
    }

    // M�todo que lleva te hace salir del juego
    public void GameQuit()
    {
        Application.Quit();
    }

    // M�todo que se ejecuta en cada actualizaci�n del juego
    void Update()
    {
        // Si el juego no est� en curso, no hace nada
        if (!isLive)
            return;

        // Aumenta el tiempo de juego
        gameTime += Time.deltaTime;

        // Si se alcanza el tiempo m�ximo, termina el juego con victoria
        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime; // Ajusta el tiempo a su valor m�ximo
            GameVictory();
        }
    }

    // M�todo para ganar experiencia
    public void GetExp()
    {
        // Si el juego no est� en curso, no hace nada
        if (!isLive)
            return;

        exp++; // Incrementa la experiencia

        // Si se alcanza el umbral de experiencia para subir de nivel
        if (exp == nextExp[Mathf.Min(level, nextExp.Length - 1)])
        {
            level++; // Aumenta el nivel
            exp = 0; // Resetea la experiencia
            uiLevelUp.Show(); // Muestra la interfaz de subida de nivel
        }
    }

    // M�todo para detener el juego (pausa)
    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0; // Detiene el tiempo del juego
        uiJoy.localScale = Vector3.zero; // Oculta el joystick
    }

    // M�todo para reanudar el juego (despausa)
    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1; // Reactiva el tiempo del juego
        uiJoy.localScale = Vector3.one; // Muestra el joystick
    }
}
