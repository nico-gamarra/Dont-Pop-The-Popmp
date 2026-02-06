using UnityEngine;

public class OndaClick : MonoBehaviour
{
    [SerializeField] private GameObject prefab; // Prefab que deseas instanciar
    [SerializeField] private float tiempoAnimacion = 0.6f;
    private MenuPausa menuPausa;

    private void Start()
    {
        menuPausa = FindFirstObjectByType<MenuPausa>();
    }


    public void SpawnPrefabAtCursor()
    {
        // Obtén la posición del cursor en pantalla
        Vector3 mousePosition = Input.mousePosition;

        // Convierte la posición del cursor a coordenadas del mundo
        mousePosition.z = 10f; // Ajusta la profundidad (distancia desde la cámara)
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Instancia el prefab en la posición calculada
        GameObject spawnedPrefab = Instantiate(prefab, worldPosition, Quaternion.identity); // Usa Quaternion.identity para rotación

        // Destruye el prefab instanciado después del tiempo especificado
        Destroy(spawnedPrefab, tiempoAnimacion);
    }
}
