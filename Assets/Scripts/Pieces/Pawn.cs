using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece {
    public override List<Board.Move> GetAvailableMoves(Board.Square[,] board) {
        List<Board.Move> moves = new List<Board.Move>();
        if(color == Color.white) {
            moves.AddRange(GetMovesInDirection(Board.Direction.North, square, board, false, true));
        } else {
            moves.AddRange(GetMovesInDirection(Board.Direction.South, square, board, false, true));
        }
        if(moveHistory != null && moveHistory.Count == 0 && moves.Count > 0) {
            if (color == Color.white) {
                moves.AddRange(GetMovesInDirection(Board.Direction.North, Board.instance.GetSquareInDirection(square, Board.Direction.North, board), board, false, true));
            } else {
                moves.AddRange(GetMovesInDirection(Board.Direction.South, Board.instance.GetSquareInDirection(square, Board.Direction.South, board), board, false, true));
            }
        }

        List<Board.Move> captures = new List<Board.Move>();
        if (color == Color.white) {
            captures.AddRange(GetMovesInDirection(Board.Direction.Northeast, square, board, false));
            captures.AddRange(GetMovesInDirection(Board.Direction.Northwest, square, board, false));
        } else {
            captures.AddRange(GetMovesInDirection(Board.Direction.Southeast, square, board, false));
            captures.AddRange(GetMovesInDirection(Board.Direction.Southwest, square, board, false));
        }
        foreach (Board.Move m in captures) {
            if (Board.instance.GetPiece(m.file, m.rank) != null && Board.instance.GetPiece(m.file, m.rank).color != color) {
                moves.Add(m);
            }
        }
        return moves;
    }
}
