using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenbankBackupTool
{
    /// <summary>
    /// Eine Helferklasse, die diverse nützliche Erweiterungsfunktionen definiert.<br/>
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// Eine Tabelle mit der Hex-Darstellung der Byte-Werte 0 bis 256
        /// </summary>
        public static string[] HexTbl = Enumerable.Range(0, 256).Select(v => v.ToString("X2")).ToArray();
        /// <summary>
        /// Wandelt eine Aufzählung von bytes (z.B.<code>List&lt;byte&gt;</code>) in einen String in
        /// Hex-Darstellung der Bytes um.
        /// </summary>
        /// <param name="array">Die umzuwandelnde Byte-Aufzählung</param>
        /// <returns>Die übergebenen Bytes in Hex-Darstellung als String</returns>
        public static string ToHex(this IEnumerable<byte> array)
        {
            StringBuilder s = new StringBuilder();
            foreach (var v in array)
                s.Append(HexTbl[v]);
            return s.ToString();
        }

        /// <summary>
        /// Wandelt eine Byte-Array in einen String in
        /// Hex-Darstellung der Bytes um.
        /// </summary>
        /// <param name="array">Das umzuwandelnde byte-Array</param>
        /// <returns>Die übergebenen Bytes in Hex-Darstellung als String</returns>
        public static string ToHex(this byte[] array)
        {
            StringBuilder s = new StringBuilder(array.Length * 2);
            foreach (var v in array)
                s.Append(HexTbl[v]);
            return s.ToString();
        }
    }
}