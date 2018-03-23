using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece {
    public int checkCount = 0;
    public Piece checker;
    public override List<Board.Move> GetAvailableMoves() {
        List<Board.Move> moves = new List<Board.Move>();
        moves.AddRange(GetMovesInDirection(Board.Direction.East, square, false));
        moves.AddRange(GetMovesInDirection(Board.Direction.West, square, false));
        moves.AddRange(GetMovesInDirection(Board.Direction.South, square, false));
        moves.AddRange(GetMovesInDirection(Board.Direction.North, square, false));
        moves.AddRange(GetMovesInDirection(Board.Direction.Northeast, square, false));
        moves.AddRange(GetMovesInDirection(Board.Direction.Southwest, square, false));
        if (CanKingsideCastle()) {
            moves.Add(color == Color.white ? new Board.Move(0, 6, this) : new Board.Move(7, 6, this));
        }
        if (CanQueensideCastle()){
            moves.Add(color == Color.white ? new Board.Move(0, 2, this) : new Board.Move(7, 2, this));
        }
        List<Board.Move> itemsToRemove = new List<Board.Move>();
        foreach(Board.Move m in moves) {
            foreach(Piece p in Board.instance.GetOpposingPieces(color)) {
                foreach(Board.Move move in p.GetAvailableMoves()) {
                    if(m.file == move.file && m.rank == move.rank) {
                        itemsToRemove.Add(m);
                    }
                }
            }
        }
        foreach(Board.Move m in itemsToRemove) {
            moves.Remove(m);
        }
        return moves;
    }

    public bool CheckForCheck() {
        checkCount = 0;
        foreach (Piece p in Board.instance.GetOpposingPieces(color)) {
            foreach (Board.Move move in p.GetAvailableMoves()) {
                if (move.file == square.file && move.rank == square.rank) {
                    checkCount++;
                    checker = move.piece;
                }
            }
        }
        return checkCount > 0;
    }

    private bool CanKingsideCastle() {
        bool overAndIntoCheck = false;
        foreach (Piece p in Board.instance.GetOpposingPieces(color)) {
            foreach (Board.Move move in p.GetAvailableMoves()) {
                Board.Square s = (color == Color.white ? Board.instance.GetSquare(0, 5) : Board.instance.GetSquare(7, 5));
                Board.Square s2 = (color == Color.white ? Board.instance.GetSquare(0, 6) : Board.instance.GetSquare(7, 6));
                if ((move.file == s.file && move.rank == s.rank) || (move.file == s2.file && move.rank == s2.rank)) {
                    overAndIntoCheck = true;
                }
            }
        }
        return overAndIntoCheck && moveHistory.Count == 0 && checkCount == 0 && (color == Color.white ? Board.instance.GetSquare(0, 5).piece == null : Board.instance.GetSquare(7, 5).piece == null)
            && (color == Color.white ? Board.instance.GetSquare(0, 6).piece == null : Board.instance.GetSquare(7, 6).piece == null)
            && (color == Color.white ? Board.instance.GetSquare(0, 7).piece != null && Board.instance.GetSquare(0, 7).piece.moveHistory.Count == 0 :
            Board.instance.GetSquare(7, 7).piece != null && Board.instance.GetSquare(7, 7).piece.moveHistory.Count == 0);
    }

    private bool CanQueensideCastle() {
        bool overAndIntoCheck = false;
        foreach (Piece p in Board.instance.GetOpposingPieces(color)) {
            foreach (Board.Move move in p.GetAvailableMoves()) {
                Board.Square s = (color == Color.white ? Board.instance.GetSquare(0, 1) : Board.instance.GetSquare(7, 1));
                Board.Square s2 = (color == Color.white ? Board.instance.GetSquare(0, 2) : Board.instance.GetSquare(7, 2));
                Board.Square s3 = (color == Color.white ? Board.instance.GetSquare(0, 3) : Board.instance.GetSquare(7, 3));
                if ((move.file == s.file && move.rank == s.rank) || (move.file == s2.file && move.rank == s2.rank) || (move.file == s3.file && move.rank == s3.rank)) {
                    overAndIntoCheck = true;
                }
            }
        }
        return overAndIntoCheck && moveHistory.Count == 0 && checkCount == 0 && (color == Color.white ? Board.instance.GetSquare(0, 1).piece == null : Board.instance.GetSquare(7, 1).piece == null)
            && (color == Color.white ? Board.instance.GetSquare(0, 2).piece == null : Board.instance.GetSquare(7, 2).piece == null)
            && (color == Color.white ? Board.instance.GetSquare(0, 3).piece == null : Board.instance.GetSquare(7, 3).piece == null)
            && (color == Color.white ? Board.instance.GetSquare(0, 0).piece != null && Board.instance.GetSquare(0, 0).piece.moveHistory.Count == 0 :
            Board.instance.GetSquare(7, 0).piece != null && Board.instance.GetSquare(7, 0).piece.moveHistory.Count == 0);
    }
}
