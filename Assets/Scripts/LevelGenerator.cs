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
	public string m_AnimationSequenceFilepath = "Data/AnimationSequence.txt" ;
	public Dictionary< string , AnimationSequenceStruct > m_AnimationSequenceData = 
		new Dictionary<string, AnimationSequenceStruct>() ;
	// Use this for initialization
	void Start () 
	{
		LoadSysInit() ;
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
		
	}
	
	void LoadQuestionTable()
	{
		
	}
	
	void QuestionDetectionDefinition()
	{
		
	}
	
}
