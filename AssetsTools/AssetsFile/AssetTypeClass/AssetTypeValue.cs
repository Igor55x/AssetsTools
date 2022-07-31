using System.Text;

namespace AssetsTools
{
    public class AssetTypeValue
    {
        public EnumValueTypes type;
        public ValueTypes value;
        public struct ValueTypes
        {
            private object value;
            public AssetTypeArray asArray
            {
                get => (AssetTypeArray)value;
                set => this.value = value;
            }

            public AssetTypeByteArray asByteArray
            {
                get => (AssetTypeByteArray)value;
                set => this.value = value;
            }

            public bool asBool
            {
                get => (bool)value;
                set => this.value = value;
            }

            public sbyte asInt8
            {
                get => (sbyte)value;
                set => this.value = value;
            }

            public byte asUInt8
            {
                get => (byte)value;
                set => this.value = value;
            }

            public short asInt16
            {
                get => (short)value;
                set => this.value = value;
            }

            public ushort asUInt16
            {
                get => (ushort)value;
                set => this.value = value;
            }

            public int asInt32
            {
                get => (int)value;
                set => this.value = value;
            }

            public uint asUInt32
            {
                get => (uint)value;
                set => this.value = value;
            }

            public long asInt64
            {
                get => (long)value;
                set => this.value = value;
            }

            public ulong asUInt64
            {
                get => (ulong)value;
                set => this.value = value;
            }

            public float asFloat
            {
                get => (float)value;
                set => this.value = value;
            }

            public double asDouble
            {
                get => (double)value;
                set => this.value = value;
            }

            public byte[] asString
            {
                get => (byte[])value;
                set => this.value = value;
            }
        }

        public AssetTypeValue(EnumValueTypes type, object valueContainer)
        {
            this.type = type;
            if (valueContainer != null)
                Set(valueContainer);
        }

        public EnumValueTypes GetValueType() => type;

        public void Set(object valueContainer)
        {
            unchecked
            {
                switch (type)
                {
                    case EnumValueTypes.Bool:
                        value.asBool = Convert.ToByte(valueContainer) == 1;
                        break;
                    case EnumValueTypes.Int8:
                        value.asInt8 = Convert.ToSByte(valueContainer);
                        break;
                    case EnumValueTypes.UInt8:
                        value.asUInt8 = Convert.ToByte(valueContainer);
                        break;
                    case EnumValueTypes.Int16:
                        value.asInt16 = Convert.ToInt16(valueContainer);
                        break;
                    case EnumValueTypes.UInt16:
                        value.asUInt16 = Convert.ToUInt16(valueContainer);
                        break;
                    case EnumValueTypes.Int32:
                        value.asInt32 = Convert.ToInt32(valueContainer);
                        break;
                    case EnumValueTypes.UInt32:
                        value.asUInt32 = Convert.ToUInt32(valueContainer);
                        break;
                    case EnumValueTypes.Int64:
                        value.asInt64 = Convert.ToInt64(valueContainer);
                        break;
                    case EnumValueTypes.UInt64:
                        value.asUInt64 = Convert.ToUInt64(valueContainer);
                        break;
                    case EnumValueTypes.Float:
                        value.asFloat = Convert.ToSingle(valueContainer);
                        break;
                    case EnumValueTypes.Double:
                        value.asDouble = Convert.ToDouble(valueContainer);
                        break;
                    case EnumValueTypes.String:
                        {
                            value.asString = valueContainer switch
                            {
                                byte[] byteArr => byteArr,
                                string str => Encoding.UTF8.GetBytes(str),
                                _ => Array.Empty<byte>(),
                            };
                            break;
                        }
                    case EnumValueTypes.Array:
                        value.asArray = (AssetTypeArray)valueContainer;
                        break;
                    case EnumValueTypes.ByteArray:
                        {
                            value.asByteArray = valueContainer switch
                            {
                                AssetTypeByteArray byteArray => byteArray,
                                byte[] data => new AssetTypeByteArray(data),
                                _ => new AssetTypeByteArray(Array.Empty<byte>())
                            };
                            break;
                        }
                    default:
                        break;
                }
            }
        }

        public AssetTypeArray AsArray()
        {
            return (type == EnumValueTypes.Array) ? value.asArray : new AssetTypeArray { size = 0xFFFF };
        }

        public AssetTypeByteArray AsByteArray()
        {
            return (type == EnumValueTypes.ByteArray) ? value.asByteArray : new AssetTypeByteArray { size = 0xFFFF };
        }

        public string AsString()
        {
            return type switch
            {
                EnumValueTypes.Bool => value.asBool ? "true" : "false",
                EnumValueTypes.Int8 => value.asInt8.ToString(),
                EnumValueTypes.UInt8 => value.asUInt8.ToString(),
                EnumValueTypes.Int16 => value.asInt16.ToString(),
                EnumValueTypes.UInt16 => value.asUInt16.ToString(),
                EnumValueTypes.Int32 => value.asInt32.ToString(),
                EnumValueTypes.UInt32 => value.asUInt32.ToString(),
                EnumValueTypes.Int64 => value.asInt64.ToString(),
                EnumValueTypes.UInt64 => value.asUInt64.ToString(),
                EnumValueTypes.Float => value.asFloat.ToString(),
                EnumValueTypes.Double => value.asDouble.ToString(),
                EnumValueTypes.String => Encoding.UTF8.GetString(value.asString),
                _ => string.Empty,
            };
        }

