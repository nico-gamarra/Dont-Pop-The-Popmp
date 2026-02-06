using UnityEngine;

[CreateAssetMenu(fileName = "LevelGrading", menuName = "Scriptable Objects/LevelGrading")]
public class LevelGrading : ScriptableObject
{
    [Header("Grades")]
    public int pointsForS = 6000;
    public int pointsForA = 4900;
    public int pointsForB = 4100;
    public int pointsForC = 3400;
    public int pointsForD = 2300;
    public int pointsForE = 800;
}
