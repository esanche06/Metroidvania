﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ResourceDirectory
{
	public static Dictionary<System.Type, ResourceData> resource = new Dictionary<System.Type, ResourceData>()
	{
		{typeof(Player), new ResourceData("Prefabs/Mobiles/Player/Player", 0)},
		{typeof(Lever), new ResourceData("Prefabs/Immobiles/Lever/Lever", 1)},
		{typeof(UpdraftGoo), new ResourceData("Prefabs/Mobiles/AI/UpdraftGoo/UpdraftGoo", 2)},
		{typeof(DependantPlatform), new ResourceData("Prefabs/Immobiles/DependantPlatform/DependantPlatform", 3)},
		{typeof(Button), new ResourceData("Prefabs/Immobiles/Button/Button", 4)}

	};

}

public struct ResourceData
{
	public string path;
	public int index;
	public ResourceData(string p, int i)
	{
		path = p;
		index = i;
	}
}
