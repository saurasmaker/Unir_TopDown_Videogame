using Patterns.Singletons;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UiController))]
public class DialogueManager : SingletonMonoBehaviour<DialogueManager>
{
    [SerializeField]
    private UiController _uiController;
    [SerializeField]
    private TMP_Text _text;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private Image _waitForInputImage;

    bool _nextDialogue = false;

    override protected void OnAwake()
    {
        base.OnAwake();

        if (_uiController == null)
            _uiController = GetComponent<UiController>();

        if (_waitForInputImage != null)
            _waitForInputImage.gameObject.SetActive(false);

        _uiController.OnOpenInferface.AddListener(InputsManager.Instance.EnableDialogueActions);
        _uiController.OnCloseInterface.AddListener(InputsManager.Instance.EnablePlayerActions);
    }

    protected override void OnStart()
    {
        base.OnStart();

        InputsManager.Instance.DialogueNextIcon.performed += (_) => _nextDialogue = true;
    }

    public void StartDialogue(string[] texts)
    {
        StartCoroutine(DialogueRoutine(texts));
    }

    private IEnumerator DialogueRoutine(string[] texts)
    {
        _text.text = "";
        _uiController.OpenInterface();

        foreach (string s in texts)
        {
            foreach (char c in s)
            {
                _text.text += c;
                yield return new WaitForSeconds(_speed);
            }

            _waitForInputImage.gameObject.SetActive(true);
            while (!_nextDialogue)
                yield return new WaitForEndOfFrame();
            _waitForInputImage.gameObject.SetActive(false);
            _nextDialogue = false;
        }

        _uiController.CloseInterface();
    }
}
