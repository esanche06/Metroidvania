﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using System.Threading;

/* TileEditor
 * 
 * IMPORTANT! Do not put this on any object. It will place itself if the scene name is "Tile Editor"
 * 
 */
public class TileEditor : MonoBehaviour
{
	private string map_name = ""; // name of map
	private GameObject spawn_point_indicator;
	private GameObject tile_indicator;
	
	private string[] paint_mode = new string[]{"Tile", "Spawn"};
	private int paint_mode_index = 0;
	private bool show_tab = true;
	private Rect window_rect = new Rect(0,0,Screen.width,Screen.height); // for displaying GUI
	private float camera_speed;
	private int FPS;


	void Start()
	{
		ReformatGame();
		spawn_point_indicator = (GameObject)Instantiate(Resources.Load("Prefabs/TileEditor/SpawnPointIndicator", typeof(GameObject)), new Vector3(0,0,0), transform.rotation);
		spawn_point_indicator.gameObject.renderer.material.color = new Color(1,1,1,0);
		tile_indicator = (GameObject)Instantiate(Resources.Load("Prefabs/TileEditor/TileIndicator", typeof(GameObject)), new Vector3(0,0,0), transform.rotation);
		tile_indicator.gameObject.renderer.material.color = new Color(1,1,1,0.1f);
		StartCoroutine(UpdateFPS());
	}

