using System;

namespace Share.Pipe
{
    public abstract class Binary<T>
        where T : Binary<T>
    {
        public abstract byte Head { get; }
        public byte CheckHead { get; set; }
        public Binary()
        {
            this.CheckHead = this.Head;
        }
        public bool Check => this.Head == this.CheckHead;
        public byte[] Serialize()
        {
            return FastSerialize.Serialize(this);
        }
        public static bool TryFromBytes(byte[] bytes, out T t)
        {
            try
            {
                t = FastSerialize.Deserialize<T>(bytes);
                return t.Check;
            }
            catch (Exception ex)
            {
                t = default(T);
                return false;
            }
        }
    }
}
