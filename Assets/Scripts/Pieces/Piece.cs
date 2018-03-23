using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour {
    public Color color;
    public Board.Square square;
    public string notation;
    public List<Board.Move> moveHistory;

    void Start() {
        moveHistory = new List<Board.Move>();
        notation = gameObject.name;
    }

    public virtual List<Board.Move> GetAvailableMoves(Board.Square[,] board) {
        List<Board.Move> moves = new List<Board.Move>();
        return moves;
    }

    protected List<Board.Move> GetMovesInDirection(Board.Direction direction, Board.Square square, Board.Square[,] board, bool multipleSquares = true, bool isPawn = false) {
        List<Board.Move> moves = new List<Board.Move>();
        Board.Square s = Board.instance.GetSquareInDirection(square, direction, board);
        Board.Square prevS = square;
        while (s != prevS){
            if (s.piece == null) {
                moves.Add(new Board.Move(s.file, s.rank, this));
            } else if (s.piece.color != color) {
                if (!isPawn) {
                    moves.Add(new Board.Move(s.file, s.rank, this));
                }
                break;
            } else {
                break;
            }
            if (!multipleSquares) {
                break;
            }
            
            prevS = s;
            s = Board.instance.GetSquareInDirection(s, direction, board);
        }
        return moves;
    }
}
