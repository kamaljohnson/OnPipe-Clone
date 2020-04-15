using UnityEngine;

public class Destructible : MonoBehaviour
{

    public float lifeSpan;

    private float _lifeTimer;

    private void Update()
    {
        if (Game.gameState == GameStatus.GameOver) return;
        
        _lifeTimer += Time.deltaTime;
        if (_lifeTimer >= lifeSpan)
        {
            Destroy(gameObject);
        }
    }
}
