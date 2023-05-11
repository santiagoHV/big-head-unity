using UnityEngine;
using System.Collections;

public class CoinCollector : MonoBehaviour
{
    public AudioClip pickupSFX, spawnSFX;
    public float acceleration = 20;
    public float screenTarget = 0.05f;
    AudioSource audioSource;
    Ray ray;
    Collider triggerCollider;
    float position, maxDistance, speed;
    
    void ComputePath()
    {
        var screenPoint = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, screenTarget, 0));
        var screenRay = Camera.main.ScreenPointToRay(screenPoint);
        var delta = screenRay.origin - transform.position;
        maxDistance = delta.magnitude + 1;
        ray.direction = delta.normalized;
        ray.origin = transform.position;
        position = 0;
        speed = 0;
    }

    IEnumerator Move()
    {
        transform.parent = null;

        while (position < maxDistance)
        {
            position += speed * Time.deltaTime;
            speed += acceleration * Time.deltaTime;
            transform.position = ray.GetPoint(position);
            yield return null;
        }

        Destroy(gameObject);
    }

    void OnMouseEnter()
    {
        ComputePath();
        audioSource.PlayOneShot(pickupSFX);
        StartCoroutine("Move");
        triggerCollider.enabled = false;
    }
    void Awake()
    {
        triggerCollider = GetComponent<Collider>();
        triggerCollider.enabled = false;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }
    void PlayCoinSpawnSFX()
    {
        audioSource.PlayOneShot(spawnSFX);
    }
    void OnSpawnAnimationEnded()
    {
        GetComponent<Animator>().enabled = false;
        triggerCollider.enabled = true;
    }
}
