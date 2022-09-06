using FluentAssertions;
using Moq;

namespace Battleships.Tests
{
    public class GameTests
    {
        /// <summary>
        /// Field of game.
        /// </summary>
        private readonly Game _game;

        /// <summary>
        /// Field of Board.
        /// </summary>
        private readonly Board _board;

        /// <summary>
        /// Constructor of GameTests.
        /// </summary>
        public GameTests()
        {
            _board = new Board();
            var ships = ShipService.GetShips();
            _board.GenerateBoardWithShips(ships);
            _game = new Game(ships);
        }

        /// <summary>
        /// Check is hit in part of ship and returns true if hits in ship.
        /// </summary>
        [Fact]
        public void HitField_UpdateTemporaryBoard_ReturnsIsHitShip()
        {
            // Given
            var pointBoard = FindShipPoint(_board.FinalBoard);
            var inputPoint = ConvertPointToString(pointBoard);
            _game.HitField(_board, inputPoint);

            // When
            var result = IsFoundPoint(_board.TemporaryBoard, pointBoard);

            // Then
            result.Should().BeTrue();
        }

        /// <summary>
        /// Check is hit in part of ship and returns true if hit missed in ship.
        /// </summary>
        [Fact]
        public void HitField_UpdateTemporaryBoard_ReturnsIsHitMissed()
        {
            // Given
            var pointBoard = FindEmptyPoint(_board.FinalBoard);
            var inputPoint = ConvertPointToString(pointBoard);
            _game.HitField(_board, inputPoint);

            // When
            var result = IsFoundPoint(_board.TemporaryBoard, pointBoard);

            // Then
            result.Should().BeTrue();
        }

        /// <summary>
        /// Check is passed correct input point and throws exception if incorrect input.
        /// </summary>
        /// <param name="inputPoint">Input point (string) from user.</param>
        [Theory]
        [InlineData("A-1")]
        [InlineData("A11")]
        [InlineData("Z1")]
        [InlineData("xxx2341")]
        [InlineData("")]
        [InlineData(null)]
        public void HitField_UpdateTemporaryBoard_ThrowsExceptionIncorrectInput(string inputPoint)
        {
            // When
            var action = () => _game.HitField(_board, inputPoint);

            // Then
            action.Should().Throw<Exception>("Incorrect input point.");
        }

        /// <summary>
        /// Check is hit point has been already hit and returns exception.
        /// </summary>
        [Fact]
        public void HitField_UpdateTemporaryBoard_ThrowsExceptionPointHasBeenAlreadyHit()
        {
            // Given
            var pointBoard = FindShipPoint(_board.FinalBoard);
            var inputPoint = ConvertPointToString(pointBoard);
            _game.HitField(_board, inputPoint);

            // When
            var action = () => _game.HitField(_board, inputPoint);

            // Then
            action.Should().Throw<Exception>("Point has been already hit.");
        }

        /// <summary>
        /// Check updated correct hits counter and returns correct result.
        /// </summary>
        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(7)]
        [InlineData(10)]
        [InlineData(13)]
        public void HitField_UpdateCorrectHitsCounter_ReturnsCorrectResult(int amountOfCorrectHits)
        {
            // When
            CorrectHitPoints(amountOfCorrectHits);

            // Then
            _game.CorrectHits.Should().Be(amountOfCorrectHits);
        }

        /// <summary>
        /// Check updated correct hits counter and returns correct result.
        /// </summary>
        [Theory]
        [InlineData(1)]
        [InlineData(7)]
        [InlineData(13)]
        [InlineData(20)]
        public void HitField_UpdateMissesHitsCounter_ReturnsCorrectResult(int amountOfMissHits)
        {
            // When
            MissHitPoints(amountOfMissHits);

            // Then
            _game.MissesHits.Should().Be(amountOfMissHits);
        }

        /// <summary>
        /// Check is sunk every ship and returns true if sunk every ship.
        /// </summary>
        [Fact]
        public void IsSunkEveryShip_CheckIsFoundEveryShipPoint_ReturnsTrue()
        {
            // Given
            MarkAsFoundEveryPointShips(_board);

            // When
            var result = _game.IsFoundEveryShipPoint(_board);

            // Then
            result.Should().BeTrue();
        }

        /// <summary>
        /// Check is sunk every ship and returns false if haven't sunk every ship.
        /// </summary>
        [Fact]
        public void IsSunkEveryShip_CheckIsFoundEveryPointShips_ReturnsFalse()
        {
            // When
            var result = _game.IsFoundEveryShipPoint(_board);

            // Then
            result.Should().BeFalse();
        }

