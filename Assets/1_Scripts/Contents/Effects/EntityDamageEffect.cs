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

    private void BarricateEffect()
    {

    }
}
