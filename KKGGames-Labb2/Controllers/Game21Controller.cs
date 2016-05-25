using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KKGGames_Labb2.Models;

namespace KKGGames_Labb2.Controllers
{
    public class Game21Controller : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult _Game()
        {
            Game21Model model = new Game21Model();
            if (model.IsComputerTurn())
                model.ComputerAi();
            return PartialView(model);
        }

        [HttpPost]
        public PartialViewResult _Game(Game21Model model)
        {
            ModelState.Remove("CurrentValue");
            ModelState.Remove("Counter");
            switch (model.PlayerTurn())
            {
                case GameState.Win:
                    return PartialView("_Win");
                case GameState.Lose:
                    return PartialView("_Lose");
                default:
                    switch (model.ComputerAi())
                    {
                        case GameState.Win:
                            return PartialView("_Win");
                        case GameState.Lose:
                            return PartialView("_Lose");
                        default:
                            return PartialView(model);
                    }
            }
        }
    }
}