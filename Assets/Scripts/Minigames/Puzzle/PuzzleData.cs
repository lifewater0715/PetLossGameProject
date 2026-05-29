using UnityEngine;

[CreateAssetMenu(fileName = "PuzzleData", menuName = "Scriptable Objects/PuzzleData")]
public class PuzzleData : ScriptableObject
{
    public Sprite puzzleImage;
    public int PuzzleNumsber;
    public GameObject PuzzleTarg;
}
