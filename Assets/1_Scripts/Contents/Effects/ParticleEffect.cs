using System.Collections;
using UnityEngine;

public class ParticleEffect : MonoBehaviour
{
    private ParticleSystem _particle;

    private float _effectDuration;

    private void Awake()
    {
        _particle = GetComponent<ParticleSystem>();
        _effectDuration = _particle.main.duration;
    }

    private void OnEnable()
    {
        StartCoroutine(Co_ParticlePlaying());
    }

    private IEnumerator Co_ParticlePlaying()
    {
        _particle.Play();
        yield return Util.GetCachedWaitForSeconds(_effectDuration);

        ReturnToPool();
    }

    private void ReturnToPool()
    {
        _particle.Stop();
        PoolManager.Instance.Return(gameObject);
    }
}
