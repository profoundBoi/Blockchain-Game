using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Thirdweb;
using Thirdweb.Unity;
using System.Threading.Tasks;
using System.Numerics;

public class BlockchainManager : MonoBehaviour
{
    [Header("Contract Settings")]
    public string contractAddress = "YOUR_CONTRACT_ADDRESS";
    public BigInteger tokenId = 0;

    [Header("Admin Settings")]
    [SerializeField] private string adminPrivateKey = "";

    private ThirdwebContract contract;
    private IThirdwebWallet adminWallet;
    private IThirdwebWallet playerWallet;
    private string playerWalletAddress;

    public static BlockchainManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Connect admin wallet on start - silent, player never sees this
    public async Task InitializeAdmin()
    {
        adminWallet = await PrivateKeyWallet.Create(
            ThirdwebManager.Instance.Client,
            adminPrivateKey
        );
        Debug.Log("Admin wallet ready");

        contract = await ThirdwebManager.Instance.GetContract(
            contractAddress,
            80002
        );
        Debug.Log("Contract loaded!");
    }

    // Option 1: Guest wallet - auto generated, no login needed
    public async Task ConnectAsGuest()
    {
        var walletOptions = new WalletOptions(
            provider: WalletProvider.InAppWallet,
            chainId: 80002,
            inAppWalletOptions: new InAppWalletOptions(
                authprovider: AuthProvider.Guest
            )
        );

        playerWallet = await ThirdwebManager.Instance.ConnectWallet(walletOptions);
        playerWalletAddress = await playerWallet.GetAddress();
        Debug.Log("Guest wallet connected: " + playerWalletAddress);
    }

    // Option 2: Email/OTP - persistent across sessions
    public async Task ConnectWithEmail(string email)
    {
        var walletOptions = new WalletOptions(
            provider: WalletProvider.InAppWallet,
            chainId: 80002,
            inAppWalletOptions: new InAppWalletOptions(
                email: email
            )
        );

        playerWallet = await ThirdwebManager.Instance.ConnectWallet(walletOptions);
        playerWalletAddress = await playerWallet.GetAddress();
        Debug.Log("Email wallet connected: " + playerWalletAddress);
    }

    // Option 3: MetaMask/external wallet via WalletConnect
    public async Task ConnectWithWallet()
    {
        var walletOptions = new WalletOptions(
            provider: WalletProvider.WalletConnectWallet,
            chainId: 80002
        );

        playerWallet = await ThirdwebManager.Instance.ConnectWallet(walletOptions);
        playerWalletAddress = await playerWallet.GetAddress();
        Debug.Log("External wallet connected: " + playerWalletAddress);
    }

    // Mint item to player - admin pays gas, player receives item
    public async Task ClaimItem()
    {
        if (contract == null || adminWallet == null)
        {
            Debug.LogError("Not initialized.");
            return;
        }

        if (string.IsNullOrEmpty(playerWalletAddress))
        {
            Debug.LogError("Player wallet not connected.");
            return;
        }

        Debug.Log("Minting item to player: " + playerWalletAddress);

        var result = await contract.Write(
            adminWallet,
            "claim",
            0,
            playerWalletAddress,
            tokenId,
            new BigInteger(1),
            "0xeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee",
            new BigInteger(0),
            new object[] { new byte[0], new BigInteger(0), new BigInteger(0), "0x0000000000000000000000000000000000000000" },
            new byte[0]
        );

        Debug.Log("Item minted to player! Transaction: " + result.TransactionHash);
    }

    public async Task<string> GetPlayerBalance()
    {
        if (contract == null || string.IsNullOrEmpty(playerWalletAddress)) return "0";

        var balance = await contract.Read<BigInteger>(
            "balanceOf",
            playerWalletAddress,
            tokenId
        );

        return balance.ToString();
    }

    public string GetPlayerAddress()
    {
        return playerWalletAddress;
    }
}