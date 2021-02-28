using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    /*
     * Team based Turn Manager
     * Each player can chose any member in the team
     * This implements the subscriber design pattern
     */
    public class TurnManager : MonoBehaviour
    {
        //A dictionary for all units within the game. The list holds all members of that particular team
        static Dictionary<string, List<MovementRules>> units = new Dictionary<string, List<MovementRules>>();

        //The key for whose turn it is
        private static Queue<string> turnKey = new Queue<string>();

        //Queue for which team has the turn
        private static Queue<MovementRules> turnTeam = new Queue<MovementRules>();

        /*
         * 
         */
        void Update()
        {
            //Called on first run
            if (turnTeam.Count == 0)
                Init();
        }

        /*
         * Initialise the team turns
         */
        static void Init()
        {
            //peek into the queue to return the head, and grab this list so we can add to team queue
            List<MovementRules> teamList = units[turnKey.Peek()];

            foreach (MovementRules unit in teamList)
                turnTeam.Enqueue(unit);
            
            StartTurn();
        }

        /*
         * 
         */
        private static void StartTurn()
        {
            if (turnTeam.Count > 0)
                turnTeam.Peek().BeginUnitTurn(); //peek into the turn team queue, and begin it's turn
        }

        /*
         * Invoked when a unit has ended moving or manually ended turn
         */
        public static void EndTurn()
        {
            //remove this unit from the queue
            MovementRules unit = turnTeam.Dequeue();
            unit.EndUnitTurn();
            
            if (turnTeam.Count > 0 )
                StartTurn(); //start turn for next item
            else
            {
                //creates an infinite loop where every team is added to the queue (move last)
                string team = turnKey.Dequeue();
                turnKey.Enqueue(team);
                Init();
            }
        }

        /*
         * Employs subscriber pattern, unit adds themselves to the dictionary
         */
        public static void AddUnit(MovementRules unit)
        {
            List<MovementRules> list;

            //if never been added, we add it to dictionary and assign brand new list
            if (!units.ContainsKey(unit.tag))
            {
                list = new List<MovementRules>();
                units[unit.tag] = list;
                
                //if the turn key contains this, enqueue so we do not duplicate the turn
                if (!turnKey.Contains(unit.tag))
                    turnKey.Enqueue(unit.tag);
            }
            
            // else if it does find the tag in the dictionary, grab that list
            else
                list = units[unit.tag];

            list.Add(unit);
        }
        
        
        /*
         * TODO add a remove unit function that removes unit from the queue,
         * and if its the last member of its team, remove that fro, its turnkey
         */
        private static void RemoveUnit()
        {
            
        }
        
        
    }
}