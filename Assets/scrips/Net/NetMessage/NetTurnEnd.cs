using Unity.Networking.Transport;
using UnityEngine;

public class NetTurnEnd : NetMessage
{
    public NetTurnEnd()
    {
        Code = OpCode.TURN_END;
    }

    public NetTurnEnd(Unity.Collections.DataStreamReader reader)
    {
        Code = OpCode.TURN_END;
        Deserialize(reader);
    }

    public override void Serialize(ref Unity.Collections.DataStreamWriter writer)
    {
        writer.WriteByte((byte)Code);
    }

    public override void Deserialize(Unity.Collections.DataStreamReader reader)
    {
    }

    public override void ReceivedOnClient()
    {
        NetUtility.C_TURN_END?.Invoke(this);
    }

    public override void ReceivedOnServer(NetworkConnection cnn)
    {
        NetUtility.S_TURN_END?.Invoke(this, cnn);
    }

}
