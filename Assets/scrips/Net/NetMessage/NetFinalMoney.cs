using Unity.Networking.Transport;
public class NetFinalMoney : NetMessage
{
    public bool team;
    public int gold;

    public NetFinalMoney() // <-- Making the box
    {
        Code = OpCode.MONEY;
    }
    public NetFinalMoney(Unity.Collections.DataStreamReader reader) // <-- Receiving the box
    {
        Code = OpCode.MONEY;
        Deserialize(reader);
    }

    public override void Serialize(ref Unity.Collections.DataStreamWriter writer)
    {
        writer.WriteByte((byte)Code);

        writer.WriteByte(team ? (byte)1 : (byte)0);
        writer.WriteInt(gold);

    }

    public override void Deserialize(Unity.Collections.DataStreamReader reader)
    {

        team = reader.ReadByte() != 0;
        gold = reader.ReadInt();

    }

    public override void ReceivedOnClient()
    {
        NetUtility.C_MONEY?.Invoke(this);
    }
    public override void ReceivedOnServer(NetworkConnection cnn)
    {
        NetUtility.S_MONEY?.Invoke(this, cnn);
    }

}


