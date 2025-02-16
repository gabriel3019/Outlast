using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreManager : MonoBehaviour
{
    private string highScoreFilePath;
    public Text highScoreText;

    void Start()
    {
        // Ruta del archivo donde se guardar� el r�cord
        highScoreFilePath = Application.persistentDataPath + "/highscore.txt";

        // Cargar el r�cord al inicio del juego
        int highScore = LoadHighScore();
        highScoreText.text = ("Record: " + highScore);
    }

    public void SaveHighScore(int kill)
    {
        // Guardar la puntuaci�n solo si es mayor que la actual
        if (kill > LoadHighScore())
        {
            using (StreamWriter writer = new StreamWriter(highScoreFilePath))
            {
                writer.WriteLine(kill); // Escribir la nueva puntuaci�n
            }
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
        return 0; // Si no hay archivo, devolver 0 o un valor por defecto
    }
}