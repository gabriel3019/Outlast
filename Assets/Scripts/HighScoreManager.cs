using System.IO;
using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
    private string highScoreFilePath;

    void Start()
    {
        // Ruta del archivo donde se guardará el récord
        highScoreFilePath = Application.persistentDataPath + "/highscore.txt";

        // Cargar el récord al inicio del juego
        int highScore = LoadHighScore();
        Debug.Log("Récord actual: " + highScore);
    }

    public void SaveHighScore(int kill)
    {
        // Guardar la puntuación solo si es mayor que la actual
        if (kill > LoadHighScore())
        {
            using (StreamWriter writer = new StreamWriter(highScoreFilePath))
            {
                writer.WriteLine(kill); // Escribir la nueva puntuación
            }

            Debug.Log("Nuevo récord guardado: " + kill);
        }
    }

    public int LoadHighScore()
    {
        if (File.Exists(highScoreFilePath))
        {
            using (StreamReader reader = new StreamReader(highScoreFilePath))
            {
                string scoreText = reader.ReadLine();
                if (int.TryParse(scoreText, out int highScore))
                {
                    return highScore;
                }
            }
        }

        Debug.Log("No se encontró un récord previo.");
        return 0; // Si no hay archivo, devolver 0 o un valor por defecto
    }
}