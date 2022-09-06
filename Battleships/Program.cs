// BATTLESHIPS GAME
using Battleships;

var board = new Board();
var ships = ShipService.GetShips();
board.GenerateBoardWithShips(ships);
var game = new Game(ships);
board.ShowTemporaryBoard();

do
{
    var input = Console.ReadLine();
    Console.Clear();
    try
    {
        game.HitField(board, input);
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
    board.ShowTemporaryBoard();
} while (!game.IsFoundEveryShipPoint(board));

game.ShowFinalStats();