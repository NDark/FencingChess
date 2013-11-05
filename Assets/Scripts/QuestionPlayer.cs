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
	
	public AnimationPlayer m_AnimationPlayer = null ;
		
	// Use this for initialization
	void Start () 
	{
		if( null == m_AnimationPlayer )
		{
			GameObject obj = GameObject.Find( "GlobalSingleton" ) ;
			m_AnimationPlayer = obj.GetComponent<AnimationPlayer>() ;
		}
		
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
			m_GUIReady.gameObject.SendMessage( "ClearIsDown" ) ;
			m_QuestionState = QuestionState.Ready ;
			break ;
		case QuestionState.Ready :
			{
				GUI_IsDown guiIsDown = m_GUIReady.gameObject.GetComponent<GUI_IsDown>() ;
				if( true == guiIsDown.m_IsDown )
				{
					// close ready sign
					m_GUIReady.enabled = false ;
					// play question
					m_AnimationPlayer.Setup( "Anim1" ) ;
					m_QuestionState = QuestionState.QuestionAnimation ;
				}
			}
			break ;
			
		case QuestionState.QuestionAnimation :
			// wait for animation complete
			if( false == m_AnimationPlayer.m_IsActive )
			{
				// show drag sword sign
				m_QuestionState = QuestionState.DragSword ;
			}
			break ;
			
		case QuestionState.DragSword :
			// wait for mouse input down and up
			
			// press down for start point 
			// drag for end point adjustment
			
			break ;			
		}
		
	
	}
}
