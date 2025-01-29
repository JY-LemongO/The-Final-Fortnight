using System.Collections;
using UnityEngine;

public class BulletShell : MonoBehaviour
{
    private SpriteRenderer _rend;
    private Rigidbody2D _rigid;

    [Header("Timer")]
    [SerializeField] private float disappearTime;
    [SerializeField] private float disappearWaitTime;

    [Header("Random Multiplier")]
    [SerializeField] private float min;
    [SerializeField] private float max;

    [Header("Gravity Scale")]
    [SerializeField] private float gravityScale;

    [Header("Ejection Values")]
    [SerializeField] private float trqPower;
    [SerializeField] private float accPower;
    [SerializeField] private float gravityTime;

    [Header("Bounce Values")]
    [SerializeField] private float bounceTrqPower;
    [SerializeField] private float bounceAccPower;
    [SerializeField] private float bounceGravityTime;

    [Header("Direction")]
    [SerializeField] private Vector2 accDir;

    private void Awake()
    {
        Init();        
    }

#if UNITY_EDITOR
    private void Update()
    {
        // Test
        if (Input.GetMouseButtonDown(0))
            StartCoroutine(Co_ShellMove());
    }
#endif

    private void Init()
    {
        _rend = GetComponent<SpriteRenderer>();
        _rigid = GetComponent<Rigidbody2D>();

        GameManager.Instance.OnRestartGame += OnActiveObjectDispose;
    }

    public void Setup(Vector3 position)
    {
        transform.position = position;
        StartCoroutine(Co_ShellMove());
    }

    private void OnActiveObjectDispose()
    {
        if (gameObject.activeSelf)
            Dispose();
    }

    private void StopShell()
    {
        _rigid.linearVelocity = Vector2.zero;
        _rigid.angularVelocity = 0f;
    }

    private Vector2 CalcRandomAccDir(float power)
    {
        float randomMultiplier = Random.Range(min, max);
        float randomAccPower = power * randomMultiplier;
        return accDir * randomAccPower;
    }

    private void AddForceAndTorque(Vector2 forceDir, float torquePower)
    {
        _rigid.AddForce(forceDir, ForceMode2D.Impulse);
        _rigid.AddTorque(torquePower, ForceMode2D.Impulse);
    }

    private IEnumerator Co_ShellMove()
    {
        _rigid.gravityScale = gravityScale;
        StopShell();
        AddForceAndTorque(CalcRandomAccDir(accPower), trqPower);
        yield return Util.GetCachedWaitForSeconds(gravityTime);
        StopShell();

        AddForceAndTorque(CalcRandomAccDir(bounceAccPower), bounceTrqPower);
        yield return Util.GetCachedWaitForSeconds(bounceGravityTime);
        StopShell();
        _rigid.gravityScale = 0f;

        yield return Util.GetCachedWaitForSeconds(disappearWaitTime);
        StartCoroutine(Co_Disappear());
    }

    private IEnumerator Co_Disappear()
    {
        float current = 0f;
        float percent = 0f;

        Color color = _rend.color;
        while (percent < 1f)
        {
            current += Time.deltaTime;
            percent = current / disappearTime;

            color.a = Mathf.Lerp(1f, 0f, percent);
            _rend.color = color;

            yield return null;
        }
        color.a = 0f;
        _rend.color = color;

        Dispose();
    }

    public void Dispose()
    {
        transform.rotation = Quaternion.identity;
        StopAllCoroutines();
        PoolManager.Instance.Return(gameObject);
        _rend.color = Color.white;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnRestartGame -= OnActiveObjectDispose;
    }
}
