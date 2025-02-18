using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreManager : MonoBehaviour
{
    // Ruta del archivo donde se guardar� el r�cord de puntuaci�n
    private string highScoreFilePath;
    // Referencia al texto donde se mostrar� el r�cord en la UI
    public Text highScoreText;

    // M�todo que se llama al iniciar el juego
    void Start()
    {
        // Establece la ruta del archivo para guardar el r�cord usando la ruta persistente de la aplicaci�n
        highScoreFilePath = Application.persistentDataPath + "/highscore.txt";

        // Cargar el r�cord al inicio del juego y mostrarlo en el UI
        int highScore = LoadHighScore();
        highScoreText.text = ("Record: " + highScore);
    }

    // M�todo para guardar un nuevo r�cord si la puntuaci�n es mayor que la actual
    public void SaveHighScore(int kill)
    {
        // Verifica si la puntuaci�n actual (kill) es mayor que el r�cord cargado
        if (kill > LoadHighScore())
        {
            // Abre el archivo para escribir la nueva puntuaci�n
            using (StreamWriter writer = new StreamWriter(highScoreFilePath))
            {
                writer.WriteLine(kill); // Escribe la nueva puntuaci�n en el archivo
            }
        }
    }

    // M�todo para cargar el r�cord actual desde el archivo
    public int LoadHighScore()
    {
        // Verifica si el archivo de r�cord existe
        if (File.Exists(highScoreFilePath))
        {
            // Abre el archivo para leer el valor del r�cord
            using (StreamReader reader = new StreamReader(highScoreFilePath))
            {
                string scoreText = reader.ReadLine(); // Lee la l�nea que contiene el r�cord
                // Intenta convertir el valor le�do en un n�mero entero
                if (int.TryParse(scoreText, out int highScore))
                {
                    return highScore; // Si se convierte correctamente, devuelve el r�cord
                }
            }
        }
        // Si el archivo no existe o no se puede leer correctamente, devuelve 0 como valor por defecto
        return 0;
    }
}
