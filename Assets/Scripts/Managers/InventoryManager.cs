using Patterns.Singletons;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UiController))]
public class InventoryManager : SingletonMonoBehaviour<InventoryManager>
{
    [SerializeField]
    private UiStorableController _uiStorableControllerPrefab;
    [SerializeField]
    private RectTransform _itemsContainer;


    private UiController _uiController;
    private Dictionary<string, Storable> _storablesList;


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

        _storablesList = new Dictionary<string, Storable>();
       // _storablesList.
    }

    public void StoreItem(StorableController storableController, int amount)
    {
        if (_storablesList.TryGetValue(storableController.Uid, out Storable s))
        {
            s.count += amount;
            s.uiController.CountText.text = s.count.ToString();
        }
        else
        {
            UiStorableController uiControllerAux = Instantiate(_uiStorableControllerPrefab);
            if (uiControllerAux.TryGetComponent(out RectTransform rectTransform))
            {
                rectTransform.SetParent(_itemsContainer);
                uiControllerAux.Initialize(storableController, 1);
            }
            else
                Destroy(uiControllerAux);
            

            _storablesList.Add(storableController.Uid, new Storable(storableController, uiControllerAux, amount));
        }
    }

    public void RemoveItem(StorableController storableController, int amount)
    {
        if (_storablesList.TryGetValue(storableController.Uid, out Storable s) && s.count > 1)
        {
            s.count -= amount;
            if(s.count < 1)
            {
                Destroy(s.uiController);
                _storablesList.Remove(storableController.Uid);
            }
        }
    }

    public struct Storable
    {
        public StorableController controller;
        public UiStorableController uiController;
        public int count;

        public Storable(StorableController controller, UiStorableController uiController, int count)
        {
            this.controller = controller;
            this.uiController = uiController;
            this.count = count;
        }
    }
}
