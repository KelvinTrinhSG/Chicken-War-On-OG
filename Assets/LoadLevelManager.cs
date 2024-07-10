using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Thirdweb;
using UnityEngine.UI;
using TMPro;

namespace UDEV.ChickenMerge
{
    public class LoadLevelManager : MonoBehaviour
    {
        public Button loadLevelBtn;
        public TextMeshProUGUI LoadLevelBtnText;

        public Button IAPShopBtn;
        public Button QuestBtn;
        public Button ChestBtn;
        public Button BoosterBtn;
        public Button SettingBtn;

        private void Start()
        {
            loadLevelBtn.gameObject.SetActive(false);
            LoadLevelBtnText.text = "Load Level";
            loadLevelBtn.interactable = true;
        }
        //After connect wallet then show
        //Only show when balance > 0
        public void OnWalletConnected()
        {
            GetLevelBalance();
        }

        public async void GetLevelBalance()
        {
            string address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
            Contract contract = ThirdwebManager.Instance.SDK.GetContract("0x1fB9f744E2d63DF21818AaB4B21b31eFd2831d2A");

            var data = await contract.ERC20.BalanceOf(address);

            if (float.Parse(data.displayValue) > 0f)
            {
                loadLevelBtn.gameObject.SetActive(true);
                LoadLevelBtnText.text = "Load Level";
                loadLevelBtn.interactable = true;
            }
            else
            {
                loadLevelBtn.gameObject.SetActive(false);
                LoadLevelBtnText.text = "Load Level";
                loadLevelBtn.interactable = true;
            }
        }

        //Not enable remaining buttons
        private void HideAllButtons()
        {
            loadLevelBtn.interactable = false;
            IAPShopBtn.interactable = false;
            QuestBtn.interactable = false;
            ChestBtn.interactable = false;
            BoosterBtn.interactable = false;
            SettingBtn.interactable = false;
        }

        private void ShowAllButtons()
        {
            IAPShopBtn.interactable = true;
            QuestBtn.interactable = true;
            ChestBtn.interactable = true;
            BoosterBtn.interactable = true;
            SettingBtn.interactable = true;
        }
        //Loading text
        public async void LoadLevelButtonClick()
        {
            HideAllButtons();
            LoadLevelBtnText.text = "Loading...";
            string address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
            Contract contract = ThirdwebManager.Instance.SDK.GetContract("0x1fB9f744E2d63DF21818AaB4B21b31eFd2831d2A");
            var data = await contract.ERC20.BalanceOf(address);

            if (float.Parse(data.displayValue) >= 1f && float.Parse(data.displayValue) <= 19f)
            {
                int intValue = Mathf.FloorToInt(float.Parse(data.displayValue)); // Convert float to int

                for (int i = 1; i <= intValue; i++)
                {
                    LoadLevelDataToGame(i);
                }
                Debug.Log("Loading Successfully");
                LoadLevelBtnText.text = "Level Loaded";
                ShowAllButtons();
            }
            else
            {
                Debug.LogWarning("displayValue is out of range. It should be between 1 and 19.");
            }
        }
        //Return normal text
        public void LoadLevelDataToGame(int value)
        {
            UserDataHandler.Ins.UpdateLevelUnlocked(value, true);
            Debug.Log("Github");
        }
    }
}

