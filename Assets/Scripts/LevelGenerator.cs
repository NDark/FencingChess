/*
@file LevelGenerator.cs
@author NDark
@date 20131105 file started.
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class LevelGenerator : MonoBehaviour 
{
	public string m_AnimationSequenceFilepath = "Data/AnimationSequence" ;
	public Dictionary< string , AnimationSequenceStruct > m_AnimationSequenceData = 
		new Dictionary<string, AnimationSequenceStruct>() ;
	
	public string m_QuestionTableFilepath = "Data/QuestionDefinition" ;
	public Dictionary< string , QuestionTableStruct > m_QuestionTable = 
		new Dictionary<string, QuestionTableStruct>() ;

	// Use this for initialization
	void Start () 
	{
		LoadSysInit() ;
		LoadAnimationSequence() ;
		LoadQuestionTable() ;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	void LoadSysInit() 
	{
		
	}
	
	void LoadAnimationSequence()
	{
		TextAsset content = (TextAsset) Resources.Load( m_AnimationSequenceFilepath ) ;
		if( null == content )
		{
			Debug.LogError( "LoadAnimationSequence() null == content" ) ;
			return ;
		}
		
		if( 0 == content.text.Length )
		{
			Debug.LogError( "LoadAnimationSequence() 0 == content.text" ) ;
			return ;
		}
		
		ParseAnimationSequence( content.text ) ;
	}
	
	void ParseAnimationSequence( string _Content )
	{
		string [] spliter = { "\n" } ;
		string [] strLines = _Content.Split( spliter , System.StringSplitOptions.None ) ;
		// Debug.Log( "ParseAnimationSequence() strLines.Length" + strLines.Length ) ;
		
		for( int i = 0 ; i < strLines.Length ; ++i )
		{
			if( 0 == strLines[ i ].Length )
				continue ;
			// Debug.Log( "ParseAnimationSequence() strLines[ i ]=" + strLines[ i ] ) ;
			string [] spliter2 = { "," } ;
			string [] strSegments = strLines[ i ].Split( spliter2 , System.StringSplitOptions.None ) ;			
			// Debug.Log( "ParseAnimationSequence() strSegments.Length=" + strSegments.Length ) ;
			if( strSegments.Length > 0 )
			{
				AnimationSequenceStruct newStruct = new AnimationSequenceStruct() ;
				newStruct.m_AnimationTag = strSegments[ 0 ] ;
				for( int j = 1 ; j < strSegments.Length ; ++j )
				{
					if( strSegments[ j ].Length > 0 )
					{
						// Debug.Log( "strSegments[ j ]=" + strSegments[ j ] ) ;
						newStruct.m_ImageFilepath.Add( strSegments[ j ] ) ;
					}
				}
				m_AnimationSequenceData.Add( newStruct.m_AnimationTag , newStruct ) ;
			}
		}
		
		Debug.Log( "ParseAnimationSequence() m_AnimationSequenceData.Count=" + m_AnimationSequenceData.Count ) ;
	}
	
	void LoadQuestionTable()
	{
		TextAsset content = (TextAsset) Resources.Load( m_QuestionTableFilepath ) ;
		if( null == content )
		{
			Debug.LogError( "LoadQuestionTable() null == content" + m_QuestionTableFilepath ) ;
			return ;
		}
		XmlDocument doc = new XmlDocument() ;
		doc.LoadXml( content.text ) ;
		XmlNode rootChild = doc.FirstChild ;
		if( "QuestionTable" != rootChild.Name )
			return ;
		
		for( int i = 0 ; i < rootChild.ChildNodes.Count ; ++i )
		{
			XmlNode childNode = rootChild.ChildNodes[ i ] ;
			if( "QuestionDefine" == childNode.Name )
			{
				QuestionTableStruct question = new QuestionTableStruct() ;
				question.ParseXML( childNode ) ;
				if( 0 < question.m_QuestionAnimationString.Length )
				{
					m_QuestionTable.Add( question.m_QuestionAnimationString , question ) ;
				}
			}
		}
		Debug.Log( "LoadQuestionTable() m_QuestionTable.Count=" + m_QuestionTable.Count ) ;
	}
	
	
}
