using UnityEngine;

public class BubbleMovement : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] private float pushForce;
    [SerializeField] private float maxDistance;
    [SerializeField] private AnimationCurve distanceForceCurve;
    
    [Header("Config")]
    [SerializeField] private BubbleAnimations bubbleAnimations;

    private Rigidbody2D _rigidbody;
    private Camera _camera;
    private Vector2 _mouseWorldPos;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ApplyPush();
            bubbleAnimations.PlayWaveAnimation(_mouseWorldPos);
        }
    }

    private void ApplyPush()
    {
        _mouseWorldPos = GetMouseWorldPosition();
        Vector2 direction = (_mouseWorldPos - (Vector2)transform.position).normalized;

        float distance = Vector2.Distance(_mouseWorldPos, transform.position);
        float forceMultiplier = CalculateDistanceMultiplier(distance);

        if (forceMultiplier <= 0f)
            return;

        Vector2 force = -direction * (pushForce * forceMultiplier);
        _rigidbody.AddForce(force, ForceMode2D.Impulse);
    }

    private Vector2 GetMouseWorldPosition()
    {
        Vector3 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        return mousePos;
    }

    private float CalculateDistanceMultiplier(float distance)
    {
        float normalized = Mathf.Clamp01(distance / maxDistance);
        return distanceForceCurve.Evaluate(normalized);
    }
}