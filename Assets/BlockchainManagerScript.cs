using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Thirdweb;
using UnityEngine.UI;
using TMPro;
using UDEV.ChickenMerge;

public class BlockchainManagerScript : MonoBehaviour
{

    public string Address { get; private set; }
    public static BlockchainManagerScript Instance { get; private set; }
    public GameObject claimPanel;
    public Button claimNFTPassButton;
    public TextMeshProUGUI claimNFTPassButtonText;
    public GameObject playGameBtn;
    public GameObject bottomBtn;
    public TextMeshProUGUI text_Address_Detail;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public async void Login()
    {
        Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
        Contract contract = ThirdwebManager.Instance.SDK.GetContract("0x3F194b3651817307cdB12673d5970F3E773Bd84e");
        text_Address_Detail.text = Address;
        List<NFT> nftList = await contract.ERC721.GetOwned(Address);
        if (nftList.Count > 0)
        {
            playGameBtn.SetActive(true);
            bottomBtn.SetActive(true);
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
        else
        {
            playGameBtn.SetActive(false);
            claimPanel.SetActive(true);
            bottomBtn.SetActive(false);
        }
    }

    public async void ClaimNFTPass() {
        claimNFTPassButtonText.text = "Claiming...";
        claimNFTPassButton.interactable = false;
        Contract contract = ThirdwebManager.Instance.SDK.GetContract("0x3F194b3651817307cdB12673d5970F3E773Bd84e");
        Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
        var result = await contract.ERC721.ClaimTo(Address, 1);

        claimNFTPassButtonText.text = "Claimed NFT Pass!";
        claimPanel.SetActive(false);
        playGameBtn.SetActive(true);
        bottomBtn.SetActive(true);
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

    public void HideAllButton() {
        bottomBtn.SetActive(false);
        playGameBtn.SetActive(false);
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }



}
