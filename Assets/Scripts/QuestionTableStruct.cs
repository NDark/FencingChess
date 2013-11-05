/*
@file QuestionPlayer.cs
@author NDark
@date 20131105 file started.
*/
using UnityEngine;
using System.Collections.Generic;
using System.Xml;

public class DetectionPose
{
	public string m_AnimationString = "" ;
	public Vector2 m_Start = Vector2.zero ;
	public Vector2 m_End = Vector2.zero ;
}

public class QuestionTableStruct 
{
	public string m_QuestionAnimationString = "" ;
	public Dictionary< string , DetectionPose > m_DetectionZoness = new Dictionary<string, DetectionPose>() ;
	
	public bool ParseXML( XmlNode _Node )
	{
		if( null != _Node.Attributes[ "QuestionAnimation" ] )
		{
			m_QuestionAnimationString = _Node.Attributes[ "QuestionAnimation" ].Value ;
		}
		
		for( int i = 0 ; i < _Node.ChildNodes.Count ; ++i )
		{
			XmlNode detectionNode = _Node.ChildNodes[ i ] ;
			if( "DetectionPose" == detectionNode.Name )
			{
				DetectionPose newPose = new DetectionPose() ;
				if( null != detectionNode.Attributes[ "AnimationString" ] )
				{
					newPose.m_AnimationString = detectionNode.Attributes[ "AnimationString" ].Value ;
				}
				
				if( null != detectionNode.Attributes[ "StartPosX" ] &&
					null != detectionNode.Attributes[ "StartPosY" ] )
				{
					string startX = detectionNode.Attributes[ "StartPosX" ].Value ;
					string startY = detectionNode.Attributes[ "StartPosY" ].Value ;
					float x = 0 ;
					float y = 0 ; 
					float.TryParse( startX , out x ) ;
					float.TryParse( startY , out y ) ;
					newPose.m_Start = new Vector2( x , y ) ;
				}	
				
				if( null != detectionNode.Attributes[ "EndPosX" ] &&
					null != detectionNode.Attributes[ "EndPosY" ] )
				{
					string startX = detectionNode.Attributes[ "EndPosX" ].Value ;
					string startY = detectionNode.Attributes[ "EndPosY" ].Value ;
					float x = 0 ;
					float y = 0 ; 
					float.TryParse( startX , out x ) ;
					float.TryParse( startY , out y ) ;
					newPose.m_End = new Vector2( x , y ) ;
				}		
			}
		}
		return true ;
	}
}
