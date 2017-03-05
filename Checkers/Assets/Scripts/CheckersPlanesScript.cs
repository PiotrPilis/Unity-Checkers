using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;

public class CheckersPlanesScript : MonoBehaviour {

	private const float TILE_SIZE = 1.0f;
	private const float TILE_OFFSET = TILE_SIZE / 2.0f;

	public List<GameObject> pieceModels;
	private GameObject[,] activePiece = new GameObject[8,8];
	private GameObject selectedPlane;

	public GameObject possibleMovePlane;
	private List<GameObject> possibleMovesPlane = new List<GameObject>();
	private bool[] possibleMoves = new bool[11];

	private bool isMoveWhite = true;

	private float timer = 0.0f;
	private bool changeMove = false;

	private int numActivePiecesW = 0;
	private int numActivePiecesB = 0;

	public Canvas gameOverMenu;

	// Use this for initialization
	void Start () {
		selectedPlane = GameObject.FindGameObjectWithTag ("SelectedPlane");
		selectedPlane.active = false;
		RespawnPieces ();
		SetPiecePosition (7, 7, 2, 2);
	}
	
	// Update is called once per frame
	void Update () {
		if (changeMove) {
			timer += Time.deltaTime;
			if (timer >= 2.0f) {
				changeMove = false;
				timer = 0.0f;
				ChangeMove ();
			}
		} else
			UpdateSelection ();
	}

	public void ResetGame()
	{
		RespawnPieces ();
	}

	private bool CanMove()
	{
		for (int x = 0; x < 8; ++x) {
			for (int y = 0; y < 8; ++y) {
				SelectPlane (x, y);
				selectedPlane.active = false;
				for (int i = 0; i < 11; ++i) {
					if (possibleMoves [i] == true) {
						HidePossibleMoves ();
						return true;
					}
				}
			}
		}
		HidePossibleMoves ();
		return false;
	}

	private void SelectPlane(int selectionX, int selectionY)
	{
		Vector3 position = (selectionX * TILE_SIZE + TILE_OFFSET) * Vector3.right + (selectionY * TILE_SIZE + TILE_OFFSET) * Vector3.forward;
		if ((selectedPlane.transform.position.x == position.x && selectedPlane.transform.position.z == position.z && selectedPlane.active == true) || (activePiece [selectionX, selectionY] == null && selectedPlane.active == true)) {
			Move ((int)((selectedPlane.transform.position.x - TILE_OFFSET) / TILE_SIZE), (int)((selectedPlane.transform.position.z - TILE_OFFSET) / TILE_SIZE), selectionX, selectionY);
			HidePossibleMoves ();
			selectedPlane.active = false;
		}
		else  if (activePiece [selectionX, selectionY] != null) {
			if (activePiece [selectionX, selectionY].GetComponent<PieceScript> ().isWhite != isMoveWhite) {
				if (selectedPlane.active == true) {
					Fight ((int)((selectedPlane.transform.position.x - TILE_OFFSET) / TILE_SIZE), (int)((selectedPlane.transform.position.z - TILE_OFFSET) / TILE_SIZE), selectionX, selectionY);
					HidePossibleMoves ();
					selectedPlane.active = false;
				}
			} else {
				HidePossibleMoves ();
				selectedPlane.active = true;
				selectedPlane.transform.position = position;
				ShowPossibleMoves (selectionX, selectionY);
			}
		}
	}

	private void ChangeMove()
	{
		isMoveWhite = !isMoveWhite;

		GameObject cameraController = Camera.main.gameObject;//GameObject.FindWithTag ("Player");
		Vector3 toPosition = Vector3.zero;
		toPosition.z = 8.0f - cameraController.transform.position.z;
		toPosition.x = cameraController.transform.position.x;
		toPosition.y = cameraController.transform.position.y;
		cameraController.GetComponent<CameraControllerScript> ().isMoving = true;
		cameraController.GetComponent<CameraControllerScript> ().toPosition = toPosition;

		if(!CanMove())
			gameOverMenu.GetComponent<KoniecGryMenuScript> ().GameOver ();
	}
		
