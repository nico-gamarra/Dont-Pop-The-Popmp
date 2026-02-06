using UnityEngine;

public class BubbleMovement : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] private float pushForce = 20f;
    [SerializeField] private float maxDistance = 5f;
    [SerializeField] private AnimationCurve distanceForceCurve;

    private Rigidbody2D _rigidbody;
    private Camera _camera;

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
        }
    }

    private void ApplyPush()
    {
        Vector2 mouseWorldPos = GetMouseWorldPosition();
        Vector2 direction = (mouseWorldPos - (Vector2)transform.position).normalized;

        float distance = Vector2.Distance(mouseWorldPos, transform.position);
        float forceMultiplier = CalculateDistanceMultiplier(distance);

        if (forceMultiplier <= 0f)
            return;

        Vector2 force = -direction * pushForce * forceMultiplier;
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