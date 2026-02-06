using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPausa : MonoBehaviour
{
    [SerializeField] private Timer timer;
    [SerializeField] private SceneTransition transition;
    public GameObject MenuPausaPanel; // Asigna el panel del men� de pausa en el Inspector
    private bool isPaused = false;
    private bool preventClick = false; // Nueva bandera para evitar clics
    public string uniqueName = "UniquePrefab";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        PlayerPrefs.SetInt("Paused", 1);
        MenuPausaPanel.SetActive(true); // Activa el men� de pausa
        Time.timeScale = 0f;            // Detiene el tiempo del juego
        isPaused = true;
        timer.PauseTimer();
    }

    public void ResumeGame()
    {
        PlayerPrefs.SetInt("Paused", 0);
        MenuPausaPanel.SetActive(false); // Desactiva el men� de pausa
        Time.timeScale = 1f;             // Restaura el tiempo del juego
        isPaused = false;
        timer.ResumeTimer();

        preventClick = true; // Activa la bandera para ignorar clics
        Invoke(nameof(ResetPreventClick), 0.1f); // Espera 0.1 segundos para permitir clics
    }

    private void ResetPreventClick()
    {
        preventClick = false; // Restablece la capacidad de hacer clic
    }

    public void LoadMainMenu()
    {
        PlayerPrefs.SetInt("Paused", 0);
        Time.timeScale = 1f;    // Restaura el tiempo antes de cargar la escena
        GameObject existingObject = GameObject.Find(uniqueName);
        Destroy(existingObject);
        PlayerPrefs.SetInt("CantidadIntentos", 0);
        string nombreEscena = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        PlayerPrefs.SetInt(nombreEscena, 0);
        PlayerPrefs.Save();
        transition.StartGame("MenuNiveles");
        timer.RestartTimer();
    }

    public void RestartGame()
    {
        PlayerPrefs.SetInt("Paused", 0);
        MenuPausaPanel.SetActive(false);

        // Obtener la escena actual y cargarla nuevamente
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);

        Time.timeScale = 1f;
        isPaused = false;
        timer.RestartTimer();
        PlayerPrefs.SetInt("CantidadIntentos", 0);
        PlayerPrefs.Save();
    }

    public bool IsPaused()
    {
        return isPaused;
    }

    public bool PreventClick() // M�todo para consultar el estado de preventClick
    {
        return preventClick;
    }
}
