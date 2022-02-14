using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AnimalActing;

public class AnimalFreeAnim : MonoBehaviour
{
    Animator _anim;
    AnimalState _state = AnimalState.Idle;

    private int _ranStateIndex = -1;

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
    }

    private void OnEnable()
    {
        _ranStateIndex = Random.Range(1, 4);

        switch (_ranStateIndex)
        {
            case 1:
                State = AnimalState.Idle;
                break;
            case 2:
                State = AnimalState.Eat;
                break;
            case 3:
                State = AnimalState.Sleep;
                break;
        }

    }

    void Update()
    {
        UpdateController();
    }


    void UpdateController()
    {
        switch (State)
        {
            case AnimalState.Idle:
                _anim.Play("idle");
                break;
            case AnimalState.Walk:
                _anim.Play("walk");
                break;
            case AnimalState.Jump:
                _anim.Play("jump");
                break;
            case AnimalState.Sleep:
                _anim.Play("sleep");
                break;
            case AnimalState.Eat:
                _anim.Play("eat");
                break;
        }
    }
}
