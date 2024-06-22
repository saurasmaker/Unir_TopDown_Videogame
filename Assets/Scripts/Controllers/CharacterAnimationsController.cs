using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimationsController : MonoBehaviour
{
    [SerializeField]
    private string _animationsKeyName;

    private Animator _animator;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public bool SetIdleAnimation(Vector2 dir)
    {
        if (dir == Vector2.left)
            _animator.Play("IdleLeft@" + _animationsKeyName);
        if (dir == Vector2.right)
            _animator.Play("IdleRight@" + _animationsKeyName);
        if (dir == Vector2.up)
            _animator.Play("IdleUp@" + _animationsKeyName);
        if (dir == Vector2.down)
            _animator.Play("IdleDown@" + _animationsKeyName);
        else
            return false;

        return true;
    }

    public bool SetWalkingAnimation(Vector2 dir)
    {
        if (dir == Vector2.left)
            _animator.Play("WalkLeft@" + _animationsKeyName);
        else if (dir == Vector2.right)
            _animator.Play("WalkRight@" + _animationsKeyName);
        else if (dir == Vector2.up)
            _animator.Play("WalkUp@" + _animationsKeyName);
        else if (dir == Vector2.down)
            _animator.Play("WalkDown@" + _animationsKeyName);
        else
            return false;

        return true;
    }
}
