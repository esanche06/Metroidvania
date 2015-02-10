﻿using UnityEngine;
using System;
using System.Collections;

public class Entity : MonoBehaviour
{
	public enum GooColor{Blue, Red, Purple};

	public int WhatIndexAmI()
	{
		return GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].entities[3].IndexOf(this);
	}
}
