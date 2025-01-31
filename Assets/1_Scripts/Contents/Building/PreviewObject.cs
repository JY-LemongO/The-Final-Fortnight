using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    [SerializeField] private Color _buildableColor;
    [SerializeField] private Color _unbuildableColor;

    private SpriteRenderer _renderer;
    private Vector2 _touchPosition;
    [SerializeField]private float _offset;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        _renderer.enabled = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            ShowPreview();
        else if (Input.GetMouseButtonUp(0))
            BuildObject();
    }

    private void LateUpdate()
    {
        FollowTouchPosition();
    }

    private void ShowPreview()
    {
        _renderer.enabled = true;
    }

    private void BuildObject()
    {
        if (IsBuildablePlace())
        {
            // To Do
            BuildingSystem.Instance.Build();
            PoolManager.Instance.Return(gameObject);
        }            
        else
        {
            _renderer.enabled = false;
            // To Do
            // Noti - 설치 불가능 문구 띄우기
            // 다시 설치할 수 있도록 유도
        }
    }

    private void FollowTouchPosition()
    {
        _touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        transform.position = ClampedBuildablePosition() + Vector2.up * _offset;
        _renderer.color = CheckBuildableByColor();
    }

    private bool IsBuildablePlace()
    {
        if (_touchPosition.x < Constants.Min_Build_PosX || _touchPosition.x > Constants.Max_Build_PosX ||
            _touchPosition.y < Constants.Min_Build_PosY || _touchPosition.y > Constants.Max_Build_PosY)
            return false;

        return true;
    }

    private Color CheckBuildableByColor()
        => IsBuildablePlace() ? _buildableColor : _unbuildableColor;

    private Vector2 ClampedBuildablePosition()
    {
        if (IsBuildablePlace())
            return _touchPosition;

        float clampedX = Mathf.Clamp(_touchPosition.x, Constants.Min_Build_PosX, Constants.Max_Build_PosX);
        float clampedY = Mathf.Clamp(_touchPosition.y, Constants.Min_Build_PosY, Constants.Max_Build_PosY);

        return new Vector2(clampedX, clampedY);
    }

    public void SetPreview(Sprite sprite)
    {
        _renderer.sprite = sprite;
        _offset = sprite.textureRect.height / sprite.pixelsPerUnit * 0.5f;
    }
}
