namespace Battleships
{
    public class Game : IGame
    {
        /// <summary>
        /// Correct hits.
        /// </summary>
        public int CorrectHits { get; set; }

        /// <summary>
        /// Misses hits.
        /// </summary>
        public int MissesHits { get; set; }

        /// <summary>
        /// Ships.
        /// </summary>
        private readonly List<Ship> _ships;

        /// <summary>
        /// Constructor of game.
        /// </summary>
        /// <param name="ships">Ships.</param>
        public Game(List<Ship> ships)
        {
            _ships = ships;
        }

        /// <summary>
        /// Hit field on board.
        /// </summary>
        /// <param name="board">Board.</param>
        /// <param name="inputPoint">Input point from user.</param>
        /// <exception cref="Exception">Incorrect input point.</exception>
        public void HitField(IBoard board, string? inputPoint)
        {
            if (string.IsNullOrEmpty(inputPoint) || !IsCorrectPoint(inputPoint))
            {
                throw new Exception("Incorrect input point.");
            }

            var (letter, number) = GetDividedPoint(inputPoint);
            var (rowIndex, colIndex) = GetParsedIndexes(letter, number);

            if (HasPointBeenAlreadyHit(board.TemporaryBoard, rowIndex, colIndex))
            {
                throw new Exception("Point has been already hit.");
            }

            AssignValueOfPointToTemporaryBoardIfShip(board, rowIndex, colIndex);
            UpdateHitsCounter(board, rowIndex, colIndex);
            MarkPointAsFound(board.TemporaryBoard, rowIndex, colIndex);
            ShowNotificationIfShipHasSunk(board, rowIndex, colIndex);
        }

        /// <summary>
        /// Is found every ship point.
        /// </summary>
        /// <param name="board">Board.</param>
        /// <returns>Returns boolean value. Returns true if found every point on board, otherwise returns false.</returns>
        public bool IsFoundEveryShipPoint(Board board)
        {
            for (var y = 0; y < board.SquareSize; y++)
            {
                for (var x = 0; x < board.SquareSize; x++)
                {
                    if (board.FinalBoard[y][x].Value != 0 && board.FinalBoard[y][x].Value != board.TemporaryBoard[y][x].Value)
                    {
                        return false;
                    }
                }
            }

            Console.WriteLine("You won!");
            return true;
        }

        /// <summary>
        /// Show final stats about game.
        /// </summary>
        public void ShowFinalStats()
        {
            Console.WriteLine("FINAL STATS");
            Console.WriteLine($"Correct hits: {CorrectHits}");
            Console.WriteLine($"Misses hits: {MissesHits}");
        }

        /// <summary>
        /// Assign value of point to temporary board if ship.
        /// </summary>
        /// <param name="board">Board.</param>
        /// <param name="rowIndex">Row index.</param>
        /// <param name="colIndex">Column index.</param>
        private static void AssignValueOfPointToTemporaryBoardIfShip(IBoard board, int rowIndex, int colIndex)
        {
            if (board.FinalBoard[rowIndex][colIndex].Value != 0)
            {
                board.TemporaryBoard[rowIndex][colIndex].Value = board.FinalBoard[rowIndex][colIndex].Value;
            }
        }

        /// <summary>
        /// Mark point as found.
        /// </summary>
        /// <param name="temporaryBoard">Board.</param>
        /// <param name="rowIndex">Row index.</param>
        /// <param name="colIndex">Column index.</param>
        private static void MarkPointAsFound(BoardPoint[][] temporaryBoard, int rowIndex, int colIndex)
        {
            temporaryBoard[rowIndex][colIndex].IsFound = true;
        }

        /// <summary>
        /// Get parsed indexes.
        /// </summary>
        /// <param name="letter">Letter.</param>
        /// <param name="number">Number.</param>
        /// <returns>Returns index of row and index of column.</returns>
        private static (int, int) GetParsedIndexes(string letter, string number)
        {
            var rowIndex = int.Parse(number) - 1;
            var colIndex = GetRowIndexFromLetter(letter);

            return (rowIndex, colIndex);
        }

