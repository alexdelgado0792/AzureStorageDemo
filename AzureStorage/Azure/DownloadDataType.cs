using System;

namespace Azure
{
    public enum DownloadDataType
    {
        Text = 1,
        ByteArray,
        [Obsolete("Not Implemented")]
        Stream
    }
}
