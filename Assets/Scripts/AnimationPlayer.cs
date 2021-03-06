/*
@file AnimationPlayer.cs
@author NDark
@date 20131105 file started.
*/
using UnityEngine;
using System.Collections;

public class AnimationPlayer : MonoBehaviour 
{
	public float m_AnimationLastTime = 0 ;
	public float m_AnimationSpeedSec = 1 ;
	public string m_CurrentAnimationTag = "" ;
	public bool m_IsActive = false ;
	public int m_AnimationIndex = 0 ;
	public string m_GUIAnimationObjectName = "GUI_AnimationObject" ;
	public GUITexture m_GUIAnimationTexture = null ;
	LevelGenerator m_LevelGeneratorPtr = null ;
	public float m_PictureScale = 0.5625f ;
	
	public void Setup( string _AnimationTag )
	{
		Debug.Log( "AnimationPlayer::Setup() _AnimationTag=" + _AnimationTag ) ;
		m_IsActive = true ;
		m_AnimationLastTime = Time.timeSinceLevelLoad ;
		m_CurrentAnimationTag = _AnimationTag ;
		m_AnimationIndex = 0 ;
		SwitchTexture() ;
	}
	
	public void Stop()
	{
		m_IsActive = false ; 
	}
	
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

			m_GUIAnimationTexture.pixelInset = new Rect( 0 , (Camera.main.pixelHeight - Camera.main.pixelWidth * m_PictureScale) / 2 , Camera.main.pixelWidth , Camera.main.pixelWidth * m_PictureScale ) ;
		}
		
		if( null == m_LevelGeneratorPtr )
		{
			GameObject obj = GameObject.Find( "GlobalSingleton" ) ;
			m_LevelGeneratorPtr = obj.GetComponent<LevelGenerator>() ;
		}
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( false == m_IsActive )
			return ;
		
		if( null == m_LevelGeneratorPtr )
		{
			return ;
		}
		if( false == m_LevelGeneratorPtr.m_AnimationSequenceData.ContainsKey( m_CurrentAnimationTag ) )
		{
			return ;
		}
		
		if( Time.timeSinceLevelLoad - m_AnimationLastTime > m_AnimationSpeedSec )
		{
			AnimationSequenceStruct animSeq = 
				m_LevelGeneratorPtr.m_AnimationSequenceData[ m_CurrentAnimationTag ] ;
			++m_AnimationIndex ;
			m_AnimationLastTime = Time.timeSinceLevelLoad ;
			if( m_AnimationIndex >= animSeq.m_ImageFilepath.Count )
			{
				Debug.Log( "AnimationPlayer::Update() m_AnimationIndex >= animSeq.m_ImageFilepath.Count" ) ;
				m_IsActive = false ;
			}
			else
			{
				SwitchTexture() ;
			}
		}
	}
	
	private void SwitchTexture()
	{
		if( null == m_LevelGeneratorPtr )
		{
			Debug.LogError( "AnimationPlayer::SwitchTexture() null == m_LevelGeneratorPtr" ) ;
			return ;
		}
		
		if( false == m_LevelGeneratorPtr.m_AnimationSequenceData.ContainsKey( m_CurrentAnimationTag ) )
		{
			Debug.LogError( "AnimationPlayer::SwitchTexture() false == m_LevelGeneratorPtr.m_AnimationSequenceData.ContainsKey=" + m_CurrentAnimationTag ) ;
			return ;
		}
		
		if( null != m_GUIAnimationTexture )
		{
			AnimationSequenceStruct animSeq = 
				m_LevelGeneratorPtr.m_AnimationSequenceData[ m_CurrentAnimationTag ] ;
			string imagepath = animSeq.m_ImageFilepath[ m_AnimationIndex ] ;
			
			Texture2D tex2D = (Texture2D)Resources.Load( "Texture/" + imagepath ) ;
			// Debug.Log( "AnimationPlayer::SwitchTexture() imagepath=" + m_AnimationIndex + " " + imagepath ) ;
			if( null == tex2D )
			{
				Debug.Log( "AnimationPlayer::SwitchTexture() null == tex2D" ) ;
				return ;
			}
			
			m_GUIAnimationTexture.texture = tex2D ;
			
		}
		
	}
}
