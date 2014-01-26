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
	List<DungeonTile> characterItemSelections = new List<DungeonTile>();

	public Camera mainCamera;
	Slider cameraSlider;
	public LayerMask tileClickMask;
	public Vector3 tileFocusOffset;
	public GameObject characterSelectionMenu;

	DungeonTile currentRoom;

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
//		PositionCameraForCharacterSelection();

//		CardManager.instance.SetGUIActive(false);
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

		SetupDeck();
		ChangePhase(GamePhase.RoomSelection);
	}

	void SetupDeck()
	{
		int fighterCards = 0;
		int mageCards = 0;
		int thiefCards = 0;
		
		foreach (var tile in dungeonTiles)
		{
			var equipment = tile.GetEquipment();
			fighterCards += equipment.fighterCardsGenerated;
			mageCards += equipment.mageCardsGenerated;
			thiefCards += equipment.thiefCardsGenerated;
		}

		CardManager.instance.CreateDeck(fighterCards, mageCards, thiefCards);
	}

	public void SetClearColor(Color clearColor)
	{
		mainCamera.backgroundColor = clearColor;
	}

	void ChangePhase(GamePhase newPhase)
	{
		switch (newPhase)
		{
		case GamePhase.RoomSelection:
			CardManager.instance.SetGUIActive(false);
			PositionCameraForGamePlay();
			break;
		case GamePhase.DungeonAction:
			CardManager.instance.SetGUIActive(true);
			break;
		default:
			// whatever
			break;
		}
		currentPhase = newPhase;
	}

	public void OpponentDefeated()
	{
		if (null != currentRoom)
		{
			currentRoom.opponentType = OpponentType.None;
			currentRoom.SetOpponent(null);
			currentRoom = null;
		}

		// Check for win condition
		bool wonGame = true;
		foreach (var tile in dungeonTiles)
		{
			if (tile.opponentType != OpponentType.None)
			{
				wonGame = false;
				break;
			}
		}
		if (wonGame)
		{
			DisplayYouWin();
		}
		else
		{
			ChangePhase(GamePhase.RoomSelection);
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
						if (tileOver.opponentType != OpponentType.None)
						{
							currentRoom = tileOver;
							currentRoom.FlipToDungeon();
							ZoomIntoTile(currentRoom);
							var opponent = OpponentManager.instance.PullRandomOpponent(currentRoom.opponentType);
							currentRoom.SetOpponent(opponent);
							CardManager.instance.opponentCard.ShowOpponent(opponent);
							ChangePhase(GamePhase.DungeonAction);
						}
						break;
					case GamePhase.DungeonAction:
						break;
				}

			}
			var menuButton = hit.collider.gameObject.GetComponent<CreationMenu>();
			if (menuButton != null) {
				FlipMenu(menuButton.equpmentSlot);
			}

		}


	}

	void FlipMenu (EquipmentSlot equipmentSlot)
	{
		characterSelectionMenu.animation.Play ("CardFlipDungeon");
		int index = 0;
		foreach (EquipmentDefinition equip in EquipmentManager.instance.GetEquipmentList (equipmentSlot) ) {
			var tile = (GameObject)Instantiate(dungeonTilePrefab);
			tile.transform.position = new Vector3(4.3f + index%3 * 0.75f, 3.2f - index/3 * 0.5f, 0);
			tile.transform.localScale = new Vector3(0.75f, 0.5f, 1f);
			var dungeonTile = tile.GetComponent<DungeonTile>();
			dungeonTile.tileIndex = index;
			dungeonTile.AttributeSideSelected=false;
			dungeonTile.DungeonSideSelected=false;
			dungeonTile.SetEquipmentDefinition(equip);
			characterItemSelections.Add(dungeonTile);
			index++;
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
		if (null == currentRoom)
		{
			return false;
		}

		var opponent = currentRoom.GetOpponent();

		int fightersNeeded = opponent.fightersNeeded;
		int magesNeeded = opponent.magesNeeded;
		int thievesNeeded = opponent.thievesNeeded;

		int overflowCardCount = 0;

		foreach (var selectedCard in CardManager.instance.GetSelectedCards())
		{
			switch (selectedCard)
			{
			case CardIcons.Fighter:
				if (fightersNeeded > 0)
				{
					--fightersNeeded;
				}
				else
				{
					++overflowCardCount;
				}
				break;
			case CardIcons.Mage:
				if (magesNeeded > 0)
				{
					--magesNeeded;
				}
				else
				{
					++overflowCardCount;
				}
				break;
			case CardIcons.Thief:
				if (thievesNeeded > 0)
				{
					--thievesNeeded;
				}
				else
				{
					++overflowCardCount;
				}
				break;
			}
		}

		return (overflowCardCount / 2) >= (fightersNeeded + magesNeeded + thievesNeeded);
	}

	public void DisplayYouLose()
	{
		// TODO: the card handler has run the deck out of cards
		// The game is over, change the phase and continue
		ChangePhase(GamePhase.EpicFail);
	}

	public void DisplayYouWin()
	{
		// TODO: do this
		ChangePhase(GamePhase.WinGame);
	}
}
