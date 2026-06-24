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
    public string contractAddress = "0x1C96cfcE46919B7c19549CC8271080F7fA86dfeD";
    public BigInteger tokenId = 0;

    private ThirdwebContract contract;
    private IThirdwebWallet wallet;
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

    // Call this to connect the players wallet
    public async Task ConnectWallet()
    {
        var walletOptions = new WalletOptions(
            provider: WalletProvider.InAppWallet,
            chainId: 80002,
            inAppWalletOptions: new InAppWalletOptions(authprovider: AuthProvider.Google)
        );

        wallet = await ThirdwebManager.Instance.ConnectWallet(walletOptions);

        playerWalletAddress = await wallet.GetAddress();
        Debug.Log("Wallet connected: " + playerWalletAddress);

        contract = await ThirdwebManager.Instance.GetContract(
            contractAddress,
            80002
        );

        Debug.Log("Contract loaded!");
    }

    // Call this when player collects an item
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
            0,
            playerWalletAddress,
            tokenId,
            1,
            new object[] { }
        );

        Debug.Log("Item claimed! Transaction: " + result.TransactionHash);
    }

    // Check how many items the player owns
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