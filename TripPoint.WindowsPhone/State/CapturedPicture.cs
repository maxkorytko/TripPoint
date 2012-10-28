using System.IO;
using System.Runtime.Serialization;

using TripPoint.Model.Utils;

namespace TripPoint.WindowsPhone.State
{
    [DataContract]
    public class CapturedPicture
    {
        private byte[] _picture;

        public CapturedPicture(Stream picture)
        {
            Picture = TripPointConvert.ToBytes(picture);
        }

        [DataMember]
        public byte[] Picture { get; set; }
    }
}
