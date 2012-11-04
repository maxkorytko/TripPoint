using System;
using System.IO;
using System.Runtime.Serialization;
using System.Windows.Media;

using TripPoint.Model.Utils;
using TripPoint.WindowsPhone.Utils;

namespace TripPoint.WindowsPhone.State
{
    [DataContract]
    public class CapturedPicture
    {
        private ImageSource _source;

        public CapturedPicture(string fileName, Stream picture) :
            this(fileName, TripPointConvert.ToBytes(picture))
        {   }

        public CapturedPicture(string fileName, byte[] picture)
        {
            if (String.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("fileName");

            if (picture == null)
                throw new ArgumentException("picture");

            FileName = fileName;
            RawBytes = picture;
        }

        [DataMember]
        public byte[] RawBytes { get; set; }

        [DataMember]
        public string FileName { get; set; }

        [IgnoreDataMember]
        public ImageSource Source
        {
            get
            {
                if (_source == null)
                {
                    using (var stream = new MemoryStream(RawBytes))
                    {
                        _source = ImageUtils.CreateWriteableBitmapFromStream(stream);
                    }
                }

                return _source;
            }
        }
    }
}
