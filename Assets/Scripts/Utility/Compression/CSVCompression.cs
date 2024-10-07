using SimpleJSONFixed;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utility
{
    class CSVCompression
    {
        private static readonly char[] Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

        public static object[] Compress(string source, int deltaRows, char containerDelimiter = ';', char rowDelimiter = ',')
        {
            List<string[]> list = new List<string[]>();
            foreach (string row in source.Split(containerDelimiter))
                list.Add(row.Trim().Split(rowDelimiter));
            string[][] outputArr = list.ToArray();
            Dictionary<string, string> symbolTable = new Dictionary<string, string>();
            if (outputArr.Length > 0)
            {
                CompressDelta(outputArr, deltaRows);
                // CompressSymbol(outputArr, symbolTable);
            }
            List<string> outputList = new List<string>();
            foreach (string[] strArray in outputArr)
                outputList.Add(string.Join(rowDelimiter.ToString(), strArray));
            byte[] compressed = StringCompression.Compress(string.Join(containerDelimiter.ToString(), outputList.ToArray()));
            JSONNode symbolTableAsJson = new JSONObject();
            foreach (string key in symbolTable.Keys)
                symbolTableAsJson.Add(key, symbolTable[key]);
            return new object[] { compressed, symbolTableAsJson };
        }

        public static string Decompress(byte[] source, JSONNode symbolTable, int deltaRows, char containerDelimiter = ';', char rowDelimiter = ',')
        {
            string uncompressed = StringCompression.Decompress(source);
            List<string[]> list = new List<string[]>();
            foreach (string row in uncompressed.Split(containerDelimiter))
                list.Add(row.Split(rowDelimiter));
            string[][] outputArr = list.ToArray();
            if (outputArr.Length > 0)
            {
                // DecompressSymbol(outputArr, symbolTable);
                DecompressDelta(outputArr, deltaRows);
            }
            List<string> outputList = new List<string>();
            foreach (string[] strArray in outputArr)
                outputList.Add(string.Join(rowDelimiter.ToString(), strArray));
            return string.Join(containerDelimiter.ToString(), outputList.ToArray());
        }

        private static void CompressDelta(string[][] outputArr, int deltaRows)
        {
            string[] lastRow = new string[100];
            for (int i = 0; i < outputArr[0].Length; i++)
                lastRow[i] = outputArr[0][i];
            for (int i = 1; i < outputArr.Length; i++)
            {
                string[] row = outputArr[i];
                for (int j = 0; j < deltaRows; j++)
                {
                    if (row[j] == lastRow[j])
                        row[j] = string.Empty;
                    else
                        lastRow[j] = row[j];
                }
            }
        }

        private static void CompressSymbol(string[][] outputArr, Dictionary<string, string> symbolTable)
        {
            int currentSymbolCount = 0;
            string currentSymbol = ToBase62(currentSymbolCount);
            int currentSize = 0;
            Dictionary<string, int> itemCounter = new Dictionary<string, int>();
            Dictionary<string, string> itemToSymbol = new Dictionary<string, string>();
            foreach (string[] row in outputArr)
            {
                foreach (string item in row)
                {
                    if (itemCounter.ContainsKey(item))
                        itemCounter[item] += 1;
                    else
                        itemCounter.Add(item, 1);
                }
            }
            while (itemCounter.ContainsKey(currentSymbol))
            {
                currentSymbolCount += 1;
                currentSymbol = ToBase62(currentSymbolCount);
            }
            foreach (KeyValuePair<string, int> pair in itemCounter.OrderByDescending(x => x.Value))
            {
                if (pair.Value < 3)
                    break;
                if (currentSize > 10000)
                    break;
                if (pair.Key.Length <= currentSymbol.Length)
                    continue;
                symbolTable.Add(currentSymbol, pair.Key);
                itemToSymbol.Add(pair.Key, currentSymbol);
                currentSymbolCount += 1;
                currentSymbol = ToBase62(currentSymbolCount);
                currentSize += currentSymbol.Length + pair.Key.Length + 6;
            }
            foreach (string[] row in outputArr)
            {
                for (int i = 0; i < row.Length; i++)
                {
                    if (itemToSymbol.ContainsKey(row[i]))
                        row[i] = itemToSymbol[row[i]];
                }
            }
        }

        private static void DecompressDelta(string[][] outputArr, int deltaRows)
        {
            string[] lastRow = (string[])outputArr[0].Clone();
            for (int i = 1; i < outputArr.Length; i++)
            {
                string[] row = outputArr[i];
                for (int j = 0; j < deltaRows; j++)
                {
                    if (row[j] == string.Empty)
                        row[j] = lastRow[j];
                    else
                        lastRow[j] = row[j];
                }
            }
        }

        private static void DecompressSymbol(string[][] outputArr, JSONNode symbolTable)
        {
            foreach (string[] row in outputArr)
            {
                for (int i = 0; i < row.Length; i++)
                {
                    if (symbolTable.HasKey(row[i]))
                        row[i] = symbolTable[row[i]];
                }
            }
        }

        private static string ToBase62(int index)
        {
            if (index == 0)
                return Alphabet[index].ToString();
            List<string> list = new List<string>();
            int alphabetLength = Alphabet.Length;
            while (index > 0)
            {
                int remainder = index % alphabetLength;
                index = index / alphabetLength;
                list.Add(Alphabet[remainder].ToString());
            }
            list.Reverse();
            return string.Join("", list.ToArray());
        }

    }
}
