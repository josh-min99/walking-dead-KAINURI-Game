using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using Unity.Networking.Transport;
using System.Collections;

public enum UsingType
{
    None = 0,
    leveling = 1,
    armoring = 2,
    bigbombing = 3,
    changing = 4,
    illusioning = 5,
    reversing = 6,
    fitCar = 7,
    teleporting = 8,
    mining = 9,
    deleteItem = 10,
    deletePiece = 11,
    useDrone = 12
}

public class ItemInstance
{
    public string effectName;
    public int turnsLeft;
    public int value;
    public Piece piece;

    public ItemInstance(string name, int turns, int val, Piece pc = null)
    {
        effectName = name;
        turnsLeft = turns;
        value = val;
        piece = pc;
    }
}

public class PieceDTO {
    public int tileNum;
    public int level;
    public bool armor;
    public bool reverse;
    public bool aidKit;
    public bool raibo;
    public bool advantage;
    public bool team;
    public bool hiding;
    public bool isIllusion;
    public PieceDTO(int tn, int lv, bool ar, bool re, bool aid, bool rai, bool ad, bool tm, bool hd, bool illu)
    {
        tileNum = tn;
        level = lv;
        armor = ar;
        reverse = re;
        aidKit = aid;
        raibo = rai;
        advantage = ad;
        team = tm;
        hiding = hd;
        isIllusion = illu;
    }
}


public class board : MonoBehaviour
{
    [Header("Art stuff")]
    [SerializeField] private Color normalColor = new Color(1, 1, 1, 0.001f);
    [SerializeField] private Color hoverColor = Color.yellow;
    [SerializeField] private Color activeColor = Color.green;
    [SerializeField] private Sprite tileSprite;

    [Header("Prefabs & Materials")]
    [SerializeField] private GameObject[] prefabs;
    // [SerializeField] private Material[] teamMaterials;


    [Header("UI")]

    public float timeRemaining = 10f;
    public bool timerIsRunning = false;
    [SerializeField] TMP_Text timeText;
    [SerializeField] TMP_Text goldText;
    [SerializeField] TMP_Text moveText;
    public RectTransform marketTrans;
    public RectTransform teamInfoTrans;
    public RectTransform ItemPageTrans;
    public RectTransform oppoInfoPage;
    public GameObject mainUI;
    public GameObject waitBattleUI;
    public GameObject marketUI;
    public GameUI gameUI;
    public GameObject display;
    public GameObject armorPrefab;
    public GameObject reversePrefab;
    public GameObject aidKitPrefab;
    public GameObject vaccinePrefab;
    public GameObject gasBombPrefab;
    public GameObject raiboPrefab;
    public TextUICreater textcreater;
    public GameObject armyWinPage;
    public GameObject ZWinPage;
    public GameObject finalWin;
    public GameObject finalLose;
    public GameObject notionPage;
    public float duration = 1f;
    private List<GameObject> spawnedObjects = new List<GameObject>();

    public UsingType usingItem = 0; //leveling = 1, armoring = 2, 
    public float cameraMoveAmount = 15f;
    public int round;
    public int state;

    [Header("ITEM")]
    public int mine = 0;
    public TMP_Text mineText;
    public TMP_Text ZmineText;
    public bool mining = false;
    public GameObject minePrefab;
    public int illusion = 0;
    public TMP_Text illusionText;
    public TMP_Text ZillusionText;
    public bool hiding = false;
    public int gps = 0;
    public TMP_Text gpsText;
    public TMP_Text ZgpsText;
    public GameObject gpsPrefab;
    public int bigbomb = 0;
    public TMP_Text bigbombText;
    public TMP_Text ZbigbombText;
    public GameObject bombPrefab;
    public GameObject bombLightPrefab;
    public bool bombing = false;
    public int changePlace = 0;
    public TMP_Text changePlaceText;
    public TMP_Text ZchangePlaceText;
    public Piece changing = null;
    public int changeIndex = -1;
    public int reverse = 0;
    public TMP_Text reverseText;
    public TMP_Text ZreverseText;
    public int teleport = 0;
    public TMP_Text teleportText;
    public TMP_Text ZteleportText;
    public int armor = 0;
    public TMP_Text armorText;
    public int drone = 0;
    public TMP_Text droneText;
    public int car = 0;
    public TMP_Text carText;
    public int aidKit = 0;
    public TMP_Text aidKitText;
    public Piece aidKitPiece = null;


    public List<ItemInstance> ItemEffects = new List<ItemInstance>();
    public int[] indices;

    public Piece[] Pieces;
    public PieceDTO[] OppositePieces = new PieceDTO[TILE_NUMBER];
    public bool oppoReady = false;
    public bool ready = false;
    public GameObject pcPrefab;
    public Piece currentlyDragging = null;
    private Camera mainCamera;
    private int currentTile = -1;

    public List<Vector2> customPositions;

    private const int TILE_NUMBER = 147;
    public GameObject[] tiles;
    private List<int> availableMoves = new List<int>();
    public int id;
    public int movePerTurn = 10;
    public bool isminigame = false;

    public int gold;
    public int prevGold;
    public int oppoGold;
    // public GameObject pieceObj;
    public bool team;

    public GameObject miniGame;
    public bool gameIsEnd = false;
    public bool isNotionPage = false;

    //건물

    public int library;
    public List<int> kaiHelp;
    public int clinic;
    public List<int> centerMeca;
    public int stemcell;
    public List<int> bank;
    public int sports;
    public List<int> store;
    public int raiboLab;
    public int domitory;
    public List<int> gasLab;
    public List<int> lab;
    private static int rnd = 0;
    public List<GameObject> eventNotion;

    //server
    public Server server;
    public Client client;
    public int turnCount = -1;
    public bool startBattle = false;
    public int battleCount = -1;
    private void Start()
    {
        RegisterEvents();
    }

