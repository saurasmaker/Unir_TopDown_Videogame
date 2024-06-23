using Patterns.Singletons;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterAnimationsController))]
[RequireComponent(typeof(ForwardDetector))]
public class PlayerController : SingletonMonoBehaviour<PlayerController>
{
    public UnityEvent<Vector2> OnLastDirChange;
    public UnityEvent OnStartMoving, OnStopMoving;

    [SerializeField]
    private float _movementSpeed = 2f, _movementDistance = 1f;

    private ForwardDetector _forwardDetector;
    private CharacterAnimationsController _animationsController;

    private bool _move = false, _isMoving = false;
    private Vector2 _lastDir = Vector3.zero;


    #region Properties
    public Vector2 LastDir
    {
        get { return _lastDir; }
        set
        {
            if(_lastDir != value)
            {
                _lastDir = value;
                OnLastDirChange?.Invoke(value);
            }       
        }
    }

    public bool IsMoving 
    { 
        get => _isMoving; 
        set
        {
            if(_isMoving != value)
            {
                _isMoving = value;
                if (_isMoving)
                    OnStartMoving?.Invoke();
                else
                    OnStopMoving?.Invoke();
            }
        }
    }
    #endregion


    #region MonoBehaviour Methods
    protected override void OnAwake()
    {
        base.OnAwake();

        _forwardDetector = GetComponent<ForwardDetector>();
        _animationsController = GetComponent<CharacterAnimationsController>();
    }

    override protected void OnStart()
    {
        base.OnStart();

        OnStopMoving.AddListener(() => _animationsController.SetIdleAnimation(LastDir));

        InputsManager.Instance.MoveAction.started += StartMoving;
        InputsManager.Instance.MoveAction.canceled += StopMoving;

        LastDir = Vector2.down;
        _animationsController.SetIdleAnimation(LastDir);
    }
    #endregion

    private Coroutine _movingRoutine = null;
    private void StartMoving(InputAction.CallbackContext context)
    {
        _move = true;
        if(_movingRoutine == null)
            _movingRoutine = StartCoroutine(MovingRoutine());
    }

    private IEnumerator MovingRoutine()
    {
        yield return StartCoroutine(FirstStepRoutine());

        IsMoving = true;
        while (_move)
        {
            if(!TryReadMovementInput(out Vector2 movementInput) || _forwardDetector.DetectEntityInDirection(movementInput))
            {
                _animationsController.SetIdleAnimation(LastDir);
                yield return new WaitForEndOfFrame();
                continue;
            }

            float t = 0f;
            Vector2 startPos = transform.position;
            Vector2 endPos = startPos + movementInput * _movementDistance;
            
            LastDir = movementInput;

            _animationsController.SetWalkingAnimation(movementInput);
                
            while (t < 1f)
            {
                t += Time.deltaTime * _movementSpeed;
                if (t >= 1f) t = 1f;
                transform.position = Vector3.Lerp(startPos, endPos, t);

                yield return new WaitForEndOfFrame();
            }
        }
        _movingRoutine = null;
        IsMoving = false;
    }

    private IEnumerator FirstStepRoutine()
    {
        if (TryReadMovementInput(out Vector2 movementInput))
        {
            _animationsController.SetIdleAnimation(movementInput);
            if (LastDir != movementInput)
                yield return new WaitForSeconds(0.1f);
        }

        LastDir = movementInput;
    }

    private bool TryReadMovementInput(out Vector2 res)
    {
        Vector2 moveInput = InputsManager.Instance.MoveAction.ReadValue<Vector2>();
        if (Mathf.Abs(moveInput.x) > 0 && Mathf.Abs(moveInput.y) > 0)
            moveInput = Vector2.zero;

       
        if(moveInput != Vector2.zero)
        {
            res = moveInput.normalized;
            _forwardDetector.RayDirection = res;
            return true;
        }

        res = moveInput;
        return false;
    }


    private void StopMoving(InputAction.CallbackContext context)
    {
        _move = false;
    }


    
}
