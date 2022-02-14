using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;
using static AnimalActing;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    [Header("Input")]
    public KeyCode ActionKey = KeyCode.Space;
    public KeyCode ActionKeyAlt = KeyCode.Joystick1Button0;

    [Header("Movement")]
    public float Speed = 5f;
    public Vector3 Direction = Vector2.right;

    [Header("BodyParts")]
    public AnimalInheritance AnimalInheritance_Prefab;
    public int BodyPartsOffset = 7;
    public float MinTimeBetweenLostParts = 1f;
    public int MaxAmountOfBodyParts = 20;

    [Header("Bindings")]
    public Text PointsCounter;
    public Text HP;

    [Header("Feedbacks")]
    public MMFeedbacks TurnFeedback;
    public MMFeedbacks TeleportFeedback;
    public MMFeedbacks TeleportOnceFeedback;
    public MMFeedbacks EatFeedback;
    public MMFeedbacks LoseFeedback;

    [Header("Debug")]
    [MMReadOnly]
    public int AnimalPoints = 0;
    [MMReadOnly]
    public int PlayerHp = 3;
    [MMReadOnly]
    public float _speed;
    [MMReadOnly]
    public float _lastFoodEatenAt = -100f;
   
    protected Vector3 _newPosition;
    protected MMPositionRecorder _recorder;
    public List<AnimalInheritance> _animalPrefabs;
    protected float _lastLostPart = 0f;
    public bool _blockTouch = false;
    public int _xFlipIndex = 0;

    protected virtual void Awake()
    {
        AnimalPoints = 0;
        _recorder = this.gameObject.GetComponent<MMPositionRecorder>();
        PointsCounter.text = "0";
        HP.text = "HP : " + PlayerHp.ToString();
        _animalPrefabs = new List<AnimalInheritance>();
    }

    protected virtual void Update()
    {
        _speed = Speed;
        HandleInput();
        HandleMovement();

        if (_xFlipIndex == 4)
            _xFlipIndex = 0;
    }

    protected void OnTriggerStay2D(Collider2D other)
    {
        if (_blockTouch) return;
        if (other.gameObject.name == "Left" && _xFlipIndex == 0) _blockTouch = true;
        if (other.gameObject.name == "Right" && _xFlipIndex == 2) _blockTouch = true;
        if (other.gameObject.name == "Up" && _xFlipIndex == 3) _blockTouch = true;
        if (other.gameObject.name == "Down" && _xFlipIndex == 1) _blockTouch = true;
    }
    
    public void AnimalListManage()
    {
        var _animalPrefabsClone = from animal in _animalPrefabs
                                  group animal by animal._kind into kind
                                  select (Category: kind.Key, ProductCount: kind.Count());

        foreach (var child in _animalPrefabsClone)
        {
            Debug.Log(child.ProductCount);

            //if (child.ProductCount >= 5)
            //{
            //    for (int i = -5; i < -1; i++)
            //        Destroy(_animalPrefabs[_animalPrefabs.Count - i].gameObject);

            //    _animalPrefabs.RemoveRange(_animalPrefabs.Count - 5,_animalPrefabs.Count - 1);

            //}
        }
    }
    protected virtual void HandleInput()
    {
        if (Input.GetKeyDown(ActionKey) || Input.GetKeyDown(ActionKeyAlt) || Input.GetMouseButtonDown(0))
        {
            _xFlipIndex ++;
            Turn();
        }
    }
    protected virtual void HandleMovement()
    {
        _newPosition = (_speed * Time.deltaTime * Direction);
        this.transform.position += _newPosition;
    }
    public virtual void Turn()
    {
        TurnFeedback?.PlayFeedbacks();
        Direction = MMMaths.RotateVector2(Direction, 90f);
     //   this.transform.Rotate(new Vector3(0f, 0f, 90f));
    }
    public virtual void Teleport()
    {
           StartCoroutine(TeleportCo());
    }
    public virtual void Eat()
    {
        EatEffect();

        EatFeedback?.PlayFeedbacks();
        AnimalPoints++;
        PointsCounter.text = AnimalPoints.ToString();
        StartCoroutine(EatCo());

    }

    public virtual void EatEffect()
    {
        _lastFoodEatenAt = Time.time;

        AnimalInheritance part = Instantiate(AnimalInheritance_Prefab);
        part.transform.position = this.transform.position;
        part.TargetRecorder = _recorder;
        part.Offset = ((AnimalPoints) * BodyPartsOffset) + BodyPartsOffset + 7;
        part.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        part.Index = _animalPrefabs.Count;
        part.name = "AnimalPrefabs_" + part.Index;
        _animalPrefabs.Add(part);
    }
    protected virtual IEnumerator TeleportCo()
    {
        TeleportFeedback?.PlayFeedbacks();

        TeleportOnceFeedback?.PlayFeedbacks();

        yield return MMCoroutine.WaitForFrames(BodyPartsOffset);

        int total = _animalPrefabs.Count;
        float feedbacksIntensity = 0f;
        float part = 1 / (float)total;

        for (int i = 0; i < total; i++)
        {
            yield return MMCoroutine.WaitForFrames(BodyPartsOffset / 2);
            feedbacksIntensity = 1 - i * part;
            TeleportFeedback?.PlayFeedbacks(this.transform.position, feedbacksIntensity);
        }
    }
    protected virtual IEnumerator EatCo()
    {
        int total = _animalPrefabs.Count;
        float feedbacksIntensity = 0f;
        float part = 1 / (float)total;

        for (int i = 0; i < total; i++)
        {
            if (i >= _animalPrefabs.Count)
            {
                yield break;
            }
            yield return MMCoroutine.WaitForFrames(BodyPartsOffset / 2);
            feedbacksIntensity = 1 - i * part;
            if (i == total - 1)
            {
                if ((i < MaxAmountOfBodyParts - 1) && (_animalPrefabs.Count > i))
                {
                    if (_animalPrefabs[i] != null)
                    {
                        _animalPrefabs[i].New();
                    }
                }
            }
            else
            {
                _animalPrefabs[i].Eat(feedbacksIntensity);
            }
        }
    }

    public virtual void Lose(AnimalInheritance part)
    {
        if (Time.time - _lastLostPart < MinTimeBetweenLostParts)
        {
            return;
        }
     
        _lastLostPart = Time.time;
        LoseFeedback?.PlayFeedbacks(part.transform.position);
        Destroy(_animalPrefabs[_animalPrefabs.Count - 1].gameObject);
        _animalPrefabs.RemoveAt(_animalPrefabs.Count - 1);

        PlayerHp--;
        HP.text = "HP : " + PlayerHp.ToString();

        AnimalPoints--;
        PointsCounter.text = AnimalPoints.ToString();
    }
}