        /// <summary>
        /// Check is correct point.
        /// </summary>
        /// <param name="inputPoint">Input point from user.</param>
        /// <returns>Returns boolean value. Returns true if correct point, otherwise returns false.</returns>
        private bool IsCorrectPoint(string inputPoint)
        {
            var (letter, number) = GetDividedPoint(inputPoint);

            return IsCorrectPointLetter(letter) && IsCorrectPointNumber(number);
        }

        /// <summary>
        /// Get divided point.
        /// </summary>
        /// <param name="inputPoint">Input point from user.</param>
        /// <returns>Returns letter and number as a string.</returns>
        private (string, string) GetDividedPoint(string inputPoint)
        {
            var letter = inputPoint[..1];
            var number = inputPoint[1..];

            return (letter, number);
        }

        /// <summary>
        /// Is correct point letter.
        /// </summary>
        /// <param name="letter">Letter.</param>
        /// <returns>Returns boolean value. Returns true if correct point letter, otherwise returns false.</returns>
        private static bool IsCorrectPointLetter(string letter)
        {
            var enums = Enum.GetValues(typeof(Letter));

            return enums.Cast<object?>().Any(e => e?.ToString() == letter);
        }

        /// <summary>
        /// Is correct point number.
        /// </summary>
        /// <param name="number">Number.</param>
        /// <returns>Returns boolean value. Returns true if correct point number, otherwise returns false.</returns>
        private static bool IsCorrectPointNumber(string number)
        {
            return int.TryParse(number, out var numberParsed) && numberParsed is <= 10 and > 0;
        }

        /// <summary>
        /// Get row index from letter.
        /// </summary>
        /// <param name="letter">Letter.</param>
        /// <returns>Returns index from letter.</returns>
        private static int GetRowIndexFromLetter(string letter)
        {
            foreach (var el in Enum.GetNames(typeof(Letter)))
            {
                if (el == letter)
                {
                    return (int)Enum.Parse(typeof(Letter), el);
                }
            }

            return -1;
        }

        /// <summary>
        /// Check is point has been already hit.
        /// </summary>
        /// <param name="temporaryBoard">Temporary board.</param>
        /// <param name="rowIndex">Index of row.</param>
        /// <param name="colIndex">Index of column.</param>
        /// <returns>Returns boolean value. Returns true if point has been already hit, otherwise returns false.</returns>
        private static bool HasPointBeenAlreadyHit(BoardPoint[][] temporaryBoard, int rowIndex, int colIndex)
        {
            return temporaryBoard[rowIndex][colIndex].IsFound;
        }

        /// <summary>
        /// Show notification if ship has been sunk.
        /// </summary>
        /// <param name="board"></param>
        /// <param name="rowIndex"></param>
        /// <param name="colIndex"></param>
        private void ShowNotificationIfShipHasSunk(IBoard board, int rowIndex, int colIndex)
        {
            var pointValue = board.TemporaryBoard[rowIndex][colIndex].Value;
            if (pointValue == 0)
            {
                return;
            }

            var ship = _ships.FirstOrDefault(x => x.Id == pointValue);

            var hasSunk = ship != null && ship.Points.TrueForAll(p => ship.Id == board.TemporaryBoard[p.Y][p.X].Value);

            if (hasSunk)
            {
                Console.WriteLine("Ship has sunk.");
            }
        }

        /// <summary>
        /// Update hits counter.
        /// </summary>
        /// <param name="board">Board.</param>
        /// <param name="rowIndex">Index of row.</param>
        /// <param name="colIndex">Index of column.</param>
        private void UpdateHitsCounter(IBoard board, int rowIndex, int colIndex)
        {
            if (board.FinalBoard[rowIndex][colIndex].Value == 0)
            {
                MissesHits++;
            }
            else
            {
                CorrectHits++;
            }
        }
    }
}