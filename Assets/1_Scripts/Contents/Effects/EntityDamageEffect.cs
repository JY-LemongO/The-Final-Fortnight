using System.Collections;
using UnityEngine;

public class EntityDamageEffect : MonoBehaviour
{
    private Entity _context;
    private Material _mat;

    private float _hitFlashTime;
    private string _flashAmountKey;
    private Coroutine _hitCoroutine;

    private void Awake()
    {
        _mat = GetComponent<SpriteRenderer>().material;
        _hitFlashTime = Constants.HitFlashTime;
        _flashAmountKey = Constants.HitFlashAmountShaderKey;
    }

    public void Setup(Entity context)
    {
        _context = context;
        _context.Status.OnDead += ResetEffect;
    }

    public void EffectDamaged()
    {
        if (_hitCoroutine != null)
            StopCoroutine(_hitCoroutine);
        _hitCoroutine = StartCoroutine(Co_HitFlash());

        switch (_context.EntityType)
        {
            case Define.EntityType.Survivor:
                break;
            case Define.EntityType.Zombie:
                ZombieEffect();
                break;
            case Define.EntityType.Structure:
                break;
        }
    }

    private void SurvivorEffect()
    {

    }

    private void ZombieEffect()
    {
        GameObject effectObject = ResourceManager.Instance.Instantiate(Constants.Key_Z_HitAnimation);
        effectObject.transform.position = transform.position;
    }

    private void StructureEffect()
    {

    }

    private void ResetEffect()
    {
        _mat.SetFloat(_flashAmountKey, 0f);
    }

    private IEnumerator Co_HitFlash()
    {
        _mat.SetFloat(_flashAmountKey, 0.7f);
        yield return Util.GetCachedWaitForSeconds(_hitFlashTime);
        _mat.SetFloat(_flashAmountKey, 0f);
    }
}
