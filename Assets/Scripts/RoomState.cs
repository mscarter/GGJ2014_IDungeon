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

	public enum OpponentType
	{
		Mook,
		Monster,
		Trap,
		MAX
	}

	public GameObject[] opponents;

	public enum CardType
	{
		Fighter,
		Mage,
		Thief,
		MAX
	}

	public Material[] cardMaterials;

	public GamePhase currentPhase;
	public OpponentType currentOpponent;

	public GameObject dungeonTilePrefab;
	List<DungeonTile> dungeonTiles = new List<DungeonTile>();
	List<DungeonTile> characterItemSelections = new List<DungeonTile>();

	public Camera mainCamera;
	Slider cameraSlider;
	public LayerMask tileClickMask;
	public Vector3 tileFocusOffset;
	public GameObject characterSelectionMenu;
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
		currentOpponent = (OpponentType)Random.Range(0, (int)OpponentType.MAX);
//		SetOpponentGraphic();

		BuildDungeon();
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

	void SetOpponentGraphic()
	{
		opponents[0].SetActive(currentOpponent == OpponentType.Mook);
		opponents[1].SetActive(currentOpponent == OpponentType.Monster);
		opponents[2].SetActive(currentOpponent == OpponentType.Trap);
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
				dungeonTile.tileIndex = j * dungeonWidth + i;
				dungeonTile.AttributeSideSelected=false;
				dungeonTile.DungeonSideSelected=false;
				dungeonTiles.Add(dungeonTile);
			}
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