        public byte[] AsStringBytes()
        {
            return (type == EnumValueTypes.String) ? value.asString : null;
        }

        public bool AsBool()
        {
            return type switch
            {
                EnumValueTypes.Float or EnumValueTypes.Double or EnumValueTypes.String or EnumValueTypes.ByteArray or EnumValueTypes.Array => false,
                EnumValueTypes.Int8 => value.asInt8 == 1,
                EnumValueTypes.Int16 => value.asInt16 == 1,
                EnumValueTypes.Int32 => value.asInt32 == 1,
                EnumValueTypes.Int64 => value.asInt64 == 1,
                EnumValueTypes.UInt8 => value.asUInt8 == 1,
                EnumValueTypes.UInt16 => value.asUInt16 == 1,
                EnumValueTypes.UInt32 => value.asUInt32 == 1,
                EnumValueTypes.UInt64 => value.asUInt64 == 1,
                _ => value.asBool,
            };
        }

        public int AsInt()
        {
            return type switch
            {
                EnumValueTypes.Float => (int)value.asFloat,
                EnumValueTypes.Double => (int)value.asDouble,
                EnumValueTypes.String or EnumValueTypes.ByteArray or EnumValueTypes.Array => 0,
                EnumValueTypes.Int8 => value.asInt8,
                EnumValueTypes.Int16 => value.asInt16,
                EnumValueTypes.Int64 => (int)value.asInt64,
                EnumValueTypes.UInt8 => value.asUInt8,
                EnumValueTypes.UInt16 => value.asUInt16,
                EnumValueTypes.UInt32 => (int)value.asUInt32,
                EnumValueTypes.UInt64 => (int)value.asUInt64,
                _ => value.asInt32,
            };
        }

        public uint AsUInt()
        {
            return type switch
            {
                EnumValueTypes.Float => (uint)value.asFloat,
                EnumValueTypes.Double => (uint)value.asDouble,
                EnumValueTypes.String or EnumValueTypes.ByteArray or EnumValueTypes.Array => 0,
                EnumValueTypes.UInt8 => value.asUInt8,
                EnumValueTypes.UInt16 => value.asUInt16,
                EnumValueTypes.UInt64 => (uint)value.asUInt64,
                EnumValueTypes.Int8 => value.asUInt8,
                EnumValueTypes.Int16 => value.asUInt16,
                EnumValueTypes.Int32 => value.asUInt32,
                EnumValueTypes.Int64 => (uint)value.asUInt64,
                _ => value.asUInt32,
            };
        }

        public long AsInt64()
        {
            return type switch
            {
                EnumValueTypes.Float => (long)value.asFloat,
                EnumValueTypes.Double => (long)value.asDouble,
                EnumValueTypes.String or EnumValueTypes.ByteArray or EnumValueTypes.Array => 0,
                EnumValueTypes.Int8 => value.asInt8,
                EnumValueTypes.Int16 => value.asInt16,
                EnumValueTypes.Int32 => value.asInt32,
                EnumValueTypes.UInt8 => value.asUInt8,
                EnumValueTypes.UInt16 => value.asUInt16,
                EnumValueTypes.UInt32 => value.asUInt32,
                EnumValueTypes.UInt64 => (long)value.asUInt64,
                _ => value.asInt64,
            };
        }

        public ulong AsUInt64()
        {
            return type switch
            {
                EnumValueTypes.Float => (ulong)value.asFloat,
                EnumValueTypes.Double => (ulong)value.asDouble,
                EnumValueTypes.String or EnumValueTypes.ByteArray or EnumValueTypes.Array => 0,
                EnumValueTypes.UInt8 => value.asUInt8,
                EnumValueTypes.UInt16 => value.asUInt16,
                EnumValueTypes.UInt32 => value.asUInt32,
                EnumValueTypes.Int8 => value.asUInt8,
                EnumValueTypes.Int16 => value.asUInt16,
                EnumValueTypes.Int32 => value.asUInt32,
                EnumValueTypes.Int64 => value.asUInt64,
                _ => value.asUInt64,
            };
        }

        public float AsFloat()
        {
            return type switch
            {
                EnumValueTypes.Float => value.asFloat,
                EnumValueTypes.Double => (float)value.asDouble,
                EnumValueTypes.String or EnumValueTypes.ByteArray or EnumValueTypes.Array => 0,
                EnumValueTypes.Int8 => value.asInt8,
                EnumValueTypes.Int16 => value.asInt16,
                EnumValueTypes.Int32 => value.asInt32,
                EnumValueTypes.UInt8 => value.asUInt8,
                EnumValueTypes.UInt16 => value.asUInt16,
                EnumValueTypes.UInt32 => value.asUInt32,
                _ => value.asUInt64,
            };
        }

        public double AsDouble()
        {
            return type switch
            {
                EnumValueTypes.Float => value.asFloat,
                EnumValueTypes.Double => value.asDouble,
                EnumValueTypes.String or EnumValueTypes.ByteArray or EnumValueTypes.Array => 0,
                EnumValueTypes.Int8 => value.asInt8,
                EnumValueTypes.Int16 => value.asInt16,
                EnumValueTypes.Int32 => value.asInt32,
                EnumValueTypes.UInt8 => value.asUInt8,
                EnumValueTypes.UInt16 => value.asUInt16,
                EnumValueTypes.UInt32 => value.asUInt32,
                _ => value.asUInt64,
            };
        }
    }
}
