using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using KKGGames_Labb2.Controllers;

namespace KKGGames_Labb2.Models
{
    public class Game21Model
    {
        public int CurrentValue { get; set; }
        public int ChoosenNumber { get; set; }
        public string ComputersChoice { get; set; }
        public GameState ComputerAi()
        {
            int nextOne;
            if (CurrentValue > 14)
                nextOne = (CurrentValue + 1) % 3 == 0 ? 1 : 2;
            else
            {
                Random random = new Random();
                nextOne = random.Next(1, 3);
            }
            ComputersChoice = nextOne.ToString();
            CurrentValue += nextOne;
            if (CurrentValue == 21)
                return GameState.Lose;
            if (CurrentValue > 21)
                return GameState.Win;
            return GameState.Playing;
        }
        public bool IsComputerTurn()
        {
            Random random = new Random();
            int randomnumber = random.Next(0, 2);
            if (randomnumber == 0)
            {
                return true;
            }
            return false;
        }

        public Game21Model()
        {
            Initiate();
        }

        public GameState PlayerTurn()
        {
            CurrentValue += ChoosenNumber;
            if (CurrentValue == 21)
                return GameState.Win;
            if (CurrentValue > 21)
                return GameState.Lose;
            return GameState.Playing;
        }

        private void Initiate()
        {
            CurrentValue = 0;
        }
    }

    public enum GameState
    {
        Playing, Win, Lose
    }
}