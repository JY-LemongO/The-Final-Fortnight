using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    [SerializeField] private Color _buildableColor;
    [SerializeField] private Color _unbuildableColor;

    private SpriteRenderer _renderer;
    private float _offset;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
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
        gameObject.SetActive(true);
    }

    private void BuildablePositionCheck()
    {

    }

    private void BuildObject()
    {
        if (IsBuildablePlace())
            return;
        else
        {
            gameObject.SetActive(false);
        }
    }

    private bool IsBuildablePlace()
    {


        return false;
    }

    private void FollowTouchPosition()
    {
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        touchPosition.z = 0;

        transform.position = touchPosition;
    }

    public void SetSprite(float spriteHeight, Sprite sprite)
    {
        _renderer.sprite = sprite;
        _offset = spriteHeight;
    }
}
