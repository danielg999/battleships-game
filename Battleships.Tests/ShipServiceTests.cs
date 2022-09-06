using FluentAssertions;

namespace Battleships.Tests
{
    public class ShipServiceTests
    {
        private const int AmountOfBattleships = 1;
        private const int AmountOfDestroyers = 2;
        private const int SumOfShips = AmountOfBattleships + AmountOfDestroyers;

        /// <summary>
        /// Check is correct amount of different ships and returns true if correct amount of ships.
        /// </summary>
        [Fact]
        public void GetShips_ForAmountOfDifferentShips_ReturnsTrueIfCorrect()
        {
            // Given
            var ships = ShipService.GetShips();

            // When
            var result = CheckAmountOfDifferentShips(ships);

            //Then
            result.Should().BeTrue();
        }

        /// <summary>
        /// Check is every ship has unique ID and returns true if every ship has unique ID.
        /// </summary>
        [Fact]
        public void GetShips_CheckIsEveryShipHasUniqueId_ReturnsTrue()
        {
            // Given
            var ships = ShipService.GetShips();

            // When
            var result = IsEveryShipHasUniqueId(ships);

            // Then
            result.Should().BeTrue();
        }

        /// <summary>
        /// Check is correct amount of different ships.
        /// </summary>
        /// <param name="ships">Ships.</param>
        /// <returns>Returns boolean value. Returns true if amount of different ships is correct, otherwise returns false.</returns>
        private static bool CheckAmountOfDifferentShips(List<Ship> ships)
        {
            var destroyers = ships.Count(x => x.Name == "Destroyer");
            var battleships = ships.Count(x => x.Name == "Battleship");

            return destroyers == AmountOfDestroyers && battleships == AmountOfBattleships && ships.Count == SumOfShips;
        }

        /// <summary>
        /// Check is every ship has unique ID.
        /// </summary>
        /// <param name="ships">Ships.</param>
        /// <returns>Returns boolean value. Returns true if every ship has unique ID, otherwise returns false.</returns>
        private static bool IsEveryShipHasUniqueId(List<Ship> ships)
        {
            var ids = new List<int>();

            foreach (var ship in ships)
            {
                if (ids.Contains(ship.Id))
                {
                    return false;
                }
                ids.Add(ship.Id);
            }

            return true;
        }
    }
}