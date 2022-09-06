namespace Battleships
{
    public class Battleship : Ship
    {
        /// <summary>
        /// Constructor of Battleship
        /// </summary>
        /// <param name="id">ID</param>
        public Battleship(int id)
        {
            Id = id;
            Size = 5;
            Name = "Battleship";
        }
    }
}