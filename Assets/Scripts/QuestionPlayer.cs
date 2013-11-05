/*
@file QuestionPlayer.cs
@author NDark
@date 20131105 file started.
*/
using UnityEngine;
using System.Collections;

public class QuestionPlayer : MonoBehaviour 
{
	public enum QuestionState 
	{
		UnActive ,
		Ready ,
		QuestionAnimation ,
		DragSword ,
		Judge ,
		CorrectAnimation ,
		FalseAnimation ,
		Again ,
	}
	
	public QuestionState m_QuestionState = QuestionState.UnActive ;
	
	public GUIText m_GUIReady = null ;
	
	// Use this for initialization
	void Start () 
	{
		{
			GameObject obj = GameObject.Find( "GUI_Ready" ) ;
			m_GUIReady = obj.guiText ;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		switch( m_QuestionState )
		{
		case QuestionState.UnActive :
			// show ready mark.
			m_GUIReady.enabled = true ;
			m_QuestionState = QuestionState.Ready ;
			break ;
		case QuestionState.Ready :
			break ;
		}
	
	}
}
