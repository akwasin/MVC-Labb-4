using System;
using System.Collections.Generic;

namespace KKGGames_Labb2.Models
{
    public class TicTacToeModel
    {
        public Cell[][] CellBoard { get; set; }
        public bool IsGameComplete { get; private set; }
        public bool IsTie { get; private set; }

        private int Xmax { get; set; } = 3;
        private int Ymax { get; set; } = 3;
        private int Winstreak { get; set; } = 3;

        private int[][] PointBoard { get; set; }
        private List<Coordinate[]> PossiblePlacementList { get; set; }
        private Coordinate ChosenCell { get; set; }

        public TicTacToeModel()
        {
            CellBoard = new Cell[Xmax][];
            for (int i = 0; i < Xmax; i++)
                CellBoard[i] = new Cell[Ymax];
        }
        public TicTacToeModel(int xmax, int ymax, int winstreak)
        {
            Xmax = xmax;
            Ymax = ymax;
            Winstreak = winstreak;
            CellBoard = new Cell[Xmax][];
            for (int i = 0; i < Xmax; i++)
                CellBoard[i] = new Cell[Ymax];
        }

        public bool IsComputerTurn()
        {
            Random random = new Random();
            int randomnumber = random.Next(0, 2);
            if (randomnumber == 0)
                return true;
            return false;
        }

        public void ParseChosenCell(string chosenCell)
        {
            ChosenCell = new Coordinate
            {
                X = int.Parse(chosenCell.Split(',')[0]),
                Y = int.Parse(chosenCell.Split(',')[1])
            };
        }
        public bool TryPlaceChosenCell()
        {
            if (CellBoard[ChosenCell.X][ChosenCell.Y] == Cell.Empty)
            {
                CellBoard[ChosenCell.X][ChosenCell.Y] = Cell.Player;
                if (IsWin(ChosenCell, Cell.Player) || CheckTie())
                    IsGameComplete = true;
                return true;
            }
            return false;
        }

        private bool CheckTie()
        {
            for (int x = 0; x < Xmax; x++)
            {
                for (int y = 0; y < Ymax; y++)
                {
                    if (CellBoard[x][y] == Cell.Empty)
                        return false;
                }
            }
            IsTie = true;
            return true;
        }

        private bool IsWin(Coordinate coordinate, Cell actor)
        {
            if (CheckDirection(coordinate, -1, 0, actor))
                return true;
            if (CheckDirection(coordinate, 0, -1, actor))
                return true;
            if (CheckDirection(coordinate, 1, -1, actor))
                return true;
            if (CheckDirection(coordinate, -1, -1, actor))
                return true;
            return false;
        }
        private bool CheckDirection(Coordinate coordinate, int xd, int yd, Cell actor)
        {
            int counter = Winstreak - 1;
            counter = CheckCell(coordinate, xd, yd, actor, counter);
            if (counter == 0)
                return true;
            counter = CheckCell(coordinate, xd * (-1), yd * (-1), actor, counter);
            if (counter == 0)
                return true;
            return false;
        }
        private int CheckCell(Coordinate coordinate, int xd, int yd, Cell actor, int counter)
        {
            Coordinate currentCoordinate = new Coordinate { X = coordinate.X + xd, Y = coordinate.Y + yd };
            bool isInBounds = currentCoordinate.X >= 0 && currentCoordinate.X < Xmax && currentCoordinate.Y >= 0 &&
                              currentCoordinate.Y < Ymax;
            if (isInBounds && CellBoard[currentCoordinate.X][currentCoordinate.Y] == actor)
            {
                counter--;
                if (counter != 0)
                    counter = CheckCell(currentCoordinate, xd, yd, actor, counter);
                return counter;
            }
            return counter;
        }

