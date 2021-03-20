using Managers;
using TMPro;
using Units;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameUI : MonoBehaviour
    {
        [Header("Simple Information")] 
        public TextMeshProUGUI leftPlayerText;
        public TextMeshProUGUI rightPlayerText;
        public TextMeshProUGUI waitingUnitsText;
        public TextMeshProUGUI roundText;
        public TextMeshProUGUI winText;
        public Button endTurnButton;
        
        [Header("Our Unit Information ")] 
        public TextMeshProUGUI unitInfoText;
        public TextMeshProUGUI unitNameText;
        
        [Header("Enemy Unit Information ")] 
        public TextMeshProUGUI enemyNameText;
        public TextMeshProUGUI enemyInfoText;
        
        [Header("Unit Bar Options")]
        public GameObject UnitBarScout;
        public GameObject UnitBarHacker;
        public GameObject UnitBarHeavy;
        public GameObject UnitBarDatabase;
        public GameObject UnitBarWebServer;
       
        [Header("Information Window")]
        public TextMeshProUGUI informationNameText;
        public TextMeshProUGUI informationBodyText;
        
        //Singleton reference of UI
        public static GameUI instance;
        private int round;

        /*
        * Assign singleton reference to this script
        */
        private void Awake()
        {
            instance = this;
            DisableInformationBars();
            round = 0;
        }

        /*
         * Sets player text upon intialisation
         */
        public void SetPlayerText(PlayerController player)
        {
            TextMeshProUGUI text = player == GameManager.instance.leftPlayer ? leftPlayerText : rightPlayerText;
            text.text = player.photonPlayer.NickName;
        }

        /*
         * Invoked when End Turn is selected
         */
        public void OnEndTurnButton()
        {
            DisableInformationBars();
            PlayerController.me.EndTurn();
        }

        // Toggles button interactivity
        public void ToggleEndTurnButton(bool toggle)
        {
            endTurnButton.interactable = toggle;
            waitingUnitsText.gameObject.SetActive(toggle);
        }

        /*
         * Displays number of units left to select
         */
        public void UpdateWaitingUnitsText(int waitingUnits)
        {
            waitingUnitsText.text = waitingUnits + " Units Waiting";
        }

        /*
         * Updates round text
         */
        public void UpdateRoundText()
        {
            round++;
            roundText.text = "Round: " + round;
        }

        /*
         * Updates unit stats for selected unit
         */
        public void SetUnitInfoText(Unit unit)
        {
            unitInfoText.gameObject.SetActive(true);
            unitNameText.text = unit.GetUnitName();
            
            unitInfoText.text = "";
            unitInfoText.text += string.Format("<b>Hp:</b> {0} / {1}", unit.GetCurrentHp(), unit.GetMaxHp());
            unitInfoText.text += string.Format("<b>Defence:</b> {0} / {1}", unit.GetCurrentDef(), unit.GetMaxDef());
            unitInfoText.text += string.Format("\n<b>Move Range:</b> {0}", unit.GetMovementDistance());
        }

        /*
         * TODO: Change this to hover!! 
         * Updates enemy stats when selecting / hovering over an enemy unit
         */
        public void SetEnemyInfoText(Unit unit)
        {
            enemyInfoText.gameObject.SetActive(true);
            enemyNameText.text = unit.GetUnitName();
            
            enemyInfoText.text = "";
            enemyInfoText.text += string.Format("<b>Hp:</b> {0} / {1}", unit.GetCurrentHp(), unit.GetMaxHp());
            enemyInfoText.text += string.Format("<b>Defence:</b> {0} / {1}", unit.GetCurrentDef(), unit.GetMaxDef());
            enemyInfoText.text += string.Format("\n<b>Move Range:</b> {0}", unit.GetMovementDistance());
        }

        // displays the win text
        public void SetWinText(string winnerName)
        {
            winText.gameObject.SetActive(true);
            winText.text = winnerName + " Wins";
        }
        
        /*
         * Invoked when player Quits
         * TODO: Implement functionality
         */
        public void QuitGame()
        {
            Debug.Log("Change scene to menu and disconnect from unity network here");
        }


        /*
         * Invoked by player controller when a unit IS selected, to display the relevant stats
         */
        public void ToggleUnitBar(Unit unit)
        {
            DisableInformationBars();

            int unitID = unit.GetUnitID();
            
            switch (unitID)
            {
                //Scout
                case 1 : 
                    UnitBarScout.SetActive(true);
                    break;
                
                //Hacker
                case 2 : 
                    UnitBarHacker.SetActive(true);
                    break;
                
                //Heavy
                case 3 : 
                    UnitBarHeavy.SetActive(true);
                    break;
                
                //Database
                case 4 : 
                    UnitBarDatabase.SetActive(true);
                    break;
                
                //Web server
                case 5 : 
                    UnitBarWebServer.SetActive(true);
                    break;
                
                default : 
                    Debug.Log("No valid unit ID selected when setting unit bar!");
                    break;
            }
        }
        
        
        /*
         * Disables all action bars when a unit is selected 
         */
        private void DisableInformationBars()
        {
            UnitBarScout.SetActive(false);
            UnitBarHacker.SetActive(false);
            //UnitBarHeavy.SetActive(false);
            //UnitBarDatabase.SetActive(false);
            //UnitBarWebServer.SetActive(false);
        }
        

        /*
         * Displays Unit Information
         */
        public void DisplayUnitInfo()
        {
            Unit unit = PlayerController.me.selectedUnit;
            informationNameText.text = unit.GetUnitName();
            informationBodyText.text = unit.GetUnitInformation();


        }
        
        /*
         * Displays Enemy Unit Information
         * TODO: Implement from hover selection class
         */
        public void DisplayEnemyInfo(Unit unit)
        {
            informationNameText.text = unit.GetUnitName();
            informationBodyText.text = unit.GetUnitInformation();
            
        }

        /*
         * Displays Move Information of selected move
         * Attach this script to each unit move, and link to the info button on that move
         */
        public void DisplayMoveInfo(UnitAbility ability)
        {
            informationNameText.text = ability.Name();
            informationBodyText.text = ability.Information();
        }
    }
}