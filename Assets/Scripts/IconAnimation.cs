using System.Collections;
using UnityEngine;

public class IconAnimation : MonoBehaviour
{
    [Header("Scale")]
    [SerializeField]
    private bool _enableScale = false;
    [SerializeField]
    private bool _scaleSmooth = false;
    [SerializeField]
    [Range(0, 100)]
    private float _scaleDuration = 1f;
    [SerializeField]
    private Vector2 _targetLocalScale = Vector2.one * 1.5f;

    [Header("Position")]
    [SerializeField]
    private bool _enablePosition = false;
    [SerializeField]
    private bool _positionSmooth = false;
    [SerializeField]
    [Range(0, 100)]
    private float _positionDuration = 1f;
    [SerializeField]
    private Vector2 _targetLocalPosition = Vector2.down;


    private RectTransform _rt;
    private Vector2 _initLocalScale, _initLocalPosition;


    private void Awake()
    {
        _rt = GetComponent<RectTransform>();
        _initLocalScale = _rt.localScale;
        _initLocalPosition = _rt.localPosition;

        _targetLocalPosition = _rt.localPosition + (Vector3)_targetLocalPosition;
    }

    private void OnEnable()
    {
        if(_enablePosition)
            StartCoroutine(Modify(IconAnimationType.Position, _initLocalPosition, _targetLocalPosition, _positionDuration, _positionSmooth));
        if (_enableScale)
            StartCoroutine(Modify(IconAnimationType.Scale, _initLocalScale, _targetLocalScale, _scaleDuration, _scaleSmooth));
        //if (_enablePosition)
          //  modifyRotation = StartCoroutine(Modify(IconAnimationType.Position, ));
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator Modify(IconAnimationType iat, Vector2 start, Vector2 target, float duration, bool isSmooth)
    {
        while (true)
        {
            float timer = 0;
            float progress;
            Vector2 res;

            while (timer < duration)
            {
                progress = timer / duration;
                if (isSmooth)
                    progress = SmoothProgress(progress);

                res = Vector2.Lerp(start, target, progress);

                switch (iat)
                {
                    case IconAnimationType.Position:
                        res = new Vector3(res.x, res.y, _rt.localPosition.z);
                        _rt.localPosition = res;
                        break;
                    case IconAnimationType.Rotation:
                        res = new Vector3(res.x, res.y, _rt.localEulerAngles.z);
                        _rt.localEulerAngles = res;
                        break;
                    case IconAnimationType.Scale:
                        res = new Vector3(res.x, res.y, _rt.localScale.z);
                        _rt.localScale = res;
                        break;
                }

                timer += Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }

            (start, target) = (target, start);
        }
    }

    public float SmoothProgress(float progress)
    {
        progress = Mathf.Lerp(-Mathf.PI / 2, Mathf.PI / 2, progress);
        progress = Mathf.Sin(progress);
        progress = (progress / 2) + 0.5f;
        return progress;
    }

    private enum IconAnimationType
    {
        None, 
        Scale,
        Position,
        Rotation
    }
}
