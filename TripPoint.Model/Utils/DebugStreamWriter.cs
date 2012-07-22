using System.Text;
using System.IO;
using System.Diagnostics;

namespace TripPoint.Model.Utils
{
    /// <summary>
    /// Writer for the System.Diagnostics.Debug stream
    /// </summary>
    public class DebugStreamWriter : TextWriter
    {
        private const int DefaultBufferSize = 256;
        private StringBuilder _buffer;

        public DebugStreamWriter()
        {
            BufferSize = 8;//256;
            _buffer = new StringBuilder(BufferSize);
        }

        public int BufferSize
        {
            get;
            private set;
        }

        public override System.Text.Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }

        #region StreamWriter Overrides
        public override void Write(char value)
        {
            _buffer.Append(value);
            if (_buffer.Length >= BufferSize)
                Flush();
        }

        public override void WriteLine(string value)
        {
            Flush();

            using(var reader = new StringReader(value))
            {
                string line; 
                while( null != (line = reader.ReadLine()))
                    Debug.WriteLine(line);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                Flush();
        }

        public override void Flush()
        {
            if (_buffer.Length > 0)
            {
                Debug.WriteLine(_buffer);
                _buffer.Clear();
            }
        }
        #endregion
    }
}
