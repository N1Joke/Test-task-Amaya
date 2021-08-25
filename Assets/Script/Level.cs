using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Level")]
public class Level : ScriptableObject
{
    [SerializeField] public string Task;
    [Header("Element - count of elements generated on level")]
    [SerializeField] public int[] Level—omplexity;
    [SerializeField] public Sprite[] SpriteSheet;    
}
