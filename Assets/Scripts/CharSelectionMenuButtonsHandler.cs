using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class CharSelectionMenuButtonsHandler : MonoBehaviour
{
    ulong myId;

    [Header("Character Stats Assets")]
    [SerializeField] private PlayerStats[] characterStats;

    [Header("UI")]
    // Arrastra aquí el botón de "Empezar Partida" que vas a crear en el Canvas
    [SerializeField] private GameObject startButton;

    private void Start()
    {
        myId = NetworkManager.Singleton.LocalClientId;

        int safeIndex = (int)(myId % (ulong)characterStats.Length);

        if (NetworkManager.Singleton.IsServer)
        {
            startButton.SetActive(true);
        }
        else
        {   
            selectCharacterAndStartGame(characterStats[safeIndex]);
            startButton.SetActive(false); 
        }
    }

    /// <summary>
    /// Vuelve al menú principal desde la pantalla de selección de personaje.
    /// </summary>
    public void OnBackButtonClicked()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            NetworkManager.Singleton.Shutdown();
            NetworkManager.Singleton.SceneManager.LoadScene(SceneNames.MainMenu, LoadSceneMode.Single);
        }
    }

    public void OnStartButtonClicked()
    {
        int safeIndex = (int)(myId % (ulong)characterStats.Length);
        selectCharacterAndStartGame(characterStats[safeIndex]);
    }


    /*
    public void OnStartButtonClicked()
    {
        selectCharacterAndStartGame(characterStats[myId]);
    }
    /// <summary>
    /// Selecciona el personaje verde e inicia la partida.
    /// </summary>

    public void OnGreenButtonClicked()
    {
        selectCharacterAndStartGame(greenCharacterStats);
    }

    /// <summary>
    /// Selecciona el personaje morado e inicia la partida.
    /// </summary>
    public void OnPurpleButtonClicked()
    {
        selectCharacterAndStartGame(purpleCharacterStats);
    }

    /// <summary>
    /// Selecciona el personaje rojo e inicia la partida.
    /// </summary>
    public void OnRedButtonClicked()
    {
        selectCharacterAndStartGame(redCharacterStats);
    }

    /// <summary>
    /// Selecciona el personaje amarillo e inicia la partida.
    /// </summary>
    public void OnYellowButtonClicked()
    {
        selectCharacterAndStartGame(yellowCharacterStats);
    }
    */

    /// <summary>
    /// Valida la selección del personaje y delega el inicio de partida en GameManager.
    /// </summary>
    private void selectCharacterAndStartGame(PlayerStats characterStats)
    {
        if (characterStats == null)
        {
            Debug.LogError("[CharSelection] No se ha asignado PlayerStats para este personaje");
            return;
        }

        GameManager.Instance?.StartGame(characterStats);
    }
}
