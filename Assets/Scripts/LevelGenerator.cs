/*
@file LevelGenerator.cs
@author NDark
@date 20131105 file started.
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour 
{
	public string m_AnimationSequenceFilepath = "Data/AnimationSequence" ;
	public Dictionary< string , AnimationSequenceStruct > m_AnimationSequenceData = 
		new Dictionary<string, AnimationSequenceStruct>() ;
	

	// Use this for initialization
	void Start () 
	{

		
		LoadSysInit() ;
		LoadAnimationSequence() ;
		
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
		string [] spliter = { "/r/n" } ;
		string [] strLines = _Content.Split( spliter , System.StringSplitOptions.None ) ;
		Debug.Log( "ParseAnimationSequence() strLines.Length" + strLines.Length ) ;
		
		for( int i = 0 ; i < strLines.Length ; ++i )
		{
			if( 0 == strLines[ i ].Length )
				continue ;
			Debug.Log( "ParseAnimationSequence() strLines[ i ]=" + strLines[ i ] ) ;
			string [] spliter2 = { "," } ;
			string [] strSegments = strLines[ i ].Split( spliter2 , System.StringSplitOptions.None ) ;			
			Debug.Log( "ParseAnimationSequence() strSegments.Length=" + strSegments.Length ) ;
			if( strSegments.Length > 0 )
			{
				AnimationSequenceStruct newStruct = new AnimationSequenceStruct() ;
				newStruct.m_AnimationTag = strSegments[ 0 ] ;
				for( int j = 1 ; j < strSegments.Length ; ++j )
				{
					newStruct.m_ImageFilepath.Add( strSegments[ j ] ) ;
				}
				m_AnimationSequenceData.Add( newStruct.m_AnimationTag , newStruct ) ;
			}
		}
		
		Debug.Log( "ParseAnimationSequence() m_AnimationSequenceData.Count=" + m_AnimationSequenceData.Count ) ;
	}
	
	void LoadQuestionTable()
	{
		
	}
	
	void QuestionDetectionDefinition()
	{
		
	}
	
}
