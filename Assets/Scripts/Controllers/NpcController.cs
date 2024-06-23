using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterAnimationsController))]
[RequireComponent(typeof(InteractableController))]
public class NpcController : MonoBehaviour
{
    private CharacterAnimationsController _animationsController;
    private InteractableController _interactableController;

    [SerializeField]
    private Vector2 _lookingTo = Vector2.down;

    void Awake()
    {
        _animationsController = GetComponent<CharacterAnimationsController>();
        _interactableController = GetComponent<InteractableController>();

        _interactableController.OnInteract.AddListener(() => {
            Vector2 dir = PlayerController.Instance.transform.position - transform.position;
            Debug.Log(dir);
            _animationsController.SetIdleAnimation(dir);
        });
 
    }

    private void Start()
    {
        _animationsController.SetIdleAnimation(_lookingTo);
    }




}
