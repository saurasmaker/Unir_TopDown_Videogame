using UnityEngine;

public class WorldIconAnimation : MonoBehaviour
{
    [SerializeField]
    private float _speed = 1f;
    [SerializeField]
    private Vector3 _targetScale = Vector3.one * 1.5f;

    private RectTransform _rt;
    private Vector3 _initScale;
    private float t = 0f;

    private void Awake()
    {
        _rt = GetComponent<RectTransform>();
        _initScale = _rt.localScale;
    }

    void Update()
    {
        _rt.localScale = Vector3.Slerp(_initScale, _targetScale, t);
        t += Time.deltaTime * _speed;
        if (t >= 1)
        {
            (_initScale, _targetScale) = (_targetScale, _initScale);
            t = 0f;
        }
    }
}
