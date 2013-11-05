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
		Ready ,
		QuestionAnimation ,
		DragSword ,
		Judge ,
		CorrectAnimation ,
		FalseAnimation ,
		Again ,
	}
	
	public QuestionState m_QuestionState = QuestionState.Ready ;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
