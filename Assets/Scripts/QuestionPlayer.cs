/*
@file QuestionPlayer.cs
@author NDark
@date 20131105 file started.
@date 20131108 by NDark . remove CheckJudge()

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
		DecideTheAnswer ,
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
	
	public GameObject m_GUISwordStart = null ;
	public GameObject m_GUISwordEnd = null ;
	public GameObject m_GUIAnswerSymbol = null ;
	public float m_AnswerSymbolWidth = 0 ;
	public float m_AnswerSymbolHeight = 20 ;
	public int m_AnswerIndex = 0 ; 
	
	public AnimationPlayer m_AnimationPlayer = null ;
	
	public GameObject m_GUIInstruction = null ;
	public string m_InstructionMoveSword = "Wählen Sie Ihre Reaktion." ;
	public string m_InstructionQuestion = "Der Angriff kommt." ;
	public GameObject m_GUIYouHaveAlreadyDead = null ;
	
	public string m_QuestionString = "" ;
	public string m_ResultString = "" ;
	
	public GameObject m_GUICorrect = null ;
	public GameObject m_GUIWrong = null ;
	public GameObject m_GUIAgain = null ;
	
	public string m_GUIAnimationObjectName = "GUI_AnimationObject" ;
	public GUITexture m_GUIAnimationTexture = null ;
	
	public string m_GUIQuestionZoneAnswerName = "GUI_QuestionZoneAnswer" ;
	public GameObject m_GUIQuestionZoneAnswer = null ;	
	
	
	LevelGenerator m_LevelGeneratorPtr = null ;
	
	
	// Use this for initialization
	void Start () 
	{
		
		if( null == m_GUIAnimationTexture )
		{
			GameObject obj = GameObject.Find( m_GUIAnimationObjectName ) ;
			if( null == obj )
			{
				Debug.Log( "AnimationPlayer::Start() null == obj" ) ;
			}
			m_GUIAnimationTexture = obj.guiTexture ;
			m_GUIAnimationTexture.texture = null ;
		}
		
		if( null == m_GUIQuestionZoneAnswer )
		{
			m_GUIQuestionZoneAnswer = GameObject.Find( m_GUIQuestionZoneAnswerName ) ;
			if( null == m_GUIQuestionZoneAnswer )
			{
				Debug.Log( "AnimationPlayer::Start() null == m_GUIQuestionZoneAnswer" ) ;
			}
			m_GUIQuestionZoneAnswer.guiTexture.texture = null ;
			m_GUIQuestionZoneAnswer.guiTexture.pixelInset = new Rect( 0 , 0 , Camera.main.pixelWidth , Camera.main.pixelHeight ) ;
			ShowGUITexture.Show( m_GUIQuestionZoneAnswer , false , true , true ) ;
		}		
		
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
		
		if( null == m_GUIInstruction )
		{
			m_GUIInstruction = GameObject.Find( "GUI_Instruction" ) ;
			
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
		
		SetupForAnswerZone() ;
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		switch( m_QuestionState )
		{
		case QuestionState.UnActive :
			
			if( null != m_GUIAnimationTexture )
			{
				m_GUIAnimationTexture.texture = null ;
			}		
			
			{
				ShowGUITexture.Show( m_GUIQuestionZoneAnswer , false , true , true ) ;
				ShowGUITexture.Show( m_GUISwordStart , false , true , true ) ;
				ShowGUITexture.Show( m_GUISwordEnd , false , true , true ) ;
				ShowGUITexture.Show( m_GUIWrong , false , true , true ) ;
				ShowGUITexture.Show( m_GUICorrect , false , true , true ) ;
				ShowGUITexture.Show( m_GUIInstruction , false , true , true ) ;
				ShowGUITexture.Show( m_GUIAgain , false , true , true ) ;
				ShowGUITexture.Show( m_GUIAnswerSymbol , false , true , true ) ;
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
					ShowGUITexture.Show( m_GUIInstruction , true , true , true ) ;
					m_GUIInstruction.guiText.text = m_InstructionQuestion ;
					m_QuestionState = QuestionState.QuestionAnimation ;
				}
			}
			break ;
			
		case QuestionState.QuestionAnimation :
			// wait for animation complete
			if( false == m_AnimationPlayer.m_IsActive )
			{
				// show drag sword sign
				ShowGUITexture.Show( m_GUIInstruction , true , true , true ) ;
				m_GUIInstruction.guiText.text = m_InstructionMoveSword ;
				m_QuestionState = QuestionState.DecideTheAnswer ;
			}
			break ;
			
		case QuestionState.DecideTheAnswer :
			DetectAmongAnswers() ;
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
				ShowGUITexture.Show( m_GUIInstruction , false , true , true ) ;
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
			CheckJudge2() ;
			
			
			break ;	
			
		case QuestionState.CorrectAnimation :
			
			if( false == m_AnimationPlayer.m_IsActive )
			{
				// show drag sword sign
				QuestionTableStruct question = m_LevelGeneratorPtr.m_QuestionTable[ m_QuestionString ] ;
				m_AnimationPlayer.Setup( question.m_FinishAnimationString ) ;
			 	ShowGUITexture.Show( m_GUISwordEnd , false , true , true ) ;
			 	ShowGUITexture.Show( m_GUISwordStart , false , true , true ) ;				
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
	
	private void SetupForAnswerZone()
	{
		// m_GUIAnswerZones
		m_GUIAnswerSymbol = GameObject.Find( "GUI_AnswerSymbol" ) ;
		if( null == m_GUIAnswerSymbol )
		{
			Debug.LogError( "SetupForAnswerZone() null == m_GUIAnswerSymbol" ) ;
			return ;
		}
		
		if( false == m_LevelGeneratorPtr.m_QuestionTable.ContainsKey( m_QuestionString ) )
		{
			Debug.LogError( "SetupForAnswerZone() false == ContainsKey( m_QuestionString" + m_QuestionString ) ;
			return ;
		}
		
		QuestionTableStruct question = m_LevelGeneratorPtr.m_QuestionTable[ m_QuestionString ] ;
		int answerNum = question.m_DetectionZones.Count ;
		m_AnswerSymbolWidth = Camera.main.pixelWidth / answerNum ;
		
		m_GUIAnswerSymbol.guiTexture.pixelInset = 
			new Rect( 0 , 0 , m_AnswerSymbolWidth , m_AnswerSymbolHeight ) ;
			
	}
	
	
	private void CheckJudge2()
	{	
		if( false == m_LevelGeneratorPtr.m_QuestionTable.ContainsKey( m_QuestionString ) )
			return ;
		
		string minAnimKey = "" ;
		QuestionTableStruct question = m_LevelGeneratorPtr.m_QuestionTable[ m_QuestionString ] ;
		int detectIndex = 0 ;
		Debug.Log( "question.m_DetectionZones" + question.m_DetectionZones.Count ) ;
		Dictionary< string , DetectionPose >.Enumerator iQuestion = question.m_DetectionZones.GetEnumerator() ;
		while( iQuestion.MoveNext() )
		{
			if( detectIndex == m_AnswerIndex )
			{
				minAnimKey = iQuestion.Current.Key ;
				Debug.Log( "m_AnswerIndex=" + m_AnswerIndex ) ;
				break ;
			}
			++detectIndex ;
		}
		
		// Debug.Log( "minAnimKey" + minAnimKey ) ;
		if( 0 != minAnimKey.Length )
		{
			m_ResultString = minAnimKey ;
			Debug.Log( "m_ResultString" + m_ResultString ) ;
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

		}		
	}

	private void DetectAmongAnswers()
	{
		// 判斷滑鼠在哪裡
		// 依照question的answer的數目
		QuestionTableStruct question = m_LevelGeneratorPtr.m_QuestionTable[ m_QuestionString ] ;
		int answerNum = question.m_DetectionZones.Count ;
		float ratioOfMouse = (float) Input.mousePosition.x / (float) Camera.main.pixelWidth ;
		m_AnswerIndex = (int)( ratioOfMouse * answerNum ) ;
		// Debug.Log( "DetectAmongAnswers() answerIndex= " + answerIndex ) ;
		// 決定question要顯示的透明半身圖
		string imagePath = "" ;
		int i = 0 ;
		foreach( DetectionPose pose in question.m_DetectionZones.Values )
		{
			if( i == m_AnswerIndex )
			{
				imagePath = pose.m_AnswerImagePath ;
				// Debug.Log( "DetectAmongAnswers() imagePath= " + imagePath ) ;
				break ;
			}
			++i ;
		}
		
		// 調整answersymbol的位置
		if( null != m_GUIAnswerSymbol )
		{
			m_GUIAnswerSymbol.guiTexture.pixelInset = 
				new Rect( m_AnswerIndex * m_AnswerSymbolWidth , 
					0 , 
					m_AnswerSymbolWidth , 
					m_AnswerSymbolHeight ) ;
			ShowGUITexture.Show( m_GUIAnswerSymbol , true , true , true ) ;
		}
		
		// switch and show the image
		if( 0 != imagePath.Length )
		{
			m_GUIQuestionZoneAnswer.guiTexture.texture = 
				(Texture2D) Resources.Load( "Texture/" + imagePath ) ;
			ShowGUITexture.Show( m_GUIQuestionZoneAnswer , true , true , true ) ;
		}
		
		// 如果滑鼠按下,則進入判斷流程
		if( true == Input.GetMouseButton( 0 ) )
		{
			ShowGUITexture.Show( m_GUIAnswerSymbol , false , true , true ) ;
			ShowGUITexture.Show( m_GUIQuestionZoneAnswer , false , true , true ) ;
			ShowGUITexture.Show( m_GUIInstruction , false , true , true ) ;
			m_QuestionState = QuestionState.Judge ;
		}
	}
}
