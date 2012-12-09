using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace MB.Crammer
{
    static class BinaryBufferConverter
    {
        public static IBuffer getBufferFromString(String encodedText)
        {
            using (InMemoryRandomAccessStream memoryStream = new InMemoryRandomAccessStream())
            {
                using (DataWriter dataWriter = new DataWriter(memoryStream))
                {
                    byte[] decodedBytes = Convert.FromBase64String(encodedText);
                    dataWriter.WriteBytes(decodedBytes);
                    return dataWriter.DetachBuffer();
                }
            }
        }

        public static string getStringFromBuffer(IBuffer buffer)
        {
            using (DataReader dataReader = DataReader.FromBuffer(buffer))
            {
                byte[] bufferBytes = new byte[buffer.Length];
                dataReader.ReadBytes(bufferBytes);
                return (Convert.ToBase64String(bufferBytes));
            }
        }


    }
}
