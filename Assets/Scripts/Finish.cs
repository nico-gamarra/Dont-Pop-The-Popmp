using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    [SerializeField] private GameObject timer;

    private Timer timerScript;

    private void Start()
    {
        timerScript = timer.GetComponent<Timer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerPrefs.SetString("LastLevel", SceneManager.GetActiveScene().name);
            timerScript.EndGame();
            SceneManager.LoadScene("Score");
        }
    }

}
