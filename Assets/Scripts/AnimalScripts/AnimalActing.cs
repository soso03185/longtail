using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalActing : MonoBehaviour
{
    public enum AnimalState
    {
        Idle,
        Walk,
        WalkRight,
        Jump,
        JumpRight,
        Bark,
        Eat,
        Sleep,
        Dead,
    }

    public enum AnimalKind
    {
        Guineapig3_baby,
    }

}
