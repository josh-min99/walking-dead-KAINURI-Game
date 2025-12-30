using UnityEngine;
using Unity.Networking.Transport;

public class MiniGame : MonoBehaviour
{
    public bool winGame;
    public bool isReady = false;
    public bool oppoReady = false;
    public GameObject board;
    public board bod;
    public GameUI gameUI;
    public bool choose = false;
    public int plCount = -1;
    public bool sended = false;
    public void Awake()
    {
        RegisterEvents();
    }

    private void OnEnable()
    {
        isReady = false;
        oppoReady = false;
        choose = false;
        sended = false;
    }

    // Update is called once per frame
    void Update()
    {
        //서버통신
        if (isReady && oppoReady)  //(isReady && 상대방 ready)
        {
            board.SetActive(true);
            if (bod.round < 4)
            {
                if (winGame)
                {
                    bod.gold += 25; // 원래는 25여야하긴 해! 
                }
                else
                {
                    /// 상대방에게 25골드 줘야하거든 // 서버통신
                }
                bod.gold += 100;
            }
            else
            {
                if (winGame)
                {
                    bod.gold += 50;
                }
                bod.gold += 150;
            }
            gameUI.ReturnMain();
            GetComponent<TimedMessage>().HideMessage();
            gameObject.SetActive(false);



        }
    }

    public void WinGame()
    {
        winGame = true;
        choose = true;
        GetComponent<TimedMessage>().ShowMessage("WIN", 0.5f);
        // 서버통신
        
    }

    public void LoseGame()
    {
        winGame = false;
        choose = true;
        GetComponent<TimedMessage>().ShowMessage("LOSE", 0.5f);
        //서버통신

    }

    public void IsReady()
    {
        if (choose)
        {
            NetMiniGame mm = new NetMiniGame();
            if (winGame)
            {
                mm.isWin = 1;   
            }
            else
            {
                mm.isWin = 0;
            }
            
            mm.team = GameUI.team;
            if (!sended)
            {
                Client.Instance.SendToServer(mm);
                sended = true;
            }
            
            isReady = true;
            GetComponent<TimedMessage>().HideMessage();
        }
        else
        {
            GetComponent<TimedMessage>().ShowMessage("choose win or lose", 0.3f);
        }

        //서버통신
    }


    private void RegisterEvents()
    {
        NetUtility.S_MINIGAME += OnMiniGameServer;
        NetUtility.C_MINIGAME += OnMiniGameClient;

    }

    private void UnRegisterEvents()
    {
    }

    private void OnMiniGameServer(NetMessage msg, NetworkConnection cnn)
    {
        // Client has connected, assign a team and return the message back to him
        NetMiniGame nw = msg as NetMiniGame;
        Server.Instance.Broadcast(nw);
    }

    // Client
    private void OnMiniGameClient(NetMessage msg)
    {
        NetMiniGame nw = msg as NetMiniGame;
        // Receive the connection message
        if ( nw.team != GameUI.team)
        {
            if ((nw.isWin == 0) &&(bod.round < 4))
                bod.gold += 25;
            oppoReady = true;
        }
        
    }
}



