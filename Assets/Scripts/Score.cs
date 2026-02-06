using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [Header("Config")] [SerializeField] private TMP_Text timeText; // 0 - 99... s
    [SerializeField] private TMP_Text scoreText; // 0 - 99... pts
    [SerializeField] private TMP_Text gradeText; // S, A, B...
    [SerializeField] private int pointsPerBubble; // Puntos por cada burbuja obtenida
    [SerializeField] private float pointCountDuiration = 3f;
    [SerializeField] private float bubblePointCountDuration = 0.75f;

    [Header("Grades")] 
    [SerializeField] private LevelGrading levelGrades;

    [Header("Bubbles")]
    [SerializeField] private Image bubble1;
    [SerializeField] private Image bubble2;
    [SerializeField] private Image bubble3;
    private Image[] bubblesImages;
    [SerializeField] private Sprite filledBubble;
    [SerializeField] private Sprite goldenBubble;

    [Header("Sound")]
    [SerializeField] private AudioSource bubbleSound;
    [SerializeField] private AudioSource pointsSound;

    private int finalScore;
    private float finalTime;
    private bool enableButtons = false;

    private void Awake()
    {
        bubblesImages = new Image[] { bubble1, bubble2, bubble3 };
    }

    private void Start()
    {
        finalTime = PlayerPrefs.GetFloat("FinalTime", 0);
        StartCoroutine(MostrarPantallaFinal());
    }

    private IEnumerator MostrarPantallaFinal()
    {
        // Show time in UI
        if (timeText != null)
        {
            timeText.text = "TIME: " + finalTime.ToString("F2");
        }
        
        CalculatePointsFromTime();

        pointsSound.Play();

        // Show points from time
        yield return StartCoroutine(ShowScoreProgressively(0, pointCountDuiration));

        // Add points from bubbles
        int cantBurbujas = PlayerPrefs.GetInt("NumberOfBubbles", 0);
        yield return StartCoroutine(AddPointsPerBubble(cantBurbujas));

        pointsSound.Pause();

        // Show final grades
        if (gradeText != null)
        {
            string notaFinal = NotaFinal(finalScore);
            gradeText.text = notaFinal;
            string nivel = PlayerPrefs.GetString("CurrentLevel");
            int maxScore = PlayerPrefs.GetInt("Score" + nivel, 0);
            float bestTime = PlayerPrefs.GetFloat("Time" + nivel, 0);
            if (finalScore > maxScore) {
                PlayerPrefs.SetInt("Score" + nivel, finalScore);
                PlayerPrefs.SetString("Grade" + nivel, notaFinal);
            }

            if (finalTime != 0 && bestTime == 0) {
                PlayerPrefs.SetFloat("Time" + nivel, finalTime);
            }
            if (finalTime < bestTime) {
                PlayerPrefs.SetFloat("Time" + nivel, finalTime);
            }
            bubbleSound.Play();
            StartCoroutine(ScaleAnimation(gradeText.transform));
            enableButtons = true;
        }
    }

    private IEnumerator AddPointsPerBubble(int numberOfBubbles)
    {
        for (int i = 0; i < numberOfBubbles; i++)
        {
            if (i < 2)
            {
                ChangeImage(bubblesImages[i], filledBubble);
            } 
            else 
            {
                ChangeImage(bubblesImages[0], goldenBubble);
                ChangeImage(bubblesImages[1], goldenBubble);
                ChangeImage(bubblesImages[2], goldenBubble);
            }

            int previousScore = finalScore;
            finalScore += pointsPerBubble;

            yield return StartCoroutine(ShowScoreProgressively(previousScore, bubblePointCountDuration));
        }
    }

    private IEnumerator ShowScoreProgressively(int currentScore, float duration)
    {
        if (scoreText != null)
        {
            scoreText.text = $"SCORE: {currentScore}";
        }

        int initialScore = currentScore;
        int pointDifference = finalScore - initialScore;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / duration);

            int interpolatedScore = initialScore + Mathf.RoundToInt(pointDifference * progress);

            if (scoreText != null)
            {
                scoreText.text = $"SCORE: {interpolatedScore}";
            }

            yield return null;
        }

        if (scoreText != null)
        {
            scoreText.text = $"SCORE: {finalScore}";
        }
    }

    private void ChangeImage(Image target, Sprite sprite)
    {
        if (target != null)
        {
            if (bubbleSound != null)
            {
                bubbleSound.Play();
            }

            target.sprite = sprite;

            StartCoroutine(ScaleAnimation(target.transform));
        }
    }

    private IEnumerator ScaleAnimation(Transform target)
    {
        if (target == null) yield break;

        Vector3 originalScale = target.localScale;
        Vector3 enlargedScale = originalScale * 1.2f;

        float duration = 0.2f;
        float elapsedTime = 0f;
        
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / duration;
            target.localScale = Vector3.Lerp(originalScale, enlargedScale, progress);
            yield return null;
        }

        elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / duration;
            target.localScale = Vector3.Lerp(enlargedScale, originalScale, progress);
            yield return null;
        }

        target.localScale = originalScale;
    }

    private string NotaFinal(int puntuacion)
    {
        if (puntuacion >= levelGrades.pointsForS)
        {
            return "S";
        }
        
        if (puntuacion >= levelGrades.pointsForA)
        {
            return "A";
        }
        
        if (puntuacion >= levelGrades.pointsForB)
        {
            return "B";
        }
        
        if (puntuacion >= levelGrades.pointsForC)
        {
            return "C";
        }
        
        if (puntuacion >= levelGrades.pointsForD)
        {
            return "D";
        }

        if (puntuacion >= levelGrades.pointsForE)
        {
            return "E";
        }

        return "F";
    }

    public void CalculatePointsFromTime()
    {
        if (finalTime < 148.295f)
        {
            finalScore = Mathf.FloorToInt((18759 - (3752.4f * Mathf.Log(finalTime)))/10);
        }
        else
        {
            finalScore = 0;
        }
    }
    
    public void RestartGame()
    {
        if (enableButtons) {
            PlayerPrefs.SetInt("Paused", 0);
            string lastLevel = PlayerPrefs.GetString("LastLevel", "Nivel1");
            SceneManager.LoadScene(lastLevel);
            Time.timeScale = 1f;
            PlayerPrefs.SetInt("Attempts", 0);
        }
        
    }
    
    public void LoadMainMenu()
    {
        if (enableButtons) {
            PlayerPrefs.SetInt("Paused", 0);
            Time.timeScale = 1f;
            GameObject prefMenu = GameObject.Find("UniquePrefab");
            Destroy(prefMenu);
            SceneManager.LoadScene("MenuNiveles");
        }
        
    }
}
