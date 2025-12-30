using Unity.Networking.Transport;
using UnityEngine;

public class NetBigBomb : NetMessage
{
    public int tileIndex;
    public bool team;
    

    public NetBigBomb()
    {
        Code = OpCode.BIG_BOMB;
    }

    public NetBigBomb(Unity.Collections.DataStreamReader reader)
    {
        Code = OpCode.BIG_BOMB;
        Deserialize(reader);
    }

    public override void Serialize(ref Unity.Collections.DataStreamWriter writer)
    {
        writer.WriteByte((byte)Code);
        writer.WriteInt(tileIndex);
        writer.WriteByte(team ? (byte)1 : (byte)0);
    }

    public override void Deserialize(Unity.Collections.DataStreamReader reader)
    {
        // We already read the byte in the NetUtility::OnData
        tileIndex = reader.ReadInt();
        team = reader.ReadByte() != 0;
    }

    public override void ReceivedOnClient()
    {
        NetUtility.C_BIG_BOMB?.Invoke(this);
    }

    public override void ReceivedOnServer(NetworkConnection cnn)
    {
        NetUtility.S_BIG_BOMB?.Invoke(this, cnn);
    }
}



