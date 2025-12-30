using Unity.Networking.Transport;
public class NetMakeMove : NetMessage
{
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
    public bool delete;

    public NetMakeMove() // <-- Making the box
    {
        Code = OpCode.MAKE_MOVE;
    }
    public NetMakeMove(Unity.Collections.DataStreamReader reader) // <-- Receiving the box
    {
        Code = OpCode.MAKE_MOVE;
        Deserialize(reader);
    }

    public override void Serialize(ref Unity.Collections.DataStreamWriter writer)
    {
        writer.WriteByte((byte)Code);
        writer.WriteInt(tileNum);
        writer.WriteInt(level);
        writer.WriteByte(armor ? (byte)1 : (byte)0);
        writer.WriteByte(reverse ? (byte)1 : (byte)0);
        writer.WriteByte(aidKit ? (byte)1 : (byte)0);
        writer.WriteByte(raibo ? (byte)1 : (byte)0);
        writer.WriteByte(advantage ? (byte)1 : (byte)0);
        writer.WriteByte(team ? (byte)1 : (byte)0);
        writer.WriteByte(hiding ? (byte)1 : (byte)0);
        writer.WriteByte(isIllusion ? (byte)1 : (byte)0);
        writer.WriteByte(delete ? (byte)1 : (byte)0);
    }

    public override void Deserialize(Unity.Collections.DataStreamReader reader)
    {
        tileNum = reader.ReadInt();
        level = reader.ReadInt();
        armor = reader.ReadByte() != 0;
        reverse = reader.ReadByte() != 0;
        aidKit = reader.ReadByte() != 0;
        raibo = reader.ReadByte() != 0;
        advantage = reader.ReadByte() != 0;
        team = reader.ReadByte() != 0;
        hiding = reader.ReadByte() != 0;
        isIllusion = reader.ReadByte() != 0;
        delete = reader.ReadByte() != 0;
    }

    public override void ReceivedOnClient()
    {
        NetUtility.C_MAKE_MOVE?.Invoke(this);
    }
    public override void ReceivedOnServer(NetworkConnection cnn)
    {
        NetUtility.S_MAKE_MOVE?.Invoke(this, cnn);
    }

}