	void OnGUI()
	{
		if (show_tab)
		{
			window_rect = GUI.Window(0, window_rect, TabWindowFunction, "Tile Editor");
		}
		else
		{
			int mouseX = (int)(Camera.main.ScreenToWorldPoint(Input.mousePosition).x);
			int mouseY = (int)(Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
			GUI.Label(new Rect(Input.mousePosition.x + 10,Screen.height - Input.mousePosition.y + 10,100, 20), "(" + mouseX + ", " + mouseY + ")");
		}
		if (!GetComponent<TileManager>().read_done)
		{
			window_rect = GUI.Window(0, window_rect, IntroWindowFunction, "Tile Editor");
		}
		GUI.Label(new Rect(Screen.width - 120, 20, Screen.width, 20), "Tiles in view: " + (RenderingSystem.unit_shown.y - RenderingSystem.unit_shown.x) * (RenderingSystem.unit_shown.w - RenderingSystem.unit_shown.z));
		GUI.Label(new Rect(Screen.width - 70, 40, Screen.width, 20), "FPS: " + FPS);
	}


	private IEnumerator UpdateFPS()
	{
		while (true)
		{
			FPS = (int)(1 / Time.deltaTime);
			yield return new WaitForSeconds(0.5f);
		}
	}
	
	/// <summary>
	/// Window shown in the beginning of Tile Editor. Define the map name, and whether to load or create a new Map.
	/// </summary>
	void IntroWindowFunction(int windowID)
	{
		GUI.Label(new Rect(40,20,window_rect.width, 40), "Define a map name to load:");
		map_name = GUI.TextField(new Rect(10,40,Screen.width/2,20), map_name);
		
		if (GUI.Button(new Rect(10,60,100,40), "New"))
		{
			GameManager.current_game.progression.maps.Add("TileEditorMap", new Map());
			GetComponent<TileManager>().LoadAll();
			GetComponent<RenderingSystem>().LoadedDone();
		}
		if (GUI.Button(new Rect(Screen.width/2 - 90,60,100,40), "Load"))
		{
			try
			{
				GameManager.current_game.progression.maps.Add("TileEditorMap", new Map(map_name));
			}
			catch
			{
				Debug.LogWarning("Could not load file: " + map_name);
				throw;
			}

			GetComponent<TileManager>().LoadAll((Map)(GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map])); //TODO
			GetComponent<RenderingSystem>().LoadedDone();

			spawn_point_indicator.transform.position = new Vector2(((Map)(GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map])).spawn_point.x,
			                                                       ((Map)(GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map])).spawn_point.y);
		}
	}



	/// <summary>
	/// Window shown when Tab pressed when editing tile.
	/// </summary>
	void TabWindowFunction(int windowID)
	{
		// Left side
		GUI.Label(new Rect(5,20,window_rect.width, 40), "Loaded Map:\n   <" + map_name + ".md>");
		if (GUI.Button(new Rect(5,60,100,40), "Save")) // Save Map object into file
		{
			if (Directory.Exists(Application.dataPath + "/Maps"))
			{
				Save();
			}
			else
			{
				Debug.LogError("There is no path (" + Application.dataPath + "/Maps), so data will not be saved");
			}
		}
		if (GUI.Button(new Rect(5,100,100,40), "Save and Test"))
		{
			if (Directory.Exists(Application.dataPath + "/Maps"))
			{
				Save();
				PlayerPrefs.SetString("Map Name", map_name);
				Application.LoadLevel("LevelTester");
			}
			else
			{
				Debug.LogError("There is no path (" + Application.dataPath + "/Maps), so data will not be saved");
			}
		}
		if (GUI.Button(new Rect(20,Screen.height-25,100,20), "Tile"))
		{
			paint_mode_index = 0;
		}
		if (GUI.Button(new Rect(20,Screen.height-45,100,20), "Spawn"))
		{
			paint_mode_index = 1;
		}
		if (GUI.Button(new Rect(20,Screen.height-65,100,20), "n/a"))
		{
		}
		if (GUI.Button(new Rect(20,Screen.height-85,100,20), "n/a"))
		{
		}
		if (GUI.Button(new Rect(20,Screen.height-105,100,20), "n/a"))
		{
		}
		GUI.Label(new Rect(10,Screen.height-25-20*paint_mode_index,10, 20), "x");


		// Right side
		GUI.Label(new Rect(window_rect.width/2, 60, window_rect.width/2-5, 20), "<TAB> - show/hide instructions");
		GUI.Label(new Rect(window_rect.width/2, 80, window_rect.width/2-5, 20), "<Arrow Keys> - move camera around");
		GUI.Label(new Rect(window_rect.width/2, 100, window_rect.width/2-5, 20), "<Mouse Scroll> - Zoom in/out");
		GUI.Label(new Rect(window_rect.width/2, 120, window_rect.width/2-5, 20), "<Q> to paste at mouse position");
		GUI.Label(new Rect(window_rect.width/2, 140, window_rect.width/2-5, 20), "<W> to disable at mouse position");
	}


	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Tab) && GetComponent<TileManager>().read_done)
		{
			show_tab = !show_tab;
		}
		if (Input.GetKey(KeyCode.RightArrow))
		{
			transform.position += new Vector3(camera_speed,0,0);
		}
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			transform.position += new Vector3(-camera_speed,0,0);
		}
		if (Input.GetKey(KeyCode.UpArrow))
		{
			transform.position += new Vector3(0,camera_speed,0);
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			transform.position += new Vector3(0,-camera_speed,0);
		}
		manageZoom();
		managePaint();
	}

	void manageZoom()
	{
		if (Input.GetAxis("Mouse ScrollWheel") <= 0 || Time.deltaTime < 0.15f)
		{
			GetComponent<Camera>().orthographicSize += Input.GetAxis("Mouse ScrollWheel");
		}
		else
		{
			Debug.LogWarning("Restricted from zooming out due to lag! Frame rate: " + 1/Time.deltaTime);
		}
		if (GetComponent<Camera>().orthographicSize < 0)
		{
			GetComponent<Camera>().orthographicSize = 0.01f;
		}
		camera_speed = GetComponent<Camera>().orthographicSize/30;
	}


	void managePaint()
	{
		if (GetComponent<TileManager>().read_done)
		{
			int mouseX = (int)(Camera.main.ScreenToWorldPoint(Input.mousePosition).x);
			int mouseY = (int)(Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
			tile_indicator.transform.position = new Vector3(mouseX, mouseY, -9);

			if (Input.GetKey(KeyCode.Q))
			{
				if (paint_mode[paint_mode_index] == "Tile")
				{
					GetComponent<TileManager>().UpdateTiles(mouseY, mouseX, true);
				}
				if (paint_mode[paint_mode_index] == "Spawn")
				{
					spawn_point_indicator.transform.position = new Vector2(mouseX,mouseY);
					((Map)(GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map])).spawn_point = new Vector2(mouseX, mouseY);
					spawn_point_indicator.renderer.material.color = new Color(1,1,1,0.5f);
				}
			}
			else if (Input.GetKey(KeyCode.W))
			{
				if (paint_mode[paint_mode_index] == "Tile")
				{
					GetComponent<TileManager>().UpdateTiles(mouseY, mouseX, false);
				}
				if (paint_mode[paint_mode_index] == "Spawn")
				{
					spawn_point_indicator.transform.position = new Vector2(mouseX,mouseY);
					((Map)(GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map])).spawn_point = new Vector2(mouseX, mouseY);
					spawn_point_indicator.renderer.material.color = new Color(1,1,1,0.5f);
				}
			}
			else
			{
				spawn_point_indicator.renderer.material.color = new Color(1,1,1,1);
			}
		}
	}


	/// <summary>
	/// When TileEditor scene is being run, game needs to be reformatted to allow new editing capabilities
	/// </summary>
	private void ReformatGame()
	{
		Debug.Log("TileEditor Scene detected. Reformatting current_game session...");
		GameManager.current_game = new GameManager();
		GameManager.current_game.progression.loaded_map = "TileEditorMap";
		Debug.Log("Done");
	}


	public void SaveScene()
	{
		print(Application.dataPath + "/Scenes/TileEditorScenes/" + map_name + ".unity");
	}


	/// <summary>
	/// Called when "Save Map" button is pressed.
	/// </summary>
	private void Save()
	{
		// Serialize Map object into a file
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.dataPath + "/Maps/" + map_name + ".md");
		bf.Serialize(file, GameManager.current_game.progression.maps[GameManager.current_game.progression.loaded_map]);
		Debug.Log("Saved: " + Application.dataPath + "/Maps/" + map_name + ".md");
		file.Close();
	}
}