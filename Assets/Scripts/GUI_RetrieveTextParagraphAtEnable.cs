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
@file GUI_RetrieveTextParagraphAtEnable.cs
@author NDark 
 
顯示時取得整段文字

# GUI_RetrieveTextAtEnable 的陣列版，索引值必須指定為陣列形式
# 會依據目前取得的GUIText來複製其文字格式

@date 20130122 file started.
@date 20130122 by NDark
. add class method IsEnable()
. add code of RetrieveText() at Start()
*/
using UnityEngine;

public class GUI_RetrieveTextParagraphAtEnable : MonoBehaviour 
{
	public Color m_TextColor = Color.black;
	
	private bool m_Show = false ;
	private GUIText []m_GUITexts = null ;
	private GUI_TextParagraph m_TextParagraph = null ;
	
	public int [] m_TextIndice ;
	public void RetrieveText()
	{
		if( null == m_TextParagraph )
			return ;
		
		if( m_TextParagraph.m_StrArray.Length != m_TextIndice.Length )
		{
			m_TextParagraph.m_StrArray = new string[ m_TextIndice.Length ] ;
		}
		
		for( int i = 0 ; i < m_TextIndice.Length && 
						 i < m_TextParagraph.m_StrArray.Length ; ++i )
		{
			m_TextParagraph.m_StrArray[ i ] = StrsManager.Get( m_TextIndice[ i ] ) ;
		}
		m_TextParagraph.m_TextColor = m_TextColor ;
		m_TextParagraph.CreateParagraph() ;
	}	
	
	// Use this for initialization
	void Start () 
	{
		
		m_TextParagraph = this.gameObject.GetComponentInChildren<GUI_TextParagraph>() ;
		RetrieveText() ;
		StrsManager.Register( this.gameObject ) ;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( null == m_TextParagraph )
			return ;
		
		if( false == m_Show )
		{
			if( true == IsEnable() )
			{
				RetrieveText() ;
				m_Show = true ;
			}
		}
		else
		{
			if( false == IsEnable() )
			{
				m_Show = false ;
			}
		}			
	}
	
	private bool IsEnable()
	{
		m_GUITexts = this.gameObject.GetComponentsInChildren<GUIText>() ;
		bool ret = false ;
		for( int i = 0 ; i < m_GUITexts.Length ; ++i )
		{
			// Debug.Log( "m_GUITexts[ i ].text=" + m_GUITexts[ i ].text ) ;
			if( m_GUITexts[ i ].text != "Test" )
			{
				ret = m_GUITexts[ i ].enabled ;
				break ;
			}
		}
		// Debug.Log( "IsEnable=" + ret ) ;
		return ret ;
	}
}
