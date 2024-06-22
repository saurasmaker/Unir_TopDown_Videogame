using Patterns.Singletons;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(ForwardDetector))]
public class PlayerController : SingletonMonoBehaviour<PlayerController>
{
    public UnityEvent<Vector2> OnLastDirChange;
    public UnityEvent OnStartMoving, OnStopMoving;

    [SerializeField]
    private float _movementSpeed = 2f, _movementDistance = 1f;

    private ForwardDetector _forwardDetector;
    private Animator _animator;

    private bool _isMoving = false;
    private Vector2 _lastDir = Vector3.zero;


    public ForwardDetector InteractableDetector { get { return _forwardDetector; } }
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

    protected override void OnAwake()
    {
        base.OnAwake();

        _forwardDetector = GetComponent<ForwardDetector>();
        _animator = GetComponent<Animator>();
    }

    override protected void OnStart()
    {
        base.OnStart();

        OnLastDirChange.AddListener((dir) =>
        {
            if(dir == Vector2.left)
                _animator.Play("WalkLeft@Player");
            if (dir == Vector2.right)
                _animator.Play("WalkRight@Player");
            if (dir == Vector2.up)
                _animator.Play("WalkUp@Player");
            if (dir == Vector2.down)
                _animator.Play("WalkDown@Player");
        });

        OnStopMoving.AddListener(() =>
        {
            if (_lastDir == Vector2.left)
                _animator.Play("IdleLeft@Player");
            if (_lastDir == Vector2.right)
                _animator.Play("IdleRight@Player");
            if (_lastDir == Vector2.up)
                _animator.Play("IdleUp@Player");
            if (_lastDir == Vector2.down)
                _animator.Play("IdleDown@Player");
        });


        InputsManager.Instance.MoveAction.started += StartMoving;
        InputsManager.Instance.MoveAction.canceled += StopMoving;



    }

    private void Initialize()
    {
        
    }

    private Coroutine _movingRoutine = null;
    private void StartMoving(InputAction.CallbackContext context)
    {
        _isMoving = true;
        _movingRoutine ??= StartCoroutine(MovingRoutine());
    }

    private IEnumerator MovingRoutine()
    {
        yield return StartCoroutine(FirstStepRoutine());

        OnStartMoving?.Invoke();
        while (_isMoving)
        {
            if(!TryReadMovementInput(out Vector2 movementInput))
            {
                yield return new WaitForEndOfFrame();
                continue;
            }
            yield return new WaitForEndOfFrame();
            if (_forwardDetector.CurrentObstacleDetected != null)
                continue;

            float t = 0f;
            Vector2 startPos = transform.position;
            Vector2 endPos = startPos + movementInput * _movementDistance;

            while(t < 1f)
            {
                t += Time.deltaTime * _movementSpeed;
                if (t >= 1f) t = 1f;
                transform.position = Vector3.Lerp(startPos, endPos, t);

                yield return new WaitForEndOfFrame();
            }
        }
        OnStopMoving?.Invoke();
        _movingRoutine = null;

    }

    private IEnumerator FirstStepRoutine()
    {
        if(TryReadMovementInput(out Vector2 movementInput) && LastDir != movementInput)
            yield return new WaitForSeconds(0.1f);

        LastDir = movementInput;
    }

    private bool TryReadMovementInput(out Vector2 res)
    {
        Vector2 moveInput = InputsManager.Instance.MoveAction.ReadValue<Vector2>();
        if (moveInput.x > 0 && moveInput.y > 0)
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
        _isMoving = false;
        
        /*
        if (_movingRoutine != null)
        {
            StopCoroutine(_movingRoutine);
            _movingRoutine = null;
        } 
        */
    }
}
