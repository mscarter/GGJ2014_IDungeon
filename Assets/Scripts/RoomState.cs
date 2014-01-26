using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomState : MonoBehaviour
{
	static public RoomState instance;

	const int handCardSize = 6;

	const int dungeonWidth = 3;
	const int dungeonHeight = 4;

	const float tileOffset = 1f;

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

	public OpponentType currentOpponent;

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
		currentOpponent = (OpponentType)Random.Range(0, (int)OpponentType.MAX);
//		SetOpponentGraphic();

		BuildDungeon();
		PositionCamera();

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
				if (tileOver.dungeonSideDisplayed)
				{
					return;
//					cardHandDisabler.SetActive(false);
//					tileOver.FlipToAttribute();
//					PositionCamera();
				}
				else
				{
					CardManager.instance.SetGUIActive(true);
					tileOver.FlipToDungeon();
					ZoomIntoTile(tileOver);
				}
			}
		}
	}

	void PositionCamera()
	{
		cameraSlider.toPosition = new Vector3( tileOffset * dungeonWidth / 2f - 0.5f, tileOffset * dungeonHeight / 2f  - 0.5f, -4f);
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
