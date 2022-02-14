using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;
using static AnimalActing;

public class AnimalInheritance : MonoBehaviour
{
    /// a position recorder this body part will look at to know where to go to
    public MMPositionRecorder TargetRecorder;
    /// a feedback to play when food gets eaten
    public MMFeedbacks EatFeedback;
    /// a feedback to play when this part appears
    public MMFeedbacks NewFeedback;

    public int Offset = 0;
    public int Index = 0;
    protected PlayerController _player;
    protected BoxCollider2D _collider2D;

    public AnimalKind _kind = AnimalKind.Guineapig3_baby;

    protected virtual void Awake()
    {
        _collider2D = this.gameObject.MMGetComponentNoAlloc<BoxCollider2D>();
        StartCoroutine(ActivateCollider());
    }
    protected virtual IEnumerator ActivateCollider()
    {
        yield return MMCoroutine.WaitFor(1f);
        _collider2D.enabled = true;
    }
    protected virtual void Update()
    {
        this.transform.position = TargetRecorder.Positions[Offset];
    }

    public virtual void Eat(float intensity)
    {
        EatFeedback?.PlayFeedbacks(this.transform.position, intensity);
    }
    public virtual void New()
    {
        NewFeedback?.Initialization();
        NewFeedback?.PlayFeedbacks();
    }
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (Index == 0) 
            return;

        _player = other.GetComponent<PlayerController>();

        if (_player != null) 
            _player.Lose(this);
    }
}
