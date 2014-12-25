﻿using UnityEngine;
using System.Collections;

/* Player.
 * Always put this on the player prefab, but make sure there is only one on the scene at any given time.
 * Not to be put on the clone of player. There's another script for that
 * 
 */

public class PlayerClone : Mobile
{
	void Start()
	{
		renderer.material.color = new Vector4(renderer.material.color.r,
		                                      renderer.material.color.g,
		                                      renderer.material.color.b,
		                                      0.5f);
	}
	
	public override void NormalUpdate()
	{
		Destroy(gameObject);
	}
	
	public override void Record()
	{
	}

	public override void Rewind()
	{
	}
	
	public override void Playback()
	{
		base.Playback();
	}
	
	
	
	
}
