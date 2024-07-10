using UDEV.DMTool;
using Thirdweb;
using UnityEngine.UI;
using UnityEngine;

namespace UDEV.ChickenMerge
{
    public class LevelCompleteDialog : Dialog
    {
        public Button homeBtn;
        public Button replayBtn;
        public Button nextLevelBtn;
        public Button saveLevelBtn;
        public Text saveLevelBtnText;

        public override void Show()
        {
            base.Show();
            GameController.ChangeState(GameState.Completed);
            title.SetText($"LEVEL {UserDataHandler.Ins.curLevelId} CLEARED");
            AdmobController.Ins.ShowInterstitial();
        }

        public async void NextLevel()
        {
            homeBtn.interactable = false;
            replayBtn.interactable = false;
            nextLevelBtn.interactable = false;
            saveLevelBtn.interactable = false;
            Contract contract = ThirdwebManager.Instance.SDK.GetContract("0x5bF24FFC1d501FDa3bA4Bfc359ce50b8Fe2e024A");
            var data = await contract.ERC20.Claim("1");
            Debug.Log("Gold were claimed!");
            homeBtn.interactable = true;
            replayBtn.interactable = true;
            nextLevelBtn.interactable = true;
            saveLevelBtn.interactable = true;

            Close();
            if (DataGroup.Ins.IsMaxGameLevel)
            {
                Helper.LoadScene(GameScene.Menu.ToString(), true);
            }
            else
            {
                Helper.ReloadScene();
            }
        }

        public void BackHome()
        {
            Close();
            Helper.LoadScene(GameScene.Menu.ToString(), true);
        }

        public async void Replay()
        {
            homeBtn.interactable = false;
            replayBtn.interactable = false;
            nextLevelBtn.interactable = false;
            saveLevelBtn.interactable = false;
            Contract contract = ThirdwebManager.Instance.SDK.GetContract("0x5bF24FFC1d501FDa3bA4Bfc359ce50b8Fe2e024A");
            var data = await contract.ERC20.Claim("1");
            Debug.Log("Gold were claimed!");
            homeBtn.interactable = true;
            replayBtn.interactable = true;
            nextLevelBtn.interactable = true;
            saveLevelBtn.interactable = true;

            Close();
            Helper.ReloadScene();
        }

        public int ConvertFloatToInt(float input)
        {
            if (input < 2f)
            {
                return 1;
            }
            else if (input > 53f)
            {
                return 53;
            }
            else
            {
                return Mathf.RoundToInt(input);
            }
        }

        private int ConvertLevelToInt(float valueToConvert)
        {
            int result1 = ConvertFloatToInt(valueToConvert);
            return result1;
        }

        public async void SaveLevelForPlayer()
        {
            homeBtn.interactable = false;
            replayBtn.interactable = false;
            nextLevelBtn.interactable = false;
            saveLevelBtn.interactable = false;
            saveLevelBtnText.text = "Saving...";
            string address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
            Contract contract = ThirdwebManager.Instance.SDK.GetContract("0x1fB9f744E2d63DF21818AaB4B21b31eFd2831d2A");
            var data = await contract.ERC20.BalanceOf(address);
            int blockchainLevel = ConvertLevelToInt(float.Parse(data.displayValue));

            int CurrentLevel = UserDataHandler.Ins.curLevelId;
            if (blockchainLevel < CurrentLevel)
            {
                int levelAdded = CurrentLevel - blockchainLevel;
                var data1 = await contract.ERC20.Claim(levelAdded.ToString());
                Debug.Log("Tokens were claimed");
                saveLevelBtnText.text = CurrentLevel.ToString();
                homeBtn.interactable = true;
                replayBtn.interactable = true;
                nextLevelBtn.interactable = true;
            }
            else
            {
                saveLevelBtnText.text = CurrentLevel.ToString();
                homeBtn.interactable = true;
                replayBtn.interactable = true;
                nextLevelBtn.interactable = true;
            }

        }
    }

}