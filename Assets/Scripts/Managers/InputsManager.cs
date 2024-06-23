using Patterns.Singletons;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputsManager : SingletonMonoBehaviour<InputsManager>
{
    #region Serialized Attributes
    [SerializeField]
    private PlayerInput _playerInput;
    #endregion


    #region Attributes
    private InputActionAsset _inputActions;

    private InputActionMap _playerActionMap, _uiActionMap, _dialogueActionMap;

    // Player Actions
    private InputAction _moveAction = null, _interactAction = null,
        _openInventoryAction = null;

    // UI Actions 
    private InputAction _closeInventoryAction = null;

    // Dialogue Actions
    private InputAction _dialogueNextAction = null, _dialogueOmitAction = null;
    #endregion



    #region Properties

    #region PlayerActions
    public InputAction MoveAction { get => _moveAction; }
    public InputAction InteractAction { get => _interactAction; }
    public InputAction OpenInventoryAction { get => _openInventoryAction; }
    #endregion

    #region UI Actions
    public InputAction CloseInventoryAction { get => _closeInventoryAction; }
    #endregion

    #region Dialogue Actions
    public InputAction DialogueNextIcon { get => _dialogueNextAction; }
    public InputAction DialogueOmitAction { get => _dialogueOmitAction; }
    #endregion


    #endregion




    #region MonoBehaviour Methods
    protected override void OnAwake()
    {
        base.OnAwake();
        Initialize();
    }

    protected override void OnStart()
    {
        base.OnStart();

        CloseInventoryAction.performed += (_) => EnablePlayerActions();
        OpenInventoryAction.performed += (_) => EnableUiActions();
    }
    #endregion


    #region Initialize Methods
    private void Initialize()
    {
        if (_playerInput == null)
            _playerInput = GetComponent<PlayerInput>();

        if (_inputActions == null && _playerInput != null)
            _inputActions = _playerInput.actions;

        _playerActionMap = _inputActions.FindActionMap("Player");
        _uiActionMap = _inputActions.FindActionMap("UI");
        _dialogueActionMap = _inputActions.FindActionMap("Dialogue");

        _moveAction = _playerActionMap.FindAction("Move");
        _interactAction = _playerActionMap.FindAction("Interact");
        _openInventoryAction = _playerActionMap.FindAction("OpenInventory");

        _closeInventoryAction = _uiActionMap.FindAction("CloseInventory");

        _dialogueOmitAction = _dialogueActionMap.FindAction("Omit");
        _dialogueNextAction = _dialogueActionMap.FindAction("Next");
    }
    #endregion

    public void EnablePlayerActions()
    {
        _playerInput.SwitchCurrentActionMap("Player");
    }

    public void EnableUiActions()
    {
        _playerInput.SwitchCurrentActionMap("UI");
    }

    public void EnableDialogueActions()
    {
        _playerInput.SwitchCurrentActionMap("Dialogue");
    }
    
}
