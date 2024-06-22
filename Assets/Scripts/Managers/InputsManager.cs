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
    private InputAction _moveAction = null,
        _interactAction = null, _openInventoryAction = null, _closeInventoryAction;
    #endregion



    #region Properties

    #region Actions
    public InputAction MoveAction
    {
        get
        {
            _moveAction ??= _inputActions.FindAction("Move");
            return _moveAction;
        }
    }

    public InputAction InteractAction
    {
        get
        {
            _interactAction ??= _inputActions.FindAction("Interact");
            return _interactAction;
        }
    }

    public InputAction OpenInventoryAction
    {
        get
        {
            _openInventoryAction ??= _inputActions.FindAction("OpenInventory");
            return _openInventoryAction;
        }
    }

    public InputAction CloseInventoryAction
    {
        get
        {
            _closeInventoryAction ??= _inputActions.FindAction("CloseInventory");
            return _closeInventoryAction;
        }
    }
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
    }

    public void EnablePlayerActions()
    {
        _playerInput.SwitchCurrentActionMap("Player");
    }

    public void EnableUiActions()
    {
        _playerInput.SwitchCurrentActionMap("UI");
    }
    #endregion
}
