using UnityEngine;

public class EffectTestScene : MonoBehaviour
{
    [SerializeField] Zombie _zombiePrefab;
    [SerializeField] int spawnCount;

    private void Start()
    {
        for (int i = 0; i < spawnCount; i++)
            Instantiate(_zombiePrefab);
    }
}
