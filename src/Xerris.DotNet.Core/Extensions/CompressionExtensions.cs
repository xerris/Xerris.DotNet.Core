using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Xerris.DotNet.Core.Extensions
{
    public static class CompressionExtensions
    {
        public static byte[] Zip(this string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);

            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                {
                    CopyTo(msi, gs);
                }

                return mso.ToArray();
            }
        }

        public static string Unzip(this byte[] bytes)
        {
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    CopyTo(gs, mso);
                }

                return Encoding.UTF8.GetString(mso.ToArray());
            }
        }
        
        public static string ToBase64(this string value) {
            return ToBase64(Encoding.UTF8.GetBytes(value));
        }
        
        public static string ToBase64(this byte[] value)
        {
            return Convert.ToBase64String(value);
        }

        public static string FromBase64(this string value)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(value));
        }

        public static byte[] GetBytes(this string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }

        public static string FromBytes(this byte[] value)
        {
            return Encoding.UTF8.GetString(value);
        }

        private static void CopyTo(Stream src, Stream dest)
        {
            var bytes = new byte[4096];
            int cnt;
            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0) dest.Write(bytes, 0, cnt);
        }
    }
}