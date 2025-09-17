using UnityEngine;
using UnityEngine.Pool;


public class TargetManager : MonoBehaviour
{
    [Header("Prefab to shoot")]
    [SerializeField] Target targetPrefab;
    // [SerializeField] Target target;

    // I might need to the target script instead of the gameobject itself... because all targets will have the target scripts.


    // throw an exception if we try to return an existing item, already in the pool
    [SerializeField] bool collectionCheck = true;
    [SerializeField] int initialCapacity = 10;
    [SerializeField] int maxCapacity = 50;



    private IObjectPool<Target> objectPool;

    private void Awake()
    {
        objectPool = new ObjectPool<Target>(CreateTarget, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject, collectionCheck, initialCapacity, maxCapacity);
    }

//============The objectPool interface constructor implementation===================================================
    private Target CreateTarget()
    {
        // the CreateFunc inplementation of the construtor (try use lambda expressions)
        Target targetInstance = Instantiate(targetPrefab);
        targetInstance.ObjectPool = objectPool;
        return targetInstance;
    }

    private void OnGetFromPool(Target pooledObject)
    {
        //Activates the pooled object
        pooledObject.gameObject.SetActive(true);
    }

    private void OnReleaseToPool(Target pooledObject)
    {
        //Deactivates the pooled object
        pooledObject.gameObject.SetActive(false);
    }

    private void OnDestroyPooledObject(Target pooledObject)
    {
        //Deletes the pooled object
        Destroy(pooledObject.gameObject);
    }
}

//Code reference:
// The introduction to the object pooling logic: https://www.youtube.com/watch?v=U08ScgT3RVM