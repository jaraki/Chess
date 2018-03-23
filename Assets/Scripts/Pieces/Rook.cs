using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Piece {
    public override List<Board.Move> GetAvailableMoves() {
        List<Board.Move> moves = new List<Board.Move>();
        moves.AddRange(GetMovesInDirection(Board.Direction.East, square));
        moves.AddRange(GetMovesInDirection(Board.Direction.West, square));
        moves.AddRange(GetMovesInDirection(Board.Direction.South, square));
        moves.AddRange(GetMovesInDirection(Board.Direction.North, square));
        return moves;
    }
}
