/*
@file QuestionPlayer.cs
@author NDark
@date 20131105 file started.
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
		FinishAnimation ,
		Again ,
	}
	
	public QuestionState m_QuestionState = QuestionState.UnActive ;
	
	public GUIText m_GUIReady = null ;
	public GUIText m_GUIDragSword = null ;
	public GameObject m_GUISwordStart = null ;
	public GameObject m_GUISwordEnd = null ;
	
	public AnimationPlayer m_AnimationPlayer = null ;
		
	public string m_QuestionString = "" ;
	public string m_ResultString = "" ;
	
	public GameObject m_GUICorrect = null ;
	public GameObject m_GUIWrong = null ;
	public GameObject m_GUIAgain = null ;
	
	LevelGenerator m_LevelGeneratorPtr = null ;
	
	// Use this for initialization
	void Start () 
	{
		if( null == m_LevelGeneratorPtr )
		{
			GameObject obj = GameObject.Find( "GlobalSingleton" ) ;
			m_LevelGeneratorPtr = obj.GetComponent<LevelGenerator>();
		}
		
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
		
		if( null == m_GUICorrect )
		{
			m_GUICorrect = GameObject.Find( "GUI_CorrectSymbol" ) ;
			ShowGUITexture.Show( m_GUICorrect , false , true , true ) ;
		}
		
		if( null == m_GUIWrong )
		{
			m_GUIWrong = GameObject.Find( "GUI_WrongSymbol" ) ;
			ShowGUITexture.Show( m_GUIWrong , false , true , true ) ;
		}
		
		if( null == m_GUIAgain )
		{
			m_GUIAgain = GameObject.Find( "GUI_Again" ) ;
			ShowGUITexture.Show( m_GUIAgain , false , true , true ) ;
		}		
		
		
		
		m_QuestionString = StaticData.QuestionString ;
		Debug.Log( "m_QuestionString=" + m_QuestionString ) ;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		switch( m_QuestionState )
		{
		case QuestionState.UnActive :
			
			{
				ShowGUITexture.Show( m_GUISwordStart , false , true , true ) ;
				ShowGUITexture.Show( m_GUISwordEnd , false , true , true ) ;
				ShowGUITexture.Show( m_GUIWrong , false , true , true ) ;
				ShowGUITexture.Show( m_GUICorrect , false , true , true ) ;
				ShowGUITexture.Show( m_GUIDragSword.gameObject , false , true , true ) ;
				ShowGUITexture.Show( m_GUIAgain , false , true , true ) ;
			}		
						
			// show ready mark.
			m_GUIReady.enabled = true ;
			m_GUIReady.gameObject.SendMessage( "ClearIsDown" ) ;
			m_QuestionState = QuestionState.Ready ;
			break ;
		case QuestionState.Ready :
			{
				GUI_IsDown guiIsDown = m_GUIReady.gameObject.GetComponent<GUI_IsDown>() ;
				if( true == guiIsDown.m_IsDown && 0 != m_QuestionString.Length )
				{
					// close ready sign
					m_GUIReady.enabled = false ;
					// play question
					m_AnimationPlayer.Setup( m_QuestionString ) ;
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
			CheckJudge() ;
			
			
			break ;	
			
		case QuestionState.CorrectAnimation :
			
			if( false == m_AnimationPlayer.m_IsActive )
			{
				// show drag sword sign
				QuestionTableStruct question = m_LevelGeneratorPtr.m_QuestionTable[ m_QuestionString ] ;
				m_AnimationPlayer.Setup( question.m_FinishAnimationString ) ;
				m_QuestionState = QuestionState.FinishAnimation ;
			}
			
			break ;		
			
		case QuestionState.FalseAnimation :
			if( false == m_AnimationPlayer.m_IsActive )
			{
				// show drag sword sign
				ShowGUITexture.Show( m_GUIAgain , true , true , true ) ;
			}

			GUI_IsDown guiIsDown = m_GUIAgain.gameObject.GetComponent<GUI_IsDown>() ;
			if( true == guiIsDown.m_IsDown )	
			{
				m_QuestionState = QuestionState.UnActive ;
			}
			break ;	
			
		case QuestionState.FinishAnimation :
			break ;					
			
		}		
	
	}
	
	private void CheckJudge()
	{
		if( false == m_LevelGeneratorPtr.m_QuestionTable.ContainsKey( m_QuestionString ) )
			return ;
		
		string minAnimKey = "" ;
		float min = 999999 ;
		QuestionTableStruct question = m_LevelGeneratorPtr.m_QuestionTable[ m_QuestionString ] ;
		// Debug.Log( "question.m_DetectionZones" + question.m_DetectionZones.Count ) ;
		Dictionary< string , DetectionPose >.Enumerator iQuestion = question.m_DetectionZones.GetEnumerator() ;
		while( iQuestion.MoveNext() )
		{
			
			Vector2 diffStart = m_GUISwordStart.transform.position ;
			diffStart = diffStart - iQuestion.Current.Value.m_Start ;
			
			Vector2 diffEnd = m_GUISwordEnd.transform.position ;
			diffEnd = diffEnd - iQuestion.Current.Value.m_End ;
						
			float tmp = diffStart.sqrMagnitude + diffEnd.sqrMagnitude ;
			if( tmp < min )
			{
				min = tmp ;
				minAnimKey = iQuestion.Current.Key ;
			}
		}
		// Debug.Log( "minAnimKey" + minAnimKey ) ;
		if( 0 != minAnimKey.Length )
		{
			m_ResultString = minAnimKey ;
			// Debug.Log( "m_ResultString" + m_ResultString ) ;
			m_AnimationPlayer.Setup( m_ResultString ) ;
			if( -1 != m_ResultString.IndexOf( "_f" ) )
			{
				// show false alarm
				ShowGUITexture.Show( m_GUIWrong , true , true , true ) ;
				m_GUIAgain.gameObject.SendMessage( "ClearIsDown" ) ;
				m_QuestionState = QuestionState.FalseAnimation ;
			}
			else
			{
				ShowGUITexture.Show( m_GUICorrect , true , true , true ) ;
				m_QuestionState = QuestionState.CorrectAnimation ;
				
			}
			// ShowGUITexture.Show( m_GUISwordEnd , false , true , true ) ;
			// ShowGUITexture.Show( m_GUISwordStart , false , true , true ) ;
		}
		
	}
	
}
