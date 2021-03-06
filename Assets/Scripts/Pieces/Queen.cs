﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Piece {
    public override List<Board.Move> GetAvailableMoves(Board.Square[,] board) {
        List<Board.Move> moves = new List<Board.Move>();
        moves.AddRange(GetMovesInDirection(Board.Direction.East, square, board));
        moves.AddRange(GetMovesInDirection(Board.Direction.West, square, board));
        moves.AddRange(GetMovesInDirection(Board.Direction.South, square, board));
        moves.AddRange(GetMovesInDirection(Board.Direction.North, square, board));
        moves.AddRange(GetMovesInDirection(Board.Direction.Northeast, square, board));
        moves.AddRange(GetMovesInDirection(Board.Direction.Southwest, square, board));
        moves.AddRange(GetMovesInDirection(Board.Direction.Northwest, square, board));
        moves.AddRange(GetMovesInDirection(Board.Direction.Southeast, square, board));
        return moves;
    }
}
