using UnityEngine;

public class GameEndTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Ring"))
        {
            FindObjectOfType<Game>().GameWon();
        }
        
    }
}
