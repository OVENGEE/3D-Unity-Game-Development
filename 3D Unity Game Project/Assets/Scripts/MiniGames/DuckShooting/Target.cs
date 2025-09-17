using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Target : MonoBehaviour
{
    //Deactivate after delay
    [SerializeField] private float timeoutDelay = 1f;

    private IObjectPool<Target> objectPool;
    public IObjectPool<Target> ObjectPool { set => objectPool = value; }


    public void PlayAfterShotRoutine()
    {
        StartCoroutine(AfterShotRoutine());
    }

    public IEnumerator AfterShotRoutine()
    {
        // Target disappears
        this.GetComponent<Collider>().enabled = false;
        this.GetComponent<Renderer>().enabled = false;

        yield return new WaitForSecondsRealtime(timeoutDelay);

        // Target reappears
        this.GetComponent<Collider>().enabled = true;
        this.GetComponent<Renderer>().enabled = true;
    }
}

//Code reference:
// The introduction to the object pooling logic: https://www.youtube.com/watch?v=U08ScgT3RVM