﻿using UnityEngine;
using System.Collections;

/* DependantPlatform.
 * Give this DependantPlatform a lever whose event_state will affect this platform's behavior
 * 
 * 
 * Here are the behaviors based on given int:
 * 0	Collisions off when lever's event_state is 0
 * 		Collisions on when lever's event_state is 1
 * 1	Collisions on when lever's event_state is 1
 * 		Collisions off when lever's event_state is 0
 * 
 */

public class DependantPlatform : Immobile
{
	public Recordable dependant_on; // Change this to a lever type when we have a lever class
	public int behavior;


	void changeCollisions(bool on)
	{
		transform.collider2D.enabled = on;
	}



	//--------------------------- Behaviors ---------------------------
	void behavior0()
	{
		if (dependant_on.this_info.eventState == 0) {changeCollisions(false);}
		else if (dependant_on.this_info.eventState == 1) {changeCollisions(true);}
	}

	void behavior1()
	{
		if (dependant_on.this_info.eventState == 0) {changeCollisions(true);}
		else if (dependant_on.this_info.eventState == 1) {changeCollisions(false);}
	}



	//--------------------------- Overrides ---------------------------
	public override void NormalUpdate()
	{
		base.NormalUpdate();
		/*
		if (behavior == 0) {behavior0();}
		else if (behavior == 1){behavior1();}
		*/
	}
	
	public override void Record()
	{
		base.Record();
	}
	
	public override void RecordAct()
	{
		base.RecordAct();
	}
	
	public override void Rewind()
	{
		base.Rewind();
	}
	
	public override void Playback()
	{
		base.Playback();
	}
}