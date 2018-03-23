using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour {
    public GameObject lighting;
    private GameObject selected;
    private Piece selectedPiece;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 finalPosition;
    private Quaternion finalRotation;
    private Color turnColor;
    // Use this for initialization
    void Start () {
        initialPosition = Camera.main.transform.position;
        initialRotation = Camera.main.transform.rotation;
        finalPosition = lighting.transform.position;
        finalRotation = lighting.transform.rotation;
        turnColor = Color.white;
	}

    private void SwitchTurns() {
        turnColor = turnColor == Color.white ? Color.black : Color.white;
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
                    if(selectedPiece.color != turnColor) {
                        return;
                    }
                    Debug.Log(Board.GetNotation(selectedPiece.square.file, selectedPiece.square.rank));
                    Board.instance.ClearMoves();
                    foreach (Board.Move m in selectedPiece.GetAvailableMoves()) {
                        Debug.Log(Board.GetNotation(m.file, m.rank));
                        Board.instance.CreateMove(m.file, m.rank);
                    }
                    hitInfo.transform.GetComponent<Light>().enabled = true;
                } else if (hitInfo.transform.gameObject.tag == "Move"){
                    Board.Square square = Board.instance.GetSquareFromString(hitInfo.transform.parent.name);
                    Board.instance.MakeMove(new Board.Move(square.file, square.rank, selectedPiece), selected);
                    SwitchTurns();
                    StartCoroutine(RotateCamera(selectedPiece.color == Color.white));
                }
            }
        }
    }
    IEnumerator RotateCamera(bool white) {
        float x = Camera.main.transform.position.x;
        float travelTime = 3f;
        for(float i = 0; i < travelTime; i += Time.deltaTime) {
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