        public void ComputerTurn()
        {
            CreatePossiblePlacementList();
            FilterPossiblePlacementList();
            InitiatePointBoard();
            GeneratePointBoard();
            PlaceRandomHighestPointCell();
        }
        private void CreatePossiblePlacementList()
        {
            //TBI Generic version
            PossiblePlacementList = new List<Coordinate[]>();
            for (int x = 0; x < Xmax; x++)
            {
                Coordinate[] horizontalCombo = {
                    new Coordinate {X = x, Y = 0},
                    new Coordinate {X = x, Y = 1},
                    new Coordinate {X = x, Y = 2}
                };
                PossiblePlacementList.Add(horizontalCombo);
            }
            for (int y = 0; y < Xmax; y++)
            {
                Coordinate[] verticalCombo = {
                    new Coordinate {X = 0, Y = y},
                    new Coordinate {X = 1, Y = y},
                    new Coordinate {X = 2, Y = y}
                };
                PossiblePlacementList.Add(verticalCombo);
            }
            Coordinate[] diagonal1Combo = {
                new Coordinate {X = 0, Y = 0},
                new Coordinate {X = 1, Y = 1},
                new Coordinate {X = 2, Y = 2}
            };
            PossiblePlacementList.Add(diagonal1Combo);
            Coordinate[] diagonal2Combo = {
                new Coordinate {X = 0, Y = 2},
                new Coordinate {X = 1, Y = 1},
                new Coordinate {X = 2, Y = 0}
            };
            PossiblePlacementList.Add(diagonal2Combo);
        }
        private void FilterPossiblePlacementList()
        {
            List<Coordinate[]> tempList = new List<Coordinate[]>();
            bool currentIsDefensive = false;
            int currentWeight = 0;
            foreach (var winCombo in PossiblePlacementList)
            {
                int weightComputer = 0;
                int weightPlayer = 0;
                foreach (var coordinate in winCombo)
                {
                    if (CellBoard[coordinate.X][coordinate.Y] == Cell.Computer)
                        weightComputer++;
                    if (CellBoard[coordinate.X][coordinate.Y] == Cell.Player)
                        weightPlayer++;
                }
                if (weightComputer == 0 || weightPlayer == 0)
                {
                    bool isDefensive = weightPlayer > weightComputer;
                    int weight = weightComputer + weightPlayer;

                    if (weight > currentWeight || (weight == currentWeight && !isDefensive && currentIsDefensive))
                    {
                        tempList.Clear();
                        currentWeight = weight;
                        currentIsDefensive = isDefensive;
                        tempList.Add(winCombo);
                    }
                    else if (weight == currentWeight && isDefensive == currentIsDefensive)
                        tempList.Add(winCombo);
                }
                PossiblePlacementList = tempList;
            }

        }

        private void InitiatePointBoard()
        {
            PointBoard = new int[Xmax][];
            for (int i = 0; i < Xmax; i++)
                PointBoard[i] = new int[Ymax];
        }
        private void GeneratePointBoard()
        {
            int count = 0;
            foreach (var winCombo in PossiblePlacementList)
            {
                count++;
                foreach (var coordinate in winCombo)
                {
                    if (CellBoard[coordinate.X][coordinate.Y] == Cell.Empty)
                        PointBoard[coordinate.X][coordinate.Y] += 1;
                }
            }
            if(count == 0)
                for (int x = 0; x < Xmax; x++)
                {
                    for (int y = 0; y < Ymax; y++)
                    {
                        if (CellBoard[x][y] == Cell.Empty)
                        {
                            PointBoard[x][y] += 1;
                            return;
                        }
                    }
                }
        }
        private void PlaceRandomHighestPointCell()
        {
            List<Coordinate> HighestPoints = new List<Coordinate>();
            int maxPoint = 0;
            for (int x = 0; x < Xmax; x++)
            {
                for (int y = 0; y < Ymax; y++)
                {
                    if (PointBoard[x][y] > maxPoint)
                    {
                        maxPoint = PointBoard[x][y];
                        HighestPoints.Clear();
                    }
                    if (PointBoard[x][y] >= maxPoint)
                        HighestPoints.Add(new Coordinate { X = x, Y = y });
                }
            }
            Random random = new Random();
            int randomHighest = random.Next(0, HighestPoints.Count);
            Coordinate ComputerChosenCell = HighestPoints[randomHighest];
            CellBoard[ComputerChosenCell.X][ComputerChosenCell.Y] = Cell.Computer;
            if (IsWin(ComputerChosenCell, Cell.Computer) || CheckTie())
                IsGameComplete = true;
        }
    }

    public class Coordinate
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public enum Cell
    {
        Empty,
        Player,
        Computer
    }
}