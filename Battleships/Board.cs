using ConsoleTables;

namespace Battleships
{
    public class Board : IBoard
    {
        /// <summary>
        /// Temporary board.
        /// </summary>
        public BoardPoint[][] TemporaryBoard { get; set; }

        /// <summary>
        /// Final board.
        /// </summary>
        public BoardPoint[][] FinalBoard { get; set; }

        /// <summary>
        /// Square size of board.
        /// </summary>
        public int SquareSize { get; set; }

        /// <summary>
        /// Dictionary of directions with values of move in certain direction.
        /// </summary>
        private readonly Dictionary<string, int> _direction = new()
        {
            { nameof(Direction.Up), -1 },
            { nameof(Direction.Down), 1 },
            { nameof(Direction.Left), -1 },
            { nameof(Direction.Right), 1 }
        };

        /// <summary>
        /// Constructor of Board.
        /// </summary>
        public Board()
        {
            SquareSize = 10;
            TemporaryBoard = GenerateEmptyBoard();
            FinalBoard = GenerateEmptyBoard();
        }

        /// <summary>
        /// Generate empty board.
        /// </summary>
        /// <returns>Returns empty board.</returns>
        private BoardPoint[][] GenerateEmptyBoard()
        {
            var board = GetClearBoard();

            SetCorrectPositionOfPoints(board);

            return board;
        }

        /// <summary>
        /// Get clear board.
        /// </summary>
        /// <returns>Array of points.</returns>
        public BoardPoint[][] GetClearBoard()
        {
            var boards = new List<BoardPoint[]>();
            var points = new List<BoardPoint>();

            for (var y = 0; y < SquareSize; y++)
            {
                for (var x = 0; x < SquareSize; x++)
                {
                    points.Add(new BoardPoint());
                }

                boards.Add(points.ToArray());
                points.Clear();
            }

            return boards.ToArray();
        }

        /// <summary>
        /// Set correct position of points.
        /// </summary>
        /// <param name="board">Board.</param>
        private void SetCorrectPositionOfPoints(BoardPoint[][] board)
        {
            for (var i = 0; i < SquareSize; i++)
            {
                for (var j = 0; j < SquareSize; j++)
                {
                    board[i][j].Y = i;
                    board[i][j].X = j;
                }
            }
        }

        /// <summary>
        /// Show temporary board for user.
        /// </summary>
        public void ShowTemporaryBoard()
        {
            var columns = GetColumnsLabel();
            var rows = GetRowsLabel();

            var table = new ConsoleTable(columns)
            {
                Options =
                {
                    EnableCount = false
                }
            };

            for (var i = 0; i < SquareSize; i++)
            {
                var row = GetRowToAddToConsoleTable(rows[i], i);
                table.AddRow(row);
            }

            table.Write();
        }

        /// <summary>
        /// Get label of each column.
        /// </summary>
        /// <returns>Returns array of string with columns.</returns>
        private static string[] GetColumnsLabel()
        {
            string[] cornerSign = { "\\" };
            var letters = Enum.GetNames<Letter>();

            return cornerSign.Concat(letters).ToArray();
        }

        /// <summary>
        /// Get label for each row.
        /// </summary>
        /// <returns>Returns array of numbers for rows.</returns>
        private string[] GetRowsLabel()
        {
            return Enumerable.Range(1, SquareSize).Select(n => n.ToString()).ToArray();
        }

        /// <summary>
        /// Get row to add to ConsoleTable.
        /// </summary>
        /// <param name="labelRow">Label for row.</param>
        /// <param name="indexRow">Index of row.</param>
        /// <returns>Returns object of row to add to ConsoleTable.</returns>
        private object[] GetRowToAddToConsoleTable(string labelRow, int indexRow)
        {
            string[] rowNumber = { labelRow };
            var rowPoints = TemporaryBoard[indexRow].Select(x => x.Label).ToArray();

            var finalRow = rowNumber.Concat(rowPoints).ToArray<object>();

            return finalRow;
        }

        /// <summary>
        /// Generate board with ships.
        /// </summary>
        /// <param name="ships">List of ships.</param>
        public void GenerateBoardWithShips(List<Ship> ships)
        {
            foreach (var ship in ships)
            {
                ship.Points = GetPointsOfShip(ship);
                SetShipPointsOnBoard(ship);
            }
        }

        /// <summary>
        /// Get points of ship.
        /// </summary>
        /// <param name="ship">List of ships.</param>
        /// <returns>Returns list of BoardPoints to add on board.</returns>
        private List<BoardPoint> GetPointsOfShip(Ship ship)
        {
            var points = new List<BoardPoint>();
            var boardPoint = new BoardPoint();
            var tempSize = ship.Size;

            do
            {
                points.Clear();
                boardPoint.SetRandomPoint(SquareSize);

                if (IsPointOfShip(boardPoint))
                {
                    continue;
                }

                points.Add(boardPoint);
                var orientation = ShipService.GetRandomOrientation();

                if (orientation == Orientation.Horizontal)
                {
                    GetHorizontalPoints(tempSize, boardPoint, points, ship.Size);
                }
                else if (orientation == Orientation.Vertical)
                {
                    GetVerticalPoints(tempSize, boardPoint, points, ship.Size);
                }
            } while (IsNeedMorePointsFromAnotherSide(points.Count, ship.Size));

            return points;
        }

