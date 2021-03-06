/*
IMPORTANT: READ BEFORE DOWNLOADING, COPYING, INSTALLING OR USING. 

By downloading, copying, installing or using the software you agree to this license.
If you do not agree to this license, do not download, install, copy or use the software.

    License Agreement For Kobayashi Maru Commander Open Source

Copyright (C) 2013, Chih-Jen Teng(NDark) and Koguyue Entertainment, 
all rights reserved. Third party copyrights are property of their respective owners. 

Redistribution and use in source and binary forms, with or without modification,
are permitted provided that the following conditions are met:

  * Redistribution's of source code must retain the above copyright notice,
    this list of conditions and the following disclaimer.

  * Redistribution's in binary form must reproduce the above copyright notice,
    this list of conditions and the following disclaimer in the documentation
    and/or other materials provided with the distribution.

  * The name of Koguyue or all authors may not be used to endorse or promote products
    derived from this software without specific prior written permission.

This software is provided by the copyright holders and contributors "as is" and
any express or implied warranties, including, but not limited to, the implied
warranties of merchantability and fitness for a particular purpose are disclaimed.
In no event shall the Koguyue or all authors or contributors be liable for any direct,
indirect, incidental, special, exemplary, or consequential damages
(including, but not limited to, procurement of substitute goods or services;
loss of use, data, or profits; or business interruption) however caused
and on any theory of liability, whether in contract, strict liability,
or tort (including negligence or otherwise) arising in any way out of
the use of this software, even if advised of the possibility of such damage.  
*/
/*
@file GUI_TextParagraph.cs
@author NDark 將文字產生為不同的GUIText來顯示
 
# 會依據目前的文字節點來複製其格式
# 目前的預設值是警告頁面的文字，還未分離
# m_ActiveInStart 是否開始時連帶啟動，需先設定好文字
# m_StrArray 文字陣列 
# m_ActiveAnimation 是否要有動畫
# m_AnimationSpeed 動畫的方向速度
# m_ActiveUpperBound 動畫是否有上限的限制
# m_ExceedUpperBound 超過上限的長度
# m_AutoDetectAnimatinMax 是否自動偵測動畫的長度及結束
# m_AnimationMaximum 動畫的長度
# m_ChildList 產生出來的文字物件

@date 20130103 file started.
@date 20130113 by NDark . comment.
@date 20130122 by NDark . add code of GameObject.Destroy() at CreateParagraph()

*/
using UnityEngine;
using System.Collections.Generic;

public class GUI_TextParagraph : MonoBehaviour 
{
	public Color m_TextColor = Color.white ;
	
	public bool m_ActiveAnimation = false ;
	public Vector2 m_AnimationSpeed = Vector2.zero ;
	
	public bool m_ActiveUpperBound = false ;
	public Vector2 m_ExceedUpperBound = Vector2.zero ;
	
	public bool m_AutoDetectAnimatinMax = false ;
	public Vector2 m_AnimationMaximum = Vector2.zero ;
	
	public bool m_ActiveInStart = false ;
	public string [] m_StrArray = { 
		"Schwertkampf Schach(Fencing Chess) is a prototype project " ,
		"made by NDark in Karlsruhe Game Jam 2013." ,
		"Copyright of all source media is belong to" ,
		"learn-sword-fighting.com and www.gladiatores.de ." ,
		"Please notice me if there is any inproper material." ,
	} ;	
	
	private Vector2 m_AnimationSum = Vector2.zero ;
	private bool m_AnimationIsEnd = false ;
	
	public List<GameObject> m_ChildList = new List<GameObject>() ;
	
	public void CreateParagraph() 
	{
		// Debug.Log( "CreateParagraph" ) ;

		GUIText firstGUIText = this.GetComponent<GUIText>();
		if( null == firstGUIText )
			return; 
		firstGUIText.enabled = false ;
		
		foreach( GameObject obj in m_ChildList )
		{
			GameObject.Destroy( obj ) ;
		}
		
		Vector2 tempPos = firstGUIText.pixelOffset ;
		for( int i = 0 ; i < m_StrArray.Length ; ++i )
		{
			// for other paragraph
			GameObject obj = new GameObject( "Paragraph" + i ) ;
			
			GUIText text = obj.AddComponent<GUIText>() ;
			
			text.anchor = firstGUIText.anchor ;
			text.font = firstGUIText.font ;
			text.fontSize = firstGUIText.fontSize ;
			text.fontStyle = firstGUIText.fontStyle ;
			text.material.color = m_TextColor ;
				
			text.text = m_StrArray[ i ] ;
			tempPos.y -= firstGUIText.fontSize ;
			text.pixelOffset = tempPos ;
			if( true == m_AutoDetectAnimatinMax )
			{
				m_AnimationMaximum.x = Mathf.Abs( text.pixelOffset.x ) ;
				m_AnimationMaximum.y = Mathf.Abs( text.pixelOffset.y ) ;			
			}
			obj.transform.parent = this.gameObject.transform ;
			obj.transform.localPosition = Vector3.zero ;
			m_ChildList.Add( obj ) ;
		}
		
	}
	
	void Awake()
	{
		if( true == m_ActiveInStart )
			CreateParagraph() ;
	}
	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( true == m_ActiveAnimation &&
			false == m_AnimationIsEnd )
		{
			UpdateAnimation() ;
		}
	}
	
	private void UpdateAnimation()
	{
		
		Vector2 speedNow = m_AnimationSpeed * Time.deltaTime ;
		foreach( GameObject obj in m_ChildList )
		{
			GUIText guiText = obj.GetComponent<GUIText>() ;
			if( null != guiText )
			{
				Vector2 pos = guiText.pixelOffset ;
				pos += speedNow ;
				guiText.pixelOffset = pos ;
				// Debug.Log( pos ) ;
				if( true == m_ActiveUpperBound )
				{
					if( pos.x >= m_ExceedUpperBound.x &&
						pos.y >= m_ExceedUpperBound.y )
					{
						guiText.enabled = false ;
					}
				}				
			}
		}

		
		m_AnimationSum += speedNow ;
		if( m_AnimationSum.magnitude > this.m_AnimationMaximum.magnitude )
		{
			m_AnimationIsEnd = true ;
		}
	}
	
}
