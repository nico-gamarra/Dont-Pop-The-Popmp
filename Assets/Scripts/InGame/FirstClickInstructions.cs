using UnityEngine;

public class firstClickInstructions : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Destroy(gameObject);
        }
    }
}
