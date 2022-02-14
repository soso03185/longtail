using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;

public class SpawnArea : MonoBehaviour
{
    /// the food prefab to spawn
    public AnimalFree _animalFreePrefab;
    /// the maximum amount of food in the scene
    public int AmountOfFood = 3;
    /// the minimum coordinates to spawn at (in viewport units)
    public Vector2 MinRandom = new Vector2(0.1f, 0.1f);
    /// the maximum coordinates to spawn at (in viewport units)
    public Vector2 MaxRandom = new Vector2(0.9f, 0.9f);

    protected List<AnimalFree> _animalFreeList;
    protected Camera _mainCamera;

    /// <summary>
    /// On start, instantiates food
    /// </summary>
    protected virtual void Start()
    {
        _mainCamera = Camera.main;
        _animalFreeList = new List<AnimalFree>();
        for (int i = 0; i < AmountOfFood; i++)
        {
            AnimalFree food = Instantiate(_animalFreePrefab);
            food.transform.position = DetermineSpawnPosition();
            food.Spawner = this;
            _animalFreeList.Add(food);
        }
    }

    /// <summary>
    /// Determines a valid position at which to spawn food
    /// </summary>
    /// <returns></returns>
    public virtual Vector3 DetermineSpawnPosition()
    {
        Vector3 newPosition = MMMaths.RandomVector2(MinRandom, MaxRandom);
        newPosition.z = 10f;
        newPosition = _mainCamera.ViewportToWorldPoint(newPosition);
        return newPosition;
    }
}
