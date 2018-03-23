using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece {
    public override List<Board.Move> GetAvailableMoves() {
        List<Board.Move> moves = new List<Board.Move>();
        if(color == Color.white) {
            moves.AddRange(GetMovesInDirection(Board.Direction.North, square, false, true));
        } else {
            moves.AddRange(GetMovesInDirection(Board.Direction.South, square, false, true));
        }
        if(moveHistory != null && moveHistory.Count == 0 && moves.Count > 0) {
            if (color == Color.white) {
                moves.AddRange(GetMovesInDirection(Board.Direction.North, Board.instance.GetSquareInDirection(square, Board.Direction.North), false, true));
            } else {
                moves.AddRange(GetMovesInDirection(Board.Direction.South, Board.instance.GetSquareInDirection(square, Board.Direction.South), false, true));
            }
        }

        List<Board.Move> captures = new List<Board.Move>();
        if (color == Color.white) {
            captures.AddRange(GetMovesInDirection(Board.Direction.Northeast, square, false));
            captures.AddRange(GetMovesInDirection(Board.Direction.Northwest, square, false));
        } else {
            captures.AddRange(GetMovesInDirection(Board.Direction.Southeast, square, false));
            captures.AddRange(GetMovesInDirection(Board.Direction.Southwest, square, false));
        }
        foreach (Board.Move m in captures) {
            if (Board.instance.GetPiece(m.file, m.rank) != null && Board.instance.GetPiece(m.file, m.rank).color != color) {
                moves.Add(m);
            }
        }
        return moves;
    }
}
