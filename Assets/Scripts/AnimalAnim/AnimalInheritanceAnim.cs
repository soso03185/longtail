using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AnimalActing;
using System;
using UnityEngine.Rendering;

public class AnimalInheritanceAnim : MonoBehaviour
{
    [SerializeField] AnimalState _state = AnimalState.Idle;
    [SerializeField] Vector2 _parentVec2;

    private int _count;
    public GameObject _parentObj;
    private bool _xFlip;

    Animator _anim;
    AnimalInheritance _animalInheritance;
    SortingGroup _animalSortingGroup;

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
        _animalInheritance = GetComponent<AnimalInheritance>();
        _animalSortingGroup = GetComponent<SortingGroup>();
    }
    private void Start()
    {
        if (!_xFlip) State = AnimalState.Jump;
        else if (_xFlip) State = AnimalState.JumpRight;

        _count = _animalInheritance.Index - 1;

        if (_count != -1)
            _parentObj = GameObject.Find("AnimalPrefabs_" + _count);
        else
            _parentObj = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        UpdateController();
        UpdateParentVec();

        if (State != AnimalState.Walk && _anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            State = AnimalState.Walk;

            _animalSortingGroup.sortingOrder = 2;
        }
    }


    void UpdateParentVec()
    {
        if(Mathf.Abs(_parentObj.transform.position.x - this.transform.position.x) > 0.1f)
        {
            if (_parentObj.transform.position.x - this.transform.position.x > 0.1f)
            {
                _parentVec2 = Vector2.right;
                _xFlip = true;
            }
            else if (_parentObj.transform.position.x - this.transform.position.x < 0.1f)
            {
                _parentVec2 = Vector2.left;
                _xFlip = false;
            }
        }
        else if(Mathf.Abs(_parentObj.transform.position.y - this.transform.position.y) > 0.1f)
        {
            if (_parentObj.transform.position.y - this.transform.position.y > 0.1f) _parentVec2 = Vector2.up;
            else if (_parentObj.transform.position.y - this.transform.position.y < 0.1f) _parentVec2 = Vector2.down;
        }
    }

    protected void OnTriggerStay2D(Collider2D other)
    {
        if (_parentVec2 == Vector2.right && other.gameObject.name == "Left")
        {
            if (!_xFlip) State = AnimalState.Jump;
            else if (_xFlip) State = AnimalState.JumpRight;
        }
        if (_parentVec2 == Vector2.left && other.gameObject.name == "Right")
        {
            if (!_xFlip) State = AnimalState.Jump;
            else if(_xFlip) State = AnimalState.JumpRight;
        }
        if (_parentVec2 == Vector2.down && other.gameObject.name == "Up")
        {
            if (!_xFlip) State = AnimalState.Jump;
            else if(_xFlip) State = AnimalState.JumpRight;
        }
        if (_parentVec2 == Vector2.up && other.gameObject.name == "Down")
        {
            if (!_xFlip) State = AnimalState.Jump;
            else if(_xFlip) State = AnimalState.JumpRight;
        }
    }

    void UpdateController()
    {
        switch (State)
        {
            case AnimalState.Walk:
                if (!_xFlip) _anim.Play("walk");
                if (_xFlip) _anim.Play("walkRight");
                break;
            case AnimalState.WalkRight:
                _anim.Play("walkRight");
                break;
            case AnimalState.Jump:
                _anim.Play("jump");
                if (_parentVec2 == Vector2.up) _animalSortingGroup.sortingOrder = 6;
                else _animalSortingGroup.sortingOrder = 3;
                break;
            case AnimalState.JumpRight:
                _anim.Play("jumpRight");
                if (_parentVec2 == Vector2.up) _animalSortingGroup.sortingOrder = 6;
                else _animalSortingGroup.sortingOrder = 3;
                break;
        }
    }
}
