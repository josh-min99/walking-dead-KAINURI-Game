using Unity.Networking.Transport;
using UnityEngine;

public class NetTileEvent : NetMessage
{
    public int tileIndex;
    public bool team;
    public bool isMine;
    public bool isVisited;
    public bool bomb;
    

    public NetTileEvent()
    {
        Code = OpCode.TILE_EVENT;
    }

    public NetTileEvent(Unity.Collections.DataStreamReader reader)
    {
        Code = OpCode.TILE_EVENT;
        Deserialize(reader);
    }

    public override void Serialize(ref Unity.Collections.DataStreamWriter writer)
    {
        writer.WriteByte((byte)Code);
        writer.WriteInt(tileIndex);
        writer.WriteByte(team ? (byte)1 : (byte)0);
        writer.WriteByte(isMine ? (byte)1 : (byte)0);
        writer.WriteByte(isVisited ? (byte)1 : (byte)0);
        writer.WriteByte(bomb ? (byte)1 : (byte)0);
    }

    public override void Deserialize(Unity.Collections.DataStreamReader reader)
    {
        // We already read the byte in the NetUtility::OnData
        tileIndex = reader.ReadInt();
        team = reader.ReadByte() != 0;
        isMine = reader.ReadByte() != 0;
        isVisited = reader.ReadByte() != 0;
        bomb = reader.ReadByte() != 0;
    }

    public override void ReceivedOnClient()
    {
        NetUtility.C_TILE_EVENT?.Invoke(this);
    }

    public override void ReceivedOnServer(NetworkConnection cnn)
    {
        NetUtility.S_TILE_EVENT?.Invoke(this, cnn);
    }
}



