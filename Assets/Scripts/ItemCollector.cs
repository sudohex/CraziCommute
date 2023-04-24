using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    int coins = 0;
    [SerializeField] Text coinText;
    [SerializeField] AudioSource coinSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectible"))
        {
            Destroy(other.gameObject);
            coins++;
            coinText.text = "Coins: " + coins;
            coinSound.Play();
        }
    }
}
