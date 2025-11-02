using UnityEngine;
using TMPro;

public class CoinCounter : MonoBehaviour
{
    public static int coinCount;
    public TextMeshProUGUI coinText;

    private void Start()
    {
        coinCount = 0;
        coinText.text = coinCount.ToString();
    }

    public void AddCoins(int amount)
    {
        coinCount += amount;
        coinText.text = coinCount.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            AddCoins(1);
            Destroy(other.gameObject);
            Debug.Log("triggerts");
        }  
    }

}
