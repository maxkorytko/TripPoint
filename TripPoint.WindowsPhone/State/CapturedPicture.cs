using System;
using System.IO;
using System.Runtime.Serialization;
using System.Windows.Media;
using Microsoft.Phone;

using TripPoint.Model.Utils;

namespace TripPoint.WindowsPhone.State
{
    [DataContract]
    public class CapturedPicture
    {
        private ImageSource _source;

        public CapturedPicture(Stream picture)
        {
            if (picture == null)
                throw new ArgumentException("picture");

            RawBytes = TripPointConvert.ToBytes(picture);
        }

        [DataMember]
        public byte[] RawBytes { get; set; }

        [IgnoreDataMember]
        public ImageSource Source
        {
            get
            {
                if (_source == null)
                {
                    using (var stream = new MemoryStream(RawBytes))
                    {
                        _source = PictureDecoder.DecodeJpeg(stream);
                    }
                }

                return _source;
            }
        }

    }
}
