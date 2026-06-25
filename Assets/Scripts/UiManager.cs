using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [Header("Buttons")]
    public Button connectWalletButton;
    public Button claimItemButton;

    [Header("Text")]
    public TextMeshProUGUI statusText;

    void Start()
    {
        connectWalletButton.onClick.AddListener(OnConnectWalletClicked);
        claimItemButton.onClick.AddListener(OnClaimItemClicked);
        claimItemButton.interactable = false;
        statusText.text = "Not connected";
    }

    void OnConnectWalletClicked()
    {
        StartCoroutine(ConnectWalletCoroutine());
    }

    IEnumerator ConnectWalletCoroutine()
    {
        statusText.text = "Connecting...";
        connectWalletButton.interactable = false;

        var task = BlockchainManager.Instance.ConnectWallet();
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            statusText.text = "Connection failed!";
            Debug.LogError(task.Exception);
        }
        else
        {
            statusText.text = "Wallet connected!";
            claimItemButton.interactable = true;
        }

        connectWalletButton.interactable = true;
    }

    void OnClaimItemClicked()
    {
        StartCoroutine(ClaimItemCoroutine());
    }

    IEnumerator ClaimItemCoroutine()
    {
        statusText.text = "Claiming item...";
        claimItemButton.interactable = false;

        var task = BlockchainManager.Instance.ClaimItem();
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            statusText.text = "Claim failed!";
            Debug.LogError(task.Exception);
        }
        else
        {
            statusText.text = "Item claimed!";
        }


        claimItemButton.interactable = true;
    }
}