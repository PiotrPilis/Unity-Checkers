using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MainMenuScript : MonoBehaviour {
	
	private CheckersPlanesScript checkersGame;
	private Canvas optionMenu;
	private Canvas menu;

	public Button newGameButton;
	public Button quitGameButton;
	public Button optionButton; 

	// Use this for initialization
	void Start () {
		checkersGame = GameObject.FindGameObjectWithTag ("CheckersGame").GetComponent<CheckersPlanesScript> ();
		optionMenu = (Canvas)GameObject.FindGameObjectWithTag ("OptionMenu").GetComponent<Canvas> ();
		menu = (Canvas)GetComponent<Canvas> ();

	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) {
			menu.enabled = true;
			checkersGame.enabled = false;
		}
	}

	public void Wyjdz()
	{
		Application.Quit ();
	}

	public void Option()
	{
		newGameButton.enabled = false;
		optionButton.enabled = false;
		quitGameButton.enabled = false;

		optionMenu.enabled = true;
	}

	public void StartGame()
	{
		checkersGame.ResetGame ();
		checkersGame.enabled = true ;
		menu.enabled = false;
	}
}
