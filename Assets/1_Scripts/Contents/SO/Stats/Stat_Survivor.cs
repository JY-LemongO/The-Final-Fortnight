using UnityEngine;

public class Stat_Survivor : Stat_SO
{
    [Header("Survivor Stats")]
    [SerializeField] private int _magazine;
    [SerializeField] private float _fireRange;
    [SerializeField] private bool _isSplash;

    public int Magazine => _magazine;
    public float FireRange => _fireRange;
    public bool IsSplash => _isSplash;
}
