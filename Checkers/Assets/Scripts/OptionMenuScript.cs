using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionMenuScript : MonoBehaviour {

	private Canvas mainMenu;
	private Canvas menu;
	public GameObject checkersBoard;

	public Button newGameButton;
	public Button quitGameButton;
	public Button optionButton; 

	public Dropdown dropdown;

	public Material[] materials;

	// Use this for initialization
	void Start () {
		menu = (Canvas)GetComponent<Canvas> ();
		mainMenu = (Canvas)GameObject.FindGameObjectWithTag ("MainMenu").GetComponent<Canvas> ();

		checkersBoard.GetComponent<Renderer> ().material= materials[1];
	}
	
	public void Back()
	{
		newGameButton.enabled = true;
		optionButton.enabled = true;
		quitGameButton.enabled = true;

		menu.enabled = false;
	}

	public void Confirm()
	{
		Back ();
		checkersBoard.GetComponent<Renderer> ().material= materials[dropdown.value];
	}
}
