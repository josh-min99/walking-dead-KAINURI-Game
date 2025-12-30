using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;
using Unity.Collections;
using System;

public enum OpCode
{
    KEEP_ALIVE = 1,
    WELCOME = 2,
    START_GAME = 3,
    MAKE_MOVE = 4,
    TURN_END = 5,
    MINIGAME = 6,
    BATTLE_END = 7,
    BIG_BOMB = 8,
    TILE_EVENT = 9,
    OPPO_ITEM = 10,
    MONEY = 11
}
public class NetUtility
{
    public static void OnData(DataStreamReader stream, NetworkConnection cnn, Server server = null)
    {
        NetMessage msg = null;
        var opCode = (OpCode)stream.ReadByte();
        switch (opCode)
        {
            case OpCode.KEEP_ALIVE:
                msg = new NetKeepAlive(stream);
                break;
            case OpCode.WELCOME: msg = new NetWelcome(stream); break;
            case OpCode.START_GAME: msg = new NetStartGame(stream); break;
            case OpCode.MAKE_MOVE: msg = new NetMakeMove(stream); break;
            case OpCode.TURN_END: msg = new NetTurnEnd(stream); break;
            case OpCode.MINIGAME: msg = new NetMiniGame(stream); break;
            case OpCode.BATTLE_END: msg = new NetBattleEnd(stream); break;
            case OpCode.BIG_BOMB: msg = new NetBigBomb(stream); break;
            case OpCode.TILE_EVENT: msg = new NetTileEvent(stream); break;
            case OpCode.OPPO_ITEM: msg = new NetOppoItem(stream); break;
            case OpCode.MONEY: msg = new NetFinalMoney(stream); break;

            default:
                Debug.LogError("Message received had no OpCode");
                break;
        }

        if (server != null)
            msg.ReceivedOnServer(cnn);
        else
            msg.ReceivedOnClient();
    }

    // Net messages
    public static Action<NetMessage> C_KEEP_ALIVE;
    public static Action<NetMessage> C_WELCOME;
    public static Action<NetMessage> C_START_GAME;
    public static Action<NetMessage> C_MAKE_MOVE;
    public static Action<NetMessage> C_TURN_END;
    public static Action<NetMessage> C_MINIGAME;
    public static Action<NetMessage> C_BATTLE_END;
    public static Action<NetMessage> C_BIG_BOMB;
    public static Action<NetMessage> C_TILE_EVENT;
    public static Action<NetMessage> C_OPPO_ITEM;
    public static Action<NetMessage> C_MONEY;

    public static Action<NetMessage, NetworkConnection> S_KEEP_ALIVE;
    public static Action<NetMessage, NetworkConnection> S_WELCOME;
    public static Action<NetMessage, NetworkConnection> S_START_GAME;
    public static Action<NetMessage, NetworkConnection> S_MAKE_MOVE;
    public static Action<NetMessage, NetworkConnection> S_TURN_END;
    public static Action<NetMessage, NetworkConnection> S_MINIGAME;
    public static Action<NetMessage, NetworkConnection> S_BATTLE_END;
    public static Action<NetMessage, NetworkConnection> S_BIG_BOMB;
    public static Action<NetMessage, NetworkConnection> S_TILE_EVENT;
    public static Action<NetMessage, NetworkConnection> S_OPPO_ITEM;
    public static Action<NetMessage, NetworkConnection> S_MONEY;

}
