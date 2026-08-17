using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance;

    [Header("Claim Popup")]
    public GameObject claimPanel;
    public TextMeshProUGUI claimTitleText;
    public TextMeshProUGUI claimStatusText;
    public Button claimConfirmButton;
    public Button claimCancelButton;

    [Header("HUD")]
    public TextMeshProUGUI walletAddressText;
    public TextMeshProUGUI itemCountText;

    private CollectibleCoin currentCoin;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        claimPanel.SetActive(false);
        claimConfirmButton.onClick.AddListener(OnClaimConfirmed);
        claimCancelButton.onClick.AddListener(OnClaimCancelled);

        if (BlockchainManager.Instance != null)
        {
            string address = BlockchainManager.Instance.GetPlayerAddress();
            if (!string.IsNullOrEmpty(address))
            {
                walletAddressText.text = address.Substring(0, 6) + "..." + address.Substring(address.Length - 4);
            }
        }
    }

    public void ShowClaimPopup(CollectibleCoin coin)
    {
        currentCoin = coin;
        claimTitleText.text = "Gold Coin Found!";
        claimStatusText.text = "";
        claimPanel.SetActive(true);
    }

    void OnClaimConfirmed()
    {
        StartCoroutine(ClaimCoroutine());
    }

    void OnClaimCancelled()
    {
        currentCoin = null;
        claimPanel.SetActive(false);
    }

    IEnumerator ClaimCoroutine()
    {
        claimConfirmButton.interactable = false;
        claimCancelButton.interactable = false;
        claimStatusText.text = "Recording on blockchain...";

        var task = BlockchainManager.Instance.ClaimItem();
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            claimStatusText.text = "Claim failed. Try again.";
            Debug.LogError(task.Exception);
            claimConfirmButton.interactable = true;
            claimCancelButton.interactable = true;
        }
        else
        {
            claimStatusText.text = "Owned on blockchain!";
            currentCoin.OnClaimed();
            currentCoin = null;

            yield return new WaitForSeconds(1f);
            UpdateItemCount();

            yield return new WaitForSeconds(2f);
            claimPanel.SetActive(false);
        }
    }

    public async void UpdateItemCount()
    {
        string balance = await BlockchainManager.Instance.GetPlayerBalance();
        itemCountText.text = "Gold Coins: " + balance;
    }
}