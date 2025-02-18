using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreManager : MonoBehaviour
{
    // Ruta del archivo donde se guardará el récord de puntuación
    private string highScoreFilePath;
    // Referencia al texto donde se mostrará el récord en la UI
    public Text highScoreText;

    // Método que se llama al iniciar el juego
    void Start()
    {
        // Establece la ruta del archivo para guardar el récord usando la ruta persistente de la aplicación
        highScoreFilePath = Application.persistentDataPath + "/highscore.txt";

        // Cargar el récord al inicio del juego y mostrarlo en el UI
        int highScore = LoadHighScore();
        highScoreText.text = ("Record: " + highScore);
    }

    // Método para guardar un nuevo récord si la puntuación es mayor que la actual
    public void SaveHighScore(int kill)
    {
        // Verifica si la puntuación actual (kill) es mayor que el récord cargado
        if (kill > LoadHighScore())
        {
            // Abre el archivo para escribir la nueva puntuación
            using (StreamWriter writer = new StreamWriter(highScoreFilePath))
            {
                writer.WriteLine(kill); // Escribe la nueva puntuación en el archivo
            }
        }
    }

    // Método para cargar el récord actual desde el archivo
    public int LoadHighScore()
    {
        // Verifica si el archivo de récord existe
        if (File.Exists(highScoreFilePath))
        {
            // Abre el archivo para leer el valor del récord
            using (StreamReader reader = new StreamReader(highScoreFilePath))
            {
                string scoreText = reader.ReadLine(); // Lee la línea que contiene el récord
                // Intenta convertir el valor leído en un número entero
                if (int.TryParse(scoreText, out int highScore))
                {
                    return highScore; // Si se convierte correctamente, devuelve el récord
                }
            }
        }
        // Si el archivo no existe o no se puede leer correctamente, devuelve 0 como valor por defecto
        return 0;
    }
}
