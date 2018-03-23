using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {
    public enum Direction {
        East,
        West,
        South,
        North,
        Northeast,
        Southwest,
        Northwest,
        Southeast
    }

    public class Square {
        public Piece piece;
        public int file;
        public int rank;
        public Color color;

        public Square(int file, int rank, Color color, Piece piece = null) {
            this.file = file;
            this.rank = rank;
            this.color = color;
            this.piece = piece;
        }
    }

    public class Move {
        public int file;
        public int rank;
        public Piece piece;

        public Move(int file, int rank, Piece piece) {
            this.file = file;
            this.rank = rank;
            this.piece = piece;
        }
    }
    public float boardScale;
    public float pieceScale;
    public GameObject pawnPrefab;
    public GameObject queenPrefab;
    public GameObject rookPrefab;
    public GameObject bishopPrefab;
    public GameObject kingPrefab;
    public GameObject knightPrefab;
    public const int FileCount = 8;
    public const int RankCount = 8;
    public static string[] Files = { "A", "B", "C", "D", "E", "F", "G", "H" };
    private Square[,] board;
    private GameObject[,] physicalBoard;
    private List<GameObject> possibleMoves;
    private List<Piece> whitePieces;
    private List<Piece> blackPieces;
    public static Board instance;
    // Use this for initialization
    void Start() {
        instance = this;
        possibleMoves = new List<GameObject>();
        whitePieces = new List<Piece>();
        blackPieces = new List<Piece>();
        Initialize();

        //Debug.Log(BoardToString(board));
    }

    public Square[,] CopyBoard() {
        return board.Clone() as Square[,];
    }

    public Square[,] GetBoard() {
        return board;
    }
    public static string GetNotation(int file, int rank) {
        return Files[rank] + (file + 1);
    }

    public Piece GetPiece(int file, int rank) {
        return board[file, rank].piece;
    }

    public Square GetSquareFromString(string s) {
        return board[GetIntFromFile(s[0].ToString()), int.Parse(s[1].ToString()) - 1];
    }

    public Square GetSquare(int file, int rank) {
        return board[file, rank];
    }

    public string GetFileFromInt(int file) {
        return Files[file];
    }

    public int GetIntFromFile(string fileName) {
        return Array.FindIndex(Files, element => element == fileName);
    }

    public King GetKing(Color color) {
        foreach(King k in GetComponentsInChildren<King>()) {
            if(k.color == color) {
                return k;
            }
        }
        return null;
    }

    public List<Piece> GetOpposingPieces(Color color) {
        if(color == Color.white) {
            return blackPieces;
        } else {
            return whitePieces;
        }
    }

    public static Color GetOppositeColor(Color color) {
        if(color == Color.white) {
            return Color.black;
        } else {
            return Color.white;
        }
    }

    public bool SimulateMove(Move move, Square[,] copy) {
        int tempFile = move.piece.square.file;
        int tempRank = move.piece.square.rank;
        Piece p = null;
        if (GetPiece(move.rank, move.file) != null) {
            p = copy[move.rank, move.file].piece;
        }
        copy[move.piece.square.file, move.piece.square.rank].piece = null;
        copy[move.file, move.rank].piece = move.piece;
        move.piece.square = GetSquare(move.file, move.rank);
        bool returnVal = !GetKing(move.piece.color).CheckForCheck(copy);
        move.piece.square = GetSquare(tempFile, tempRank);
        copy[move.file, move.rank].piece = p;
        copy[move.piece.square.file, move.piece.square.rank].piece = move.piece;
        return returnVal;
    }

    public void MakeMove(Move move, GameObject piece) {
        ClearMoves();
        if (GetPiece(move.rank, move.file) != null) {
            Piece p = physicalBoard[move.rank, move.file].GetComponentInChildren<Piece>();
            if (p) {
                Destroy(p.gameObject);
            }
        }
        if (move.piece.GetType() == typeof(King) && move.piece.moveHistory.Count == 0 && (move.piece.color == Color.white ? move.file == 6 && move.rank == 0 : move.file == 6 && move.rank == 7)) {
            Piece rook = board[move.piece.color == Color.white ? 0 : 7, 7].piece;
            board[move.piece.color == Color.white ? 0 : 7, 7].piece = null;
            board[move.piece.color == Color.white ? 0 : 7, 5].piece = rook;
            rook.square = GetSquare(move.piece.color == Color.white ? 0 : 7, 5);
            rook.transform.SetParent(physicalBoard[move.piece.color == Color.white ? 0 : 7, 5].transform, false);
        }
        if (move.piece.GetType() == typeof(King) && move.piece.moveHistory.Count == 0 && (move.piece.color == Color.white ? move.file == 2 && move.rank == 0 : move.file == 2 && move.rank == 7)) {
            Piece rook = board[move.piece.color == Color.white ? 0 : 7, 0].piece;
            board[move.piece.color == Color.white ? 0 : 7, 0].piece = null;
            board[move.piece.color == Color.white ? 0 : 7, 3].piece = rook;
            rook.square = GetSquare(move.piece.color == Color.white ? 0 : 7, 3);
            rook.transform.SetParent(physicalBoard[move.piece.color == Color.white ? 0 : 7, 3].transform, false);
        }
        board[move.piece.square.file, move.piece.square.rank].piece = null;
        board[move.rank, move.file].piece = move.piece;
        move.piece.square = GetSquare(move.rank, move.file);
        move.piece.moveHistory.Add(move);
        piece.transform.SetParent(physicalBoard[move.rank, move.file].transform, false);
    }

    public Square GetSquareInDirection(Square square, Direction direction, Square[,] board) {
        switch (direction) {
            case Direction.East:
                if (IsValidMove(square.file, square.rank + 1)) {
                    return board[square.file, square.rank + 1];
                } else {
                    return square;
                }
            case Direction.West:
                if (IsValidMove(square.file, square.rank - 1)) {
                    return board[square.file, square.rank - 1];
                } else {
                    return square;
                }
            case Direction.South:
                if (IsValidMove(square.file - 1, square.rank)) {
                    return board[square.file - 1, square.rank];
                } else {
                    return square;
                }
            case Direction.North:
                if (IsValidMove(square.file + 1, square.rank)) {
                    return board[square.file + 1, square.rank];
                } else {
                    return square;
                }
            case Direction.Northeast:
                if (IsValidMove(square.file + 1, square.rank + 1)) {
                    return board[square.file + 1, square.rank + 1];
                } else {
                    return square;
                }
            case Direction.Southwest:
                if (IsValidMove(square.file - 1, square.rank - 1)) {
                    return board[square.file - 1, square.rank - 1];
                } else {
                    return square;
                }
            case Direction.Northwest:
                if (IsValidMove(square.file + 1, square.rank - 1)) {
                    return board[square.file + 1, square.rank - 1];
                } else {
                    return square;
                }
            case Direction.Southeast:
                if (IsValidMove(square.file - 1, square.rank + 1)) {
                    return board[square.file - 1, square.rank + 1];
                } else {
                    return square;
                }
            default:
                return square;
        }
    }

    private void Initialize() {
        board = new Square[FileCount, RankCount];
        physicalBoard = new GameObject[FileCount, RankCount];
        for (int i = 0; i < FileCount; ++i) {
            for (int j = 0; j < RankCount; ++j) {
                Color c = (i + i * RankCount + j) % 2 == 0 ? Color.black : Color.white;
                board[i, j] = new Square(i, j, c);
                physicalBoard[i, j] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                physicalBoard[i, j].transform.position = new Vector3(i * boardScale, 0, -j * boardScale);
                physicalBoard[i, j].transform.parent = transform;
                physicalBoard[i, j].transform.localScale = new Vector3(boardScale, 0.25f * boardScale, boardScale);
                physicalBoard[i, j].name = GetNotation(i, j);
                physicalBoard[i, j].GetComponent<Renderer>().material.color = c;
            }
        }
        AddStartingPieces();
    }
    private Component CreatePiece(int file, int rank, Color color, string name, Type type) {
        GameObject piece = Instantiate(GetPrefabFromType(type));
        piece.AddComponent<MeshCollider>();
        piece.tag = "Piece";
        piece.name = name + (rank + 1);
        piece.transform.SetParent(physicalBoard[file, rank].transform, false);
        piece.transform.position += new Vector3(0, pieceScale * 0.6f, 0);
        piece.transform.localScale = new Vector3(pieceScale * 0.25f, pieceScale, pieceScale * 0.25f * (color == Color.white ? 1 : -1));
        piece.GetComponent<Renderer>().material.color = color;

        Light light = piece.AddComponent<Light>();
        light.color = Color.green;
        light.intensity = 5;
        light.range = 2;
        light.enabled = false;
        return piece.AddComponent(type);
    }

    private GameObject GetPrefabFromType(Type type) {
        if (type == typeof(Pawn)) {
            return pawnPrefab;
        } else if (type == typeof(Queen)) {
            return queenPrefab;
        } else if (type == typeof(Rook)) {
            return rookPrefab;
        } else if (type == typeof(Bishop)) {
            return bishopPrefab;
        } else if (type == typeof(King)) {
            return kingPrefab;
        } else if (type == typeof(Knight)) {
            return knightPrefab;
        }
        return pawnPrefab;
    }
    public void ClearMoves() {
        foreach (GameObject go in possibleMoves) {
            Destroy(go);
        }
    }

    public void CreateMove(int file, int rank) {
        GameObject move = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        move.tag = "Move";
        move.name = "Move";
        move.transform.SetParent(physicalBoard[file, rank].transform, false);
        move.transform.position += new Vector3(0, pieceScale * 0.5f + boardScale * 0.125f, 0);
        move.transform.localScale = new Vector3(pieceScale * 0.25f, pieceScale, pieceScale * 0.25f);
        if (GetPiece(file, rank) != null) {
            move.transform.localScale *= 2;
        }
        move.GetComponent<Renderer>().material.color = Color.green;
        possibleMoves.Add(move);
    }
    private void AddStartingPieces() {
        // add pawns
        for (int i = 0; i < RankCount; ++i) {
            Pawn p = CreatePiece(1, i, Color.white, "White Pawn ", typeof(Pawn)) as Pawn;
            p.color = Color.white;
            p.square = board[1, i];
            board[1, i].piece = p;
            whitePieces.Add(p);
            p = CreatePiece(6, i, Color.black, "Black Pawn ", typeof(Pawn)) as Pawn;
            p.color = Color.black;
            p.square = board[6, i];
            board[6, i].piece = p;
            blackPieces.Add(p);
        }
        // add rooks
        Rook r = CreatePiece(0, 0, Color.white, "White Rook ", typeof(Rook)) as Rook;
        r.color = Color.white;
        r.square = board[0, 0];
        board[0, 0].piece = r;
        whitePieces.Add(r);
        r = CreatePiece(0, 7, Color.white, "White Rook ", typeof(Rook)) as Rook;
        r.color = Color.white;
        r.square = board[0, 7];
        board[0, 7].piece = r;
        whitePieces.Add(r);
        r = CreatePiece(7, 0, Color.black, "Black Rook ", typeof(Rook)) as Rook;
        r.color = Color.black;
        r.square = board[7, 0];
        board[7, 0].piece = r;
        blackPieces.Add(r);
        r = CreatePiece(7, 7, Color.black, "Black Rook ", typeof(Rook)) as Rook;
        r.color = Color.black;
        r.square = board[7, 7];
        board[7, 7].piece = r;
        blackPieces.Add(r);
        // add knights
        Knight n = CreatePiece(0, 1, Color.white, "White Knight ", typeof(Knight)) as Knight;
        n.color = Color.white;
        n.square = board[0, 1];
        board[0, 1].piece = n;
        whitePieces.Add(n);
        n = CreatePiece(0, 6, Color.white, "White Knight ", typeof(Knight)) as Knight;
        n.color = Color.white;
        n.square = board[0, 6];
        board[0, 6].piece = n;
        whitePieces.Add(n);
        n = CreatePiece(7, 1, Color.black, "Black Knight ", typeof(Knight)) as Knight;
        n.color = Color.black;
        n.square = board[7, 1];
        board[7, 1].piece = n;
        blackPieces.Add(n);
        n = CreatePiece(7, 6, Color.black, "Black Knight ", typeof(Knight)) as Knight;
        n.color = Color.black;
        n.square = board[7, 6];
        board[7, 6].piece = n;
        blackPieces.Add(n);
        // add bishops
        Bishop b = CreatePiece(0, 2, Color.white, "White Bishop ", typeof(Bishop)) as Bishop;
        b.color = Color.white;
        b.square = board[0, 2];
        board[0, 2].piece = b;
        whitePieces.Add(b);
        b = CreatePiece(0, 5, Color.white, "White Bishop ", typeof(Bishop)) as Bishop;
        b.color = Color.white;
        b.square = board[0, 5];
        board[0, 5].piece = b;
        whitePieces.Add(b);
        b = CreatePiece(7, 2, Color.black, "Black Bishop ", typeof(Bishop)) as Bishop;
        b.color = Color.black;
        b.square = board[7, 2];
        board[7, 2].piece = b;
        blackPieces.Add(b);
        b = CreatePiece(7, 5, Color.black, "Black Bishop ", typeof(Bishop)) as Bishop;
        b.color = Color.black;
        b.square = board[7, 5];
        board[7, 5].piece = b;
        blackPieces.Add(b);
        // add kings
        King k = CreatePiece(0, 4, Color.white, "White King ", typeof(King)) as King;
        k.color = Color.white;
        k.square = board[0, 4];
        board[0, 4].piece = k;
        k = CreatePiece(7, 4, Color.black, "Black King ", typeof(King)) as King;
        k.color = Color.black;
        k.square = board[7, 4];
        board[7, 4].piece = k;
        // add queens
        Queen q = CreatePiece(0, 3, Color.white, "White Queen ", typeof(Queen)) as Queen;
        q.color = Color.white;
        q.square = board[0, 3];
        board[0, 3].piece = q;
        whitePieces.Add(q);
        q = CreatePiece(7, 3, Color.black, "Black Queen ", typeof(Queen)) as Queen;
        q.color = Color.black;
        q.square = board[7, 3];
        board[7, 3].piece = q;
        blackPieces.Add(q);
    }

    public static string BoardToString(Square[,] board) {
        string s = "";
        for (int i = 0; i < FileCount; ++i) {
            for (int j = 0; j < RankCount; ++j) {
                if (board[i, j].piece == null) {
                    s += "*";
                } else {
                    s += board[i, j].piece.notation;
                }
            }
            s += "\n";
        }
        return s;
    }

    public bool IsValidMove(Move move) {
        return move.file >= 0 && move.file < FileCount && move.rank >= 0 && move.rank < RankCount;
    }

    public bool IsValidMove(int file, int rank) {
        return file >= 0 && file < FileCount && rank >= 0 && rank < RankCount;
    }

    public bool IsLegalMove(Move move) {
        return IsValidMove(move) && (GetPiece(move.file, move.rank) == null || GetPiece(move.file, move.rank).color != move.piece.color);
    }

    // Update is called once per frame
    void Update() {

    }
}
