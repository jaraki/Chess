using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece {
    public override List<Board.Move> GetAvailableMoves() {
        List<Board.Move> moves = new List<Board.Move>();
        if(Board.instance.IsLegalMove(new Board.Move(square.file + 2, square.rank + 1, this))) {
            moves.Add(new Board.Move(square.file + 2, square.rank + 1, this));
        }
        if (Board.instance.IsLegalMove(new Board.Move(square.file + 2, square.rank - 1, this))) {
            moves.Add(new Board.Move(square.file + 2, square.rank - 1, this));
        }
        if (Board.instance.IsLegalMove(new Board.Move(square.file - 2, square.rank + 1, this))) {
            moves.Add(new Board.Move(square.file - 2, square.rank + 1, this));
        }
        if (Board.instance.IsLegalMove(new Board.Move(square.file - 2, square.rank - 1, this))) {
            moves.Add(new Board.Move(square.file - 2, square.rank - 1, this));
        }
        if (Board.instance.IsLegalMove(new Board.Move(square.file + 1, square.rank + 2, this))) {
            moves.Add(new Board.Move(square.file + 1, square.rank + 2, this));
        }
        if (Board.instance.IsLegalMove(new Board.Move(square.file + 1, square.rank - 2, this))) {
            moves.Add(new Board.Move(square.file + 1, square.rank - 2, this));
        }
        if (Board.instance.IsLegalMove(new Board.Move(square.file - 1, square.rank + 2, this))) {
            moves.Add(new Board.Move(square.file - 1, square.rank + 2, this));
        }
        if (Board.instance.IsLegalMove(new Board.Move(square.file - 1, square.rank - 2, this))) {
            moves.Add(new Board.Move(square.file - 1, square.rank - 2, this));
        }
        return moves;
    }
}
