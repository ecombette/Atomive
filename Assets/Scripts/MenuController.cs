using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class MenuController : MonoBehaviour {

	void Start () 
	{
			
	}

	void Update () 
	{
			
	}

	public void OnSandBoxClick()
	{
		SceneManager.LoadScene ("TestScene");
	}

	public void OnOptionsClick()
	{
		SceneManager.LoadScene ("Options");
	}

	public void OnQuitClick()
	{
		Application.Quit ();
	}
}
