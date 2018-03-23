using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour {
    public GameObject lighting;
    public Text checkText;
    private GameObject selected;
    private Piece selectedPiece;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 finalPosition;
    private Quaternion finalRotation;
    private Color turnColor;
    private bool gameEnded;
    // Use this for initialization
    void Start() {
        initialPosition = Camera.main.transform.position;
        initialRotation = Camera.main.transform.rotation;
        finalPosition = lighting.transform.position;
        finalRotation = lighting.transform.rotation;
        turnColor = Color.white;
        gameEnded = false;
    }
    public List<Board.Move> GetBlocks() {
        List<Board.Move> blocks = new List<Board.Move>();
        foreach (Piece p in Board.instance.GetOpposingPieces(Board.GetOppositeColor(turnColor))) {
            foreach (Board.Move m in p.GetAvailableMoves(Board.instance.GetBoard())) {
                Board.Square[,] copy = Board.instance.CopyBoard();
                Debug.Log(Board.BoardToString(copy));
                Debug.Log(Board.instance.GetKing(turnColor).CheckForCheck(copy));
                if (Board.instance.SimulateMove(m, copy)) {
                    blocks.Add(m);
                }
            }
        }
        foreach(Board.Move block in blocks) {
            Debug.Log(Board.GetNotation(block.file, block.rank));
        }
        return blocks;
    }
    private void SwitchTurns() {
        turnColor = turnColor == Color.white ? Color.black : Color.white;
        if (Board.instance.GetKing(turnColor).CheckForCheck(Board.instance.GetBoard())) {
            List<Board.Move> blocks = GetBlocks();
            if (Board.instance.GetKing(turnColor).GetAvailableMoves(Board.instance.GetBoard()).Count == 0 && blocks.Count == 0 && !gameEnded) {
                checkText.text = "Checkmate!";
                gameEnded = true;
            } else {
                checkText.text = "Check!";
            }
        }
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene("Menu");
        }
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit) {
                if (hitInfo.transform.gameObject.tag == "Piece") {
                    if (selected) {
                        selected.GetComponent<Light>().enabled = false;
                    }
                    selected = hitInfo.transform.gameObject;
                    selectedPiece = selected.GetComponent<Piece>();
                    if (selectedPiece.color != turnColor) {
                        return;
                    }
                    Board.instance.ClearMoves();
                    King k = Board.instance.GetKing(turnColor);
                    foreach (Board.Move m in selectedPiece.GetAvailableMoves(Board.instance.GetBoard())) {
                        if (k.CheckForCheck(Board.instance.GetBoard())) {
                            if(k.checkCount > 1) {
                                selectedPiece = k;
                                break;
                            }
                            List<Board.Move> blocks = GetBlocks();
                            foreach(Board.Move block in blocks) {
                                if(m.piece == selectedPiece) {
                                    Board.instance.CreateMove(m.file, m.rank);
                                }
                            }
                        } else {
                            checkText.text = "";
                            Board.instance.CreateMove(m.file, m.rank);
                        }
                    }
                    hitInfo.transform.GetComponent<Light>().enabled = true;
                } else if (hitInfo.transform.gameObject.tag == "Move") {
                    Board.Square square = Board.instance.GetSquareFromString(hitInfo.transform.parent.name);
                    Board.instance.MakeMove(new Board.Move(square.file, square.rank, selectedPiece), selected);
                    Board.instance.GetKing(turnColor).checkCount = 0;
                    SwitchTurns();
                    StartCoroutine(RotateCamera(selectedPiece.color == Color.white));
                }
            }
        }
    }
    IEnumerator RotateCamera(bool white) {
        float travelTime = 3f;
        for (float i = 0; i < travelTime; i += Time.deltaTime) {
            if (white) {
                Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, finalPosition, i / travelTime);
                Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, finalRotation, i / travelTime);
                lighting.transform.position = Vector3.Lerp(lighting.transform.position, initialPosition, i / travelTime);
                lighting.transform.rotation = Quaternion.Lerp(lighting.transform.rotation, initialRotation, i / travelTime);
            } else {
                Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, initialPosition, i / travelTime);
                Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, initialRotation, i / travelTime);
                lighting.transform.position = Vector3.Lerp(lighting.transform.position, finalPosition, i / travelTime);
                lighting.transform.rotation = Quaternion.Lerp(lighting.transform.rotation, finalRotation, i / travelTime);
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