	private void Fight(int fromX, int fromY, int toX, int toY)
	{
		if (activePiece [fromX, fromY].GetComponent<PieceScript> ().isWhite == true) {
			int x = toX - fromX;
			int y = toY - fromY;
			if((activePiece [fromX, fromY].GetComponent<PieceScript> ().isKing == true ? (y == 1 || y == -1) : (y == 1)) && (x == -1 || x == 1))
			{
				activePiece [fromX, fromY].GetComponent<PieceScript>().moved = true;
				Destroy (activePiece [toX, toY]);
				activePiece [toX, toY] = null;
				SetPiecePosition (fromX, fromY, toX, toY);	

				numActivePiecesB--;
				if (numActivePiecesB == 0)
					gameOverMenu.GetComponent<KoniecGryMenuScript> ().GameOver ();
			}
		} else {
			int x = toX - fromX;
			int y = toY - fromY;
			if((activePiece [fromX, fromY].GetComponent<PieceScript> ().isKing == true ? (y == 1 || y == -1) : (y == -1)) && (x == -1 || x == 1))
			{
				activePiece [fromX, fromY].GetComponent<PieceScript>().moved = true;
				Destroy (activePiece [toX, toY]);
				activePiece [toX, toY] = null;
				SetPiecePosition (fromX, fromY, toX, toY);	

				numActivePiecesW--;
				if (numActivePiecesW == 0)
					gameOverMenu.GetComponent<KoniecGryMenuScript> ().GameOver ();
			}
		}
	}

	private void Move(int fromX, int fromY, int toX, int toY)
	{
		if (activePiece [fromX, fromY].GetComponent<PieceScript> ().isWhite == true) {
			int x = toX - fromX;
			int y = toY - fromY;
			if (y == 1 && x == 0 && possibleMoves [1] == true) {
				activePiece [fromX, fromY].GetComponent<PieceScript>().moved = true;
				SetPiecePosition (fromX, fromY, toX, toY);	
			} else if (y == 2 && x == 0 && possibleMoves [2] == true && (activePiece [fromX, fromY].GetComponent<PieceScript>().moved) == false) {
				activePiece [fromX, fromY].GetComponent<PieceScript>().moved = true;
				SetPiecePosition (fromX, fromY, toX, toY);
			}  else if (y == -1 && x == 0 && possibleMoves [0] == true) {
				SetPiecePosition (fromX, fromY, toX, toY);
			}
		} else {
			int x = toX - fromX;
			int y = toY - fromY;
			if (y == -1 && x == 0 && possibleMoves [1] == true) {
				activePiece [fromX, fromY].GetComponent<PieceScript>().moved = true;
				SetPiecePosition (fromX, fromY, toX, toY);		
			} else if (y == -2 && x == 0 && possibleMoves [2] == true && (activePiece [fromX, fromY].GetComponent<PieceScript>().moved) == false) {
				activePiece [fromX, fromY].GetComponent<PieceScript>().moved = true;
				SetPiecePosition (fromX, fromY, toX, toY);
			} else if (y == 1 && x == 0 && possibleMoves [0] == true) {
				SetPiecePosition (fromX, fromY, toX, toY);
			}
		}
	}
		
	private void HidePossibleMoves()
	{
		ResetPossibleMoves ();
		for (int i = 0; i < possibleMovesPlane.Count; ++i)
			Destroy (possibleMovesPlane[i]);
		possibleMovesPlane.Clear();
	}

	private void ResetPossibleMoves()
	{
		for(int i = 0; i < 3; ++i)
			possibleMoves [i] = false;
	}