    private void Awake()
    {

        team = GameUI.team;
        id = 1;
        round = 1;
        state = 1;
        gold = 100;
        mainCamera = Camera.main;
        if (customPositions == null || customPositions.Count == 0)
        {
            customPositions = new List<Vector2>
            {
                new Vector2(-6.04f, 6.519f),
                new Vector2(-4.13f, 6.4f),
                new Vector2(-4.88f, 5.55f),
                new Vector2(-5.69f, 5.04f),
                new Vector2(-5.92f, 4.36f),
                new Vector2(-3.12f, 6.15f),
                new Vector2(-3.45f, 4.95f),
                new Vector2(-4.42f, 4.37f),
                new Vector2(-4.96f, 3.19f),
                new Vector2(-5.6f, 3.13f),
                new Vector2(-1.99f, 6.25f),
                new Vector2(-2.41f, 5.57f),
                new Vector2(-2.77f, 4.7f),
                new Vector2(-1.94f, 4.38f),
                new Vector2(-2.5f, 3.36f),
                new Vector2(-3.89f, 3.21f),
                new Vector2(-4.77f, 2.1f),
                new Vector2(-0.972f, 5.559f),
                new Vector2(0.126f, 6.543f),
                new Vector2(0.47f, 3.67f),
                new Vector2(-0.98f, 3.22f),
                new Vector2(-2.19f, 2.18f),
                new Vector2(-3.83f, 0.89f),
                new Vector2(-4.66f, 1.07f),
                new Vector2(-5.58f, 0.58f),
                new Vector2(-4.41f, -0.17f),
                new Vector2(-5.69f, -0.59f),
                new Vector2(1.73f, 4.847f),
                new Vector2(2.909f, 3.758f),
                new Vector2(1.913f, 3.223f),
                new Vector2(1.015f, 1.329f),
                new Vector2(0.143f, 1.717f),
                new Vector2(-0.372f, 2.324f),
                new Vector2(-2.309f, 1.539f),
                new Vector2(-1.31f, 0.802f),
                new Vector2(-3.208f, 0.379f),
                new Vector2(-2.955f, -0.433f),
                new Vector2(-4.225f, -1.794f),
                new Vector2(-5.22f, -2.928f),
                new Vector2(-4.29f, -6.33f),
                new Vector2(-1.28f, -0.36f),
                new Vector2(-2.955f, -2.115f),
                new Vector2(-3.218f, -3.438f),
                new Vector2(-2.216f, -2.93f),
                new Vector2(-3.23f, -4.63f),
                new Vector2(-2.557f, -5.185f),
                new Vector2(2.789f, 6.897f),
                new Vector2(2.793f, 5.856f),
                new Vector2(3.748f, 5.672f),
                new Vector2(3.491f, 6.589f),
                new Vector2(4.75f, 6.833f),
                new Vector2(4.245f, 5.377f),
                new Vector2(3.992f, 4.199f),
                new Vector2(4.403f, 3.038f),
                new Vector2(2.703f, 2.597f),
                new Vector2(2.887f, 1.655f),
                new Vector2(2.18f, 0.935f),
                new Vector2(1.336f, 0.751f),
                new Vector2(0.021f, 0.661f),
                new Vector2(-0.639f, 0.053f),
                new Vector2(-0.857f, -1.725f),
                new Vector2(-0.866f, -2.659f),
                new Vector2(-1.894f, -3.995f),
                new Vector2(-1.08f, -3.704f),
                new Vector2(-0.151f, -3.691f),
                new Vector2(-0.357f, -5.374f),
                new Vector2(-1.406f, -5.048f),
                new Vector2(0.2f, -6.74f),
                new Vector2(3.921f, 0.923f),
                new Vector2(1.573f, -0.443f),
                new Vector2(1.014f, -1.09f),
                new Vector2(1.507f, -1.861f),
                new Vector2(1.533f, -3.442f),
                new Vector2(1.617f, -4.622f),
                new Vector2(0.705f, -5.956f),
                new Vector2(4.315f, -1.066f),
                new Vector2(3.948f, -2.741f),
                new Vector2(3.892f, -4.499f),
                new Vector2(3.281f, -5.669f),
                new Vector2(6.477f, 5.373f),
                new Vector2(7.627f, 4.992f),
                new Vector2(7.386f, 3.488f),
                new Vector2(5.238f, 2.796f),
                new Vector2(6.126f, 2.544f),
                new Vector2(6.338f, 1.331f),
                new Vector2(6.54f, -0.175f),
                new Vector2(5.619f, -1.359f),
                new Vector2(4.629f, -3.003f),
                new Vector2(6.461f, -3.538f),
                new Vector2(5.553f, -4.663f),
                new Vector2(6.927f, -5.081f),
                new Vector2(7.528f, -5.952f),
                new Vector2(8.449f, -5.965f),
                new Vector2(8.989f, -6.963f),
                new Vector2(8.321f, 6.377f),
                new Vector2(9.466f, 4.392f),
                new Vector2(9.998f, 2.948f),
                new Vector2(8.639f, 3.422f),
                new Vector2(9.184f, 1.354f),
                new Vector2(10.163f, 0.764f),
                new Vector2(10.793f, -0.209f),
                new Vector2(9.024f, -0.497f),
                new Vector2(10.151f, -1.075f),
                new Vector2(7.324f, -1.081f),
                new Vector2(10.835f, -1.688f),
                new Vector2(10.202f, -2.62f),
                new Vector2(8.559f, -2.933f),
                new Vector2(7.601f, -4.063f),
                new Vector2(9.406f, -3.311f),
                new Vector2(9.74f, -4.178f),
                new Vector2(8.889f, -4.144f),
                new Vector2(11.54f, -6.56f),
                new Vector2(9.202f, -5.05f),
                new Vector2(10.017f, -6.462f),
                new Vector2(10.312f, -5.461f),
                new Vector2(10.747f, -4.741f),
                new Vector2(11.243f, -3.854f),
                new Vector2(-0.406f, 4.483f),
                new Vector2(-3.192f, 2.234f),
                new Vector2(-4.434f, -4.988f),
                new Vector2(-0.759f, 1.057f),
                new Vector2(-1.968f, -1.956f),
                new Vector2(-1.94f, -6.9f),
                new Vector2(-0.124f, -1.116f),
                new Vector2(1.624f, 6.539f),
                new Vector2(3.217f, 4.803f),
                new Vector2(1.61f, 2.531f),
                new Vector2(2.9f, -0.62f),
                new Vector2(2.87f, -2.35f),
                new Vector2(0.29f, -4.01f),
                new Vector2(2.003f, -6.008f),
                new Vector2(2.802f, -3.63f),
                new Vector2(4.357f, -5.723f),
                new Vector2(6.228f, 5.863f),
                new Vector2(5.373f, 5.166f),
                new Vector2(5.595f, 3.423f),
                new Vector2(4.586f, 2.04f),
                new Vector2(7.396f, 2.086f),
                new Vector2(5.078f, -0.288f),
                new Vector2(6.432f, -2.297f),
                new Vector2(5.008f, -4.361f),
                new Vector2(9.573f, 6.174f),
                new Vector2(8.337f, 4.673f),
                new Vector2(7.871f, 0.4f),
                new Vector2(8.724f, -2.134f),
                new Vector2(0.65f,-0.21f),
                new Vector2(0.991f,-2.951f)
            };



        }

        for (int i = 0; i < customPositions.Count; i++)
        {
            Vector2 pos = customPositions[i];
            if (team)
            {
                customPositions[i] = new Vector2(pos.x + 24.42f, pos.y);
            }
            else
            {
                customPositions[i] = new Vector2(pos.x + 0.15f, pos.y);
            }

        }
        if (team)
        {
            timeText.GetComponent<RectTransform>().anchoredPosition = new Vector2(330f, 65.5f);
            goldText.GetComponent<RectTransform>().anchoredPosition = new Vector2(330f, 46.1f);
            moveText.GetComponent<RectTransform>().anchoredPosition = new Vector2(915f, 673f);
            marketTrans.anchoredPosition = new Vector2(1136f, -113f);
            ItemPageTrans.anchoredPosition = new Vector2(1136f, 52f);
            teamInfoTrans.anchoredPosition = new Vector2(1136f, 206f);
            oppoInfoPage.anchoredPosition = new Vector2(1180f, -252f);
        }

        GenerateBuilding();
        GenerateAllTiles();
        SpawnAllPieces();
        PositionAllPieces();

        timerIsRunning = true;
    }
    private void OnEnable()
    {
        mainUI.SetActive(true);
        display.SetActive(true);
        timerIsRunning = true;
        isminigame = false;
        notionPage.SetActive(false);

    }
    private void Update()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null) return;
        }


        if (Keyboard.current != null && Keyboard.current.pKey.wasPressedThisFrame)
        {
            // P 키가 눌려있는 동안 실행

            Debug.Log("P 키가 눌려 있습니다!");
            if (timerIsRunning)
            {
                GetComponent<TimedMessage>().ShowMessage("PAUSE ON", 3f);
                timerIsRunning = false;
            }
            else
            {
                GetComponent<TimedMessage>().ShowMessage("PAUSE OFF", 3f);
                timerIsRunning = true;
            }
        }

        UpdateTimer();

        int tileIndex = -1;

        // 마우스 위치를 월드 좌표로 변환
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 mousePos2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

        // Raycast로 타일 감지
        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Tile"));
        if (hit.collider != null && (!isNotionPage))
        {
            for (int i = 0; i < tiles.Length; i++)
            {
                if (hit.collider.gameObject == tiles[i])
                {
                    tileIndex = i;
                    break;
                }
            }
        }

        // 이전 Hover 타일의 Outline 비활성화
        if (currentTile != -1 && currentTile != tileIndex)
        {
            if (ContainsValidMove(ref availableMoves, currentTile))
            {
                Tile tile = tiles[currentTile].GetComponent<Tile>();
                tile.SetOutlineActive(true, activeColor);
            }
            else
            {
                Tile tile = tiles[currentTile].GetComponent<Tile>();
                tile.SetOutlineActive(false);
            }

        }

        // 새로 Hover된 타일의 Outline 활성화
        if (tileIndex != -1 && currentTile != tileIndex)
        {
            if (ContainsValidMove(ref availableMoves, currentTile))
            {
                Tile tile = tiles[tileIndex].GetComponent<Tile>();
                tile.SetOutlineActive(true, activeColor);
            }
            else
            {
                Tile tile = tiles[tileIndex].GetComponent<Tile>();
                tile.SetOutlineActive(true, hoverColor);
            }

        }

        currentTile = tileIndex;


        // 1. Piece 클릭 감지
        RaycastHit2D hitPiece = Physics2D.Raycast(mousePos2D, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Piece"));
        int pieceIndex = -1;
        if (hitPiece.collider != null)
        {
            // Debug.Log("piece 감지했음");
            for (int i = 0; i < Pieces.Length; i++)
            {
                if (Pieces[i] != null && Pieces[i].gameObject == hitPiece.collider.gameObject && (!isNotionPage))
                {
                    if (Pieces[i].turnMove || usingItem != 0)
                    {
                        pieceIndex = i;

                        break;
                    }

                }
            }
        }

        // 마우스 클릭 처리
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (currentlyDragging == null && (!bombing) && (changing == null) && (!mining))
            {
                // 아직 말을 선택하지 않은 상태에서, 말이 있는 타일을 클릭하면 드래그 시작
                // Debug.Log("piece 감지했음" + pieceIndex);
                if (pieceIndex != -1 && Pieces[pieceIndex] != null)
                {
                    switch (usingItem)
                    {
                        case 0:
                            currentlyDragging = Pieces[pieceIndex];
                            Pieces[pieceIndex] = null;
                            currentlyDragging.SetOutlineActive(true);

                            // Get a list of where i can go, highlight tiles as well
                            availableMoves = currentlyDragging.GetAvailableMoves(ref Pieces, TILE_NUMBER);
                            HighlightTiles();
                            break;
                        case UsingType.leveling:
                            Leveling(pieceIndex);
                            break;
                        case UsingType.armoring:
                            Armoring(pieceIndex);
                            break;
                        case UsingType.bigbombing:
                            availableMoves = Piece.GetNodesAtDepth(pieceIndex, 3);
                            availableMoves.AddRange(Piece.GetNodesAtDepth(pieceIndex, 1));
                            availableMoves.AddRange(Piece.GetNodesAtDepth(pieceIndex, 2));
                            availableMoves.Add(pieceIndex);
                            availableMoves.RemoveAll(i => tiles[i].layer == LayerMask.NameToLayer("unTile"));
                            HighlightTiles();
                            bombing = true;
                            break;
                        case UsingType.changing:
                            changing = Pieces[pieceIndex];
                            changing.SetOutlineActive(true);
                            changeIndex = pieceIndex;
                            break;
                        case UsingType.illusioning:
                            currentlyDragging = Pieces[pieceIndex];
                            currentlyDragging.SetOutlineActive(true);

                            hiding = true;

                            // Get a list of where i can go, highlight tiles as well
                            availableMoves = currentlyDragging.GetAvailableMoves(ref Pieces, TILE_NUMBER);
                            HighlightTiles();
                            break;
                        case UsingType.reversing:
                            if (!Pieces[pieceIndex].haveItem.Contains(itemType.reverse))
                            {
                                Pieces[pieceIndex].haveItem.Add(itemType.reverse);
                                reverse--;
                                reverseText.text = reverse.ToString();
                                ZreverseText.text = reverse.ToString();
                            }
                            usingItem = 0;
                            break;
                        case UsingType.fitCar:
                            if (!Pieces[pieceIndex].haveItem.Contains(itemType.car))
                            {
                                Pieces[pieceIndex].haveItem.Add(itemType.car);
                                car--;
                                carText.text = car.ToString();
                            }
                            usingItem = 0;
                            break;
                        case UsingType.teleporting:
                            currentlyDragging = Pieces[pieceIndex];
                            Pieces[pieceIndex] = null;
                            currentlyDragging.SetOutlineActive(true);

                            List<int> list = new List<int>();
                            for (int i = 1; i < 147; i++)
                            {
                                if (i != 111 && Pieces[i] == null && (tiles[i].layer != LayerMask.NameToLayer("unTile")))
                                {
                                    list.Add(i);
                                }

                            }
                            // Get a list of where i can go, highlight tiles as well
                            availableMoves = list;
                            HighlightTiles();
                            break;
                        case UsingType.mining:
                            availableMoves = Piece.GetNodesAtDepth(pieceIndex, 3);
                            availableMoves.AddRange(Piece.GetNodesAtDepth(pieceIndex, 1));
                            availableMoves.AddRange(Piece.GetNodesAtDepth(pieceIndex, 2));
                            availableMoves.Add(pieceIndex);
                            availableMoves.RemoveAll(i => tiles[i].layer == LayerMask.NameToLayer("unTile"));
                            availableMoves.RemoveAll(x => x == 0 || x == 111);
                            availableMoves.RemoveAll(i => tiles[i].GetComponent<Tile>().isMine == true);
                            HighlightTiles();
                            mining = true;
                            break;
                        case UsingType.deleteItem:
                            Pieces[pieceIndex].haveItem.Clear();
                            usingItem = 0;
                            break;
                        case UsingType.deletePiece:
                            Destroy(Pieces[pieceIndex].gameObject);
                            usingItem = 0;
                            break;
                        case UsingType.useDrone:
                            availableMoves = Piece.GetNodesAtDepth(pieceIndex, 3);
                            availableMoves.Add(pieceIndex);
                            availableMoves.AddRange(Piece.GetNodesAtDepth(pieceIndex, 1));
                            availableMoves.AddRange(Piece.GetNodesAtDepth(pieceIndex, 2));
                            int maxLevelInAvailable = availableMoves
                                    .Select(idx => OppositePieces[idx])
                                    .Where(piece => piece != null)
                                    .Select(piece => piece.level)
                                    .DefaultIfEmpty(0)   // 아무것도 없으면 0
                                    .Max();
                            if (maxLevelInAvailable == 0)
                            {
                                GetComponent<TimedMessage2>().ShowMessage("NONE", 2f);
                            }
                            else
                            {
                                GetComponent<TimedMessage2>().ShowMessage(maxLevelInAvailable.ToString(), 3f);
                            }
                            drone--;
                            droneText.text = drone.ToString();
                            // ZdroneText.text = drone.ToString();
                            usingItem = 0;
                            RemoveHighlightTiles();
                            break;
                    }
                }
                else
                {
                    usingItem = 0;
                }

            }
            else if (changing != null)
            {
                if (pieceIndex != -1)
                {
                    Pieces[changeIndex] = Pieces[pieceIndex];
                    Pieces[pieceIndex] = changing;
                    changing.SetOutlineActive(false);
                    changePlace--;
                    changePlaceText.text = changePlace.ToString();
                    ZchangePlaceText.text = changePlace.ToString();
                    PositionAllPieces();
                }
                else
                {
                    changing.SetOutlineActive(false);
                    GetComponent<TimedMessage>().ShowMessage("You should click piece", 1.5f);
                }
                usingItem = 0;
                changing = null;
            }
            else
            {
                // 이미 말을 선택한 상태에서, 타일을 클릭하면 이동
                if (tileIndex != -1)
                {
                    if (bombing)
                    {
                        if (availableMoves.Contains(tileIndex))
                        {
                            if (timeRemaining > 5)
                            {
                                ItemEffects.Add(new ItemInstance("bigbomb", 3, tileIndex));
                                NetBigBomb mm = new NetBigBomb();
                                mm.tileIndex = tileIndex;
                                mm.team = team;
                                Client.Instance.SendToServer(mm);

                                Vector2 position = customPositions[tileIndex];
                                Instantiate(bombLightPrefab, position, Quaternion.identity);

                                bigbomb--;
                                bigbombText.text = bigbomb.ToString();
                                ZbigbombText.text = bigbomb.ToString();
                            }
                            else
                            {
                                GetComponent<TimedMessage>().ShowMessage("Not enough time", 1.5f);
                            }

                        }
                        else
                        {
                            GetComponent<TimedMessage>().ShowMessage("You should click available tile", 1.5f);
                        }
                        bombing = false;
                        usingItem = 0;
                    }
                    else if (mining)
                    {
                        if (availableMoves.Contains(tileIndex))
                        {
                            Tile tl = tiles[tileIndex].GetComponent<Tile>();
                            tl.isMine = true;
                            //mine 보여주는거
                            Vector2 position = customPositions[tileIndex];
                            position.y += 0.2f;
                            position.x += 0.2f;
                            Instantiate(minePrefab, position, Quaternion.identity);
                            mine--;
                            mineText.text = mine.ToString();
                            ZmineText.text = mine.ToString();
                        }
                        else
                        {
                            GetComponent<TimedMessage>().ShowMessage("You should click available tile", 1.5f);
                        }
                        mining = false;
                        usingItem = 0;
                    }
                    else if (hiding)
                    {
                        bool validMove = true;
                        if (!ContainsValidMove(ref availableMoves, tileIndex))
                        {
                            validMove = false;
                        }

                        // Is there another piece on the target position?
                        if (Pieces[tileIndex] != null)
                        {
                            validMove = false;
                        }

                        if (!validMove)
                        {
                            currentlyDragging.SetOutlineActive(false);
                            currentlyDragging = null;
                            GetComponent<TimedMessage>().ShowMessage("You should click available tile", 1.5f);
                        }
                        else
                        {
                            Pieces[tileIndex] = SpawnSinglePiece(currentlyDragging.type, team, currentlyDragging.level, currentlyDragging.id);
                            PositionSinglePiece(tileIndex);
                            currentlyDragging.hiding = true;

                            CapsuleCollider2D collider = Pieces[tileIndex].GetComponent<CapsuleCollider2D>();
                            if (collider != null)
                            {
                                Destroy(collider);
                            }
                            Pieces[tileIndex].isIllusion = true;

                            Renderer renderer = Pieces[tileIndex].GetComponent<Renderer>();
                            Color color = renderer.material.color;
                            color.a = 0.5f; // 0.0f ~ 1.0f (0이 완전 투명)
                            renderer.material.color = color;
                            ItemEffects.Add(new ItemInstance("illusion", 1, tileIndex, currentlyDragging));
                            currentlyDragging.SetOutlineActive(false);
                            currentlyDragging = null;
                            illusion--;
                            illusionText.text = illusion.ToString();
                            ZillusionText.text = illusion.ToString();


                        }
                        hiding = false;
                        usingItem = 0;
                    }
                    else
                    {

                        bool validMove = MoveTo(currentlyDragging, tileIndex);
                        if (((!validMove) && (!(usingItem == UsingType.teleporting)))||currentlyDragging.currentN == tileIndex)
                        {
                            Pieces[currentlyDragging.currentN] = currentlyDragging;
                            currentlyDragging.SetOutlineActive(false);
                            currentlyDragging = null;
                            usingItem = 0;
                            GetComponent<TimedMessage>().ShowMessage("You should click available tile", 1.5f);
                        }
                        else
                        {
                                
                            if (usingItem == UsingType.teleporting)
                            {
                                if (availableMoves.Contains(tileIndex))
                                {
                                    Pieces[tileIndex] = currentlyDragging;
                                    PositionSinglePiece(tileIndex);
                                    // Pieces[tileIndex].turnMove = false;
                                    // currentlyDragging.transform.position = customPositions[hoverIndex];
                                    currentlyDragging.SetOutlineActive(false);
                                    currentlyDragging = null;
                                    teleport--;
                                    teleportText.text = teleport.ToString();
                                    ZteleportText.text = teleport.ToString();
                                    usingItem = 0;
                                }
                                else
                                {
                                    GetComponent<TimedMessage>().ShowMessage("You should click available tile", 1.5f);
                                }
                                
                            }
                            else
                            {
                                Pieces[tileIndex] = currentlyDragging;
                                PositionSinglePiece(tileIndex);
                                Pieces[tileIndex].turnMove = false;
                                // currentlyDragging.transform.position = customPositions[hoverIndex];
                                currentlyDragging.SetOutlineActive(false);
                                currentlyDragging = null;
                            }
                        }
                    }
                    RemoveHighlightTiles();
                }
            }
        }
    }


    private void GenerateAllTiles()
    {
        tiles = new GameObject[TILE_NUMBER];
        for (int i = 0; i < TILE_NUMBER && i < customPositions.Count; i++)
            tiles[i] = GenerateSingleTile(i, customPositions[i]);
    }

    private void PositionAllTiles()
    {
        for (int i = 0; i < TILE_NUMBER && i < customPositions.Count; i++)
        {
            tiles[i].transform.position = customPositions[i];
        }
    }

    private GameObject GenerateSingleTile(int index, Vector2 position)
    {
        GameObject tileObject = new GameObject($"Tile_{index}");
        tileObject.transform.parent = transform;
        tileObject.transform.localScale = Vector3.one * 1.2f; // 2배로 키움
        tileObject.transform.position = position;

        // Vector3 newPosition = new Vector3(position.x, position.y, 50f);
        // tileObject.transform.position = newPosition;

        // SpriteRenderer 설정
        SpriteRenderer sr = tileObject.AddComponent<SpriteRenderer>();
        sr.sprite = tileSprite;
        sr.color = new Color(normalColor.r, normalColor.g, normalColor.b, 0.001f);
        sr.sortingOrder = 1; // 렌더링 순서 지정

        // 테두리용 자식 오브젝트 생성
        GameObject outlineObj = new GameObject("Outline");
        outlineObj.transform.parent = tileObject.transform;
        outlineObj.transform.localPosition = Vector3.zero;
        outlineObj.transform.localScale = Vector3.one * 1.8f; // 본체보다 약간 크게

        SpriteRenderer outlineSr = outlineObj.AddComponent<SpriteRenderer>();
        outlineSr.sprite = tileSprite;
        outlineSr.color = new Color(hoverColor.r, hoverColor.g, hoverColor.b, 0.5f); // 알파 0.7
        outlineSr.sortingOrder = 0; // 본체 아래에 그려지게

        outlineObj.SetActive(false); // 기본은 비활성화

        // 콜라이더 설정 (반드시 필요)
        CircleCollider2D collider = tileObject.AddComponent<CircleCollider2D>();
        collider.radius = 0.2f;

        // Tile 컴포넌트 추가
        Tile tileScript = tileObject.AddComponent<Tile>();
        tileScript.outlineObj = outlineObj;

        tileObject.layer = LayerMask.NameToLayer("Tile");

        return tileObject;
    }


    // Spawning of the pieces
    private void SpawnAllPieces()
    {
        Pieces = new Piece[TILE_NUMBER];


        if (team)
        {
            Pieces[111] = SpawnSinglePiece(PieceType.zombi1, team, 1, id);
        }
        else
        {
            Pieces[0] = SpawnSinglePiece(PieceType.army1, team, 1, id);
        }
        id++;


        // Pieces[111] = SpawnSinglePiece(PieceType.zombi1, zombiTeam, 1);
        // id++;
    }

    private Piece SpawnSinglePiece(PieceType type, bool team, int level, int id)
    {
        Piece cp = Instantiate(prefabs[(int)type - 1], transform).GetComponent<Piece>();

        cp.type = type;
        cp.team = team;
        cp.level = level;
        cp.id = id;


        // outlineObj 생성 (Sprite, Color, 크기 전달)
        Sprite pieceSprite = cp.GetComponent<SpriteRenderer>().sprite;
        Color outlineColor = new Color(hoverColor.r, hoverColor.g, hoverColor.b, 0.5f); // 원하는 색상
        cp.CreateOutline(pieceSprite, outlineColor, 1.5f);

        //text 생성
        GameObject levelTextObj = new GameObject("LevelText");
        levelTextObj.transform.SetParent(cp.transform);
        levelTextObj.transform.localPosition = new Vector3(0, -3f, 0); // 발 밑에 오도록 위치 조정

        TextMeshPro tmpro = levelTextObj.AddComponent<TextMeshPro>();
        tmpro.text = "Lv " + level.ToString();
        tmpro.fontSize = 2.5f; // 크기 조정
        tmpro.alignment = TextAlignmentOptions.Center;
        tmpro.color = Color.white; // 원하는 색상

        cp.levelText = tmpro;

        // 필요하다면 Outline, Shadow 등 효과 추가
        tmpro.outlineWidth = 0.2f;
        tmpro.outlineColor = Color.black;


        //text 생성
        GameObject idTextObj = new GameObject("IdText");
        idTextObj.transform.SetParent(cp.transform);
        idTextObj.transform.localPosition = new Vector3(0, 4f, 0);

        TextMeshPro tmpro2 = idTextObj.AddComponent<TextMeshPro>();
        tmpro2.text = id.ToString();
        tmpro2.fontSize = 2.5f; // 크기 조정
        tmpro2.alignment = TextAlignmentOptions.Center;
        tmpro2.color = Color.white; // 원하는 색상

        // id++;
        return cp;
    }

    private void GenerateBuilding()
    {
        library = 135;
        kaiHelp = new List<int> { 118, 125, 140, 144 };
        clinic = 122;
        centerMeca = new List<int> { 137, 145 };
        stemcell = 133;
        bank = new List<int> { 119, 136, 141 };
        sports = 127;
        store = new List<int> { 123, 126, 131, 134, 138 };
        raiboLab = 129;
        domitory = 128;
        gasLab = new List<int> { 117, 121, 130, 139, 142 };
        lab = new List<int> { 120, 124, 132, 143, 146 };

    }
    // Positioning
    private void PositionAllPieces()
    {
        for (int i = 0; i < TILE_NUMBER; i++)
        {
            GameObject obj = null;
            if (Pieces[i] != null)
                PositionSinglePiece(i);
            
        }

    }

    private void PositionSinglePiece(int i)
    {
        if (!indices.Contains(i))
        {
            Pieces[i].currentN = i;
            Pieces[i].transform.position = customPositions[i];
        }


    }
    // Highlight Tiles
    private void HighlightTiles()
    {
        for (int i = 0; i < availableMoves.Count; i++)
        {
            int tileIndex = availableMoves[i];
            if (!indices.Contains(tileIndex))
            {
                HighlightTile(tileIndex);
            }

        }

    }

    private void HighlightTile(int index)
    {
        Tile tile = tiles[index].GetComponent<Tile>();
        tile.SetOutlineActive(true, activeColor);
    }

    private void RemoveHighlightTiles()
    {
        for (int i = 0; i < availableMoves.Count; i++)
        {
            RemoveHighlightTile(availableMoves[i]);
        }


        availableMoves.Clear();
    }

    private void RemoveHighlightTile(int index)
    {
        Tile tile = tiles[index].GetComponent<Tile>();
        tile.SetOutlineActive(false);
    }


    private bool ContainsValidMove(ref List<int> moves, int pos)
    {
        for (int i = 0; i < moves.Count; i++)
            if (moves[i] == pos)
                return true;

        return false;
    }

    private bool MoveTo(Piece cp, int n)
    {
        if (!ContainsValidMove(ref availableMoves, n))
        {
            return false;
        }

        // Is there another piece on the target position?
        if (Pieces[n] != null)
        {
            // Piece ocp = chessPieces[x, y];

            // if (cp.team == ocp.team)
            return false;
        }

        if (cp.GetNodeDepth(cp.currentN, n) > movePerTurn)
        {
            return false;
        }
        else
        {
            if (!(usingItem == UsingType.teleporting))
                movePerTurn -= cp.GetNodeDepth(cp.currentN, n);
            return true;
        }


    }

    //timer
    public void DeleteRoundTile(int round)
    {
        // Piece piece = pieceObj.GetComponent<Piece>();

        switch (round)
        {
            case 1:
                // 아무 것도 하지 않음
                break;
            case 2:
                indices = new int[] { 16, 118, 33, 23, 22, 35, 36, 37, 24, 25, 26, 131, 77, 78, 132, 130, 100, 102, 104, 17, 18, 117, 124, 27, 47, 125 };
                // piece.availableMovesPerIndex[]
                break;
            case 3:
                indices = new int[] { 38, 42, 43, 44, 45, 39, 119, 66, 122, 46, 49, 48, 50, 51, 134, 133, 79, 80, 94, 142, 141, 95, 16, 118, 33, 23, 22, 35, 36, 37, 24, 25, 26, 131, 77, 78, 132, 130, 100, 102, 104, 17, 18, 117, 124, 27, 47, 125 };
                break;
            case 4:
                indices = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 89, 90, 91, 92, 93, 105, 144, 106, 107, 108, 109, 110, 112, 113, 114, 115, 116, 38, 42, 43, 44, 45, 39, 119, 66, 122, 46, 49, 48, 50, 51, 134, 133, 79, 80, 94, 142, 141, 95, 16, 118, 33, 23, 22, 35, 36, 37, 24, 25, 26, 131, 77, 78, 132, 130, 100, 102, 104, 17, 18, 117, 124, 27, 47, 125 };
                break;
            case 5:
                indices = new int[] { 20, 21, 120, 34, 40, 59, 123, 41, 121, 60, 61, 62, 63, 64, 146, 129, 72, 73, 74, 65, 67, 52, 135, 81, 83, 137, 97, 96, 98, 99, 101, 103, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 89, 90, 91, 92, 93, 105, 144, 106, 107, 108, 109, 110, 112, 113, 114, 115, 116, 38, 42, 43, 44, 45, 39, 119, 66, 122, 46, 49, 48, 50, 51, 134, 133, 79, 80, 94, 142, 141, 95, 16, 118, 33, 23, 22, 35, 36, 37, 24, 25, 26, 131, 77, 78, 132, 130, 100, 102, 104, 17, 18, 117, 124, 27, 47, 125, 82 };
                break;
        }

        if (indices != null)
        {
            foreach (int idx in indices)
            {
                if (tiles[idx] != null)
                {
                    tiles[idx].layer = LayerMask.NameToLayer("unTile");
                    tiles[idx].GetComponent<Tile>().isMine = false;
                    if (Pieces[idx] != null)
                    {
                        Piece piece = Pieces[idx];

                        Vector3 position = new Vector3(-553f + (eventNotion.Count % 4 * 350), 32f - 60 * (eventNotion.Count / 4), -2f);
                        GameObject obj = textcreater.CreateTextUI(position, piece.id.ToString() + "번말 방사능 피폭으로\n사망", 30, Color.black);
                        obj.SetActive(false);
                        eventNotion.Add(obj);

                        
                        Destroy(piece.gameObject);
                    }
                    if (OppositePieces[idx] != null)
                    {
                        OppositePieces[idx] = null;
                    }
                }

            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);


        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        goldText.text = gold.ToString() + "G";
        moveText.text = "remain move: " + movePerTurn.ToString();

    }

    void MoveCameraUp()
    {
        Vector3 pos = mainCamera.transform.position;
        pos.y += cameraMoveAmount;
        mainCamera.transform.position = pos;

        for (int i = 0; i < customPositions.Count; i++)
        {
            Vector2 pos2 = customPositions[i];
            customPositions[i] = new Vector2(pos2.x, pos2.y + 15f);
        }

        PositionAllTiles();
        PositionAllPieces();
    }

    void UpdateTimer()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {

                if (Pieces[0] != null)
                {
                    Piece piece = Pieces[0];
                    Destroy(piece.gameObject);
                }

                if (Pieces[111] != null)
                {
                    Piece piece = Pieces[111];
                    Destroy(piece.gameObject);
                }

                if (round == 3 && state == 4)
                {
                    if (Pieces[111] != null)
                    {
                        Piece piece = Pieces[111];
                        Destroy(piece.gameObject);
                    }
                    if (Pieces[0] != null)
                    {
                        Piece piece = Pieces[0];
                        Destroy(piece.gameObject);
                    }
                }

                if (currentlyDragging != null)
                {
                    Pieces[currentlyDragging.currentN] = currentlyDragging;
                    currentlyDragging.SetOutlineActive(false);
                    currentlyDragging = null;
                    RemoveHighlightTiles();
                }
                gameUI.ReturnMain();
                GetComponent<TimedMessage>().HideMessage();
                GetComponent<TimedMessage2>().HideMessage();
                usingItem = 0;
                bombing = false; // 이거일때 턴 끝날때 버그처리 해야할듯
                mining = false;
                hiding = false;
                if (changing != null)
                {
                    changing.SetOutlineActive(false);
                    changing = null;
                }

                RemoveHighlightTiles();


                Client.Instance.SendToServer(new NetTurnEnd());
                //서버통신 여기서
                // while (!startBattle) continue;
                if (!startBattle)
                {

                    Vector3 pos = mainCamera.transform.position;
                    pos.x = -60f;
                    pos.y = 45f;
                    pos.z = -10f;
                    mainCamera.transform.position = pos;
                    waitBattleUI.SetActive(true);
                    mainUI.SetActive(false);
                    display.SetActive(false);
                    timeRemaining = 0;
                    timerIsRunning = false;



                }
            }
        }
        else if (startBattle)
        {
            OppositePieces = new PieceDTO[TILE_NUMBER];

            startBattle = false;
            //battle을 하고
            //대결 계산해서 대결 띄우기 대결!!
            StartCoroutine(PauseThreeSecond());




        }
        else if (ready && oppoReady)
        {
            if (state == 4 && round == 5)
            {
                NetFinalMoney mm = new NetFinalMoney();
                mm.team = team;
                mm.gold = gold;
                Client.Instance.SendToServer(mm);
            }
            // Debug.Log("여기는 오니?");
            ready = false;
            oppoReady = false;


            waitBattleUI.SetActive(false);
            

            gameUI.ReturnMain();
            StartCoroutine(endingTurn());
             // display 다 하고 true로 바꾸는 식으로 바꿔야함
        }
    }

    //button

    IEnumerator endingTurn()
    {
        gameUI.turnEnd = true;
        ItemApplyDelete();


        for (int i = 0; i < TILE_NUMBER; i++)
            if (Pieces[i] != null)
                Pieces[i].turnMove = true;
        //건물통신이 여기 있어야함.
        // 여기에 건물 적용하면 될듯
        ApplyBuilding();
        mainUI.SetActive(false);
        display.SetActive(false);
        yield return new WaitForSeconds(1f);
        if (state == 2)
        {
            StartCoroutine(mini());
            isminigame = true;
            //ready 띄우기
            //그리고 mainUI랑 board를 off 하면 될듯. 그러면 시간도 멈추고 
            //OnEnable 함수 활용해서 돌아가면 될듯


            // Debug.Log("이거 출력 가능?");
        }

        if (state == 4 && round == 5)
        {
            timeRemaining = 0;
            timerIsRunning = false;
            mainUI.SetActive(false);
            display.SetActive(false);
            gameUI.ReturnMain();
            gameIsEnd = true;
            
        }
        else if (state == 4)
        {
            state = 1;
            round++;
            gold += 100;

            timeRemaining = 8f;
            DeleteRoundTile(round);
        }
        else
        {
            state++;
            timeRemaining = 8f;
        }
        // timerIsRunning = false;
        MoveCameraUp();
        // DeleteRoundTile(round);
        notionPage.SetActive(true);
        isNotionPage = true;
        Vector3 posi = mainCamera.transform.position;
        posi.z = -1;
        notionPage.transform.position = posi;


        ApplyStartEffects();
        foreach (var text in eventNotion)
        {
            text.SetActive(true);
        }



        mainUI.SetActive(false);
        display.SetActive(false);
        yield return new WaitForSeconds(10f);

        foreach (var text in eventNotion)
            Destroy(text);
        eventNotion.Clear();
        notionPage.SetActive(false);
        isNotionPage = false;
        if (gameIsEnd)
        {
            GameEnding();
        }

        for (int i = 0; i < TILE_NUMBER; i++)
        {
            if (OppositePieces[i] != null)
            {
                if (!OppositePieces[i].hiding)
                {
                    Vector2 position = customPositions[i];
                    position.x += 0.25f;
                    position.y += 0.25f;
                    GameObject obj = Instantiate(pcPrefab, position, Quaternion.identity);

                    if (state == 1)
                    {
                        GameObject levelTextObj = new GameObject("LevelText");
                        levelTextObj.transform.SetParent(obj.transform);
                        levelTextObj.transform.localPosition = new Vector3(0, -1f, 0); // 발 밑에 오도록 위치 조정
                        TextMeshPro tmpro = levelTextObj.AddComponent<TextMeshPro>();
                        tmpro.text = "Lv" + OppositePieces[i].level.ToString();
                        tmpro.fontSize = 2f; // 크기 조정
                        tmpro.fontStyle = FontStyles.Bold;
                        tmpro.alignment = TextAlignmentOptions.Center;
                        tmpro.color = Color.white; // 원하는 색상
                    }
                }
            }
        }
        movePerTurn = 10;
        if (!isminigame)
        {
            timerIsRunning = true;
        }
        
        mainUI.SetActive(true);
        display.SetActive(true);
        if (gameIsEnd)
        {
            timerIsRunning = false;
        }

            
        
    }

    IEnumerator mini()
    {

        Vector3 pos = mainCamera.transform.position;
        pos.x = -60f;
        pos.y = 15f;
        pos.z = -10f;
        mainCamera.transform.position = pos;
        yield return new WaitForSeconds(12f);

        miniGame.SetActive(true);
        mainUI.SetActive(false);
        display.SetActive(false);
        gameObject.SetActive(false);

    }
    public void Battle(Piece ourPc, PieceDTO oppoPc)
    {
        waitBattleUI.SetActive(false);
        Vector3 pos = mainCamera.transform.position;
        pos.x = -60f;
        if (team)
        {
            if (oppoPc.level > 10)
            {
                pos.y = 120;
            }
            else if (oppoPc.level > 7)
            {
                pos.y = 105;
            }
            else if (oppoPc.level > 4)
            {
                pos.y = 90;
            }
            else if (oppoPc.level > 2)
            {
                pos.y = 75;
            }
            else
            {
                pos.y = 60;
            }
        }
        else
        {
            if (oppoPc.level > 10)
            {
                pos.y = 360f;
            }
            else if (oppoPc.level > 7)
            {
                pos.y = 285f;
            }
            else if (oppoPc.level > 4)
            {
                pos.y = 210f;
            }
            else if (oppoPc.level > 2)
            {
                pos.y = 135;
            }
            else
            {
                pos.y = 60;
            }
        }
        switch (ourPc.type)
        {
            case PieceType.army1:
                break;
            case PieceType.army2:
                pos.y += 15f;
                break;
            case PieceType.army3:
                pos.y += 30f;
                break;
            case PieceType.army4:
                pos.y += 45f;
                break;
            case PieceType.army5:
                pos.y += 60f;
                break;
            case PieceType.zombi1:
                break;
            case PieceType.zombi2:
                pos.y += 75f;
                break;
            case PieceType.zombi3:
                pos.y += 150f;
                break;
            case PieceType.zombi4:
                pos.y += 225f;
                break;
            case PieceType.zombi5:
                pos.y += 300f;
                break;
        }
        int c = 0;
        foreach (itemType item in ourPc.haveItem)
        {
            switch (item)
            {
                case itemType.armor:
                    GameObject armorObj = Instantiate(armorPrefab); // 미리 만들어둔 이미지 프리팹
                    if (team)
                    {
                        armorObj.transform.localPosition = new Vector3(6f + c + pos.x, 6f + pos.y, -1);
                    }
                    else
                    {
                        armorObj.transform.localPosition = new Vector3(-9f + c + pos.x, 6f + pos.y, -1);
                    }
                    c++;
                    spawnedObjects.Add(armorObj);
                    break;
                case itemType.reverse:
                    GameObject reverseObj = Instantiate(reversePrefab); // 미리 만들어둔 이미지 프리팹
                    reverseObj.transform.localPosition = new Vector3(-8f + pos.x, pos.y, -2);
                    if (team)
                    {
                        reverseObj.transform.localPosition = new Vector3(8f + pos.x, pos.y, -2);
                    }
                    spawnedObjects.Add(reverseObj);
                    break;
                case itemType.aidKit:
                    GameObject aidKitObj = Instantiate(aidKitPrefab); // 미리 만들어둔 이미지 프리팹
                    if (team)
                    {
                        aidKitObj.transform.localPosition = new Vector3(6f + c  + pos.x, 6f + pos.y, -1);
                    }
                    else
                    {
                        aidKitObj.transform.localPosition = new Vector3(-9f + c  + pos.x, 6f + pos.y, -1);
                    }
                    c++;
                    spawnedObjects.Add(aidKitObj);
                    break;
                case itemType.raibo:
                    GameObject raiboObj = Instantiate(raiboPrefab); // 미리 만들어둔 이미지 프리팹
                    raiboObj.transform.localPosition = new Vector3(-12f + pos.x, -5f + pos.y, -1);
                    if (team)
                    {
                        raiboObj.transform.localPosition = new Vector3(12f + pos.x, -5f + pos.y, -1);
                    }
                    spawnedObjects.Add(raiboObj);
                    break;
                case itemType.advantage:

                    if (team)
                    {
                        GameObject gasBombObj = Instantiate(gasBombPrefab);
                        gasBombObj.transform.localPosition = new Vector3(6f + c  + pos.x, 6f + pos.y, -1);
                        spawnedObjects.Add(gasBombObj);
                    }
                    else
                    {
                        GameObject vaccineObj = Instantiate(vaccinePrefab);
                        vaccineObj.transform.localPosition = new Vector3(-9f + c  + pos.x, 6f + pos.y, -1);
                        spawnedObjects.Add(vaccineObj);
                    }
                    // 미리 만들어둔 이미지 프리팹
                    c++;
                    break;
            }
        }
        if (team)
        {
            GameObject textobj = textcreater.CreateTextUI(new Vector2(605f, -335f), "Lv " + ourPc.level.ToString(),40);
            spawnedObjects.Add(textobj);
        }
        else
        {
            GameObject textobj = textcreater.CreateTextUI(new Vector2(-605f, -335f), "Lv " + ourPc.level.ToString(),40);
            spawnedObjects.Add(textobj);
        }

        mainCamera.transform.position = pos;
        StartCoroutine(BattleAnimation(ourPc, oppoPc));
    }
    public void GeneratePiece()
    {
        if (team)
        {
            if (Pieces[111] == null)
            {
                Pieces[111] = SpawnSinglePiece(PieceType.zombi1, team, 1, id);
                id++;
                PositionSinglePiece(111);
                gold -= 30;
                gameUI.ReturnMain();
            }
            else
            {
                GetComponent<TimedMessage>().ShowMessage("someone in your homebase", 1f);
            }
        }
        else
        {
            if (Pieces[0] == null)
            {
                Pieces[0] = SpawnSinglePiece(PieceType.army1, team, 1, id);
                id++;
                PositionSinglePiece(0);
                gold -= 30;
                gameUI.ReturnMain();
            }
            else
            {
                GetComponent<TimedMessage>().ShowMessage("someone in your homebase", 1f);
            }
        }
    }

    public void LevelUp()
    {
        usingItem = UsingType.leveling;
        gameUI.ReturnMain();
    }

    public void Leveling(int index)

    {
        Piece piece = Pieces[index];
        int pieceLv = piece.level;
        PieceType pieceType = piece.type;
        int pieceId = piece.id;
        if (gold >= pieceLv * 10)
        {
            if (pieceLv == 2 || pieceLv == 4 || pieceLv == 7 || pieceLv == 10)
            {


                Pieces[index] = SpawnSinglePiece(pieceType + 1, team, pieceLv + 1, pieceId);
                PositionSinglePiece(index);
                Pieces[index].CloneFrom(piece);
                Destroy(piece.gameObject);
            }
            else
            {
                piece.SetLevel(pieceLv + 1);
                // Pieces[index] = piece;
            }
            gold -= (pieceLv * 10);

        }
        usingItem = 0;
    }

    #region useItem
    public void UseArmor()
    {
        if (armor > 0)
        {
            usingItem = UsingType.armoring;
            gameUI.ReturnMain();
        }

    }

    public void Armoring(int index)
    {
        if (!Pieces[index].haveItem.Contains(itemType.armor))
        {
            Pieces[index].haveItem.Add(itemType.armor);
            armor--;
            armorText.text = armor.ToString();
        }
        usingItem = 0;
    }

    public void UseGPS() // 상대 가장 레벨 높은 말 위치
    {
        if (gps > 0)
        {
            // 최고 레벨 찾기 (isIllusion == false만 대상)
            int maxLevel = OppositePieces
                .Where(piece => piece != null && piece.hiding == false)
                .Max(piece => piece.level);

            // 최고 레벨 piece들 모두 찾기 (isIllusion == false)
            var maxLevelPieces = OppositePieces
                .Where(piece => piece != null && piece.hiding == false && piece.level == maxLevel)
                .ToList();


            if (maxLevelPieces.Count == 0)
            {
                GetComponent<TimedMessage>().ShowMessage("NONE", 1f);
            }
            else
            {
                // 모든 최고 레벨 piece에 대해 prefab을 생성
                foreach (var piece in maxLevelPieces)
                {
                    Vector2 position = customPositions[piece.tileNum];
                    position.y -= 0.3f;
                    position.x -= 0.3f;
                    Instantiate(gpsPrefab, position, Quaternion.identity);
                }
            }
            // 
            gameUI.ReturnMain();
            gps--;
            gpsText.text = gps.ToString();
            ZgpsText.text = gps.ToString();
        }

    }

    public void Usebigbomb() // 반경 3칸 내에 사용하고 tile 봉쇄
    {
        if (bigbomb > 0)
        {
            usingItem = UsingType.bigbombing;
            gameUI.ReturnMain();
        }
    }

    public void UseChangePlace()
    {
        if (changePlace > 0)
        {
            usingItem = UsingType.changing;
            gameUI.ReturnMain();
        }

    }

    public void UseIllusion()
    {
        if (illusion > 0)
        {
            usingItem = UsingType.illusioning;
            gameUI.ReturnMain();
        }
    }
    public void UseReverse()
    {
        if (reverse > 0)
        {
            usingItem = UsingType.reversing;
            gameUI.ReturnMain();
        }

    }

    public void UseDrone()
    {
        if (drone > 0)
        {

            // 서버로부터 받은 맵에서 정보를 받아서 반경 3칸 내에 최대 레벨을 받아와 -> maxlv
            //알림창 띄워서 거기다 text로 적자..
            usingItem = UsingType.useDrone;
            gameUI.ReturnMain();
            // drone--;
            // droneText.text = drone.ToString();
        }
    }

    public void UseCar()
    {
        if (car > 0)
        {
            usingItem = UsingType.fitCar;
            gameUI.ReturnMain();
        }
    }

    public void UseTeleport()
    {
        if (teleport > 0)
        {
            usingItem = UsingType.teleporting;
            gameUI.ReturnMain();
        }
    }

    public void UseMine()
    {
        if (mine > 0)
        {
            usingItem = UsingType.mining;
            gameUI.ReturnMain();
        }
    }
    #endregion
    public void ItemApplyDelete()
    {

        for (int i = ItemEffects.Count - 1; i >= 0; i--)
        {
            ItemEffects[i].turnsLeft--;
            if (ItemEffects[i].turnsLeft <= 0)
            {
                if (ItemEffects[i].effectName == "bigbomb" && !indices.Contains(ItemEffects[i].value))
                {
                    tiles[ItemEffects[i].value].layer = LayerMask.NameToLayer("Tile");


                }
                if (ItemEffects[i].effectName == "illusion")
                {

                    if (ItemEffects[i].piece != null)
                    {
                        ItemEffects[i].piece.hiding = false;
                        pieceServerMessage(ItemEffects[i].piece);
                    }

                    if (Pieces[ItemEffects[i].value].isIllusion)
                    {
                        Destroy(Pieces[ItemEffects[i].value].gameObject);
                        Pieces[ItemEffects[i].value] = null;
                    }
                }

                // if (ItemEffects[i].effectName == "gps")
                // {

                // }
                ItemEffects.RemoveAt(i);
            }
        }

    }

    public void ApplyStartEffects()
    {
        foreach (var effect in ItemEffects)
        {
            if (effect.effectName == "bigbomb")
            {
                if (Pieces[effect.value] != null)
                {
                    Pieces[effect.value].turnMove = false;
                }
                tiles[effect.value].layer = LayerMask.NameToLayer("unTile");
                // Pieces[i].transform.position = customPositions[i];
                Vector2 position = customPositions[effect.value];
                Instantiate(bombPrefab, position, Quaternion.identity);
            }

        }

        for (int i = 1; i < TILE_NUMBER; i++)
        {
            Tile tl = tiles[i].GetComponent<Tile>();
            if (tl.isMine)
            {
                Vector2 position = customPositions[i];
                position.y += 0.2f;
                Instantiate(minePrefab, position, Quaternion.identity);
            }
        }

        if (aidKitPiece != null)
        {
            // 제외할 인덱스를 빠르게 찾기 위해 HashSet 사용
            HashSet<int> excludeSet = new HashSet<int>(indices);

            // 비어 있고, 제외 인덱스에 포함되지 않은 자리만 골라서 따로 저장
            List<int> candidateIndices = new List<int>();
            for (int i = 0; i < Pieces.Length; i++)
            {
                if (Pieces[i] == null && !excludeSet.Contains(i))
                {
                    candidateIndices.Add(i);
                }
            }

            if (candidateIndices.Count > 0)
            {
                int chosen = candidateIndices[UnityEngine.Random.Range(0, candidateIndices.Count)];
                Pieces[chosen] = aidKitPiece;
                PositionSinglePiece(chosen);

                NetMakeMove mm = new NetMakeMove();
                mm.tileNum = chosen;
                mm.level = Pieces[chosen].level;
                mm.delete = false;
                mm.armor = false;
                mm.reverse = false;
                mm.aidKit = false;
                mm.raibo = false;
                mm.advantage = false;
                mm.team = team;
                
                mm.hiding = false;
                mm.isIllusion = false;

                Client.Instance.SendToServer(mm);

                aidKitPiece = null;
            }
            else
            {
                Debug.LogWarning("가능한 빈 자리가 없습니다(제외 인덱스 제외)!");
                // 추가 처리 필요 시 작성
            }
        }

    }



    public void ApplyBuilding()
    {
        for (int i = 1; i < TILE_NUMBER; i++)
        {

            if (tiles[i] == null)
            {
                Debug.LogWarning($"tiles[{i}]가 null임!");
                continue;
            }

            Tile tile = tiles[i].GetComponent<Tile>();
            if (tile == null)
            {
                Debug.LogWarning($"tiles[{i}]에 Tile 컴포넌트 없음!");
                continue;
            }
            if (tile.isMine) // ? 타일중에 뭐가 문제가 있나본데
            {

                Piece piece = Pieces[i];
                if (piece == null || piece.isIllusion)
                {
                    // Debug.LogWarning($"tiles[{i}]에 건물이 있는데 piece가 없음!");
                    continue;
                }
                else
                {

                    int pieceLv = piece.level;
                    PieceType pieceType = piece.type;
                    int pieceId = piece.id;
                    Vector3 position = new Vector3(-553f + (eventNotion.Count % 4 * 350), 32f - 60 * (eventNotion.Count / 4), -2f);
                    GameObject obj = textcreater.CreateTextUI(position, pieceId.ToString() + "번말 지뢰밟음", 30, Color.black);
                    obj.SetActive(false);
                    eventNotion.Add(obj);
                    if (piece.haveItem.Contains(itemType.armor))
                    {
                        piece.haveItem.Remove(itemType.armor);
                    }
                    else
                    {
                        if (pieceLv == 1)
                        {
                            NetMakeMove mm4 = new NetMakeMove();
                            mm4.tileNum = piece.currentN;
                            mm4.delete = true;
                            mm4.team = team;
                            Client.Instance.SendToServer(mm4);
                            if (piece.haveItem.Contains(itemType.aidKit))
                            {

                                piece.haveItem.Clear();
                                aidKitPiece = piece;
                                NetMakeMove mm = new NetMakeMove();
                                mm.tileNum = i;
                                mm.team = team;
                                mm.delete = true;
                                Client.Instance.SendToServer(mm);
                            }
                            else
                            {

                                Destroy(piece.gameObject);

                            }
                            Pieces[i] = null;

                        }
                        else if (pieceLv == 3 || pieceLv == 5 || pieceLv == 8 || pieceLv == 11)
                        {

                            Pieces[i] = SpawnSinglePiece(pieceType - 1, team, pieceLv - 1, pieceId);
                            PositionSinglePiece(i);
                            Pieces[i].CloneFrom(piece);
                            Destroy(piece.gameObject);
                            pieceServerMessage(Pieces[i]);


                        }
                        else
                        {
                            piece.SetLevel(pieceLv - 1);
                            // Pieces[index] = piece;
                            pieceServerMessage(piece);
                        }

                    }
                    tiles[i].GetComponent<Tile>().isMine = false;
                    NetTileEvent mm3 = new NetTileEvent();
                    mm3.tileIndex = i;
                    mm3.team = team;
                    // Debug.Log("여기는 이제 어 그래." + i.ToString());
                    mm3.bomb = true;
                    Client.Instance.SendToServer(mm3);
                }
            }
            else if (tile.oppoIsMine)
            {
                Piece piece = Pieces[i];
                if (piece == null || piece.isIllusion)
                {
                    // Debug.LogWarning($"tiles[{i}]에 건물이 있는데 piece가 없음!");
                    continue;
                }
                else
                {

                    int pieceLv = piece.level;
                    PieceType pieceType = piece.type;
                    int pieceId = piece.id;
                    Vector3 position = new Vector3(-553f + (eventNotion.Count % 4 * 350), 32f - 60 * (eventNotion.Count / 4), -2f);
                    GameObject obj = textcreater.CreateTextUI(position, pieceId.ToString() + "번말 지뢰밟음", 30, Color.black);
                    obj.SetActive(false);
                    eventNotion.Add(obj);

                    if (piece.haveItem.Contains(itemType.armor))
                    {
                        piece.haveItem.Remove(itemType.armor);
                    }
                    else
                    {
                        if (pieceLv == 1)
                        {

                            NetMakeMove mm3 = new NetMakeMove();
                            mm3.tileNum = piece.currentN;
                            mm3.delete = true;
                            mm3.team = team;
                            Client.Instance.SendToServer(mm3);
                            if (piece.haveItem.Contains(itemType.aidKit))
                            {
                                piece.haveItem.Clear();
                                aidKitPiece = piece;
                                NetMakeMove mm1 = new NetMakeMove();
                                mm1.tileNum = i;
                                mm1.team = team;
                                mm1.delete = true;
                                Client.Instance.SendToServer(mm1);
                            }
                            else
                            {

                                Destroy(piece.gameObject);
                            }
                            Pieces[i] = null;
                        }
                        else if (pieceLv == 3 || pieceLv == 5 || pieceLv == 8 || pieceLv == 11)
                        {

                            Pieces[i] = SpawnSinglePiece(pieceType - 1, team, pieceLv - 1, pieceId);
                            PositionSinglePiece(i);
                            Pieces[i].CloneFrom(piece);
                            Destroy(piece.gameObject);
                            pieceServerMessage(Pieces[i]);
                        }
                        else
                        {
                            piece.SetLevel(pieceLv - 1);
                            // Pieces[index] = piece;
                            pieceServerMessage(piece);
                        }

                    }
                    tiles[i].GetComponent<Tile>().oppoIsMine = false;
                    NetTileEvent mm = new NetTileEvent();
                    mm.tileIndex = i;
                    mm.team = team;
                    // Debug.Log("여기는 이제 어 그래." + i.ToString());
                    mm.bomb = true;
                    Client.Instance.SendToServer(mm);
                }
            }
        }
        for (int i = 1; i < TILE_NUMBER; i++)
        {
            if (i != 111 && Pieces[i] != null && (!Pieces[i].isIllusion))
            {

                if (i == library && (!Pieces[i].visitLibrary))
                {
                    Piece piece = Pieces[i];
                    int pieceLv = piece.level;
                    PieceType pieceType = piece.type;
                    int pieceId = piece.id;
                    if (pieceLv == 2 || pieceLv == 4 || pieceLv == 7 || pieceLv == 10)
                    {
                        Pieces[i] = SpawnSinglePiece(pieceType + 1, team, pieceLv + 1, pieceId);
                        PositionSinglePiece(i);
                        Pieces[i].CloneFrom(piece);
                        Destroy(piece.gameObject);
                        
                    }
                    else
                    {
                        piece.SetLevel(pieceLv + 1);
                    }
                    Pieces[i].visitLibrary = true;
                    pieceServerMessage(Pieces[i]);
                    Vector3 position = new Vector3(-553f + (eventNotion.Count % 4 * 350), 32f - 60 * (eventNotion.Count / 4), -2f);
                    GameObject obj = textcreater.CreateTextUI(position, pieceId.ToString() + "번말 도서관에서\n레벨업", 30, Color.black);
                    obj.SetActive(false);
                    eventNotion.Add(obj);
                }
                else if (kaiHelp.Contains(i) && (!tiles[i].GetComponent<Tile>().isVisited))
                {
                    if (!Pieces[i].haveItem.Contains(itemType.armor))
                    {
                        Pieces[i].haveItem.Add(itemType.armor);
                        tiles[i].GetComponent<Tile>().isVisited = true;

                        Vector3 position = new Vector3(-553f + (eventNotion.Count % 4 * 350), 32f - 60 * (eventNotion.Count / 4), -2f);
                        GameObject obj = textcreater.CreateTextUI(position, Pieces[i].id.ToString() + "번말 카이헬프에서\n방탄복", 30, Color.black);
                        obj.SetActive(false);
                        eventNotion.Add(obj);
                    }
                    else
                    {
                        Vector3 position = new Vector3(-553f + (eventNotion.Count % 4 * 350), 32f - 60 * (eventNotion.Count / 4), -2f);
                        GameObject obj = textcreater.CreateTextUI(position, Pieces[i].id.ToString() + "번말 카이헬프\n다른 말로 시도하십시오.", 30, Color.black);
                        obj.SetActive(false);
                        eventNotion.Add(obj);
                    }

                }
                else if (i == clinic && (!tiles[i].GetComponent<Tile>().isVisited))
                {
                    Pieces[i].haveItem.Add(itemType.aidKit);
                    tiles[i].GetComponent<Tile>().isVisited = true;

                    Vector3 position = new Vector3(-553f + (eventNotion.Count % 4 * 350), 32f - 60 * (eventNotion.Count / 4), -2f);
                    GameObject obj = textcreater.CreateTextUI(position, Pieces[i].id.ToString() + "번말 카이스트클리닉에서\n구급상자를 줍다.", 30, Color.black);
                    obj.SetActive(false);
                    eventNotion.Add(obj);
                }
                else if (centerMeca.Contains(i))
                {
                    Piece piece = Pieces[i];
                    int pieceLv = piece.level;
                    PieceType pieceType = piece.type;
                    int pieceId = piece.id;
                    Vector3 position = new Vector3(-553f + (eventNotion.Count % 4 * 350), 32f - 60 * (eventNotion.Count / 4), -2f);
                    GameObject obj = textcreater.CreateTextUI(position, pieceId.ToString() + "번말 중앙기계실에서\n감전사고로 레벨 감소", 30, Color.black);
                    obj.SetActive(false);
                    eventNotion.Add(obj);
                    if (piece.haveItem.Contains(itemType.armor))
                    {
                        piece.haveItem.Remove(itemType.armor);

                    }
                    else
                    {
                        if (pieceLv == 1)
                        {
                            NetMakeMove mm3 = new NetMakeMove();
                            mm3.tileNum = piece.currentN;
                            mm3.delete = true;
                            mm3.team = team;
                            Client.Instance.SendToServer(mm3);
                            if (piece.haveItem.Contains(itemType.aidKit))
                            {
                                piece.haveItem.Clear();
                                aidKitPiece = piece;
                                NetMakeMove mm = new NetMakeMove();
                                mm.tileNum = i;
                                mm.team = team;
                                mm.delete = true;
                                Client.Instance.SendToServer(mm);
                            }
                            else
                            {

                                Destroy(piece.gameObject);
                            }
                            Pieces[i] = null;
                        }
                        else if (pieceLv == 3 || pieceLv == 5 || pieceLv == 8 || pieceLv == 11)
                        {
                            Pieces[i] = SpawnSinglePiece(pieceType - 1, team, pieceLv - 1, pieceId);
                            PositionSinglePiece(i);
                            Pieces[i].CloneFrom(piece);
                            Destroy(piece.gameObject);
                            pieceServerMessage(Pieces[i]);
                        }
                        else
                        {
                            piece.SetLevel(pieceLv - 1);
                            // Pieces[index] = piece;
                            pieceServerMessage(piece);
                        }
                    }
                }
                else if (i == stemcell && (!tiles[i].GetComponent<Tile>().isVisited))
                {
                    Vector3 position = new Vector3(-553f + (eventNotion.Count % 4 * 350), 32f - 60 * (eventNotion.Count / 4), -2f);
                    GameObject obj = textcreater.CreateTextUI(position, Pieces[i].id.ToString() + "번말 줄기세포\n배양 성공", 30, Color.black);
                    obj.SetActive(false);
                    eventNotion.Add(obj);
                    if (team)
                    {
                        if (Pieces[111] == null)
                        {
                            Pieces[111] = SpawnSinglePiece(Pieces[i].type, team, Pieces[i].level, id);
                            id++;
                            PositionSinglePiece(111);

                        }
                    }
                    else
                    {
                        if (Pieces[0] == null)
                        {
                            Pieces[0] = SpawnSinglePiece(Pieces[i].type, team, Pieces[i].level, id);
                            id++;
                            PositionSinglePiece(0);
                        }
                    }
                    tiles[i].GetComponent<Tile>().isVisited = true;
                }
                else if (bank.Contains(i) && (!tiles[i].GetComponent<Tile>().isVisited))
                {
                    gold += 100;
                    tiles[i].GetComponent<Tile>().isVisited = true;
                    Vector3 position = new Vector3(-553f + (eventNotion.Count % 4 * 350), 32f - 60 * (eventNotion.Count / 4), -2f);
                    GameObject obj = textcreater.CreateTextUI(position, Pieces[i].id.ToString() + "번말 은행에서\n돈을 100G 얻다", 30, Color.black);
                    obj.SetActive(false);
                    eventNotion.Add(obj);
                }
                else if (i == sports)
                {

                    Piece piece = Pieces[i];
                    int pieceId = piece.id;



                    if (Pieces[i].level < 5)
                    {


                        if (team)
                        {
                            Pieces[i] = SpawnSinglePiece(PieceType.zombi3, team, 5, pieceId);
                            PositionSinglePiece(i);
                        }
                        else
                        {
                            Pieces[i] = SpawnSinglePiece(PieceType.army3, team, 5, pieceId);
                            PositionSinglePiece(i);
                        }
                        Pieces[i].CloneFrom(piece);
                        Destroy(piece.gameObject);
                        pieceServerMessage(Pieces[i]);
                        Vector3 position = new Vector3(-553f + (eventNotion.Count % 4 * 350), 32f - 60 * (eventNotion.Count / 4), -2f);
                        GameObject obj = textcreater.CreateTextUI(position, Pieces[i].id.ToString() + "번말 스포츠컴플렉스에서\n헬창이 되다", 30, Color.black);
                        obj.SetActive(false);
                        eventNotion.Add(obj);
                    }
                    else if (Pieces[i].level > 7)
                    {
                        if (team)
                        {
                            Pieces[i] = SpawnSinglePiece(PieceType.zombi3, team, 7, pieceId);
                            PositionSinglePiece(i);
                        }
                        else
                        {
                            Pieces[i] = SpawnSinglePiece(PieceType.army3, team, 7, pieceId);
                            PositionSinglePiece(i);
                        }
                        Pieces[i].CloneFrom(piece);
                        Destroy(piece.gameObject);
                        pieceServerMessage(Pieces[i]);
                        Vector3 position = new Vector3(-553f + (eventNotion.Count % 4 * 350), 32f - 60 * (eventNotion.Count / 4), -2f);
                        GameObject obj = textcreater.CreateTextUI(position, Pieces[i].id.ToString() + "번말 스포츠컴플렉스에서\n헬창이 되다", 30, Color.black);
                        obj.SetActive(false);
                        eventNotion.Add(obj);
                    }
                }
                else if (store.Contains(i) && (!tiles[i].GetComponent<Tile>().isVisited))
                {
                    Vector3 position = new Vector3(-553f + (eventNotion.Count % 4 * 350), 32f - 60 * (eventNotion.Count / 4), -2f);
                    GameObject obj = textcreater.CreateTextUI(position, Pieces[i].id.ToString() + "번말은\n매점을 털어", 30, Color.black);
                    obj.SetActive(false);
                    eventNotion.Add(obj);

                    switch (rnd % 4)
                    {
                        case 0:
                            mine++;
                            mineText.text = mine.ToString();
                            ZmineText.text = mine.ToString();
                            rnd++;
                            break;
                        case 1:
                            illusion++;
                            illusionText.text = illusion.ToString();
                            ZillusionText.text = illusion.ToString();
                            rnd++;
                            break;
                        case 2:
                            bigbomb++;
                            bigbombText.text = bigbomb.ToString();
                            ZbigbombText.text = bigbomb.ToString();
                            rnd++;
                            break;
                        case 3:
                            gps++;
                            gpsText.text = gps.ToString();
                            ZgpsText.text = gps.ToString();
                            rnd++;
                            break;
                    }
                    tiles[i].GetComponent<Tile>().isVisited = true;
                }
                else if (i == raiboLab && (!tiles[i].GetComponent<Tile>().isVisited))
                {
                    Pieces[i].haveItem.Add(itemType.raibo);
                    tiles[i].GetComponent<Tile>().isVisited = true;

                    Vector3 position = new Vector3(-553f + (eventNotion.Count % 4 * 350), 32f - 60 * (eventNotion.Count / 4), -2f);
                    GameObject obj = textcreater.CreateTextUI(position, Pieces[i].id.ToString() + "번말 라이보 Lab에서\n라이보를 훔침", 30, Color.black);
                    obj.SetActive(false);
                    eventNotion.Add(obj);
                    
                }
                else if (i == domitory)
                {
                    if (Pieces[i].inDomitory)
                    {
                        Pieces[i].inDomitory = false;


                    }
                    else
                    {
                        Pieces[i].inDomitory = true;
                        Pieces[i].turnMove = false;
                        
                        Vector3 position = new Vector3(-553f + (eventNotion.Count%4 * 350), 32f -60*(eventNotion.Count/4), -2f);
                        GameObject obj = textcreater.CreateTextUI(position, Pieces[i].id.ToString() + "번말 기숙사 침대를\n못 참고 한턴 숙면", 30, Color.black);
                        obj.SetActive(false);
                        eventNotion.Add(obj);
                    }
                }
                else if (gasLab.Contains(i) && (!tiles[i].GetComponent<Tile>().isVisited))
                {
                    if (team)
                    {
                        Pieces[i].haveItem.Add(itemType.advantage);
                        tiles[i].GetComponent<Tile>().isVisited = true;

                        Vector3 position = new Vector3(-553f + (eventNotion.Count%4 * 350), 32f -60*(eventNotion.Count/4), -2f);
                        GameObject obj = textcreater.CreateTextUI(position, Pieces[i].id.ToString() + "번말 가스폭탄을\n주웠다", 30, Color.black);
                        obj.SetActive(false);
                        eventNotion.Add(obj);
                    }
                }
                else if (lab.Contains(i) && (!tiles[i].GetComponent<Tile>().isVisited))
                {
                    if (!team)
                    {
                        Pieces[i].haveItem.Add(itemType.advantage);
                        tiles[i].GetComponent<Tile>().isVisited = true;

                        Vector3 position = new Vector3(-553f + (eventNotion.Count%4 * 350), 32f -60*(eventNotion.Count/4), -2f);
                        GameObject obj = textcreater.CreateTextUI(position, Pieces[i].id.ToString() + "번말 Lab에서\n백신을 맞다.", 30, Color.black);
                        obj.SetActive(false);
                        eventNotion.Add(obj);
                    }
                }

            }
        }
    }

    public void AdminWork()
    {
        prevGold = gold;
        gold = 10000;
    }

    public void UnAdminWork()
    {
        gold = prevGold;
    }

    public void DeleteAllItem()
    {
        usingItem = UsingType.deleteItem;
        gameUI.ReturnMain();
    }

    public void DeletePiece()
    {
        usingItem = UsingType.deletePiece;
        gameUI.ReturnMain();
    }

    private void RegisterEvents()
    {
        NetUtility.S_TURN_END += OnTurnEndServer;
        NetUtility.C_TURN_END += OnTurnEndClient;
        NetUtility.S_MAKE_MOVE += OnMakeMoveServer;
        NetUtility.C_MAKE_MOVE += OnMakeMoveClient;
        NetUtility.S_BATTLE_END += OnBattleEndServer;
        NetUtility.C_BATTLE_END += OnBattleEndClient;
        NetUtility.S_BIG_BOMB += OnBigBombServer;
        NetUtility.C_BIG_BOMB += OnBigBombClient;
        NetUtility.S_TILE_EVENT += OnTileEventServer;
        NetUtility.C_TILE_EVENT += OnTileEventClient;
        NetUtility.S_MONEY += OnFinalMoneyServer;
        NetUtility.C_MONEY += OnFinalMoneyClient;

    }

    private void UnRegisterEvents()
    {
        NetUtility.S_TURN_END -= OnTurnEndServer;
        NetUtility.C_TURN_END -= OnTurnEndClient;
        NetUtility.S_MAKE_MOVE -= OnMakeMoveServer;
        NetUtility.C_MAKE_MOVE -= OnMakeMoveClient;
        NetUtility.S_BATTLE_END -= OnBattleEndServer;
        NetUtility.C_BATTLE_END -= OnBattleEndClient;
        NetUtility.S_BIG_BOMB -= OnBigBombServer;
        NetUtility.C_BIG_BOMB -= OnBigBombClient;
        NetUtility.S_TILE_EVENT -= OnTileEventServer;
        NetUtility.C_TILE_EVENT -= OnTileEventClient;
        NetUtility.S_MONEY -= OnFinalMoneyServer;
        NetUtility.C_MONEY -= OnFinalMoneyClient;
    }

    // Server
    private void OnTurnEndServer(NetMessage msg, NetworkConnection cnn)
    {
        // Client has connected, assign a team and return the message back to him
        NetTurnEnd nw = msg as NetTurnEnd;

        // // Assign a team
        // nw.AssignedTeam = ++playerCount;

        // Return back to the client
        turnCount++;


        if (turnCount % 2 == 1)
        {
            Server.Instance.Broadcast(new NetTurnEnd());
        }
    }

    // Client
    private void OnTurnEndClient(NetMessage msg)
    {
        // Receive the connection message
        NetTurnEnd nw = msg as NetTurnEnd;

        startBattle = true;
        // Assign the team
        // currentTeam = nw.AssignedTeam;

        // Debug.Log($"My assigned team is {nw.AssignedTeam}");
    }

    private void OnMakeMoveServer(NetMessage msg, NetworkConnection cnn)
    {
        NetMakeMove nw = msg as NetMakeMove;
        Server.Instance.Broadcast(nw);
    }

    private void OnMakeMoveClient(NetMessage msg)
    {
        NetMakeMove nw = msg as NetMakeMove;
        // Debug.Log(team);
        
        if (nw.team != team)
        {
            PieceDTO piece = new PieceDTO(nw.tileNum, nw.level, nw.armor, nw.reverse, nw.aidKit, nw.raibo, nw.advantage, nw.team, nw.hiding, nw.isIllusion);
            OppositePieces[nw.tileNum] = piece;

            if (nw.delete)
            {
                OppositePieces[nw.tileNum] = null;
            }
        }
    }

    private void OnBattleEndServer(NetMessage msg, NetworkConnection cnn)
    {
        // Client has connected, assign a team and return the message back to him
        NetBattleEnd nw = msg as NetBattleEnd;
        Server.Instance.Broadcast(nw);
    }

    // Client
    private void OnBattleEndClient(NetMessage msg)
    {
        NetBattleEnd nw = msg as NetBattleEnd;
        // Receive the connection message

        if (nw.team != team)
            oppoReady = true;


    }

    private void OnBigBombServer(NetMessage msg, NetworkConnection cnn)
    {
        // Client has connected, assign a team and return the message back to him
        NetBigBomb nw = msg as NetBigBomb;
        Server.Instance.Broadcast(nw);
    }

    // Client
    private void OnBigBombClient(NetMessage msg)
    {
        NetBigBomb nw = msg as NetBigBomb;
        // Receive the connection message
        if (nw.team != team)
            ItemEffects.Add(new ItemInstance("bigbomb", 3, nw.tileIndex));
    }

    private void OnTileEventServer(NetMessage msg, NetworkConnection cnn)
    {
        NetTileEvent nw = msg as NetTileEvent;
        Server.Instance.Broadcast(nw);
    }

    private void OnTileEventClient(NetMessage msg)
    {
        NetTileEvent nw = msg as NetTileEvent;
        // Debug.Log(team);
        if (nw.team != team)
        {
            tiles[nw.tileIndex].GetComponent<Tile>().oppoIsMine = nw.isMine;
            tiles[nw.tileIndex].GetComponent<Tile>().isVisited = nw.isVisited;
        }
        if (nw.bomb)
        {
            tiles[nw.tileIndex].GetComponent<Tile>().oppoIsMine = false;
            tiles[nw.tileIndex].GetComponent<Tile>().isMine = false;
        }
    }

    private void OnFinalMoneyServer(NetMessage msg, NetworkConnection cnn)
    {
        // Client has connected, assign a team and return the message back to him
        NetFinalMoney nw = msg as NetFinalMoney;
        Server.Instance.Broadcast(nw);
    }

    // Client
    private void OnFinalMoneyClient(NetMessage msg)
    {
        NetFinalMoney nw = msg as NetFinalMoney;
        // Receive the connection message
        if (nw.team != team)
            oppoGold = nw.gold;
    }
    IEnumerator PauseThreeSecond()
    {
        // 1초 쉽니다.
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < TILE_NUMBER; i++)
        {

            if (Pieces[i] != null)
            {
                NetMakeMove mm = new NetMakeMove();
                mm.tileNum = Pieces[i].currentN;
                mm.level = Pieces[i].level;
                mm.delete = false;
                mm.armor = false;
                if (Pieces[i].haveItem.Contains(itemType.armor))
                {
                    mm.armor = true;
                }
                mm.reverse = false;
                if (Pieces[i].haveItem.Contains(itemType.reverse))
                {
                    mm.reverse = true;
                }
                mm.aidKit = false;
                if (Pieces[i].haveItem.Contains(itemType.aidKit))
                {
                    mm.aidKit = true;
                }
                mm.raibo = false;
                if (Pieces[i].haveItem.Contains(itemType.raibo))
                {
                    mm.raibo = true;
                }
                mm.advantage = false;
                if (Pieces[i].haveItem.Contains(itemType.advantage))
                {
                    mm.advantage = true;
                }
                mm.team = team;
                mm.hiding = Pieces[i].hiding;
                mm.isIllusion = Pieces[i].isIllusion;

                Client.Instance.SendToServer(mm);



            }

        }
        yield return new WaitForSeconds(3f);
        for (int i = 0; i < TILE_NUMBER; i++)
        {
            if (tiles[i].GetComponent<Tile>().isMine || tiles[i].GetComponent<Tile>().isVisited)
            {
                NetTileEvent mm = new NetTileEvent();
                mm.tileIndex = i;
                mm.team = team;
                // Debug.Log("여기는 이제 어 그래." + i.ToString());
                mm.isMine = tiles[i].GetComponent<Tile>().isMine;
                mm.isVisited = tiles[i].GetComponent<Tile>().isVisited;
                mm.bomb = false;
                Client.Instance.SendToServer(mm);
            }
        }
        yield return new WaitForSeconds(2f);
        int temp = 0;
        for (int i = 0; i < TILE_NUMBER; i++)
        {
            if (OppositePieces[i] != null && Pieces[i] != null)
            {
                if ((!OppositePieces[i].isIllusion) && (!Pieces[i].isIllusion)) {
                    Vector3 position = new Vector3(-553f + (eventNotion.Count % 4 * 350), 32f - 60 * (eventNotion.Count / 4), -2f);
                    GameObject obj = textcreater.CreateTextUI(position, i.ToString() + "번 땅에서\n전투발생", 30, Color.black);
                    obj.SetActive(false);
                    eventNotion.Add(obj);
                    Battle(Pieces[i], OppositePieces[i]);
                }
                    
                else if (OppositePieces[i].isIllusion && (!Pieces[i].isIllusion))
                {
                    OppositePieces[i] = null;
                }
                yield return new WaitForSeconds(10f);
                temp++;
            }

        }
        NetBattleEnd mw = new NetBattleEnd();
        ready = true;
        mw.team = team;
        Client.Instance.SendToServer(mw);


    }

    IEnumerator BattleAnimation(Piece ourPc, PieceDTO oppoPc)
    {
        Vector3 pos = mainCamera.transform.position;
        yield return new WaitForSeconds(3f);
        int c = 0;
        if (oppoPc.armor)
        {
            GameObject armorObj = Instantiate(armorPrefab);
            if (team)
            {
                armorObj.transform.localPosition = new Vector3(-9f + (c * 2f) + pos.x, 6f + pos.y, -1);
            }
            else
            {
                armorObj.transform.localPosition = new Vector3(6f + (c * 2f) + pos.x, 6f + pos.y, -1);
            }
            c++;
            spawnedObjects.Add(armorObj);
        }
        if (oppoPc.aidKit)
        {
            GameObject aidKitObj = Instantiate(aidKitPrefab);
            if (team)
            {
                aidKitObj.transform.localPosition = new Vector3(-9f + (c * 2f) + pos.x, 6f + pos.y, -1);
            }
            else
            {
                aidKitObj.transform.localPosition = new Vector3(6f + (c * 2f) + pos.x, 6f + pos.y, -1);
            }
            c++;
            spawnedObjects.Add(aidKitObj);
        }
        if (oppoPc.raibo)
        {
            GameObject raiboObj = Instantiate(raiboPrefab);
            raiboObj.transform.localPosition = new Vector3(11f + pos.x, -5f + pos.y, -1);
            if (team)
            {
                raiboObj.transform.localPosition = new Vector3(-11f + pos.x, -5f + pos.y, -1);
            }
            spawnedObjects.Add(raiboObj);
        }
        if (oppoPc.advantage)
        {
            if (!team)
            {
                GameObject gasBombObj = Instantiate(gasBombPrefab);
                gasBombObj.transform.localPosition = new Vector3(6f + (c) + pos.x, 6f + pos.y, -1);
                spawnedObjects.Add(gasBombObj);
            }
            else
            {

                GameObject vaccineObj = Instantiate(vaccinePrefab);
                vaccineObj.transform.localPosition = new Vector3(-9f + (c) + pos.x, 6f + pos.y, -1);
                spawnedObjects.Add(vaccineObj);
            }
            // 미리 만들어둔 이미지 프리팹
            c++;
        }
        if (!team)
        {
            GameObject textobj2 = textcreater.CreateTextUI(new Vector2(605f, -335f), "Lv " + oppoPc.level.ToString());
            spawnedObjects.Add(textobj2);
        }
        else
        {
            GameObject textobj2 = textcreater.CreateTextUI(new Vector2(-605f, -335f), "Lv " + oppoPc.level.ToString());
            spawnedObjects.Add(textobj2);
        }
        yield return new WaitForSeconds(3f);
        if (oppoPc.reverse)
        {
            GameObject reverseObj = Instantiate(reversePrefab);
            reverseObj.transform.localPosition = new Vector3(-8f + pos.x, pos.y, -2);
            if (!team)
            {
                reverseObj.transform.localPosition = new Vector3(8f + pos.x, pos.y, -2);
            }
            spawnedObjects.Add(reverseObj);
        }
        yield return new WaitForSeconds(1.5f);

        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }
        // 리스트 초기화
        spawnedObjects.Clear();

        if (whoWin(ourPc, oppoPc))
        {

            if (!team)
            {
                if (oppoPc.level > 10)
                {
                    gold += 200;
                }
                else if (oppoPc.level > 7)
                {
                    gold += 120;
                }
                else if (oppoPc.level > 4)
                {
                    gold += 80;
                }
                else if (oppoPc.level > 2)
                {
                    gold += 50;
                }
                else
                {
                    gold += 20;
                }

            }
            else
            {
                if (oppoPc.level > 10)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        int idx = ourPc.currentN;
                        int pieceLv = ourPc.level;
                        PieceType pieceType = ourPc.type;
                        int pieceId = ourPc.id;
                        if (pieceLv == 2 || pieceLv == 4 || pieceLv == 7 || pieceLv == 10)
                        {
                            Piece temp = SpawnSinglePiece(pieceType + 1, team, pieceLv + 1, pieceId);
                            temp.CloneFrom(ourPc);
                            Destroy(ourPc.gameObject);
                            Pieces[idx] = temp;
                            ourPc = Pieces[idx];
                            PositionSinglePiece(idx);
                        }
                        else
                        {
                            ourPc.SetLevel(pieceLv + 1);
                        }
                    }

                }
                else if (oppoPc.level > 7)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        int idx = ourPc.currentN;
                        int pieceLv = ourPc.level;
                        PieceType pieceType = ourPc.type;
                        int pieceId = ourPc.id;
                        if (pieceLv == 2 || pieceLv == 4 || pieceLv == 7 || pieceLv == 10)
                        {
                            Piece temp = SpawnSinglePiece(pieceType + 1, team, pieceLv + 1, pieceId);
                            temp.CloneFrom(ourPc);
                            Destroy(ourPc.gameObject);
                            Pieces[idx] = temp;
                            ourPc = Pieces[idx];
                            PositionSinglePiece(idx);
                        }
                        else
                        {
                            ourPc.SetLevel(pieceLv + 1);
                        }
                    }
                }
                else if (oppoPc.level > 4)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        int idx = ourPc.currentN;
                        int pieceLv = ourPc.level;
                        PieceType pieceType = ourPc.type;
                        int pieceId = ourPc.id;
                        if (pieceLv == 2 || pieceLv == 4 || pieceLv == 7 || pieceLv == 10)
                        {
                            Piece temp = SpawnSinglePiece(pieceType + 1, team, pieceLv + 1, pieceId);
                            temp.CloneFrom(ourPc);
                            Destroy(ourPc.gameObject);
                            Pieces[idx] = temp;
                            ourPc = Pieces[idx];
                            PositionSinglePiece(idx);
                        }
                        else
                        {
                            ourPc.SetLevel(pieceLv + 1);
                        }
                    }
                }
                else if (oppoPc.level > 2)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        int idx = ourPc.currentN;
                        int pieceLv = ourPc.level;
                        PieceType pieceType = ourPc.type;
                        int pieceId = ourPc.id;
                        if (pieceLv == 2 || pieceLv == 4 || pieceLv == 7 || pieceLv == 10)
                        {
                            Piece temp = SpawnSinglePiece(pieceType + 1, team, pieceLv + 1, pieceId);
                            temp.CloneFrom(ourPc);
                            Destroy(ourPc.gameObject);
                            Pieces[idx] = temp;
                            ourPc = Pieces[idx];
                            PositionSinglePiece(idx);
                        }
                        else
                        {
                            ourPc.SetLevel(pieceLv + 1);
                        }
                    }
                }
                else
                {
                    int idx = ourPc.currentN;
                    int pieceLv = ourPc.level;
                    PieceType pieceType = ourPc.type;
                    int pieceId = ourPc.id;
                    if (pieceLv == 2 || pieceLv == 4 || pieceLv == 7 || pieceLv == 10)
                    {
                        Piece temp = SpawnSinglePiece(pieceType + 1, team, pieceLv + 1, pieceId);
                        temp.CloneFrom(ourPc);
                        Destroy(ourPc.gameObject);
                        Pieces[idx] = temp;
                        ourPc = Pieces[idx];
                        PositionSinglePiece(idx);
                    }
                    else
                    {
                        ourPc.SetLevel(pieceLv + 1);
                    }
                }
            }
            OppositePieces[oppoPc.tileNum] = null;
            var wantedTypes = new[] { itemType.car, itemType.aidKit };
            var foundItems = ourPc.haveItem.Where(x => wantedTypes.Contains(x)).ToList();



            ourPc.haveItem.Clear();
            if (foundItems.Any()) // 값 타입이면 (itemToKeep.HasValue)로 체크
                ourPc.haveItem.AddRange(foundItems);

            if (team)
            {
                // ZWinPage.transform.position = pos;
                // ZWinPage.SetActive(true);
                // StartCoroutine(FadeOutCoroutine(ZWinPage));
                pos.x = -60;
                pos.y = 435;
                mainCamera.transform.position = pos;
            }
            else
            {
                // armyWinPage.transform.position = pos;
                // armyWinPage.SetActive(true);
                // StartCoroutine(FadeOutCoroutine(armyWinPage));
                pos.x = -60;
                pos.y = 450;
                mainCamera.transform.position = pos;
            }

        }
        else
        {
            int temp = ourPc.currentN;
            if (ourPc.haveItem.Contains(itemType.aidKit))
            {
                ourPc.haveItem.Clear();
                aidKitPiece = ourPc;
                NetMakeMove mm = new NetMakeMove();
                mm.tileNum = ourPc.currentN;
                mm.team = team;
                mm.delete = true;
                Client.Instance.SendToServer(mm);

            }
            else
            {

                NetMakeMove mm = new NetMakeMove();
                mm.tileNum = ourPc.currentN;
                
                mm.team = team;
                mm.delete = true;
                Client.Instance.SendToServer(mm);
                Destroy(ourPc.gameObject);
            }
            Pieces[temp] = null;
            if (team)
            {
                // armyWinPage.SetActive(true);
                // armyWinPage.transform.position = pos;
                // StartCoroutine(FadeOutCoroutine(armyWinPage));
                pos.x = -60;
                pos.y = 450;
                mainCamera.transform.position = pos;
            }
            else
            {
                // ZWinPage.SetActive(true);
                // ZWinPage.transform.position = pos;

                // StartCoroutine(FadeOutCoroutine(ZWinPage));
                pos.x = -60;
                pos.y = 435;
                mainCamera.transform.position = pos;
            }

        }
        yield return new WaitForSeconds(1.5f); 
        // ZWinPage.SetActive(false);
        // ZWinPage.SetActive(false);
        pos = mainCamera.transform.position;
        pos.x = -60f;
        pos.y = 45f;
        pos.z = -10f;
        mainCamera.transform.position = pos;
        waitBattleUI.SetActive(true);

    }
    // IEnumerator FadeOutCoroutine(GameObject go)
    // {
    //     SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
    //     Color origColor = sr.color;
    //     float elapsed = 0f;
    //     while (elapsed < duration)
    //     {
    //         float alpha = Mathf.Lerp(0f, 1f, elapsed / duration);
    //         sr.color = new Color(origColor.r, origColor.g, origColor.b, alpha);
    //         elapsed += Time.deltaTime;
    //         yield return null;
    //     }
    //     yield return new WaitForSeconds(1f);
    //     sr.color = new Color(origColor.r, origColor.g, origColor.b, 0f);
    // }

    public void pieceServerMessage(Piece pc)
    {
        NetMakeMove mm = new NetMakeMove();
        mm.tileNum = pc.currentN;
        mm.level = pc.level;
        mm.delete = false;
        mm.armor = false;
        if (pc.haveItem.Contains(itemType.armor))
        {
            mm.armor = true;
        }
        mm.reverse = false;
        if (pc.haveItem.Contains(itemType.reverse))
        {
            mm.reverse = true;
        }
        mm.aidKit = false;
        if (pc.haveItem.Contains(itemType.aidKit))
        {
            mm.aidKit = true;
        }
        mm.raibo = false;
        if (pc.haveItem.Contains(itemType.raibo))
        {
            mm.raibo = true;
        }
        mm.advantage = false;
        if (pc.haveItem.Contains(itemType.advantage))
        {
            mm.advantage = true;
        }
        mm.team = team;
        mm.hiding = pc.hiding;
        mm.isIllusion = pc.isIllusion;

        Client.Instance.SendToServer(mm);
    }

    public bool whoWin(Piece ourPc, PieceDTO oppoPc)
    {
        bool temp;
        int ourLv = ourPc.level;
        int oppoLv = oppoPc.level;

        if (oppoPc.armor)
        {
            oppoLv++;
        }
        if (oppoPc.raibo)
        {
            ourLv--;
        }
        if (oppoPc.advantage)
        {
            ourLv--;
        }
        foreach (itemType item in ourPc.haveItem)
        {
            switch (item)
            {
                case itemType.armor:
                    ourLv++;
                    break;
                case itemType.raibo:
                    oppoLv--;
                    break;
                case itemType.advantage:
                    oppoLv--;
                    break;
            }
        }
        if (ourLv > oppoLv)
        {
            temp = true;
        }
        else if (ourLv < oppoLv)
        {
            temp = false;
        }
        else
        {
            if (team)
            {
                temp = true;
            }
            else
            {
                temp = false;
            }
        }
        if (oppoPc.reverse && ourPc.haveItem.Contains(itemType.reverse))
        {
            return temp;
        }
        else if ((!oppoPc.reverse) && ourPc.haveItem.Contains(itemType.reverse))
        {
            return !temp;
        }
        else if (oppoPc.reverse && (!ourPc.haveItem.Contains(itemType.reverse)))
        {
            return !temp;
        }
        else
        {
            return temp;
        }

    }

    public void GameEnding()
    {
        int sumLevelA = Pieces
                .Where(p => p != null && !p.isIllusion)
                .Sum(p => p.level);

        int sumLevelB = OppositePieces.Where(p => p != null && !p.isIllusion).Sum(p => p.level);
        Vector3 pos = mainCamera.transform.position;
        pos.z = -2;
        // 승자 판정
        if (sumLevelA > sumLevelB)
        {
            // pos.y += 15;
            finalWin.transform.position = pos;
        }
        else if (sumLevelA < sumLevelB)
        {
            // pos.y += 15;
            finalLose.transform.position = pos;
        }
        else
        {
            if (gold > oppoGold)
            {
                // pos.y += 15;
                finalWin.transform.position = pos;
            }
            else if (gold < oppoGold)
            {
                // pos.y += 15;
                finalLose.transform.position = pos;
            }
            else
            {
                GetComponent<TimedMessage>().ShowMessage("draw", 5f);
            }
        }
        
    }
}
