using Unity.Networking.Transport;
public class NetOppoItem : NetMessage
{
    public bool team;
    public int mine = 0;
    public int illusion = 0;
    public int gps = 0;
    public int bigbomb = 0;
    public int changePlace = 0;
    public int reverse = 0;
    public int teleport = 0;
    public int armor = 0;
    public int drone = 0;
    public int car = 0;

    public NetOppoItem() // <-- Making the box
    {
        Code = OpCode.OPPO_ITEM;
    }
    public NetOppoItem(Unity.Collections.DataStreamReader reader) // <-- Receiving the box
    {
        Code = OpCode.OPPO_ITEM;
        Deserialize(reader);
    }

    public override void Serialize(ref Unity.Collections.DataStreamWriter writer)
    {
        writer.WriteByte((byte)Code);

        writer.WriteByte(team ? (byte)1 : (byte)0);
        writer.WriteInt(mine);
        writer.WriteInt(illusion);
        writer.WriteInt(gps);
        writer.WriteInt(bigbomb);
        writer.WriteInt(changePlace);
        writer.WriteInt(reverse);
        writer.WriteInt(teleport);
        writer.WriteInt(armor);
        writer.WriteInt(drone);
        writer.WriteInt(car);

    }

    public override void Deserialize(Unity.Collections.DataStreamReader reader)
    {

        team = reader.ReadByte() != 0;
        mine = reader.ReadInt();
        illusion = reader.ReadInt();
        gps = reader.ReadInt();
        bigbomb = reader.ReadInt();
        changePlace = reader.ReadInt();
        reverse = reader.ReadInt();
        teleport = reader.ReadInt();
        armor = reader.ReadInt();
        drone = reader.ReadInt();
        car = reader.ReadInt();

    }

    public override void ReceivedOnClient()
    {
        NetUtility.C_OPPO_ITEM?.Invoke(this);
    }
    public override void ReceivedOnServer(NetworkConnection cnn)
    {
        NetUtility.S_OPPO_ITEM?.Invoke(this, cnn);
    }

}


