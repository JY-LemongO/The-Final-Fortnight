using UnityEngine;

public class BaseSO : ScriptableObject
{
    [Header("Dev")]
    [SerializeField] private int _id;
    [SerializeField] private string _codeName;

    [Header("Display Info")]
    [SerializeField] private string _displayName;
    [SerializeField][TextArea] private string _displayDesc;

    public int Id => _id;
    public string CodeName => _codeName;

    public string DisplayName => _displayName;
    public string DisplayDesc => _displayDesc;    
}
