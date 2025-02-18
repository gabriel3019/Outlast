using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Outlast"); // Cambia el nombre por el de tu escena del juego
    }

    public void ShowCredits()
    {
        SceneManager.LoadScene("Creditos"); // Cambia "Creditos" por el nombre de tu escena de créditos
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
