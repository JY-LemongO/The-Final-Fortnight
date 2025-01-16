using UnityEngine;

public class EntityDamageEffect : MonoBehaviour
{
    [SerializeField] private Define.EntityType _entityType;

    public void EffectDamaged()
    {
        Debug.Log("Effect Damaged");

        switch (_entityType)
        {
            case Define.EntityType.Survivor:
                break;
            case Define.EntityType.Zombie:
                ZombieEffect();
                break;
            case Define.EntityType.Barricate:
                break;
        }
    }
    
    private void SurvivorEffect()
    {

    }

    private void ZombieEffect()
    {
        GameObject go = ResourceManager.Instance.Instantiate(Constants.Key_Z_HitEffect);        
        go.transform.position = transform.position;
    }

    private void BarricateEffect()
    {

    }
}
