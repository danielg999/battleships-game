namespace Battleships
{
    public class Destroyer : Ship
    {
        /// <summary>
        /// Constructor of Destroyer.
        /// </summary>
        /// <param name="id">ID</param>
        public Destroyer(int id)
        {
            Id = id;
            Size = 4;
            Name = "Destroyer";
        }
    }
}