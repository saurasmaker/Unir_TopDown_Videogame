using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InteractableController : MonoBehaviour
{
    public UnityEvent OnDetected, OnUndetected, OnInteract;

    [SerializeField]
    private Image _interactIcon;

    private bool _detected;
    public bool Detected
    {
        get { return _detected; }
        set
        {
            if(_detected != value)
            {
                _detected = value;
                if (_detected)
                    OnDetected?.Invoke();
                else 
                    OnUndetected?.Invoke();
            }
        }
    }

    private void Awake()
    {
        if(_interactIcon == null)
            _interactIcon = GetComponentInChildren<Image>();

        _interactIcon.gameObject.SetActive(false);
    }

    private void Start()
    {
        OnDetected.AddListener(() => _interactIcon.gameObject.SetActive(true));
        OnUndetected.AddListener(() => _interactIcon.gameObject.SetActive(false));
    }

    public void Interact()
    {
        Detected = false;
        OnInteract?.Invoke();
    }
}