	private void ShowPossibleMoves(int x, int y)
	{
		PieceScript piece = activePiece [x, y].GetComponent<PieceScript> ();
		if (piece.isWhite == true) {

			bool canMove = true;
			if (x != 0) {
				if(y != 7)
				if (activePiece [x - 1, y + 1] != null && (activePiece [x - 1, y + 1].GetComponent<PieceScript> ().isWhite) != true) {
					possibleMovesPlane.Add (Instantiate (possibleMovePlane, GetPosition (x - 1, y + 1), Quaternion.identity) as GameObject);
					canMove = false;
					possibleMoves [3] = true;
				}

				if (piece.isKing == true) {
					if(y != 0)
					if (activePiece [x - 1, y - 1] != null && (activePiece [x - 1, y - 1].GetComponent<PieceScript> ().isWhite) != true) {
						possibleMovesPlane.Add (Instantiate (possibleMovePlane, GetPosition (x - 1, y - 1), Quaternion.identity) as GameObject);
						canMove = false;
						possibleMoves [4] = true;
					}
				}
			}

			if (x != 7) {
				if(y != 7)
				if (activePiece [x + 1, y + 1] != null && (activePiece [x + 1, y + 1].GetComponent<PieceScript> ().isWhite) != true) {
					possibleMovesPlane.Add (Instantiate (possibleMovePlane, GetPosition (x + 1, y + 1), Quaternion.identity) as GameObject);
					canMove = false;
					possibleMoves [5] = true;
				}

				if (piece.isKing == true) {
					if (y != 0)
					if (activePiece [x + 1, y - 1] != null && (activePiece [x + 1, y - 1].GetComponent<PieceScript> ().isWhite) != true) {
						possibleMovesPlane.Add (Instantiate (possibleMovePlane, GetPosition (x + 1, y - 1), Quaternion.identity) as GameObject);
						canMove = false;
						possibleMoves [6] = true;
					}
				}
			}

			if (canMove) {
				if(y != 7)
				if (activePiece [x, y + 1] == null) {
					possibleMovesPlane.Add (Instantiate (possibleMovePlane, GetPosition (x, y + 1), Quaternion.identity) as GameObject);
					possibleMoves [1] = true;

					if ((activePiece [x, y].GetComponent<PieceScript> ().moved) == false)
					if (activePiece [x, y + 2] == null) {
						possibleMovesPlane.Add (Instantiate (possibleMovePlane, GetPosition (x, y + 2), Quaternion.identity) as GameObject);
						possibleMoves [2] = true;
					}
				}
				if(y != 0 && piece.isKing == true)
				if (activePiece [x, y - 1] == null) {
					possibleMovesPlane.Add (Instantiate (possibleMovePlane, GetPosition (x, y - 1), Quaternion.identity) as GameObject);
					possibleMoves [0] = true;
				}
			}
		} else {
			
			bool canMove = true;
			if (x != 0) {
				if (y != 0)
				if (activePiece [x - 1, y - 1] != null && (activePiece [x - 1, y - 1].GetComponent<PieceScript> ().isWhite) == true) {
					possibleMovesPlane.Add (Instantiate (possibleMovePlane, GetPosition (x - 1, y - 1), Quaternion.identity) as GameObject);
					canMove = false;
					possibleMoves [7] = true;
				}

				if (piece.isKing == true) {
					if (y != 7)
					if (activePiece [x - 1, y + 1] != null && (activePiece [x - 1, y + 1].GetComponent<PieceScript> ().isWhite) == true) {
						possibleMovesPlane.Add (Instantiate (possibleMovePlane, GetPosition (x - 1, y + 1), Quaternion.identity) as GameObject);
						canMove = false;
						possibleMoves [8] = true;
					}
				}
			}

			if (x != 7) {
				if (y != 0)
				if (activePiece [x + 1, y - 1] != null && (activePiece [x + 1, y - 1].GetComponent<PieceScript> ().isWhite) == true) {
					possibleMovesPlane.Add (Instantiate (possibleMovePlane, GetPosition (x + 1, y - 1), Quaternion.identity) as GameObject);
					canMove = false;
					possibleMoves [9] = true;
				}

				if (piece.isKing == true) {
					if (y != 7)
					if (activePiece [x + 1, y + 1] != null && (activePiece [x + 1, y + 1].GetComponent<PieceScript> ().isWhite) == true) {
						possibleMovesPlane.Add (Instantiate (possibleMovePlane, GetPosition (x + 1, y + 1), Quaternion.identity) as GameObject);
						canMove = false;
						possibleMoves [10] = true;
					}
				}
			}

			if (canMove) {
				if (y != 0)
				if (activePiece [x, y - 1] == null) {
					possibleMovesPlane.Add (Instantiate (possibleMovePlane, GetPosition (x, y - 1), Quaternion.identity) as GameObject);
					possibleMoves [1] = true;

					if ((activePiece [x, y].GetComponent<PieceScript> ().moved) == false)
					if (activePiece [x, y - 2] == null) {
						possibleMovesPlane.Add (Instantiate (possibleMovePlane, GetPosition (x, y - 2), Quaternion.identity) as GameObject);
						possibleMoves [2] = true;
					}
				}
				if(y != 7 && piece.isKing == true)
				if (activePiece [x, y + 1] == null) {
					possibleMovesPlane.Add (Instantiate (possibleMovePlane, GetPosition (x, y + 1), Quaternion.identity) as GameObject);
					possibleMoves [0] = true;
				}

			}
		}
	}

