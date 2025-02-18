using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;  // Instancia est�tica de AudioManager

    [Header("#BGM")]
    public AudioClip bgmClip;  // Clip de m�sica de fondo
    public float bgmVolume;    // Volumen de la m�sica de fondo
    AudioSource bgmPlayer;     // Componente AudioSource para la m�sica de fondo
    AudioHighPassFilter bgmEffect;  // Filtro de paso alto para efectos de audio

    [Header("#SFX")]
    public AudioClip[] sfxClips;  // Clips de efectos de sonido (SFX)
    public float sfxVolume;       // Volumen de los efectos de sonido
    public int channels;          // N�mero de canales para reproducir SFX
    AudioSource[] sfxPlayers;     // Array de AudioSources para los efectos de sonido
    int channelIndex;             // �ndice del canal actual para SFX

    // Enumeraci�n para los diferentes efectos de sonido disponibles
    public enum Sfx { Dead, Hit, LevelUp = 3, Lose, Melee, Range = 7, Select, Win };

    private void Awake()
    {
        instance = this;  // Asigna la instancia del AudioManager
        Init();            // Llama al m�todo de inicializaci�n
    }

    // Inicializa los componentes de audio (BGM y SFX)
    private void Init()
    {
        // Crea un objeto para el reproductor de BGM
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;  // Establece el objeto como hijo de AudioManager
        bgmPlayer = bgmObject.AddComponent<AudioSource>();  // A�ade el componente AudioSource
        bgmPlayer.playOnAwake = false;  // No reproducir autom�ticamente al despertar
        bgmPlayer.loop = true;          // Reproducir en bucle
        bgmPlayer.volume = bgmVolume;   // Establecer el volumen
        bgmPlayer.clip = bgmClip;       // Asignar el clip de m�sica de fondo
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();  // Obtener el filtro de paso alto de la c�mara principal

        // Crea un objeto para el reproductor de SFX
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;  // Establece el objeto como hijo de AudioManager
        sfxPlayers = new AudioSource[channels];  // Crea un array para los reproductores de SFX

        // Inicializa los reproductores de SFX
        for (int i = 0; i < sfxPlayers.Length; i++)
        {
            sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();  // A�ade un AudioSource por cada canal
            sfxPlayers[i].playOnAwake = false;  // No reproducir autom�ticamente al despertar
            sfxPlayers[i].bypassListenerEffects = true;  // Evita efectos de escucha en estos AudioSources
            sfxPlayers[i].volume = sfxVolume;  // Establece el volumen de SFX
        }
    }

    // Reproduce o detiene la m�sica de fondo (BGM)
    public void PlayBgm(bool isPlay)
    {
        if (isPlay)
        {
            bgmPlayer.Play();  // Reproduce la m�sica de fondo
        }
        else
        {
            bgmPlayer.Stop();  // Detiene la m�sica de fondo
        }
    }

    // Activa o desactiva el efecto de paso alto en la m�sica de fondo
    public void EffectBgm(bool isPlay)
    {
        bgmEffect.enabled = isPlay;  // Activa o desactiva el filtro de paso alto
    }

    // Reproduce un efecto de sonido (SFX) espec�fico
    public void PlaySfx(Sfx sfx)
    {
        // Recorre todos los reproductores de SFX para encontrar uno que no est� en uso
        for (int i = 0; i < sfxPlayers.Length; i++)
        {
            int loopIndex = (i + channelIndex) % sfxPlayers.Length;  // Usa un �ndice c�clico para los canales

            if (sfxPlayers[loopIndex].isPlaying)
                continue;  // Si el canal ya est� reproduciendo, pasa al siguiente

            int ranIndex = 0;
            // Si el efecto de sonido es "Hit" o "Melee", elige un �ndice aleatorio entre 0 y 1
            if (sfx == Sfx.Hit || sfx == Sfx.Melee)
            {
                ranIndex = Random.Range(0, 2);
            }

            channelIndex = loopIndex;  // Actualiza el �ndice del canal
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx + ranIndex];  // Asigna el clip de sonido correspondiente
            sfxPlayers[loopIndex].Play();  // Reproduce el sonido
            break;  // Sale del ciclo despu�s de reproducir un efecto
        }
    }
}
