using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Map;
using Utility;

namespace Utility
{
    class CSVCompression
    {
        public static byte[] Compress(string source, int totalColumns, char containerDelimiter = ';', char rowDelimiter = ',')
        {
            int listColumn = totalColumns - 1;

            List<string[]> list = new List<string[]>();
            foreach (string line in source.Split(containerDelimiter))
            {
                string[] row = line.Trim().Split(rowDelimiter, totalColumns, StringSplitOptions.None);
                row[listColumn] = row[listColumn].Replace(rowDelimiter, containerDelimiter);
                list.Add(row);
            }
            
            string[][] outputArr = list.ToArray();

            int[] columnOrdering = new int[totalColumns];
            HashSet<string>[] columnSets = new HashSet<string>[totalColumns];

            for (int i = 0; i < totalColumns; i++)
            {
                columnOrdering[i] = i;
                columnSets[i] = new HashSet<string>();
                for (int j = 0; j < outputArr.Length; j++)
                    columnSets[i].Add(outputArr[j][i]);
            }

            Array.Sort(columnOrdering, (a, b) => columnSets[a].Count - columnSets[b].Count);
            Array.Sort(outputArr, (a, b) =>
            {
                for (int i = 0; i < totalColumns; i++)
                {
                    int cmp = String.Compare(a[columnOrdering[i]], b[columnOrdering[i]]);
                    if (cmp != 0) return cmp;
                }
                return 0;
            });
            
            CompressDelta(outputArr);
            
            list.Clear();
            for (int i = 0; i < totalColumns; i++)
                list.Add(outputArr.Select((row) => row[i]).ToArray());
            IEnumerable<string> columns = list.Select(column => string.Join(rowDelimiter, column));

            string csv = string.Join(containerDelimiter.ToString(), columns);

            return StringCompression.Compress(csv);
        }
        
        public static string Decompress(byte[] source, int totalColumns, char containerDelimiter = ';', char rowDelimiter = ',')
        {
            string uncompressed = StringCompression.Decompress(source);
            List<string[]> list = new List<string[]>();
            foreach (string row in uncompressed.Split(containerDelimiter, totalColumns, StringSplitOptions.None))
                list.Add(row.Split(rowDelimiter).Select(csvValue => csvValue.Replace(containerDelimiter, rowDelimiter)).ToArray());
            string[][] outputArr = list.ToArray();

            list.Clear();
            for (int i = 0; i < totalColumns; i++)
                list.Add(outputArr.Select((row) => row[i]).ToArray());
            
            outputArr = list.ToArray();
            DecompressDelta(outputArr);

            IEnumerable<string> rows = outputArr.Select(row => string.Join(rowDelimiter, row));

            return string.Join(containerDelimiter, rows);
        }

        private static void CompressDelta(string[][] outputArr)
        {
            if (outputArr.Length <= 0)
                return;

            string[] lastRow = (string[])outputArr[0].Clone();
            int columnCount = lastRow.Length;

            for (int i = 1; i < outputArr.Length; i++)
            {
                string[] row = outputArr[i];
                for (int j = 0; j < columnCount; j++)
                {
                    if (row[j] == string.Empty)
                    {
                        row[j] = lastRow[j];
                        lastRow[j] = string.Empty;
                    }
                    else if (row[j] == lastRow[j])
                        row[j] = string.Empty;
                    else
                        lastRow[j] = row[j];
                }
            }
        }

        private static void DecompressDelta(string[][] outputArr)
        {
            if (outputArr.Length <= 0)
                return;
            
            string[] lastRow = (string[])outputArr[0].Clone();
            int columnCount = lastRow.Length;

            for (int i = 1; i < outputArr.Length; i++)
            {
                string[] row = outputArr[i];
                for (int j = 0; j < columnCount; j++)
                {
                    if (row[j] == lastRow[j])
                    {
                        row[j] = string.Empty;
                        lastRow[j] = string.Empty;
                    }
                    else if (row[j] == string.Empty)
                        row[j] = lastRow[j];
                    else
                        lastRow[j] = row[j];
                }
            }
        }
    }
}
