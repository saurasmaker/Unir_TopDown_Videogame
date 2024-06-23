using UnityEngine;
using UnityEngine.Events;

public class ForwardDetector : MonoBehaviour
{
    public UnityEvent<InteractableController> OnInteractableDetected, OnInteractableExit;
    public UnityEvent<GameObject> OnObstacleDetected, OnObstacleUndetected;


    private InteractableController _currentInteractableDetected = null;
    private GameObject _currentObstacleDetected = null;

    private Vector2 _rayDirection;
    
    
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

    public Vector2 RayDirection
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
        DetectEntityInDirection(_rayDirection);
    }

    /*
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 direction = _rayDirection;
        Gizmos.DrawRay(transform.position, direction);
    }
    */

    public bool DetectEntityInDirection(Vector2 dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 1f, ~LayerMask.GetMask("Player"));
        if (hit.collider != null)
        {
            GameObject hitGo = hit.collider.gameObject;
            CurrentObstacleDetected = hitGo;

            if (hitGo.TryGetComponent(out InteractableController ic))
                CurrentInteractableDetected = ic;

            return true;
        }

        CurrentInteractableDetected = null;
        CurrentObstacleDetected = null;
        return false;
    }

    private void Interact()
    {
        if(_currentInteractableDetected != null)
            _currentInteractableDetected.Interact();
    }
}
