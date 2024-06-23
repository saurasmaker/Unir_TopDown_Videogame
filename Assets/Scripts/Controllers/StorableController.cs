using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InteractableController))]
public class StorableController : MonoBehaviour
{
    [SerializeField]
    private string _uid;
    [SerializeField]
    private Sprite _uiSprite;
    [SerializeField]
    private int _stackable;


    public string Uid { get => _uid; }
    public Sprite UiSprite { get => _uiSprite; }
    public int Stackable { get => _stackable; }

    public void OnInteract()
    {
        InventoryManager.Instance.StoreItem(this, 1);
        gameObject.SetActive(false);
    }
}