        /// <summary>
        /// Show final stats and check is method was corrected executed.
        /// </summary>
        [Fact]
        public void ShowFinalStats_ShowsStats_ShouldSucceed()
        {
            // Given
            var gameMock = new Mock<IGame>();

            // When
            gameMock.Object.ShowFinalStats();

            // Then
            gameMock.Verify(x => x.ShowFinalStats(), Times.Once);
        }

        /// <summary>
        /// Find ship point on board.
        /// </summary>
        /// <param name="boardPoints">Board of points.</param>
        /// <returns>Returns board point which is part of any ship.</returns>
        private BoardPoint FindShipPoint(BoardPoint[][] boardPoints)
        {
            foreach (var rowBoardPoint in boardPoints)
            {
                foreach (var boardPoint in rowBoardPoint)
                {
                    if (boardPoint.Value == 0 || _board.TemporaryBoard[boardPoint.Y][boardPoint.X].IsFound) continue;

                    return boardPoint;
                }
            }

            return new BoardPoint();
        }

        /// <summary>
        /// Find empty point on board.
        /// </summary>
        /// <param name="boardPoints">Board of points.</param>
        /// <returns>Returns board point which is empty.</returns>
        private BoardPoint FindEmptyPoint(BoardPoint[][] boardPoints)
        {
            foreach (var rowBoardPoint in boardPoints)
            {
                foreach (var boardPoint in rowBoardPoint)
                {
                    if (boardPoint.Value != 0 || _board.TemporaryBoard[boardPoint.Y][boardPoint.X].IsFound) continue;

                    return boardPoint;
                }
            }

            return new BoardPoint();
        }

        /// <summary>
        /// Check is point found.
        /// </summary>
        /// <param name="boardPoints">Board of points.</param>
        /// <param name="point">Point to check</param>
        /// <returns>Returns true if point is found otherwise false.</returns>
        private static bool IsFoundPoint(BoardPoint[][] boardPoints, BoardPoint point)
        {
            return boardPoints[point.Y][point.X].IsFound;
        }

        /// <summary>
        /// Convert point to string.
        /// </summary>
        /// <param name="point">Board point</param>
        /// <returns>Returns converted point to string (like input of user).</returns>
        private static string ConvertPointToString(BoardPoint point)
        {
            var letter = GetLetter(point.X);
            var number = GetNumber(point.Y);

            return letter + number;
        }

        /// <summary>
        /// Get letter.
        /// </summary>
        /// <param name="xIndex">Index of Letter from axis board letters.</param>
        /// <returns>String of Letter.</returns>
        private static string GetLetter(int xIndex)
        {
            var letter = (Letter)xIndex;
            return letter.ToString();
        }

        /// <summary>
        /// Get letter.
        /// </summary>
        /// <param name="yIndex">Index of number from axis board numbers.</param>
        /// <returns>String of Letter.</returns>
        private static string GetNumber(int yIndex)
        {
            var number = yIndex + 1;
            return number.ToString();
        }

        /// <summary>
        /// Mark as found every point of ships on board.
        /// </summary>
        /// <param name="board">Board</param>
        private static void MarkAsFoundEveryPointShips(Board board)
        {
            for (var y = 0; y < board.SquareSize; y++)
            {
                for (var x = 0; x < board.SquareSize; x++)
                {
                    if (board.FinalBoard[y][x].Value == 0) continue;

                    board.TemporaryBoard[y][x].Value = board.FinalBoard[y][x].Value;
                    board.TemporaryBoard[y][x].IsFound = true;
                }
            }
        }

        /// <summary>
        /// Correct hit points.
        /// </summary>
        /// <param name="amountOfPoints">Amount of correct points to hit.</param>
        private void CorrectHitPoints(int amountOfPoints)
        {
            for (var i = 0; i < amountOfPoints; i++)
            {
                var pointBoard = FindShipPoint(_board.FinalBoard);
                var inputPoint = ConvertPointToString(pointBoard);
                _game.HitField(_board, inputPoint);
            }
        }

        /// <summary>
        /// Miss hit points.
        /// </summary>
        /// <param name="amountOfPoints"></param>
        private void MissHitPoints(int amountOfPoints)
        {
            for (var i = 0; i < amountOfPoints; i++)
            {
                var pointBoard = FindEmptyPoint(_board.FinalBoard);
                var inputPoint = ConvertPointToString(pointBoard);
                _game.HitField(_board, inputPoint);
            }
        }
    }
}