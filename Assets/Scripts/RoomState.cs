using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomState : MonoBehaviour
{
	static public RoomState instance;

	public Vector3 characterSelectionCameraPosition;

	const int handCardSize = 6;

	const int dungeonWidth = 3;
	const int dungeonHeight = 4;

	const float tileHeightOffset = 0.75f;
	const float tileWidthOffset = 1f;

	public enum GamePhase
	{
		CharacterSetup,
		RoomSelection,
		DungeonAction,
		WinGame, 
		EpicFail,
		MenuSelection
	}

	public GameObject[] opponents;

	public GamePhase currentPhase;

	public GameObject dungeonTilePrefab;
	List<DungeonTile> dungeonTiles = new List<DungeonTile>();

	public Camera mainCamera;
	Slider cameraSlider;
	public LayerMask tileClickMask;
	public Vector3 tileFocusOffset;

	void Awake()
	{
		instance = this;
		cameraSlider = mainCamera.GetComponent<Slider>();
	}

	void OnDestroy()
	{
		instance = null;
	}

	void Start()
	{
		currentPhase = GamePhase.CharacterSetup;

		BuildDungeon();
		RandomlyPopulateDungeon();
		PositionCameraForCharacterSelection();

		CardManager.instance.SetGUIActive(false);
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			CheckTileClick();
		}
	}

	void BuildDungeon()
	{
		for (int i = 0; i < dungeonWidth; ++i)
		{
			for (int j = 0; j < dungeonHeight; ++j)
			{
				var tile = (GameObject)Instantiate(dungeonTilePrefab);
				tile.transform.position = new Vector3(i * tileWidthOffset, j * tileHeightOffset, 0);
				tile.transform.localScale = new Vector3(1f, 0.75f, 1f);
				var dungeonTile = tile.GetComponent<DungeonTile>();
				dungeonTile.tileIndex = i * dungeonHeight + j;
				dungeonTile.AttributeSideSelected = false;
				dungeonTile.DungeonSideSelected = false;
				dungeonTile.ConfigureDungeonGraphics();
				dungeonTiles.Add(dungeonTile);
			}
		}
	}

	static public EquipmentSlot GetDungeonEquipmentSlot(int index)
	{
		switch (index)
		{
		case 0:
			return EquipmentSlot.Misc;
		case 1:
			return EquipmentSlot.Misc;
		case 2:
			return EquipmentSlot.Gauntlets;
		case 3:
			return EquipmentSlot.Misc;
		case 4:
			return EquipmentSlot.Shoes;
		case 5:
			return EquipmentSlot.Legs;
		case 6:
			return EquipmentSlot.Torso;
		case 7:
			return EquipmentSlot.Head;
		case 8:
			return EquipmentSlot.Misc;
		case 9:
			return EquipmentSlot.Misc;
		case 10:
			return EquipmentSlot.Gauntlets;
		case 11:
			return EquipmentSlot.Misc;
		default:
			Debug.LogWarning("Bad dungeon slot");
			return EquipmentSlot.Misc;
		}
	}

	static public DoorsAndWalls GetDungeonDoorsAndWalls(int index)
	{
		switch (index)
		{
		case 0:
			return DoorsAndWalls.BottomLeft;
		case 1:
			return DoorsAndWalls.Left;
		case 2:
			return DoorsAndWalls.Left;
		case 3:
			return DoorsAndWalls.TopLeft;
		case 4:
			return DoorsAndWalls.Bottom;
		case 5:
			return DoorsAndWalls.Middle;
		case 6:
			return DoorsAndWalls.Middle;
		case 7:
			return DoorsAndWalls.Top;
		case 8:
			return DoorsAndWalls.BottomRight;
		case 9:
			return DoorsAndWalls.Right;
		case 10:
			return DoorsAndWalls.Right;
		case 11:
			return DoorsAndWalls.TopRight;
		default:
			Debug.LogWarning("Bad dungeon slot");
			return DoorsAndWalls.Middle;
		}
	}

	void RandomlyPopulateDungeon()
	{
		foreach (var tile in dungeonTiles)
		{
			tile.SetEquipmentDefinition(EquipmentManager.instance.PullRandomEquipment(GetDungeonEquipmentSlot(tile.tileIndex)));
		}
	}

	void CheckTileClick()
	{
		var clickRay = mainCamera.ScreenPointToRay(Input.mousePosition);

		RaycastHit hit;
		if (Physics.Raycast(clickRay, out hit, 1000f, tileClickMask))
		{
			var tileOver = hit.collider.gameObject.GetComponent<DungeonTile>();

			if (null != tileOver)
			{
				switch (currentPhase) {
					case GamePhase.CharacterSetup:
						if (!tileOver.dungeonSideDisplayed) {
						bool toggle = tileOver.AttributeSideSelected;
						tileOver.AttributeSideSelected=!toggle;
						}
						break;
					case GamePhase.RoomSelection:
					PositionCameraForCharacterSelection();
					CardManager.instance.SetGUIActive(true);
						tileOver.FlipToDungeon();
						ZoomIntoTile(tileOver);
						currentPhase=GamePhase.DungeonAction;
						break;
					case GamePhase.DungeonAction:
					CardManager.instance.SetGUIActive(false);
						PositionCameraForGamePlay();
						currentPhase=GamePhase.RoomSelection;
						break;
				}

			}
		}
	}


	void PositionCameraForCharacterSelection() {
		cameraSlider.toPosition = characterSelectionCameraPosition;
		cameraSlider.UseFromPosition();
		cameraSlider.StartSlide(); 
	}

	void PositionCameraForGamePlay()
	{
		cameraSlider.toPosition = new Vector3( tileWidthOffset * dungeonWidth / 2f - 0.5f, tileHeightOffset * dungeonHeight / 2f  - 0.5f, -4f);
		cameraSlider.UseFromPosition();
		cameraSlider.StartSlide(); 
	}

	void ZoomIntoTile(DungeonTile tile)
	{
		Vector3 tilePosition = tile.transform.position;
		cameraSlider.toPosition = tilePosition + tileFocusOffset;
		cameraSlider.UseFromPosition();
		cameraSlider.StartSlide();
	}

	public bool DoSelectedCardsDefeatOpponent()
	{
		// TODO: for reals yo
		int cardSelectedCount = 0;

		foreach (var selectedCard in CardManager.instance.GetSelectedCards())
		{
			++cardSelectedCount;
		}

		return cardSelectedCount > 2;
	}

	public void DisplayYouLose()
	{
		// TODO: the card handler has run the deck out of cards
		// The game is over, change the phase and continue
	}
}
