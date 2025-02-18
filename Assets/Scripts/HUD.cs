using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    // Enum que define los diferentes tipos de información que se pueden mostrar en la HUD.
    public enum InFoType { Exp, Level, Kill, Time, Health }
    public InFoType type; // Tipo de información a mostrar en la HUD (Experiencia, Nivel, Muertes, Tiempo, Salud).

    // Variables privadas para almacenar los componentes UI Text y Slider.
    Text myText;
    Slider mySlider;

    // Método llamado cuando el objeto es inicializado.
    // Aquí se obtienen los componentes UI asociados a este objeto.
    void Awake()
    {
        myText = GetComponent<Text>(); // Obtiene el componente Text, usado para mostrar texto (como nivel o tiempo).
        mySlider = GetComponent<Slider>(); // Obtiene el componente Slider, usado para mostrar barras de progreso (como experiencia o salud).
    }

    // Método llamado una vez por frame después de que todas las actualizaciones se hayan completado.
    // Actualiza la UI en función del tipo de información que debe mostrar.
    void LateUpdate()
    {
        // Dependiendo del tipo de información, actualiza el texto o el valor del slider correspondiente.
        switch (type)
        {
            // Si el tipo de información es experiencia, muestra la barra de progreso de la experiencia.
            case InFoType.Exp:
                // Obtiene la experiencia actual y la experiencia máxima para el nivel actual.
                float curExp = GameManager.instance.exp;
                float maxExp = GameManager.instance.nextExp[Mathf.Min(GameManager.instance.level, GameManager.instance.nextExp.Length - 1)];
                // Actualiza el valor del slider con el porcentaje de experiencia.
                mySlider.value = curExp / maxExp;
                break;

            // Si el tipo es nivel, muestra el nivel actual del jugador.
            case InFoType.Level:
                myText.text = string.Format("Lv.{0:F0}", GameManager.instance.level); // Formatea el texto para mostrar el nivel.
                break;

            // Si el tipo es número de muertes, muestra la cantidad de muertes del jugador.
            case InFoType.Kill:
                myText.text = string.Format("{0:F0}", GameManager.instance.kill); // Formatea el texto para mostrar las muertes.
                break;

            // Si el tipo es tiempo, muestra el tiempo restante en formato mm:ss.
            case InFoType.Time:
                // Calcula el tiempo restante en el juego.
                float remainTime = GameManager.instance.maxGameTime - GameManager.instance.gameTime;
                int min = Mathf.FloorToInt(remainTime / 60); // Calcula los minutos restantes.
                int sec = Mathf.FloorToInt(remainTime % 60); // Calcula los segundos restantes.
                // Actualiza el texto con el tiempo formateado como mm:ss.
                myText.text = string.Format("{0:D2}:{1:D2}", min, sec);
                break;

            // Si el tipo es salud, muestra la barra de progreso de la salud.
            case InFoType.Health:
                // Obtiene la salud actual y la salud máxima del jugador.
                float curHealth = GameManager.instance.health;
                float maxHealth = GameManager.instance.maxHealth;
                // Actualiza el valor del slider con el porcentaje de salud.
                mySlider.value = curHealth / maxHealth;
                break;
        }
    }
}
