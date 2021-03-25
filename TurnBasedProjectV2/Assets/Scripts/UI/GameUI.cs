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
        public TextMeshProUGUI unitNameText;
        public TextMeshProUGUI unitInfoText;
        public GameObject unitStatsUI;

        [Header("Enemy Unit Information ")] 
        public TextMeshProUGUI enemyNameText;
        public TextMeshProUGUI enemyInfoText;
        public GameObject enemyStatsUI;
        
        [Header("Unit Bar Options")]
        public GameObject UnitBarScout;
        public GameObject UnitBarHacker;
        public GameObject UnitBarHeavy;
        public GameObject UnitBarAnalyst;
        public GameObject UnitBarDatabase;
        public GameObject UnitBarWebServer;
       
        [Header("Information Window")]
        public TextMeshProUGUI informationNameText;
        public TextMeshProUGUI informationBodyText;
        public GameObject informationUI;
        
        //Singleton reference of UI
        public static GameUI instance;

        /*
        * Assign singleton reference to this script
        */
        private void Awake()
        {
            instance = this;
            DisableInformationBars();
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
            waitingUnitsText.text = "Waiting for player..";
            PlayerController.me.EndTurn();
        }

        /*
         * Toggles button interactivity
         */
        public void ToggleEndTurnButton(bool toggle) => endTurnButton.interactable = toggle;

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
        public void UpdateRoundText(int roundNumber)
        {
            roundText.text = "Round: " + roundNumber;
        }

        /*
         * Display
         */
        public void DisplayWinText(string winnerName)
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
            //GameManager.instance.WinGame(0);
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
                
                //Heavy
                case 4 : 
                    UnitBarAnalyst.SetActive(true);
                    break;

                
                //Database
                case 5 : 
                    UnitBarDatabase.SetActive(true);
                    break;
                
                //Web server
                case 6 : 
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
            UnitBarHeavy.SetActive(false);
            UnitBarAnalyst.SetActive(false);
            //UnitBarDatabase.SetActive(false);
            //UnitBarWebServer.SetActive(false);

            informationUI.SetActive(false);
            unitStatsUI.SetActive(false);
            enemyStatsUI.SetActive(false);
        }

        /*
         * Updates unit stats for selected unit
         */
        public void DisplayUnitStats(Unit unit)
        {
            unitNameText.text = unit.GetUnitName();
            unitInfoText.text = "";
            unitInfoText.text += string.Format("<b>Hp:</b> {0} / {1}", unit.GetCurrentHp(), unit.GetMaxHp() + "\n");
            unitInfoText.text += string.Format("<b>Defence:</b> {0} / {1}", unit.GetCurrentDef(), unit.GetMaxDef() + "\n");
            unitInfoText.text += string.Format("<b>Move Range:</b> {0}", unit.GetMovementDistance());
            unitStatsUI.SetActive(true);
        }

        /*  
        * Displays Unit Information in the information menu
        * Invoked externally by the Unit Stats "Info" button
        * If toggle is true, it shows our units information, otherwise it shows enemy information
        */
        public void DisplayUnitInformation(bool isOurs)
        {
            //If true gets our selected unit, otherwise gets the enemy selected unit
            Unit unit = isOurs ? PlayerController.me.selectedUnit : PlayerController.me.selectedEnemyUnit;
            
            informationNameText.text = unit.GetUnitName();
            informationBodyText.text = unit.GetUnitInformation();
            informationUI.SetActive(true);
        }
        
        /*
        * Updates enemy stats when selecting / hovering over an enemy unit
        */
        public void DisplayEnemyStats(Unit unit)
        {
            enemyNameText.text = unit.GetUnitName();
            enemyInfoText.text = "";
            enemyInfoText.text += string.Format("<b>Hp:</b> {0} / {1}", unit.GetCurrentHp(), unit.GetMaxHp() + "\n");
            enemyInfoText.text += string.Format("<b>Defence:</b> {0} / {1}", unit.GetCurrentDef(), unit.GetMaxDef() + "\n");
            enemyInfoText.text += string.Format("<b>Move Range:</b> {0}", unit.GetMovementDistance());
            enemyStatsUI.SetActive(true);
        }
        
        /*
         * Displays Move Information of selected move in the information menu
         * Attach this script to each unit move, and link to the info button on that move
         */
        public void DisplayMoveInfo(UnitAbility ability)
        {
            informationNameText.text = ability.Name();
            informationBodyText.text = ability.Information();
            informationUI.SetActive(true);
        }
    }
}