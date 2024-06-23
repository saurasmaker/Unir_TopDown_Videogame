using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiStorableController : MonoBehaviour
{
    [SerializeField]
    private string _uid;
    [SerializeField]
    private Image _image;
    [SerializeField]
    private TMP_Text _countText;
    [SerializeField]
    private Image _countBackground;


    public string Uid { get => _uid; }
    public Image Image { get => _image; }
    public TMP_Text CountText { get => _countText; }


    public void Initialize(StorableController storable, int count)
    {
        _uid = storable.Uid;


        if (_image == null)
            Debug.LogWarning("Image not referenced in UiStorableController...");
        else
        {
            _image.sprite = storable.UiSprite;
            RectTransform rt = _image.GetComponent<RectTransform>();
            rt.localScale = Vector3.one;
        }

        if (_countText == null)
            Debug.LogWarning("Text not referenced in UiStorableController...");
        else if(_countBackground == null)
            Debug.LogWarning("Background Text not referenced in UiStorableController...");
        else if (storable.Stackable <= 1)
        {
            _countText.gameObject.SetActive(false);
            _countBackground.gameObject.SetActive(false);
        }
        else if (count > 0)
            _countText.text = count.ToString();
        else
        {
            _countText.gameObject.SetActive(false);
            _countBackground.gameObject.SetActive(false);
        }
    }
}
