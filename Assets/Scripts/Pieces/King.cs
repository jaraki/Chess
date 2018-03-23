using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece {
    public bool inCheck;
    public override List<Board.Move> GetAvailableMoves() {
        List<Board.Move> moves = new List<Board.Move>();
        moves.AddRange(GetMovesInDirection(Board.Direction.East, square, false));
        moves.AddRange(GetMovesInDirection(Board.Direction.West, square, false));
        moves.AddRange(GetMovesInDirection(Board.Direction.South, square, false));
        moves.AddRange(GetMovesInDirection(Board.Direction.North, square, false));
        moves.AddRange(GetMovesInDirection(Board.Direction.Northeast, square, false));
        moves.AddRange(GetMovesInDirection(Board.Direction.Southwest, square, false));
        if (CanKingsideCastle()) {
            Debug.Log("Castle");
            moves.Add(color == Color.white ? new Board.Move(0, 6, this) : new Board.Move(7, 6, this));
        }
        return moves;
    }

    private bool CanKingsideCastle() {
        return moveHistory.Count == 0 && (color == Color.white ? Board.instance.GetSquare(0, 5).piece == null : Board.instance.GetSquare(7, 5).piece == null)
            && (color == Color.white ? Board.instance.GetSquare(0, 6).piece == null : Board.instance.GetSquare(7, 6).piece == null)
            && (color == Color.white ? Board.instance.GetSquare(0, 7).piece != null && Board.instance.GetSquare(0, 7).piece.moveHistory.Count == 0 :
            Board.instance.GetSquare(7, 7).piece != null && Board.instance.GetSquare(7, 7).piece.moveHistory.Count == 0);
    }
}
