using UnityEngine;

public class CoinRotation : MonoBehaviour
{
    public float speed = 360;

    static int RandomSign
    {
        get
        {
            return Random.Range(0, 2) == 0 ? 1 : -1;
        }
    }
    public void Randomize()
    {
        speed *= RandomSign;
        transform.Rotate(Vector3.up, Random.Range(0, 360));
    }
    void Start()
    {
        Randomize();
    }
	void Update ()
    {
        transform.Rotate(Vector3.up, speed * Time.deltaTime);
	}
}
