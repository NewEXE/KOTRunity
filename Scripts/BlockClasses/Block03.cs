
class Block03 : IBlocktype
{        UnityEngine.GameObject _thisObject;
    public UnityEngine.GameObject thisObject { get => _thisObject; set => _thisObject = value; }

    public byte[] GetBytes()
    {
        byte[] buffer = new byte[16];
                UnityEngine.Debug.LogWarning("Export not implemented for object:",thisObject);

        return buffer;
    }

    public void Read(byte[] buffer, ref int pos)
    {
        throw new System.NotImplementedException();
    }
}