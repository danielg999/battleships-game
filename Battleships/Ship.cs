namespace Battleships
{
    public abstract class Ship
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// Size
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// Points
        /// </summary>
        public List<BoardPoint> Points { get; set; } = new();
    }
}