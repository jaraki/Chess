using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece {
    public override List<Board.Move> GetAvailableMoves() {
        List<Board.Move> moves = new List<Board.Move>();
        moves.AddRange(GetMovesInDirection(Board.Direction.Northeast, square));
        moves.AddRange(GetMovesInDirection(Board.Direction.Southwest, square));
        moves.AddRange(GetMovesInDirection(Board.Direction.Northwest, square));
        moves.AddRange(GetMovesInDirection(Board.Direction.Southeast, square));
        return moves;
    }
}