	private void UpdateSelection()
	{
		RaycastHit hitInfo;
		if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hitInfo, 30.0f, LayerMask.GetMask ("CheckersBoard"))) {
			int selectionX = (int)hitInfo.point.x;
			int selectionY = (int)hitInfo.point.z;

			if (Input.GetMouseButtonDown (0)) {
				SelectPlane (selectionX, selectionY);
			}
		}
	}
		
	private Vector3 GetPosition(int x, int y)
	{
		Vector3 newPosition = Vector3.zero;
		newPosition.x = x * TILE_SIZE + TILE_OFFSET;
		newPosition.z = y * TILE_SIZE + TILE_OFFSET;
		return newPosition;
	}

	private void SpawnPiece (int index, int x, int y)
	{
		activePiece [x, y] = Instantiate (pieceModels [index], GetPosition(x, y), Quaternion.identity) as GameObject;

		activePiece [x, y].GetComponent<PieceScript> ().isWhite = (index < 2);

	}

	private void RespawnPieces ()
	{
		for (int x = 0; x < 8; ++x) {
			for (int y = 0; y < 8; ++y) {
				Destroy (activePiece [x, y]);
			}
			for (int y = 0; y < 2; ++y) {
				SpawnPiece (0, x, y);
				SpawnPiece (2, x, 7-y);
			}
		}
		numActivePiecesW = 16;
		numActivePiecesB = 16;
	}

	private void SetPiecePosition(int fromX, int fromY, int toX, int toY)
	{
		if (activePiece [fromX, fromY] == null || activePiece [toX, toY] != null)
			return;
		
		if ((activePiece [fromX, fromY].GetComponent<PieceScript> ().isWhite == true && toY == 7) || (activePiece [fromX, fromY].GetComponent<PieceScript> ().isWhite == false && toY == 0)) {
			SpawnPiece (activePiece [fromX, fromY].GetComponent<PieceScript> ().isWhite == true ? 1 : 3, toX, toY);
			activePiece [toX, toY].GetComponent<PieceScript> ().isKing = true;
			Destroy (activePiece [fromX, fromY]);
		} else {
			activePiece [fromX, fromY].transform.position = GetPosition (toX, toY);
			activePiece [toX, toY] = activePiece [fromX, fromY];
		}

		activePiece [fromX, fromY] = null;
		changeMove = true;
	}
}
