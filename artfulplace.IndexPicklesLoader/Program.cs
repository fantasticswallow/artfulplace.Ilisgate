using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace artfulplace.IndexPicklesLoader
{
    class Program
    {
        // Picklesを読み込みます
        static void Main(string[] args)
        {
            var picklePath = "tag_index.pkl";
            // PROTO VERSION EMPTY_LIST BINPUT MARK
            // 6byte進めてスタート

            var bytes = File.ReadAllBytes(picklePath);
            var index = 6;
            var isEOF = false;

            var list = new List<Tuple<string, int>>();
            var dataName = "";
            var itemIndex = 1;
            var lastMode = 0;

            while (!isEOF)
            {
                var data = bytes[index];
                if (data == 0x58)
                {
                    var charCount = BitConverter.ToInt32(bytes, index + 1);

                    index += 5;
                    dataName = Encoding.ASCII.GetString(bytes, index, charCount);
                    index += charCount;
                    lastMode = 1;
                }
                else if (data == 0x4D)
                {
                    var freqCount = BitConverter.ToInt16(bytes, index + 1);
                    index += 3;
                    // データ構造的にfreqのタイミングではdataName解析済み
                    list.Add(Tuple.Create(dataName, (int)freqCount));
                    lastMode = 99;
                }
                else if (data == 0x4B)
                {
                    var freqCount = bytes[index + 1];
                    index += 2;
                    // データ構造的にfreqのタイミングではdataName解析済み
                    list.Add(Tuple.Create(dataName, (int)freqCount));
                    lastMode = 96;
                }
                else if (data == 0x71)
                {
                    // index counter
                    var currentIndex = bytes[index + 1];
                    if ((int)currentIndex == itemIndex)
                    {
                        itemIndex += 1;
                        index += 2;
                    }
                    else
                    {
                        Console.WriteLine($"Error : Index Counter is not match. current : {currentIndex}, expect : {itemIndex}, length {index}");
                    }
                    lastMode = 32;
                }
                else if (data == 0x72)
                {
                    // index counter
                    var currentIndex = BitConverter.ToInt32(bytes, index + 1);
                    if ((int)currentIndex == itemIndex)
                    {
                        itemIndex += 1;
                        index += 5;
                    }
                    else
                    {
                        Console.WriteLine($"Error : Index Counter is not match. current : {currentIndex}, expect : {itemIndex}, length {index}");
                    }
                    lastMode = 33;
                }
                else if (data == 0x75)
                {
                    // add List Limitation
                    if (bytes[index + 1] == 0x2E)
                    {
                        Console.WriteLine("File read end.");
                        isEOF = true;
                    }
                    else if (bytes[index + 1] == 0x28)
                    {
                        // 正常系なので放置
                        Console.WriteLine($"notice : index limitation. index: {itemIndex}");
                        index += 2;
                    }
                    else
                    {
                        // 期待してないエラー
                        Console.WriteLine($"Error : Unexpected Data : {data}, item Index {itemIndex}, length {index}, dataName {dataName}, lastMode {lastMode}");
                        isEOF = true;
                    }
                    lastMode = 255;
                }
                else
                {
                    Console.WriteLine($"Error : Unexpected data : {data}, item Index {itemIndex}, length {index}, dataName {dataName}, lastMode {lastMode}");
                    isEOF = true;
                }
            }

            using (var sw = new StreamWriter("hogehoge.csv"))
            {
                sw.WriteLine("tag, freq");
                foreach (var item in list)
                {
                    sw.WriteLine($"{item.Item1}, {item.Item2}");
                }
            }

        }
    }
}
