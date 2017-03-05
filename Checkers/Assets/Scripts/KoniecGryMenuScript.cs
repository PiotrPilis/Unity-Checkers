using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoniecGryMenuScript : MonoBehaviour {

	private CheckersPlanesScript checkersGame;
	private Canvas mainMenu;
	private Canvas menu;

	// Use this for initialization
	void Start () {
		checkersGame = GameObject.FindGameObjectWithTag ("CheckersGame").GetComponent<CheckersPlanesScript> ();
		menu = (Canvas)GetComponent<Canvas> ();
		mainMenu = (Canvas)GameObject.FindGameObjectWithTag ("MainMenu").GetComponent<Canvas> ();
	}

	public void GameOver()
	{
		menu.enabled = true;
		checkersGame.enabled = false;
	}

	public void Wyjdz()
	{
		mainMenu.enabled = true;
		menu.enabled = false;
	}

	public void ResetGame()
	{
		checkersGame.ResetGame ();
		checkersGame.enabled = true ;
		menu.enabled = false;
	}
}
