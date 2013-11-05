/*
@file GUI_IsDown.cs
@author NDark
@date 20131105 . file started.
*/
using UnityEngine;

public class GUI_IsDown : MonoBehaviour 
{
	public bool m_IsDown = false ;
	
	public void ClearIsDown()
	{
		m_IsDown = false ;
	}
	
	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
	}
	
	void OnMouseDown()
	{
		m_IsDown = true ;
	}
	
}
