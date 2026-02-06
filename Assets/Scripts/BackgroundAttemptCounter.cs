using UnityEngine;
using TMPro;
using UnityEngine.UI; // Si usas TextMeshPro

public class BackgroundAttemptCounter : MonoBehaviour
{
    private TextMeshProUGUI attemptText;

    private void Start()
    {
        attemptText = GetComponent<TextMeshProUGUI>();
        int attempts = PlayerPrefs.GetInt("Attempts", 0);
        attemptText.text = "ATTEMPTS: " + attempts.ToString();
    }
}
