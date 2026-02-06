using UnityEngine;
using UnityEngine.SceneManagement;

public class MovimientoBurbuja : MonoBehaviour
{
    [Header("Parametros de movimiento")]
    [SerializeField] private float pushForce = 10f; // Magnitud de la fuerza aplicada al empujar
    [SerializeField] private float rotationSpeed = 1f; // Velocidad de rotacion de la burbuja
    [SerializeField] private float maxSpeed = 15f; // Velocidad m�xima de la burbuja
    [SerializeField] private float maxRotation = 10f; // Rotaci�n m�xima de la burbuja
    [SerializeField] private float maxDistance = 8f; // Distancia m�xima en que el click tiene efecto
    [SerializeField] private float waterResistance = 2f; // Resistencia del agua
    [SerializeField] private float gravedad = -0.05f; // Fuerza con la que sube la burbuja

    [Header("Cooldowns")]
    [SerializeField] private float delay = 0.01f;
    [SerializeField] private float clickCooldown = 0.4f;
    [SerializeField] private float timerCooldown = 0;

    private bool isWaiting = false;
    private float timer;
    private Vector3 direction;

    [Header("Mejoras de movimiento")]
    [SerializeField] private float speedIncrease = 2f;
    [SerializeField] private float rotationIncrease = 2f;

    [Header("Sprites de burbuja")]
    [SerializeField] private Sprite mediumBubble;
    [SerializeField] private Sprite bigBubble;
    [SerializeField] private Sprite goldenBubble;

    [SerializeField] private GameObject prefabMasCien;

    private WaveClick scriptCamara;
    private int currentSize = 1;
    private bool movable = true;
    private Rigidbody2D burbuja;
    private CircleCollider2D burbujaCollider;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private AudioSource audioSource;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.enabled = false;
        }

        scriptCamara = Camera.main.GetComponent<WaveClick>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        burbuja = GetComponent<Rigidbody2D>();
        burbujaCollider = GetComponent<CircleCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        burbuja.gravityScale = gravedad;
        setWaterResistance();
        PlayerPrefs.SetInt("CantidadBurbujas", 0);
    }

    // Update is called once per frame
    void Update()
    {
        timerCooldown -= Time.deltaTime;

        int paused = PlayerPrefs.GetInt("Paused", 0);

        if (Input.GetMouseButtonDown(0) && movable && paused == 0 && timerCooldown <= 0)
        {
            timerCooldown = clickCooldown;
            //scriptCamara.SpawnPrefabAtCursor();

            // Obtener la posici�n del clic
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickPosition.z = 0f;

            // Obtener la posici�n actual del objeto
            Vector3 objectPosition = transform.position;

            direction = objectPosition - clickPosition;

            float distance = direction.magnitude;

            if (distance < maxDistance)
            {
                isWaiting = true;
                timer = delay * distance; // Configurar el temporizador
            }
        }

        if (isWaiting)
        {
            timer -= Time.deltaTime;
            if (timer < 0f)
                {
                    float distance = direction.magnitude;
                    burbuja.AddForce(direction.normalized * pushForce / direction.magnitude, ForceMode2D.Impulse);
                    burbuja.angularVelocity = rotationSpeed / distance;
                    isWaiting = false;
                }
        }

        // Limitar la velocidad m�xima
        if (burbuja.linearVelocity.magnitude > maxSpeed)
        {
            burbuja.linearVelocity = burbuja.linearVelocity.normalized * maxSpeed;
        }

        // Limitar la velocidad de rotaci�n m�xima
        if (burbuja.angularVelocity > maxRotation)
        {
            burbuja.angularVelocity = maxRotation;
        }
    }

    public void removeWaterResistance()
    {
        burbuja.linearDamping = 0f;
    }

    public void setWaterResistance()
    {
        burbuja.linearDamping = waterResistance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BurbujaChica"))
        {
            agrandarBurbuja();
            GameObject masCien = Instantiate(prefabMasCien);
            masCien.transform.position = transform.position;
            int cantBurbujas = PlayerPrefs.GetInt("CantidadBurbujas", 0);
            cantBurbujas++;
            PlayerPrefs.SetInt("CantidadBurbujas", cantBurbujas);
            PlayerPrefs.Save();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Obstaculo"))
        {
           resetStage();
        }
    }

    public void resetStage()
    {
        movable = false;
        burbuja.linearVelocity = Vector2.zero;
        burbuja.angularVelocity = 0f;
        burbuja.gravityScale = 0f;
        if (animator != null)
        {
            audioSource.Play();
            animator.enabled = true;
            animator.SetTrigger("Explode");
        }
    }

    public void OnExplosionEnd()
    {
        int cantIntentos = PlayerPrefs.GetInt("CantidadIntentos", 0);
        cantIntentos++;
        PlayerPrefs.SetInt("CantidadIntentos", cantIntentos);
        PlayerPrefs.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void agrandarBurbuja()
    {   
        switch (currentSize)
        {
            case 1:
                spriteRenderer.sprite = mediumBubble;
                burbujaCollider.radius =0.45f;
                pushForce += speedIncrease;
                maxSpeed += speedIncrease;
                rotationSpeed += rotationIncrease;
                maxRotation += rotationIncrease;
                currentSize++;
                break;
            case 2: 
                spriteRenderer.sprite = bigBubble;
                burbujaCollider.radius = 0.54f;
                pushForce += speedIncrease;
                maxSpeed += speedIncrease;
                rotationSpeed += rotationIncrease;
                maxRotation += rotationIncrease;
                currentSize++;
                break;
            case 3:
                spriteRenderer.sprite = goldenBubble;
                pushForce += speedIncrease;
                maxSpeed += speedIncrease;
                rotationSpeed += rotationIncrease;
                maxRotation += rotationIncrease;
                break;
            default:
                break;
        }
    }
}
