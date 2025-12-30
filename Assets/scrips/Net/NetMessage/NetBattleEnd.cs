using Unity.Networking.Transport;
using UnityEngine;

public class NetBattleEnd : NetMessage
{
    public bool team;

    public NetBattleEnd()
    {
        Code = OpCode.BATTLE_END;
    }

    public NetBattleEnd(Unity.Collections.DataStreamReader reader)
    {
        Code = OpCode.BATTLE_END;
        Deserialize(reader);
    }

    public override void Serialize(ref Unity.Collections.DataStreamWriter writer)
    {
        writer.WriteByte((byte)Code);
        writer.WriteByte(team ? (byte)1 : (byte)0);
    }

    public override void Deserialize(Unity.Collections.DataStreamReader reader)
    {
        // We already read the byte in the NetUtility::OnData
        team = reader.ReadByte() != 0;
    }

    public override void ReceivedOnClient()
    {
        Debug.Log("babttleEndClient");
        NetUtility.C_BATTLE_END?.Invoke(this);
    }

    public override void ReceivedOnServer(NetworkConnection cnn)
    {
        Debug.Log("babttleEndServer");
        NetUtility.S_BATTLE_END?.Invoke(this, cnn);
    }

}


