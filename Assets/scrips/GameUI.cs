using UnityEngine;
using TMPro;
using Unity.Networking.Transport;
using System.Net;
using System.Net.Sockets;
using System;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance { set; get; }

    public Server server;
    public Client client;

    [SerializeField] private TMP_InputField addressInput;


    public static bool team; //false이 군인 true이 좀비

    public GameObject startUI;
    public GameObject mainUI;
    public GameObject marketUI;
    public GameObject ZmarketUI;
    public GameObject ZmarketItemUI3;
    public GameObject marketItemUI3;
    public GameObject marketItemUI;
    public GameObject ZmarketItemUI;
    public GameObject itemUI;
    public GameObject ZitemUI;
    public GameObject teamInfoUI;
    public GameObject ZteamInfoUI;
    public GameObject waitingUI;
    public GameObject ZoppoInfo;
    public GameObject oppoInfo;

    public GameObject Board;
    public board board;
    public GameObject displayUI;
    public GameObject confirmImg;
    public GameObject ZconfirmImg;


    public GameObject confirmUI;
    private Camera mainCamera;
    private System.Action onConfirm;
    public int playerCount = -1;
    public int mine = 0;
    public int illusion = 0;
    public int gps = 0;
    public int bigbomb = 0;
    public int changePlace = 0;
    public int reverse = 0;
    public int teleport = 0;
    public int armor = 0;
    public int drone = 0;
    public int car = 0;
    public bool turnEnd = false;
    public OppoInfo oppo;
    public OppoInfo Zoppo;



    private void Awake()
    {
        Instance = this;
        mainCamera = Camera.main;
        RegisterEvents();
    }
    private void Update()
    {
        mainCamera = Camera.main;
        if (turnEnd)
        {
            turnEnd = false;
            NetOppoItem mm = new NetOppoItem();

            mm.team = team;
            mm.mine = mine;
            mm.illusion = illusion;
            mm.gps = gps;
            mm.bigbomb = bigbomb;
            mm.changePlace = changePlace;
            mm.reverse = reverse;
            mm.teleport = teleport;
            mm.armor = armor;
            mm.drone = drone;
            mm.car = car;

            Client.Instance.SendToServer(mm);
        }
    }

    //start page
    public void OnHostButton()
    {
        string localIP = GetLocalIPAddress();
        Debug.Log("Server IP: " + localIP);
        server.Init(8007);
        client.Init(localIP.ToString(), 8007);
        GetComponent<TimedMessage>().ShowMessage(localIP.ToString(), 60f);

        Vector3 pos = mainCamera.transform.position;
        pos.y = 45;
        pos.x = -60;
        mainCamera.transform.position = pos;

        waitingUI.SetActive(true);
        startUI.SetActive(false);
        team = false;
        // AStart();


    }

    public void OnConnectButton()
    {

        client.Init(addressInput.text, 8007);
        team = true;
        // ZStart();
    }

    public void ZStart()
    {
        Vector3 pos = mainCamera.transform.position;
        pos.y = 0;
        pos.x = 30;
        mainCamera.transform.position = pos;

        Board.SetActive(true);
        startUI.SetActive(false);
        mainUI.SetActive(true);
        displayUI.SetActive(true);
    }

    public void AStart()
    {
        Vector3 pos = mainCamera.transform.position;
        pos.y = 0;
        pos.x = 0;
        mainCamera.transform.position = pos;

        Board.SetActive(true);
        startUI.SetActive(false);
        waitingUI.SetActive(false);
        mainUI.SetActive(true);
        displayUI.SetActive(true);
    }

    public void OnBackButton()
    {
        GetComponent<TimedMessage>().HideMessage();
        server.Shutdown();
        client.Shutdown();
        Vector3 pos = mainCamera.transform.position;
        pos.y = 0;
        pos.x = -30;
        mainCamera.transform.position = pos;
        waitingUI.SetActive(false);
        startUI.SetActive(true);
    }


    //main page
    public void OppoInfoButton()
    {
        // 기존 게임 로직을 여기에 작성
        // Debug.Log("버튼이 눌렸습니다! 기존 코드 실행");
        // 예: 말 이동, 턴 넘기기, UI 갱신 등
        if (board.currentlyDragging == null && (!board.bombing) && (board.changing == null) && (!board.mining) && (board.usingItem == 0) && (!board.hiding))
        {
            Vector3 pos = mainCamera.transform.position;
            pos.x = -30;
            if (team)
            {
                pos.y = 210;
                mainUI.SetActive(false);


                ZoppoInfo.SetActive(true);

            }
            else
            {
                pos.y = 195;
                mainUI.SetActive(false);
                oppoInfo.SetActive(true);
            }
            mainCamera.transform.position = pos;
        }
        else
        {
            GetComponent<TimedMessage>().ShowMessage("finish another action", 2f);
        }
    }

    public void OppoInfoButtonNother()
    {
        ReturnMain();
        OppoInfoButton();
    }

    public void OnMarketButtonClick()
    {
        // 기존 게임 로직을 여기에 작성
        // Debug.Log("버튼이 눌렸습니다! 기존 코드 실행");
        // 예: 말 이동, 턴 넘기기, UI 갱신 등
        if (board.currentlyDragging == null && (!board.bombing) && (board.changing == null) && (!board.mining) && (board.usingItem == 0) && (!board.hiding))
        {
            Vector3 pos = mainCamera.transform.position;
            pos.x = -30;
            if (team)
            {
                pos.y = 75;
                mainUI.SetActive(false);


                ZmarketUI.SetActive(true);

            }
            else
            {
                pos.y = 15;
                mainUI.SetActive(false);
                marketUI.SetActive(true);
            }
            if (board.round > 2)
            {
                pos.y += 30;
            }


            mainCamera.transform.position = pos;
        }
        else
        {
            GetComponent<TimedMessage>().ShowMessage("finish another action", 2f);
        }



    }

    public void OnMarketButtonClickNother()
    {
        ReturnMain();
        OnMarketButtonClick();
    }

    public void TeamInfoButton()
    {
        if (board.currentlyDragging == null && (!board.bombing) && (board.changing == null) && (!board.mining) && (board.usingItem == 0) && (!board.hiding))
        {
            Vector3 pos = mainCamera.transform.position;
            pos.x = -30;
            if (team)
            {
                pos.y = 180;
                mainUI.SetActive(false);


                ZteamInfoUI.SetActive(true);

            }
            else
            {
                pos.y = 165;
                mainUI.SetActive(false);
                teamInfoUI.SetActive(true);
            }

            mainCamera.transform.position = pos;
        }
        else
        {
            GetComponent<TimedMessage>().ShowMessage("finish another action", 2f);
        }
    }

    public void TeamInfoButtonNother()
    {
        ReturnMain();
        TeamInfoButton();
    }

    public void CheckItemPage()
    {
        if (board.currentlyDragging == null && (!board.bombing) && (board.changing == null) && (!board.mining) && (board.usingItem == 0) && (!board.hiding))
        {
            if (board.currentlyDragging == null && (!board.bombing))
            {
                Vector3 pos = mainCamera.transform.position;
                pos.x = -30;
                pos.y = 135;
                if (team)
                {
                    pos.y += 15;
                }
                mainUI.SetActive(false);
                if (team)
                {
                    ZitemUI.SetActive(true);
                }
                else
                {
                    itemUI.SetActive(true);
                }

                mainCamera.transform.position = pos;
            }
        }
        else
        {
            GetComponent<TimedMessage>().ShowMessage("finish another action", 2f);
        }


    }

    public void CheckItemPageNother()
    {
        ReturnMain();
        CheckItemPage();
    }

    //market

    public void ItemPage()
    {
        Vector3 pos = mainCamera.transform.position;
        pos.y += 15;
        mainCamera.transform.position = pos;

        marketUI.SetActive(false);
        ZmarketUI.SetActive(false);
        if (board.round > 2)
        {
            if (team)
            {
                ZmarketItemUI3.SetActive(true);
            }
            else
            {
                marketItemUI3.SetActive(true);
            }


        }
        else
        {
            if (team)
            {
                ZmarketItemUI.SetActive(true);
            }
            else
            {
                marketItemUI.SetActive(true);
            }


        }



    }

    public void PiecePage()
    {
        Vector3 pos = mainCamera.transform.position;
        pos.y -= 15;
        mainCamera.transform.position = pos;

        if (team)
        {
            ZmarketUI.SetActive(true);
        }
        else
        {
            marketUI.SetActive(true);
        }


        if (board.round > 2)
        {
            marketItemUI3.SetActive(false);
            ZmarketItemUI3.SetActive(false);
        }
        else
        {
            marketItemUI.SetActive(false);
            ZmarketItemUI.SetActive(false);
        }

    }

    public void GeneratePiece()
    {
        if (board.gold >= 30)
        {
            marketItemUI.SetActive(false);
            marketItemUI3.SetActive(false);
            ZmarketItemUI.SetActive(false);
            ZmarketItemUI3.SetActive(false);
            // 팝업을 띄우고, 안내문을 설정
            confirmUI.SetActive(true);
            if (team)
            {
                ZconfirmImg.SetActive(true);
                Vector3 pos = mainCamera.transform.position;
                pos.z = -1;
                ZconfirmImg.transform.position = pos;
            }
            else
            {
                confirmImg.SetActive(true);
                Vector3 pos = mainCamera.transform.position;
                pos.z = -1;
                confirmImg.transform.position = pos;
            }

            // Debug.Log("팝업 띄우자");
            // confirmText.text = "정말 구매를 하시겠습니까?";
            // 확인 버튼 클릭 시 실행할 구매 함수 지정
            onConfirm = board.GeneratePiece;
        }
        else
        {
            GetComponent<TimedMessage>().ShowMessage("short of money", 1f);
        }
    }

    #region market
    public void BuyMine()
    {
        if (board.gold >= 20)
        {
            marketItemUI.SetActive(false);
            marketItemUI3.SetActive(false);
            ZmarketItemUI.SetActive(false);
            ZmarketItemUI3.SetActive(false);
            confirmUI.SetActive(true);
            if (team)
            {
                ZconfirmImg.SetActive(true);
                Vector3 pos = mainCamera.transform.position;
                pos.z = -1;
                ZconfirmImg.transform.position = pos;
            }
            else
            {
                confirmImg.SetActive(true);
                Vector3 pos = mainCamera.transform.position;
                pos.z = -1;
                confirmImg.transform.position = pos;
            }
            onConfirm = GetMine;
        }
        else
        {
            GetComponent<TimedMessage>().ShowMessage("short of money", 1f);
        }

    }

    public void GetMine()
    {
        board.gold -= 20;
        board.mine++;
        mine++;
        board.mineText.text = board.mine.ToString();
        board.ZmineText.text = board.mine.ToString();
    }

    public void BuyIllusion()
    {
        if (board.gold >= 10)
        {
            marketItemUI.SetActive(false);
            marketItemUI3.SetActive(false);
            ZmarketItemUI.SetActive(false);
            ZmarketItemUI3.SetActive(false);
            confirmUI.SetActive(true);
            if (team)
            {
                ZconfirmImg.SetActive(true);
                Vector3 pos = mainCamera.transform.position;
                pos.z = -1;
                ZconfirmImg.transform.position = pos;
            }
            else
            {
                confirmImg.SetActive(true);
                Vector3 pos = mainCamera.transform.position;
                pos.z = -1;
                confirmImg.transform.position = pos;
            }
            onConfirm = GetIllusion;
        }
        else
        {
            GetComponent<TimedMessage>().ShowMessage("short of money", 1f);
        }
    }

    public void GetIllusion()
    {
        board.gold -= 10;
        board.illusion++;
        illusion++;
        board.illusionText.text = board.illusion.ToString();
        board.ZillusionText.text = board.illusion.ToString();
    }

    public void BuyGps()
    {
        if (board.gold >= 20)
        {
            marketItemUI.SetActive(false);
            marketItemUI3.SetActive(false);
            ZmarketItemUI.SetActive(false);
            ZmarketItemUI3.SetActive(false);
            confirmUI.SetActive(true);
            if (team)
            {
                ZconfirmImg.SetActive(true);
                Vector3 pos = mainCamera.transform.position;
                pos.z = -1;
                ZconfirmImg.transform.position = pos;
            }
            else
            {
                confirmImg.SetActive(true);
                Vector3 pos = mainCamera.transform.position;
                pos.z = -1;
                confirmImg.transform.position = pos;
            }
            onConfirm = GetGps;
        }
        else
        {
            GetComponent<TimedMessage>().ShowMessage("short of money", 1f);
        }
    }

    public void GetGps()
    {
        board.gold -= 20;
        board.gps++;
        gps++;
        board.gpsText.text = board.gps.ToString();
        board.ZgpsText.text = board.gps.ToString();
    }

    public void BuyBigbomb()
    {
        if (board.gold >= 20)
        {
            marketItemUI.SetActive(false);
            marketItemUI3.SetActive(false);
            ZmarketItemUI.SetActive(false);
            ZmarketItemUI3.SetActive(false);
            confirmUI.SetActive(true);
            if (team)
            {
                ZconfirmImg.SetActive(true);
                Vector3 pos = mainCamera.transform.position;
                pos.z = -1;
                ZconfirmImg.transform.position = pos;
            }
            else
            {
                confirmImg.SetActive(true);
                Vector3 pos = mainCamera.transform.position;
                pos.z = -1;
                confirmImg.transform.position = pos;
            }
            onConfirm = GetBigbomb;
        }
        else
        {
            GetComponent<TimedMessage>().ShowMessage("short of money", 1f);
        }
    }

    public void GetBigbomb()
    {
        board.gold -= 20;
        board.bigbomb++;
        bigbomb++;
        board.bigbombText.text = board.bigbomb.ToString();
        board.ZbigbombText.text = board.bigbomb.ToString();
    }

    public void BuyChangePlace()
    {
        if (board.gold >= 60)
        {
            marketItemUI.SetActive(false);
            marketItemUI3.SetActive(false);
            ZmarketItemUI.SetActive(false);
            ZmarketItemUI3.SetActive(false);
            confirmUI.SetActive(true);
            if (team)
            {
                ZconfirmImg.SetActive(true);
                Vector3 pos = mainCamera.transform.position;
                pos.z = -1;
                ZconfirmImg.transform.position = pos;
            }
            else
            {
                confirmImg.SetActive(true);
                Vector3 pos = mainCamera.transform.position;
                pos.z = -1;
                confirmImg.transform.position = pos;
            }
            onConfirm = GetChangePlace;
        }
        else
        {
            GetComponent<TimedMessage>().ShowMessage("short of money", 1f);
        }
    }

    public void GetChangePlace()
    {
        board.gold -= 60;
        board.changePlace++;
        changePlace++;
        board.changePlaceText.text = board.changePlace.ToString();
        board.ZchangePlaceText.text = board.changePlace.ToString();
    }

    public void BuyReverse()
    {
        if (board.gold >= 130)
        {
            marketItemUI.SetActive(false);
            marketItemUI3.SetActive(false);
            ZmarketItemUI.SetActive(false);
            ZmarketItemUI3.SetActive(false);
            confirmUI.SetActive(true);
            if (team)
            {
                ZconfirmImg.SetActive(true);
                Vector3 pos = mainCamera.transform.position;
                pos.z = -1;
                ZconfirmImg.transform.position = pos;
            }
            else
            {
                confirmImg.SetActive(true);
                Vector3 pos = mainCamera.transform.position;
                pos.z = -1;
                confirmImg.transform.position = pos;
            }
            onConfirm = GetReverse;
        }
        else
        {
            GetComponent<TimedMessage>().ShowMessage("short of money", 1f);
        }
    }

    public void GetReverse()
    {
        board.gold -= 130;
        board.reverse++;
        reverse++;
        board.reverseText.text = board.reverse.ToString();
        board.ZreverseText.text = board.reverse.ToString();
    }

    public void BuyTeleport()
    {
        if (board.gold >= 40)
        {
            marketItemUI.SetActive(false);
            marketItemUI3.SetActive(false);
            ZmarketItemUI.SetActive(false);
            ZmarketItemUI3.SetActive(false);
            confirmUI.SetActive(true);
            if (team)
            {
                ZconfirmImg.SetActive(true);
                Vector3 pos = mainCamera.transform.position;
                pos.z = -1;
                ZconfirmImg.transform.position = pos;
            }
            else
            {
                confirmImg.SetActive(true);
                Vector3 pos = mainCamera.transform.position;
                pos.z = -1;
                confirmImg.transform.position = pos;
            }
            onConfirm = GetTeleport;
        }
        else
        {
            GetComponent<TimedMessage>().ShowMessage("short of money", 1f);
        }
    }

    public void GetTeleport()
    {
        board.gold -= 40;
        board.teleport++;
        teleport++;
        board.teleportText.text = board.teleport.ToString();
        board.ZteleportText.text = board.teleport.ToString();
    }

    public void BuyArmor()
    {
        if (board.gold >= 20)
        {
            marketItemUI.SetActive(false);
            marketItemUI3.SetActive(false);
            ZmarketItemUI.SetActive(false);
            ZmarketItemUI3.SetActive(false);
            confirmUI.SetActive(true);
            if (team)
            {
                ZconfirmImg.SetActive(true);
                Vector3 pos = mainCamera.transform.position;
                pos.z = -1;
                ZconfirmImg.transform.position = pos;
            }
            else
            {
                confirmImg.SetActive(true);
                Vector3 pos = mainCamera.transform.position;
                pos.z = -1;
                confirmImg.transform.position = pos;
            }
            onConfirm = GetArmor;
        }
        else
        {
            GetComponent<TimedMessage>().ShowMessage("short of money", 1f);
        }
    }

    public void GetArmor()
    {
        board.gold -= 20;
        board.armor++;
        armor++;
        board.armorText.text = board.armor.ToString();
    }

    public void BuyDrone()
    {
        if (board.gold >= 35)
        {
            marketItemUI.SetActive(false);
            marketItemUI3.SetActive(false);
            ZmarketItemUI.SetActive(false);
            ZmarketItemUI3.SetActive(false);
            confirmUI.SetActive(true);
            if (team)
            {
                ZconfirmImg.SetActive(true);
                Vector3 pos = mainCamera.transform.position;
                pos.z = -1;
                ZconfirmImg.transform.position = pos;
            }
            else
            {
                confirmImg.SetActive(true);
                Vector3 pos = mainCamera.transform.position;
                pos.z = -1;
                confirmImg.transform.position = pos;
            }
            onConfirm = GetDrone;
        }
        else
        {
            GetComponent<TimedMessage>().ShowMessage("short of money", 1f);
        }
    }

    public void GetDrone()
    {
        board.gold -= 35;
        board.drone++;
        drone++;
        board.droneText.text = board.drone.ToString();
    }

    public void BuyCar()
    {
        if (board.gold >= 50)
        {
            marketItemUI.SetActive(false);
            marketItemUI3.SetActive(false);
            ZmarketItemUI.SetActive(false);
            ZmarketItemUI3.SetActive(false);
            confirmUI.SetActive(true);
            if (team)
            {
                ZconfirmImg.SetActive(true);
                Vector3 pos = mainCamera.transform.position;
                pos.z = -1;
                ZconfirmImg.transform.position = pos;
            }
            else
            {
                confirmImg.SetActive(true);
                Vector3 pos = mainCamera.transform.position;
                pos.z = -1;
                confirmImg.transform.position = pos;
            }
            onConfirm = GetCar;
        }
        else
        {
            GetComponent<TimedMessage>().ShowMessage("short of money", 1f);
        }
    }

    public void GetCar()
    {
        board.gold -= 50;
        board.car++;
        car++;
        board.carText.text = board.car.ToString();
    }
    #endregion
    public void OnConfirmButtonClick()
    {

        if (team)
        {
            if (board.round > 2)
            {
                ZmarketItemUI3.SetActive(true);
            }
            else
            {
                ZmarketItemUI.SetActive(true);
            }
        }
        else
        {
            if (board.round > 2)
            {
                marketItemUI3.SetActive(true);
            }
            else
            {
                marketItemUI.SetActive(true);
            }
        }

        confirmUI.SetActive(false);
        confirmImg.SetActive(false);
        ZconfirmImg.SetActive(false);

        onConfirm?.Invoke(); // null이 아니면 호출
        onConfirm = null;
    }

    // 취소 버튼에 연결
    public void OnCancelButtonClick()
    {
        if (team)
        {
            if (board.round > 2)
            {
                ZmarketItemUI3.SetActive(true);
            }
            else
            {
                ZmarketItemUI.SetActive(true);
            }
        }
        else
        {
            if (board.round > 2)
            {
                marketItemUI3.SetActive(true);
            }
            else
            {
                marketItemUI.SetActive(true);
            }
        }





        confirmUI.SetActive(false);
        confirmImg.SetActive(false);
        ZconfirmImg.SetActive(false);
        onConfirm = null;
    }

    public void ReturnMain()
    {


        Vector3 pos = mainCamera.transform.position;
        pos.y = ((board.round - 1) * 4 + (board.state - 1)) * 15;
        if (team)
        {
            pos.x = 30;
        }
        else
        {
            pos.x = 0;
        }
        mainCamera.transform.position = pos;
        oppoInfo.SetActive(false);
        ZoppoInfo.SetActive(false);
        teamInfoUI.SetActive(false);
        ZteamInfoUI.SetActive(false);
        ZmarketUI.SetActive(false);
        ZitemUI.SetActive(false);
        marketItemUI.SetActive(false);
        ZmarketItemUI.SetActive(false);
        marketItemUI3.SetActive(false);
        ZmarketItemUI3.SetActive(false);
        marketUI.SetActive(false);
        itemUI.SetActive(false);
        confirmUI.SetActive(false);
        confirmImg.SetActive(false);
        ZconfirmImg.SetActive(false);
        mainUI.SetActive(true);


    }

    private void RegisterEvents()
    {
        NetUtility.S_WELCOME += OnWelcomeServer;
        NetUtility.C_WELCOME += OnWelcomeClient;

        NetUtility.C_START_GAME += OnStartGameClient;
        NetUtility.S_OPPO_ITEM += OnOppoItemServer;
        NetUtility.C_OPPO_ITEM += OnOppoItemClient;
    }

    private void UnRegisterEvents()
    {
    }

    // Server
    private void OnWelcomeServer(NetMessage msg, NetworkConnection cnn)
    {
        // Client has connected, assign a team and return the message back to him
        NetWelcome nw = msg as NetWelcome;

        // Assign a team
        nw.AssignedTeam = ++playerCount;

        // Return back to the client
        Server.Instance.SendToClient(cnn, nw);

        if (playerCount == 1)
        {
            Server.Instance.Broadcast(new NetStartGame());
        }
    }

    // Client
    private void OnWelcomeClient(NetMessage msg)
    {
        // Receive the connection message
        NetWelcome nw = msg as NetWelcome;

        // Assign the team
        // currentTeam = nw.AssignedTeam;

        // Debug.Log($"My assigned team is {nw.AssignedTeam}");
    }

    private void OnStartGameClient(NetMessage msg)
    {
        GetComponent<TimedMessage>().HideMessage();
        if (team)
        {
            ZStart();
        }
        else
        {
            AStart();
        }
         
    }
    
    public static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new Exception("No network adapters with an IPv4 address in the system!");
    }

    private void OnOppoItemServer(NetMessage msg, NetworkConnection cnn)
    {
        // Client has connected, assign a team and return the message back to him
        NetOppoItem nw = msg as NetOppoItem;

        // Assign a team
        Server.Instance.Broadcast(nw);

    }

    // Client
    private void OnOppoItemClient(NetMessage msg)
    {
        // Receive the connection message
        NetOppoItem nw = msg as NetOppoItem;

        // Assign the team
        // currentTeam = nw.AssignedTeam;

        // Debug.Log($"My assigned team is {nw.AssignedTeam}");
        if (nw.team != team)
        {
            if (team)
            {
                Zoppo.mine = nw.mine;
                Zoppo.illusion = nw.illusion;
                Zoppo.gps = nw.gps;
                Zoppo.bigbomb = nw.bigbomb;
                Zoppo.changePlace = nw.changePlace;
                Zoppo.reverse = nw.reverse;
                Zoppo.teleport = nw.teleport;
                Zoppo.armor = nw.armor;
                Zoppo.drone = nw.drone;
                Zoppo.car = nw.car;
            }
            else
            {
                oppo.mine = nw.mine;
                oppo.illusion = nw.illusion;
                oppo.gps = nw.gps;
                oppo.bigbomb = nw.bigbomb;
                oppo.changePlace = nw.changePlace;
                oppo.reverse = nw.reverse;
                oppo.teleport = nw.teleport;
                oppo.armor = nw.armor;
                oppo.drone = nw.drone;
                oppo.car = nw.car;
            }
            
        }
    }
}
