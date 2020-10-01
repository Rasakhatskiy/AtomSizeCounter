using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AtomSizeCounter
{
    /// <summary>
    /// Class that helps to find sizes of MOV atoms.
    /// </summary>
    static class AtomSizeCounter
    {
        /// <summary>
        /// Counts all 4 byte sizes before signature in file. If size = 1, reads other 8 bytes after signature
        /// </summary>
        /// <param name="pathSource">Path to the source file.</param>
        /// <param name="pathResultText">Path to the result file</param>
        /// <param name="signature">Signature.</param>
        /// <returns>List of sizes</returns>
        public static List<UInt64> Count(string pathSource, string pathResultText, string signature)
        {
            var encoding = Encoding.GetEncoding("ASCII");
            byte[] bytes = encoding.GetBytes(signature);
            Array.Reverse(bytes);
            return Count(pathSource, pathResultText, BitConverter.ToUInt32(bytes, 0));
        }

        /// <summary>
        /// Counts all 4 byte sizes before signature in file. If size = 1, reads other 8 bytes after signature
        /// </summary>
        /// <param name="pathSource">Path to the source file.</param>
        /// <param name="pathResultText">Path to the result file</param>
        /// <param name="signature">Signature.</param>
        /// <returns>List of sizes</returns>
        public static List<UInt64> Count(string pathSource, string pathResultText, UInt32 signature)
        {
            //64 MB
            const int BUFFER_SIZE = 64 * 1024 * 1024;

            var buffer = new byte[BUFFER_SIZE];
            var result = new List<UInt64>();
            var count = 0;

            using (var binaryReader = new BinaryReader(File.OpenRead(pathSource)))
            using (var streamWriter = new StreamWriter(File.OpenWrite(pathResultText)))
            {
                while((count = binaryReader.Read(buffer, 0, BUFFER_SIZE)) != 0)
                {
                    for(int i = 4; i < count - 12; ++i)
                    {
                        if(ReadUInt32R(i, buffer) == signature)
                        {
                            ulong size = ReadUInt32R(i - 4, buffer);
                            if (size == 1)
                                size = ReadUInt64R(i + 4, buffer);
                            streamWriter.WriteLine($"{(double)(size / 1024.0 / 1024.0 / 1024.0)} GB");
                            result.Add(size);
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Reads 32 bit number from array but reversed 0x0002 -> 2
        /// </summary>
        /// <param name="position"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static UInt32 ReadUInt32R(int position, byte[] buffer)
        {
            return (UInt32)(
                (buffer[position++] << 24) |
                (buffer[position++] << 16) |
                (buffer[position++] << 8) |
                (buffer[position++]));
        }

        /// <summary>
        /// Reads 32 bit number from array but reversed 0x0002 -> 2
        /// </summary>
        /// <param name="position"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static UInt64 ReadUInt64R(int position, byte[] buffer)
        {
            return (UInt64)(
                (buffer[position++] << 56) |
                (buffer[position++] << 48) |
                (buffer[position++] << 40) |
                (buffer[position++] << 32) |
                (buffer[position++] << 24) |
                (buffer[position++] << 16) |
                (buffer[position++] << 8) |
                (buffer[position++]));
        }
    }
}
