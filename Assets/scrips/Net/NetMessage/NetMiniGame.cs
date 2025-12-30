using Unity.Networking.Transport;
using UnityEngine;

public class NetMiniGame : NetMessage
{
    public int isWin { set; get; }
    public bool team;

    public NetMiniGame()
    {
        Code = OpCode.MINIGAME;
    }

    public NetMiniGame(Unity.Collections.DataStreamReader reader)
    {
        Code = OpCode.MINIGAME;
        Deserialize(reader);
    }

    public override void Serialize(ref Unity.Collections.DataStreamWriter writer)
    {
        writer.WriteByte((byte)Code);
        writer.WriteInt(isWin);
        writer.WriteByte(team ? (byte)1 : (byte)0);
    }

    public override void Deserialize(Unity.Collections.DataStreamReader reader)
    {
        // We already read the byte in the NetUtility::OnData
        isWin = reader.ReadInt();
        team = reader.ReadByte() != 0;
    }

    public override void ReceivedOnClient()
    {
        NetUtility.C_MINIGAME?.Invoke(this);
    }

    public override void ReceivedOnServer(NetworkConnection cnn)
    {
        NetUtility.S_MINIGAME?.Invoke(this, cnn);
    }

}

