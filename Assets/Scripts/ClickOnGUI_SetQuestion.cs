/*
@file ClickOnGUI_SetQuestion.cs
@author NDark
@date 20131105 file started.
*/
using UnityEngine;

public class ClickOnGUI_SetQuestion : MonoBehaviour 
{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnMouseDown()
	{
		StaticData.QuestionString = this.gameObject.name ;
		Application.LoadLevel( "ChessBattle" ) ;
	}
}
