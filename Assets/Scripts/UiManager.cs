using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Buttons")]
    public Button guestButton;
    public Button emailButton;
    public Button walletButton;

    [Header("Text")]
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI addressText;

    [Header("Email Input")]
    public TMP_InputField emailInput;

    void Start()
    {
        guestButton.onClick.AddListener(OnGuestClicked);
        emailButton.onClick.AddListener(OnEmailClicked);
        walletButton.onClick.AddListener(OnWalletClicked);

        statusText.text = "Choose how to connect";
        addressText.text = "";

        StartCoroutine(InitializeAdmin());
    }

    IEnumerator InitializeAdmin()
    {
        var task = BlockchainManager.Instance.InitializeAdmin();
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
            Debug.LogError("Admin init failed: " + task.Exception);
        else
            Debug.Log("Admin ready");
    }

    void OnGuestClicked()
    {
        StartCoroutine(ConnectCoroutine("guest"));
    }

    void OnEmailClicked()
    {
        if (string.IsNullOrEmpty(emailInput.text))
        {
            statusText.text = "Please enter an email";
            return;
        }
        StartCoroutine(ConnectCoroutine("email"));
    }

    void OnWalletClicked()
    {
        StartCoroutine(ConnectCoroutine("wallet"));
    }

    IEnumerator ConnectCoroutine(string method)
    {
        statusText.text = "Connecting...";
        SetButtonsInteractable(false);

        System.Threading.Tasks.Task task = null;

        if (method == "guest")
            task = BlockchainManager.Instance.ConnectAsGuest();
        else if (method == "email")
            task = BlockchainManager.Instance.ConnectWithEmail(emailInput.text);
        else if (method == "wallet")
            task = BlockchainManager.Instance.ConnectWithWallet();

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            statusText.text = "Connection failed!";
            Debug.LogError(task.Exception);
            SetButtonsInteractable(true);
        }
        else
        {
            statusText.text = "Connected! Loading game...";
            addressText.text = "Address: " + BlockchainManager.Instance.GetPlayerAddress();
            yield return new WaitForSeconds(1.5f);
            SceneManager.LoadScene("Game");
        }
    }

    void SetButtonsInteractable(bool state)
    {
        guestButton.interactable = state;
        emailButton.interactable = state;
        walletButton.interactable = state;
    }
}