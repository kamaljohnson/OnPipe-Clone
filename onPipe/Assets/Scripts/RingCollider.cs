using UnityEngine;

public class RingCollider : MonoBehaviour
{

    private Game _game;

    public void Start()
    {
        _game = transform.parent.GetComponent<Ring>().game;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstrecle") && Ring.IsActive)
        {
            _game.GameOver();
        }
    }
}
