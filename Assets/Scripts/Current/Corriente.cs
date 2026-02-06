using UnityEngine;

public class Corriente : MonoBehaviour
{
    [SerializeField] private Vector2 currentDirection = new Vector2(1, 0);
    [SerializeField] private float currentStrength = 10f; 
    
    private GameObject player;
    private Rigidbody2D rb;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = player.GetComponent<Rigidbody2D>();
    }


    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            rb.AddForce(currentDirection.normalized * currentStrength, ForceMode2D.Force);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            rb.linearDamping = 0f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            rb.linearDamping = 2f;
        }
    }
}
