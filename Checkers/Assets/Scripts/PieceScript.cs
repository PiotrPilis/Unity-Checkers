using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceScript : MonoBehaviour {

	public bool moved { get; set; }
	public bool isWhite { get; set; }
	public bool isKing = false;

	// Use this for initialization
	void Start () {
		moved = false;
	}
}
