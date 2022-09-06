using FluentAssertions;

namespace Battleships.Tests
{
    public class PointTests
    {
        /// <summary>
        /// Board point
        /// </summary>
        private readonly BoardPoint _boardPoint;

        /// <summary>
        /// Constructor of PointTests
        /// </summary>
        public PointTests()
        {
            _boardPoint = new BoardPoint();
        }

        /// <summary>
        /// Check is random point is correct.
        /// </summary>
        [Fact]
        public void SetRandomPoint_ForTwoDimensions_ReturnsCorrectPoint()
        {
            // Given
            var board = new Board();
            var maxValue = board.SquareSize;

            _boardPoint.SetRandomPoint(maxValue);

            // Then
            _boardPoint.X.Should().BeInRange(0, maxValue);
            _boardPoint.Y.Should().BeInRange(0, maxValue);
        }
    }
}