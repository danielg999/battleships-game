namespace Battleships
{
    public static class ShipService
    {
        /// <summary>
        /// Generate random orientation.
        /// </summary>
        /// <returns>Returns random orientation.</returns>
        public static Orientation GetRandomOrientation()
        {
            var random = new Random();
            var orientationAmount = Enum.GetValues(typeof(Orientation)).Length;

            return (Orientation)random.Next(orientationAmount);
        }

        /// <summary>
        /// Generate ships.
        /// </summary>
        /// <returns>Returns ships.</returns>
        public static List<Ship> GetShips()
        {
            return new List<Ship>()
            {
                new Battleship(1),
                new Destroyer(2),
                new Destroyer(3)
            };
        }
    }
}