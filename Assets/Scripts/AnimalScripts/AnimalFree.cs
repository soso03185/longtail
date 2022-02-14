using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Rendering;

public class AnimalFree : MonoBehaviour
{
    /// a duration (in seconds) during which the food is inactive before moving it to another position
    public float OffDelay = 1f;
    /// the food's visual representation
    public GameObject Model;
    /// a feedback to play when food gets eaten
    public MMFeedbacks EatFeedback;
    /// a feedback to play when food appears
    public MMFeedbacks AppearFeedback;
    /// the food spawner
    public SpawnArea Spawner { get; set; }
    SortingGroup _freeSortingGroup;

    protected PlayerController _player;

    private void Awake()
    {
        _freeSortingGroup = GetComponent<SortingGroup>();
    }

    /// <summary>
    /// When this food gets eaten, we play its eat feedback, and start moving it somewhere else in the scene
    /// </summary>
    /// <param name="other"></param>
    protected void OnTriggerEnter2D(Collider2D other)
    {
        _player = other.GetComponent<PlayerController>();

        if (_player != null)
        {
            _player.Eat();
            EatFeedback?.PlayFeedbacks();
            StartCoroutine(MoveFood());
        }

      
    }
    protected void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == 8 && this.transform.position.y > other.transform.position.y) _freeSortingGroup.sortingOrder = 0;
        else if (other.gameObject.layer == 6 && this.transform.position.y > other.transform.position.y) _freeSortingGroup.sortingOrder = -2;
        else _freeSortingGroup.sortingOrder = 1;
    }

    /// <summary>
    /// Moves the food to another spot
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator MoveFood()
    {
        Model.SetActive(false);
        yield return MMCoroutine.WaitFor(OffDelay);
        Model.SetActive(true);

        this.transform.position = Spawner.DetermineSpawnPosition();

        AppearFeedback?.PlayFeedbacks();
    }
}

