using System;

namespace Azure
{
    public enum UploadDataType
    {
        Text = 1,
        ByteArray,
        [Obsolete("Not Implemented")]
        Stream
    }
}
