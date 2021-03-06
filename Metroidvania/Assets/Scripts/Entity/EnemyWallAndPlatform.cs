﻿using UnityEngine;
using System.Collections;

/*
 * This script is for enemy walls and platforms.
 * 
 * Green 
 * - active in normal mode
 * - inactive in recording mode
 * 
 * Blue 
 * - active in recording mode
 * - inactive in normal mode
 * 
 * 
 */

public class EnemyWallAndPlatform : Recordable 
{
	public enum GooColor{Blue, Green}; //Review enum
	public GooColor color;

	public override void NormalUpdate() //Review override, NormalUpdate during normal time
	{
		if (color == GooColor.Blue) 
		{
			transform.localScale = new Vector3 (1, 1, 1);
		}

		if (color == GooColor.Green) 
		{
			transform.localScale = new Vector3 (.5f, 3, 1);
		}
	}

	public override void Record() //Record during Time Shift
	{
		if (color == GooColor.Blue) 
		{
			transform.localScale = new Vector3(.5f, 3, 1);
		}
		
		if (color == GooColor.Green) 
		{
			transform.localScale = new Vector3(1, 1, 1);
		}
	}
}
