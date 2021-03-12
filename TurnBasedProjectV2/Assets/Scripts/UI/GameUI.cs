using Managers;
using TMPro;
using Units;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameUI : MonoBehaviour
    {
        public Button endTurnButton;
        public TextMeshProUGUI leftPlayerText;
        public TextMeshProUGUI rightPlayerText;
        public TextMeshProUGUI waitingUnitsText;
        public TextMeshProUGUI unitInfoText;
        public TextMeshProUGUI enemyInfoText;
        public TextMeshProUGUI winText;
        public GameObject mockAttackBar;

        // instance
        public static GameUI instance;

        /*
        * Assign singleton reference to this script
        */
        private void Awake() => instance = this;


        // called when the "End Turn" button has been pressed
        public void OnEndTurnButton()
        {
            DisableAttackBars();
            PlayerController.me.EndTurn();
        }

        // toggles the interactivity of the end turn button
        public void ToggleEndTurnButton(bool toggle)
        {
            endTurnButton.interactable = toggle;
            waitingUnitsText.gameObject.SetActive(toggle);
        }

        // updates the current amount of units you can move this turn
        public void UpdateWaitingUnitsText(int waitingUnits)
        {
            waitingUnitsText.text = waitingUnits + " Units Waiting";
        }

        // sets the requested player's text
        public void SetPlayerText(PlayerController player)
        {
            TextMeshProUGUI text = player == GameManager.instance.leftPlayer ? leftPlayerText : rightPlayerText;
            text.text = player.photonPlayer.NickName;
        }

        public void SetUnitBar(Unit unit)
        {
            DisableAttackBars();
            mockAttackBar.SetActive(true);
        }

        private void DisableAttackBars()
        {
            mockAttackBar.SetActive(false);
        }

        // sets the unit info text
        public void SetUnitInfoText(Unit unit)
        {
            unitInfoText.gameObject.SetActive(true);
            unitInfoText.text = "";
            unitInfoText.text += string.Format("<b>Hp:</b> {0} / {1}", unit.GetCurrentHp(), unit.GetMaxHp());
            unitInfoText.text += string.Format("\n<b>Move Range:</b> {0}", unit.GetMovementDistance());
            //unitInfoText.text += string.Format("\n<b>Attack Range:</b> {0}", unit.maxAttackDistance);//unitInfoText.text += string.Format("\n<b>Damage:</b> {0} - {1}", unit.minDamage, unit.maxDamage);
        }

        // sets the unit info text
        public void SetEnemyInfoText(Unit unit)
        {
            unitInfoText.gameObject.SetActive(true);
            unitInfoText.text = "";
            unitInfoText.text += string.Format("<b>Hp:</b> {0} / {1}", unit.GetCurrentHp(), unit.GetMaxHp());
            unitInfoText.text += string.Format("\n<b>Move Range:</b> {0}", unit.GetMovementDistance());
            //unitInfoText.text += string.Format("\n<b>Attack Range:</b> {0}", unit.maxAttackDistance);//unitInfoText.text += string.Format("\n<b>Damage:</b> {0} - {1}", unit.minDamage, unit.maxDamage);
        }

        // displays the win text
        public void SetWinText(string winnerName)
        {
            winText.gameObject.SetActive(true);
            winText.text = winnerName + " Wins";
        }
    }
}