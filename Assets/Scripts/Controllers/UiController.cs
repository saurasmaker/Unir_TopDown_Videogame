using UnityEngine;

public class UiController : MonoBehaviour 
{
    private const string PANEL_CONTENT_NULL_ERROR = "Content panel is not referenced...";

    [SerializeField]
    GameObject _contentPanel = null;
    [SerializeField]
    private bool _initializeEnabled = false;

    public GameObject ContentPanel { get =>  _contentPanel; }
    
    private void Awake()
    {
        if(CheckWarnings())
            Initialize();
    }

    private bool CheckWarnings()
    {
        bool warning = _contentPanel == null;
        if (warning)
            Debug.LogWarning(PANEL_CONTENT_NULL_ERROR);

        return warning;
    }

    private void Initialize()
    {
        _contentPanel.SetActive(_initializeEnabled);
    }

    public void OpenInterface()
    {
        if (_contentPanel != null)
            _contentPanel.SetActive(true);
    }

    public void CloseInterface()
    {
        if (_contentPanel != null)
            _contentPanel.SetActive(false);
    }

    public void ToggleInterface()
    {
        if (_contentPanel != null)
            _contentPanel.SetActive(!_contentPanel.activeSelf);
    }
}
