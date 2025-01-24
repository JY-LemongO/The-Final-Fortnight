using UnityEngine;

public class AnimationEffect : MonoBehaviour
{
    private void HandleDisable()
        => PoolManager.Instance.Return(gameObject);
}
