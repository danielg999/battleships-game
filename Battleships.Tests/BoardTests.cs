using FluentAssertions;
using Moq;

namespace Battleships.Tests
{
    public class BoardTests
    {
        /// <summary>
        /// Board.
        /// </summary>
        private readonly Board _board;

        /// <summary>
        /// Constructor of BoardTests.
        /// </summary>
        public BoardTests()
        {
            _board = new Board();
        }

        /// <summary>
        /// Check is empty array is correct and returns true if board is clear.
        /// </summary>
        [Fact]
        public void GetClearBoard_CheckIsClearBoard_ReturnsClearBoard()
        {
            // Given
            var emptyBoard = _board.GetClearBoard();

            // When
            var result = IsEveryPointClear(emptyBoard);

            // Then
            result.Should().BeTrue();
        }

        /// <summary>
        /// Check is every ship placed on board and returns true if every ship is on board.
        /// </summary>
        [Fact]
        public void GenerateBoard_IsEveryShipOnBoard_ReturnTrueIfEveryShipOnBoard()
        {
            // Given
            var ships = ShipService.GetShips();
            _board.GenerateBoardWithShips(ships);

            // When
            var result = IsEveryShipOnBoard(ships);

            // Then
            result.Should().BeTrue();
        }

        /// <summary>
        /// Check is show temporary board is succeed.
        /// </summary>
        [Fact]
        public void ShowTemporaryBoard_ShowBoard_ShouldSucceed()
        {
            // Given
            var mockBoard = new Mock<IBoard>();

            // When
            mockBoard.Object.ShowTemporaryBoard();

            // Then
            mockBoard.Verify(x => x.ShowTemporaryBoard(), Times.Once);
        }

        /// <summary>
        /// Check is every ship on board.
        /// </summary>
        /// <param name="ships">Ships</param>
        /// <returns>Returns true if every ship is put on board otherwise returns false.</returns>
        private bool IsEveryShipOnBoard(IEnumerable<Ship> ships)
        {
            return !(from ship in ships from point in ship.Points where _board.FinalBoard[point.Y][point.X].Value != ship.Id select ship).Any();
        }

        /// <summary>
        /// Check is every point is clear on board.
        /// </summary>
        /// <param name="boardPoints">Points of board.</param>
        /// <returns>Returns true if every point is clear otherwise returns false.</returns>
        private static bool IsEveryPointClear(IEnumerable<BoardPoint[]> boardPoints)
        {
            return boardPoints.SelectMany(row => row).All(point => point.Value == 0);
        }
    }
}