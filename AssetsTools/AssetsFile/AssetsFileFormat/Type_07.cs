namespace AssetsTools
{
    public class Type_07
    {
        public AssetClassID ClassID;
        public TypeField_07 @base;

        public void Read(bool hasTypeTree, EndianReader reader, uint version, uint typeVersion = 0) // typeVersion not implemented yet
        {
            reader.SetEndianness(true);
            ClassID = (AssetClassID)reader.ReadInt32();
            @base = new TypeField_07();
            @base.Read(hasTypeTree, reader, version, typeVersion);
        }

        public void Write(bool hasTypeTree, EndianWriter writer)
        {
            writer.SetEndianness(true);
            writer.Write((int)ClassID);
            @base.Write(hasTypeTree, writer);
        }
    }
}
