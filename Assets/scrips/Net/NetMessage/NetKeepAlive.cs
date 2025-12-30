

public class NetKeepAlive : NetMessage
{
    public NetKeepAlive() // <-- Making the box
    {
        Code = OpCode.KEEP_ALIVE;
    }
    public NetKeepAlive(Unity.Collections.DataStreamReader reader) // <-- Receiving the box
    {
        Code = OpCode.KEEP_ALIVE;
        Deserialize(reader);
    }

    public override void Serialize(ref Unity.Collections.DataStreamWriter writer)
    {
        writer.WriteByte((byte)Code);
    }
    public override void Deserialize(Unity.Collections.DataStreamReader reader)
    {
    }
}

