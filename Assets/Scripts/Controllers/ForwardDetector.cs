using UnityEngine;
using UnityEngine.Events;

public class ForwardDetector : MonoBehaviour
{
    public UnityEvent<InteractableController> OnInteractableDetected, OnInteractableExit;
    public UnityEvent<GameObject> OnObstacleDetected, OnObstacleUndetected;


    private InteractableController _currentInteractableDetected = null;
    private GameObject _currentObstacleDetected = null;

    private Vector3 _rayDirection;
    
    
    public InteractableController CurrentInteractableDetected { 
        get { return _currentInteractableDetected; }
        private set
        {
            if(_currentInteractableDetected != value)
            {
                if(_currentInteractableDetected != null)
                    OnInteractableExit?.Invoke(_currentInteractableDetected);
                if (value != null)
                    OnInteractableDetected?.Invoke(value);

                _currentInteractableDetected = value;
            }
        }
    }

    public GameObject CurrentObstacleDetected
    {
        get { return _currentObstacleDetected; }
        private set
        {
            if (_currentObstacleDetected != value)
            {
                if (_currentObstacleDetected != null)
                    OnObstacleUndetected?.Invoke(_currentObstacleDetected);
                if (value != null)
                    OnObstacleDetected?.Invoke(value);

                _currentObstacleDetected = value;
            }
        }
    }

    public Vector3 RayDirection
    {
        get { return _rayDirection; }
        set { _rayDirection = value; }
    }


    private void Start()
    {
        OnInteractableDetected.AddListener((interactable) => { interactable.Detected = true; });
        OnInteractableExit.AddListener((interactable) => { interactable.Detected = false; });

        InputsManager.Instance.InteractAction.performed += (_) => Interact();
    }

    private void Update()
    {
        DetectEntityInForward();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 direction = _rayDirection;
        Gizmos.DrawRay(transform.position, direction);
    }


    private void DetectEntityInForward()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, _rayDirection, 1f, ~LayerMask.GetMask("Player"));
        if (hit.collider != null)
        {
            GameObject hitGo = hit.collider.gameObject;
            switch (LayerMask.LayerToName(hitGo.layer))
            {
                case "Interactable":
                    CurrentInteractableDetected = hitGo.GetComponent<InteractableController>();
                    CurrentObstacleDetected = hitGo;
                    break;
                case "Obstacle":
                    CurrentObstacleDetected = hitGo;
                    break;
            }
        }
        else
        {
            CurrentInteractableDetected = null;
            CurrentObstacleDetected = null;
        }
    }

    private void Interact()
    {
        if(_currentInteractableDetected != null)
            _currentInteractableDetected.Interact();
    }
}
