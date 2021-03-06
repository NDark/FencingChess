/*
@file QuestionPlayer.cs
@author NDark
@date 20131105 file started.
@date 20131108 by NDark . remove CheckJudge()
@date 20131201 by NDark 
. add class member m_GUIBackName
. add class member m_GUIBack
. add code to show and hide m_GUIBack

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
		Judge ,
		CorrectAnimation ,
		FalseAnimation ,
		FinishAnimation ,
		Again ,
	}
	
	public QuestionState m_QuestionState = QuestionState.UnActive ;
	
	public GUIText m_GUIReady = null ;
	
	public string m_GUIBackName = "GUI_Leave" ;
	public GameObject m_GUIBack = null ;
	
	public GameObject m_GUIAnswerSymbol = null ;
	public float m_AnswerSymbolStartY = 100 ;
	public float m_AnswerSymbolWidth = 0 ;
	public float m_AnswerSymbolHeight = 100 ;
	public int m_AnswerIndex = 0 ; 
	
	public string m_GUIAnswerDescrptionName = "GUI_AnswerDescrtiptioin" ;
	public GameObject m_GUIAnswerDescription = null ;
	public float m_AnswerDescriptionStartY = 100 ;
	
	public string m_GUIAnswerMouseCursorName = "GUI_AnswerMouseCursor" ;
	public GameObject m_GUIAnswerMouseCursor = null ;
	
	public AnimationPlayer m_AnimationPlayer = null ;
	
	public GameObject m_GUIInstruction = null ;
	public int m_InstructionMoveSword = 1212 ; // "Wählen Sie Ihre Reaktion." ;
	public int m_InstructionQuestion = 1211 ; // "Der Angriff kommt." ;
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
	public float m_PictureScale = 0.5625f ;
	
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
			m_GUIAnimationTexture = obj.GetComponent<GUITexture>() ;
			m_GUIAnimationTexture.texture = null ;
		}
		
		if( null == m_GUIQuestionZoneAnswer )
		{
			m_GUIQuestionZoneAnswer = GameObject.Find( m_GUIQuestionZoneAnswerName ) ;
			if( null == m_GUIQuestionZoneAnswer )
			{
				Debug.Log( "AnimationPlayer::Start() null == m_GUIQuestionZoneAnswer" ) ;
			}

			m_GUIQuestionZoneAnswer.GetComponent<GUITexture>().texture = null ;
			m_GUIQuestionZoneAnswer.GetComponent<GUITexture>().pixelInset = new Rect( 0 , (Camera.main.pixelHeight - Camera.main.pixelWidth * m_PictureScale) / 2 , Camera.main.pixelWidth , Camera.main.pixelWidth * m_PictureScale ) ;
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
			m_GUIReady = obj.GetComponent<GUIText>() ;
		}
		
		if( null == m_GUIBack )
		{
			m_GUIBack = GameObject.Find( m_GUIBackName ) ;
		}
		
		if( null == m_GUIInstruction )
		{
			m_GUIInstruction = GameObject.Find( "GUI_Instruction" ) ;
			
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
				ShowGUITexture.Show( m_GUIWrong , false , true , true ) ;
				ShowGUITexture.Show( m_GUICorrect , false , true , true ) ;
				ShowGUITexture.Show( m_GUIInstruction , false , true , true ) ;
				ShowGUITexture.Show( m_GUIAgain , false , true , true ) ;
				ShowGUITexture.Show( m_GUIAnswerSymbol , false , true , true ) ;
				ShowGUITexture.Show( m_GUIAnswerMouseCursor , false , true , true ) ;
				
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
					string text = StrsManager.Get( m_InstructionQuestion ) ;
					m_GUIInstruction.GetComponent<GUIText>().text = text ;
				
					// 把返回關閉
					ShowGUITexture.Show( m_GUIBack , false , true , true ) ;
				
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
				string text = StrsManager.Get( m_InstructionMoveSword ) ;
				m_GUIInstruction.GetComponent<GUIText>().text = text ;
				m_QuestionState = QuestionState.DecideTheAnswer ;
			}
			break ;
			
		case QuestionState.DecideTheAnswer :
			DetectAmongAnswers() ;
			UpdateMouseCursor() ;
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
		m_AnswerSymbolHeight = Camera.main.pixelWidth * m_PictureScale ;
		m_AnswerSymbolStartY =(Camera.main.pixelHeight - Camera.main.pixelWidth * m_PictureScale) / 2 ;
		m_GUIAnswerSymbol.GetComponent<GUITexture>().pixelInset = 
			new Rect( 0 , m_AnswerSymbolStartY , m_AnswerSymbolWidth , m_AnswerSymbolHeight ) ;
		// new Rect( 0 , (Camera.main.pixelHeight - Camera.main.pixelWidth * m_PictureScale) / 2 , 
		// Camera.main.pixelWidth , Camera.main.pixelWidth * m_PictureScale ) ;
		
		m_GUIAnswerDescription = GameObject.Find( m_GUIAnswerDescrptionName ) ;
		if( null == m_GUIAnswerDescription )
		{
			Debug.LogError( "SetupForAnswerZone() null == m_GUIAnswerDescription" ) ;
			return ;
		}
		
		m_GUIAnswerMouseCursor = GameObject.Find( m_GUIAnswerMouseCursorName ) ;
		if( null == m_GUIAnswerMouseCursor )
		{
			Debug.LogError( "SetupForAnswerZone() null == m_GUIAnswerMouseCursor" ) ;
			return ;
		}
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
	
	private void UpdateMouseCursor() 
	{
		m_GUIAnswerMouseCursor.GetComponent<GUITexture>().pixelInset = 
			new Rect( Input.mousePosition.x, Input.mousePosition.y  , 100 , 100) ;
	}
	
	private void DetectAmongAnswers()
	{
		// 判斷滑鼠在哪裡
		// 依照question的answer的數目
		if( false == m_LevelGeneratorPtr.m_QuestionTable.ContainsKey( m_QuestionString ) )
			return ;
		
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
			m_GUIAnswerSymbol.GetComponent<GUITexture>().pixelInset = 
				new Rect( m_AnswerIndex * m_AnswerSymbolWidth , 
					m_AnswerSymbolStartY , 
					m_AnswerSymbolWidth , 
					m_AnswerSymbolHeight ) ;
			
			/*if( true == m_GUIAnswerSymbol.guiTexture.pixelInset.Contains( new Vector2( Input.mousePosition.x , Input.mousePosition.y ) ) )
			{
				// Debug.LogError( "true == m_GUIAnswerSymbol.guiTexture.pixelInset.Contains" ) ;
				m_GUIAnswerDescription.guiText.color = Color.cyan ;
			}
			else 
			*/
			{
				m_GUIAnswerDescription.GetComponent<GUIText>().color = Color.red ;
			}
			
			
			m_GUIAnswerDescription.GetComponent<GUIText>().pixelOffset = 
				new Vector2( m_AnswerIndex * m_AnswerSymbolWidth + m_AnswerSymbolWidth / 2 , 
					m_AnswerDescriptionStartY ) ;
			
			ShowGUITexture.Show( m_GUIAnswerSymbol , true , true , true ) ;
			ShowGUITexture.Show( m_GUIAnswerMouseCursor , true , true , true ) ;
		}
		
		// switch and show the image
		if( 0 != imagePath.Length )
		{
			m_GUIQuestionZoneAnswer.GetComponent<GUITexture>().texture = 
				(Texture2D) Resources.Load( "Texture/" + imagePath ) ;
			ShowGUITexture.Show( m_GUIQuestionZoneAnswer , true , true , true ) ;
		}
		
		// 如果滑鼠按下,則進入判斷流程
		if( true == Input.GetMouseButtonUp( 0 ) )
		{
			ShowGUITexture.Show( m_GUIAnswerSymbol , false , true , true ) ;
			ShowGUITexture.Show( m_GUIQuestionZoneAnswer , false , true , true ) ;
			ShowGUITexture.Show( m_GUIInstruction , false , true , true ) ;
			ShowGUITexture.Show( m_GUIAnswerMouseCursor , false , true , true ) ;
			
			// 把返回開啟
			ShowGUITexture.Show( m_GUIBack , true , true , true ) ;
			
			m_QuestionState = QuestionState.Judge ;
		}
	}
}
