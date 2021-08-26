using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Level")]
public class Level : ScriptableObject
{
    [SerializeField] public string Task;
    [Header("Element - count of elements generated on level")]
    [SerializeField] public int[] Level—omplexity;
    [Range(1, 100)]
    [SerializeField] public float SpriteScaleFactor = 4;
    [SerializeField] public Sprite[] SpriteSheet;
}
