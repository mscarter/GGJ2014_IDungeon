using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomState : MonoBehaviour
{
	const int handCardSize = 6;

	const int dungeonWidth = 3;
	const int dungeonHeight = 4;

	const float tileOffset = 1f;

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
	public GameObject cardHandDisabler;
	public List<CardType> handOfCards;
	public GameObject[] cardObjects;

	public GameObject dungeonTilePrefab;
	List<DungeonTile> dungeonTiles = new List<DungeonTile>();

	public Camera mainCamera;
	Slider cameraSlider;
	public LayerMask tileClickMask;
	public Vector3 tileFocusOffset;

	void Awake()
	{
		cameraSlider = mainCamera.GetComponent<Slider>();
	}

	void Start()
	{
		currentPhase = GamePhase.CharacterSetup;
		currentOpponent = (OpponentType)Random.Range(0, (int)OpponentType.MAX);
//		SetOpponentGraphic();

		for (int i = 0; i < handCardSize; ++i)
		{
			handOfCards.Add((CardType)Random.Range(0, (int)CardType.MAX));
		}
		SetCardVisualizations();

		BuildDungeon();
		PositionCamera();
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

	void SetCardVisualizations()
	{
		for (int i = 0; i < cardObjects.Length; ++i)
		{
			if (i >= handOfCards.Count)
			{
				cardObjects[i].SetActive(false);
			}
			else
			{
				cardObjects[i].SetActive(true);
				cardObjects[i].renderer.material = cardMaterials[(int)handOfCards[i]];
			}
		}
	}

	void BuildDungeon()
	{
		for (int i = 0; i < dungeonWidth; ++i)
		{
			for (int j = 0; j < dungeonHeight; ++j)
			{
				var tile = (GameObject)Instantiate(dungeonTilePrefab);
				tile.transform.position = new Vector3(i * tileOffset, j * tileOffset, 0);
				var dungeonTile = tile.GetComponent<DungeonTile>();
				dungeonTile.tileIndex = i * dungeonWidth + dungeonHeight;
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
							print("Hello");
						}
						break;
					case GamePhase.RoomSelection:
						cardHandDisabler.SetActive(true);
						tileOver.FlipToDungeon();
						ZoomIntoTile(tileOver);
						currentPhase=GamePhase.DungeonAction;
						break;
					case GamePhase.DungeonAction:
						cardHandDisabler.SetActive (false);
						PositionCamera();
						currentPhase=GamePhase.RoomSelection;
						break;
				}

			}
		}
	}

	void PositionCamera()
	{
		cameraSlider.toPosition = new Vector3( tileOffset * dungeonWidth / 2f - 0.5f, tileOffset * dungeonHeight / 2f  - 0.5f, -4f);
		cameraSlider.StartSlide(); 
	}

	void ZoomIntoTile(DungeonTile tile)
	{
		Vector3 tilePosition = tile.transform.position;
		cameraSlider.toPosition = tilePosition + tileFocusOffset;
		cameraSlider.StartSlide();
	}
}
