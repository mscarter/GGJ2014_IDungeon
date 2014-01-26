using UnityEngine;
using System.Collections;

public enum DoorsAndWalls
{
	TopLeft,
	Top,
	TopRight,
	Left,
	Middle,
	Right,
	BottomLeft,
	Bottom,
	BottomRight
}

public class DungeonManager : MonoBehaviour
{
	static public DungeonManager instance;

	DungeonTileSet[] tileSets;
	int selectedTileSet;

	void Awake()
	{
		instance = this;

		tileSets = GetComponentsInChildren<DungeonTileSet>();
		selectedTileSet = Random.Range(0, tileSets.Length);
	}
	
	void OnDestroy()
	{
		instance = null;
	}

	void Start()
	{
		RoomState.instance.SetClearColor(tileSets[selectedTileSet].clearColor);
	}

	public Material GetBackground()
	{
		return tileSets[selectedTileSet].backgroundImages[Random.Range(0,tileSets[selectedTileSet].backgroundImages.Length)];
	}

	public Material GetDoorsAndWalls(DoorsAndWalls doorsAndWalls)
	{
		return tileSets[selectedTileSet].doorsAndWalls[(int)doorsAndWalls];
	}

	public void PlayBackgroundMusic()
	{
		tileSets[selectedTileSet].audio.Play();
	}
}
