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
		DragSwordWaitForDown ,
		DragSwordWaitForUp ,
		Judge ,
		CorrectAnimation ,
		FalseAnimation ,
		Again ,
	}
	
	public QuestionState m_QuestionState = QuestionState.UnActive ;
	
	public GUIText m_GUIReady = null ;
	public GUIText m_GUIDragSword = null ;
	public GameObject m_GUISwordStart = null ;
	public GameObject m_GUISwordEnd = null ;
	
	public AnimationPlayer m_AnimationPlayer = null ;
		
	// Use this for initialization
	void Start () 
	{
		if( null == m_AnimationPlayer )
		{
			GameObject obj = GameObject.Find( "GlobalSingleton" ) ;
			m_AnimationPlayer = obj.GetComponent<AnimationPlayer>() ;
		}
		
		if( null == m_GUIReady )
		{
			GameObject obj = GameObject.Find( "GUI_Ready" ) ;
			m_GUIReady = obj.guiText ;
		}
		
		if( null == m_GUIDragSword )
		{
			GameObject obj = GameObject.Find( "GUI_DragSword" ) ;
			m_GUIDragSword = obj.guiText ;
		}
		
		if( null == m_GUISwordStart )
		{
			m_GUISwordStart = GameObject.Find( "GUI_SwordStart" ) ;
			ShowGUITexture.Show( m_GUISwordStart , false , true , true ) ;
		}
		
		if( null == m_GUISwordEnd )
		{
			m_GUISwordEnd = GameObject.Find( "GUI_SwordEnd" ) ;
			ShowGUITexture.Show( m_GUISwordEnd , false , true , true ) ;
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
				m_GUIDragSword.enabled = true ;
				m_QuestionState = QuestionState.DragSwordWaitForDown ;
			}
			break ;
			
		case QuestionState.DragSwordWaitForDown :
			
			// press down for start point 
			if( true == Input.GetMouseButton( 0 ) )
			{
				ShowGUITexture.Show( m_GUISwordStart , true , true , true ) ;
				
				Vector3 viewPortPos = Camera.main.ScreenToViewportPoint( Input.mousePosition ) ;
				viewPortPos.z = 2 ;
				m_GUISwordStart.transform.position = viewPortPos ;
				m_QuestionState = QuestionState.DragSwordWaitForUp ;
			}
			
			break ;			

		case QuestionState.DragSwordWaitForUp :
			
			// drag for end point adjustment
			if( false == Input.GetMouseButton( 0 ) )
			{
				m_QuestionState = QuestionState.Judge ;
			}
			else
			{
				ShowGUITexture.Show( m_GUISwordEnd , true , true , true ) ;
				
				Vector3 viewPortPos = Camera.main.ScreenToViewportPoint( Input.mousePosition ) ;
				viewPortPos.z = 2 ;
				m_GUISwordEnd.transform.position = viewPortPos ;
			}
			
			
			break ;			
			
		case QuestionState.Judge :
			break ;	
		}
	
	}
}
