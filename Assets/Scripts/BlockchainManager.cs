using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Thirdweb;
using Thirdweb.Unity;
using UnityEngine;


public class BlockchainManager : MonoBehaviour
{
    [Header("Contract Settings")]
    public string contractAddress = "0x1C96cfcE46919B7c19549CC8271080F7fA86dfeD";
    public BigInteger tokenId = 0;

    private ThirdwebContract contract;
    private IThirdwebWallet wallet;
    private string playerWalletAddress;

    [Header("Admin Settings")]
    [SerializeField] private string adminPrivateKey = "";

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

    public async Task ConnectWallet()
    {
        // Connect as admin using private key
        var walletOptions = new WalletOptions(
            provider: WalletProvider.PrivateKeyWallet,
            chainId: 80002
        );

        // Override with admin key
        wallet = await PrivateKeyWallet.Create(
            ThirdwebManager.Instance.Client,
            adminPrivateKey
        );

        playerWalletAddress = await wallet.GetAddress();
        Debug.Log("Admin wallet connected: " + playerWalletAddress);

        contract = await ThirdwebManager.Instance.GetContract(
            contractAddress,
            80002
        );

        Debug.Log("Contract loaded!");
    }

    public async Task ClaimItem()
    {
        if (contract == null)
        {
            Debug.LogError("Contract not loaded. Connect wallet first.");
            return;
        }

        Debug.Log("Claiming item...");

        var result = await contract.Write(
            wallet,
            "claim",
            0,                          // value in wei
            playerWalletAddress,        // receiver
            tokenId,                    // tokenId
            new BigInteger(1),          // quantity
            "0xeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee", // currency (native)
            new BigInteger(0),          // pricePerToken
            new object[] { new byte[0], new BigInteger(0), new BigInteger(0), "0x0000000000000000000000000000000000000000" }, // allowlistProof
            new byte[0]                 // data
        );

        Debug.Log("Item claimed! Transaction: " + result.TransactionHash);
    }

    public async Task<string> GetPlayerBalance()
    {
        if (contract == null) return "0";

        var balance = await contract.Read<BigInteger>(
            "balanceOf",
            playerWalletAddress,
            tokenId
        );

        return balance.ToString();
    }
}