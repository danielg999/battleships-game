namespace Battleships
{
    public class BoardPoint
    {
        /// <summary>
        /// Index of point in horizontal position (column).
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Index of point in vertical position (row).
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Is found.
        /// </summary>
        public bool IsFound { get; set; }

        /// <summary>
        /// Value - ID of ship.
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Label.
        /// </summary>
        public string Label
        {
            get
            {
                return IsFound switch
                {
                    true when Value == 0 => "O",
                    true when Value != 0 => "X",
                    _ => " "
                };
            }
        }

        /// <summary>
        /// Constructor of BoardPoint.
        /// </summary>
        public BoardPoint()
        {
            IsFound = false;
            Value = 0;
        }

        /// <summary>
        /// Set random point.
        /// </summary>
        /// <param name="maxSquareSize">Maximum of square size.</param>
        public void SetRandomPoint(int maxSquareSize)
        {
            var random = new Random();

            X = random.Next(maxSquareSize);
            Y = random.Next(maxSquareSize);
        }
    }
}