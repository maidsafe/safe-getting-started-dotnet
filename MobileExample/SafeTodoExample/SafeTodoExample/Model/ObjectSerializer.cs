using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;

namespace SafeTodoExample.Model
{
    public static class ObjectSerialize
    {
        public static List<byte> Serialize(this object obj)
        {
            if (obj == null)
            {
                return null;
            }

            using (var memoryStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, obj);
                var compressed = Compress(memoryStream.ToArray());

                return new List<byte>(compressed);
            }
        }

        public static object Deserialize(this List<byte> arrBytes)
        {
            using (var memoryStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                var decompressed = Decompress(arrBytes.ToArray());

                memoryStream.Write(decompressed, 0, decompressed.Length);
                memoryStream.Seek(0, SeekOrigin.Begin);

                return binaryFormatter.Deserialize(memoryStream);
            }
        }

        private static byte[] Compress(byte[] input)
        {
            byte[] compressesData;

            using (var outputStream = new MemoryStream())
            {
                using (var zip = new GZipStream(outputStream, CompressionMode.Compress))
                {
                    zip.Write(input, 0, input.Length);
                }

                compressesData = outputStream.ToArray();
            }

            return compressesData;
        }

        private static byte[] Decompress(byte[] input)
        {
            byte[] decompressedData;

            using (var outputStream = new MemoryStream())
            {
                using (var inputStream = new MemoryStream(input))
                {
                    using (var zip = new GZipStream(inputStream, CompressionMode.Decompress))
                    {
                        zip.CopyTo(outputStream);
                    }
                }

                decompressedData = outputStream.ToArray();
            }

            return decompressedData;
        }
    }
}
