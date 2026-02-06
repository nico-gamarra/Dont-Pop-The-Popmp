using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public float timeInSeconds = 0;
    private bool enableTimeCounting = true;
    private TMP_Text timeText;
    bool firstClickPressed;

    private void Awake()
    {
        timeText = GetComponent<TMP_Text>();
        firstClickPressed = false;
    }
    
    void Update()
    {
        if (!firstClickPressed)
        {
            enableTimeCounting = false;
        }
        
        if (Input.GetMouseButton(0))
        {
            firstClickPressed = true;
            enableTimeCounting = true;
        }
        
        if (enableTimeCounting)
        {
            timeInSeconds += Time.deltaTime;

            if (timeText != null)
            {
                timeText.text = (timeInSeconds).ToString("F2");
            }
        }
    }

    public void PauseTimer()
    {
        enableTimeCounting = false;
    }

    public void ResumeTimer()
    {
        enableTimeCounting = true;
    }

    public void RestartTimer()
    {
        timeInSeconds = 0;

        // Opcional: actualiza la UI al reiniciar
        if (timeText != null)
        {
            timeText.text = "0.00";
        }
    }

    public void EndGame()
    {
        PlayerPrefs.SetFloat("finalTime", timeInSeconds);

        SceneManager.LoadScene("Score");
    }
}
