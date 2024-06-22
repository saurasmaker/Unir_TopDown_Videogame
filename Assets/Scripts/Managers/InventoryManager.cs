using Patterns.Singletons;
using UnityEngine;

[RequireComponent(typeof(UiController))]
public class InventoryManager : SingletonMonoBehaviour<InventoryManager>
{
    private UiController _uiController;

    protected override void OnAwake()
    {
        base.OnAwake();

        GetNecesseryComponents();
        Initialize();
    }

    protected override void OnStart()
    {
        base.OnStart();

        InputsManager.Instance.OpenInventoryAction.performed += (_) => _uiController.OpenInterface();
        InputsManager.Instance.CloseInventoryAction.performed += (_) => _uiController.CloseInterface();
    }

    private void GetNecesseryComponents()
    {
        _uiController = GetComponent<UiController>();
    }

    private void Initialize()
    {
        _uiController.CloseInterface();
    }
}
