﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Button : Immobile
{
	// Animating
	public Sprite[] still_sprites;
	public Sprite[] move_sprites;


	public List<int> platforms = new List<int>();
	public List<Collider2D> current_collisions = new List<Collider2D>();

	private bool pressed;
	private bool bycollider;


	void Start()
	{
		ChangeLoop (move_sprites);
	}
	public override void Action()
	{
		base.Action ();
		if (!bycollider)
						return;
		if (this_info.eventState == 0)
		{
			
			this_info.eventState = 1;
			GetComponent<SpriteRenderer>().sprite = still_sprites[1];
			for (int i = 0; i < platforms.Count; i++)
			{
				((DependantPlatform)GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].entities[3][platforms[i]]).changeCollisions();
			}
		}
		else if (this_info.eventState == 1)
		{
			
			this_info.eventState = 0;
			GetComponent<SpriteRenderer>().sprite = still_sprites[0];
			for (int i = 0; i < platforms.Count; i++)
			{
				((DependantPlatform)GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].entities[3][platforms[i]]).changeCollisions();
			}
		}	
		
	}
	public override void UndoAction()
	{
		Action ();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (!current_collisions.Contains(other) && other.GetComponent<Mobile>() != null)
		{
			current_collisions.Add(other);
		}

		if (!pressed && current_collisions.Contains(other))
		{
			bycollider = true;
			Action ();
			bycollider = false;
	    	pressed = true;
		}
	}
	void OnTriggerExit2D(Collider2D other)
	{
		if (current_collisions.Contains(other))
		{
			current_collisions.Remove(other);
		}
	
		if (pressed && current_collisions.Count == 0)
		{
			bycollider = true;
			Action ();
			bycollider = false;
			pressed = false;
		}
	}


	public override void NormalUpdate()
	{
		//base.NormalUpdate();
		previous_operation_mode = operation_mode;
	}
	
	public override void Record()
	{
		base.Record ();
		//base.Record();
	}
	
	public override void Rewind()
	{
		bycollider = true;
		base.Rewind();
		bycollider = false;
		//this_info.eventState = recorded_states[record_index].eventState;
	}
	
	public override void Playback()
	{
		bycollider = true;
		base.Playback ();
		bycollider = false;
		//this_info.eventState = recorded_states[record_index].eventState;
	}
	
	
	void OnDrawGizmosSelected()
	{
		try
		{
			for (int i = 0; i < platforms.Count; i++)
			{
				Gizmos.DrawLine(transform.position, ((DependantPlatform)GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map].entities[3][platforms[i]]).transform.position);
			}
		}
		catch
		{}
	}

}
