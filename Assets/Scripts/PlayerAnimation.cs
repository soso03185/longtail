using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AnimalActing;

public class PlayerAnimation : MonoBehaviour
{
    private Animator _anim;
    private PlayerController _player;
   
    private AnimalState _state = AnimalState.Walk;

    [SerializeField] bool _xFlip;
    [SerializeField] int _xFlipIndex;

    private AnimalState State
    {
        get { return _state; }
        set
        {
            if (_state == value)
                return;

            _state = value;
        }
    }

    void Awake() 
    { 
        _anim = GetComponent<Animator>();
        _player = this.transform.parent.GetComponent<PlayerController>();
    }

    void Update()
    {
        UpdateState();
        if (State != AnimalState.Walk && _anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            State = AnimalState.Walk;
        }

        _xFlipIndex = _player._xFlipIndex;
        if (_xFlipIndex == 2) _xFlip = false;
        else if (_xFlipIndex == 0) _xFlip = true;

        if(_player._blockTouch)
        {
             State = AnimalState.Jump;
          
            _player._blockTouch = false;
        }

    }


    void UpdateState()
    {
        switch (State)
        {
            case AnimalState.Walk:
                if (!_xFlip) _anim.Play("PlayerMove");
                else if (_xFlip) _anim.Play("PlayerRightMove");
                break;
            case AnimalState.Jump:
                /* if (!_xFlip) */ _anim.Play("PlayerFalldown");
             //   else if (_xFlip) _anim.Play("PlayerRightFalldown"); 
                break;
        }
    }
}
