namespace Battleships
{
    /// <summary>
    /// Interface of Board.
    /// </summary>
    public interface IBoard
    {
        public BoardPoint[][] TemporaryBoard { get; set; }
        public BoardPoint[][] FinalBoard { get; set; }

        public void ShowTemporaryBoard();
    }
}