        /// <summary>
        /// Get horizontal points.
        /// </summary>
        /// <param name="tempSize">Temp size.</param>
        /// <param name="boardPoint">Board point.</param>
        /// <param name="points">Points.</param>
        /// <param name="shipSize">Size of ship.</param>
        private void GetHorizontalPoints(int tempSize, BoardPoint boardPoint, ICollection<BoardPoint> points, int shipSize)
        {
            GetPoints(Direction.Right, tempSize, boardPoint.X + _direction[nameof(Direction.Right)], boardPoint.Y, points);

            if (IsNeedMorePointsFromAnotherSide(points.Count, shipSize))
            {
                GetPoints(Direction.Left, tempSize, boardPoint.X + _direction[nameof(Direction.Left)], boardPoint.Y, points);
            }
        }

        /// <summary>
        /// Get vertical points.
        /// </summary>
        /// <param name="tempSize">Temp size.</param>
        /// <param name="boardPoint">Board point.</param>
        /// <param name="points">Points.</param>
        /// <param name="shipSize">Size of ship.</param>
        private void GetVerticalPoints(int tempSize, BoardPoint boardPoint, ICollection<BoardPoint> points, int shipSize)
        {
            GetPoints(Direction.Up, tempSize, boardPoint.X, boardPoint.Y + _direction[nameof(Direction.Up)], points);

            if (IsNeedMorePointsFromAnotherSide(points.Count, shipSize))
            {
                GetPoints(Direction.Down, tempSize, boardPoint.X, boardPoint.Y + _direction[nameof(Direction.Down)], points);
            }
        }

        /// <summary>
        /// Check is need more points from another side.
        /// </summary>
        /// <param name="currentAmountOfPoints">Current amount of points.</param>
        /// <param name="requireAmountOfPoints">Require amount of points.</param>
        /// <returns>Returns boolean value. Returns true if needs to find more points to set ship on board, otherwise returns false.</returns>
        private static bool IsNeedMorePointsFromAnotherSide(int currentAmountOfPoints, int requireAmountOfPoints)
        {
            return currentAmountOfPoints < requireAmountOfPoints;
        }

        /// <summary>
        /// Get points to list of points for ship if point is correct and needed.
        /// </summary>
        /// <param name="direction">Direction to find next point for ship.</param>
        /// <param name="tempSize">Temp size to know how many points to add is needed.</param>
        /// <param name="x">Index of point in horizontal position (column).</param>
        /// <param name="y">Index of point in vertical position (row).</param>
        /// <param name="points">List of points for ship.</param>
        private void GetPoints(Direction direction, int tempSize, int x, int y, ICollection<BoardPoint> points)
        {
            while (true)
            {
                if (IsWrongPoint(x, y, tempSize))
                {
                    return;
                }

                AddPoint(points, y, x);

                if (IsEnoughPoints(points.Count, tempSize))
                {
                    return;
                }

                switch (direction)
                {
                    case Direction.Right:
                        x += _direction[nameof(Direction.Right)];
                        continue;
                    case Direction.Left:
                        x += _direction[nameof(Direction.Left)];
                        continue;
                    case Direction.Up:
                        y += _direction[nameof(Direction.Up)];
                        continue;
                    case Direction.Down:
                        y += _direction[nameof(Direction.Down)];
                        continue;
                }

                break;
            }
        }

        /// <summary>
        /// Check if point is part of ship.
        /// </summary>
        /// <param name="boardPoint">Point of board.</param>
        /// <returns>Returns boolean value. Returns true if point is part of ship, otherwise returns false.</returns>
        private bool IsPointOfShip(BoardPoint boardPoint)
        {
            return FinalBoard[boardPoint.Y][boardPoint.X].Value != 0;
        }

        /// <summary>
        /// Check is wrong point to add to ship.
        /// </summary>
        /// <param name="x">Index of point in horizontal position (column).</param>
        /// <param name="y">Index of point in vertical position (row).</param>
        /// <param name="tempSize">Temp size to know how many points to add is needed.</param>
        /// <returns>Returns boolean value. Returns true if point is incorrect or unwanted, otherwise returns false.</returns>
        private bool IsWrongPoint(int x, int y, int tempSize)
        {
            return y is < 0 or >= 10 || x is < 0 or >= 10 || FinalBoard[y][x].Value != 0 || tempSize == 0;
        }

        /// <summary>
        /// Check is ship have enough added points.
        /// </summary>
        /// <param name="amountOfPoints">Current founded amount of points.</param>
        /// <param name="requiredAmountOfPoints">Required amount of points.</param>
        /// <returns>Returns boolean value. Returns true if enough amount of points, otherwise returns false.</returns>
        private static bool IsEnoughPoints(int amountOfPoints, int requiredAmountOfPoints)
        {
            return amountOfPoints == requiredAmountOfPoints;
        }

        /// <summary>
        /// Add point to ship.
        /// </summary>
        /// <param name="points">Current list of points.</param>
        /// <param name="y">Index of point in vertical position (row).</param>
        /// <param name="x">Index of point in horizontal position (column).</param>
        private static void AddPoint(ICollection<BoardPoint> points, int y, int x)
        {
            var point = new BoardPoint
            {
                Y = y,
                X = x
            };

            points.Add(point);
        }

        /// <summary>
        /// Set ship points on final board.
        /// </summary>
        /// <param name="ship">Ship.</param>
        private void SetShipPointsOnBoard(Ship ship)
        {
            foreach (var point in ship.Points)
            {
                FinalBoard[point.Y][point.X].Value = ship.Id;
            }
        }
    }
}