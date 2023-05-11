using UnityEngine;
using System.Collections;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coin;
    public Transform[] locations;
    public float firstCoinDelay, spawnDelay;
    public Animator textAnimator;

    void SpawnCoinAt(Transform location)
    {
        var newCoin = Instantiate(coin, location.position, location.rotation) as GameObject;
        newCoin.transform.SetParent(location);
    }

    IEnumerator SpawnCoinsCoroutine()
    {
        textAnimator.SetTrigger("PuzzleSolved");
        yield return new WaitForSeconds(firstCoinDelay);
        for (int n = 0; n < locations.Length; ++n)
        {
            if (locations[n].childCount == 0)
            {
                SpawnCoinAt(locations[n]);
                yield return new WaitForSeconds(spawnDelay);
            }
        }
    }

    public void SpawnCoins()
    {
        StartCoroutine("SpawnCoinsCoroutine");
    }
}
