using Patterns.Singletons;
using UnityEngine;

[RequireComponent(typeof(UiController))]
public class HudManager : SingletonMonoBehaviour<HudManager>
{
    private UiController _uiController;

    protected override void OnAwake()
    {
        base.OnAwake();

        _uiController = GetComponent<UiController>();
    }

    protected override void OnStart()
    {
        base.OnStart();

        InputsManager.Instance.OpenInventoryAction.performed += (_) => _uiController.CloseInterface();
        InputsManager.Instance.CloseInventoryAction.performed += (_) => _uiController.OpenInterface();
    }
}
