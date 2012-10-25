using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace DomainChecker
{
    class Program
    {
        private const string WORDLIST_FILE_PATH = "wordlist.txt";
        private const string OUTPUT_FILE_PATH = "output.txt";
        private const string IGNORE_FILE_PATH = "ignore.txt";
        
        private static List<string> words = new List<string>();
        private static Dictionary<string, string> ignoreList = new Dictionary<string, string>();

        static void Main(string[] args)
        {
            String line;

            //if (File.Exists(IGNORE_FILE_PATH))
            //{

            //    using (StreamReader sr = new StreamReader(IGNORE_FILE_PATH))
            //    {

            //        while ((line = sr.ReadLine().Trim()) != null)
            //        {
            //            if (!ignoreList.Keys.Contains(line))
            //                ignoreList.Add(line.Trim(), line);
            //        }
            //    }
            //}

            if (File.Exists(WORDLIST_FILE_PATH))
            {

                using (StreamReader sr = new StreamReader(WORDLIST_FILE_PATH))
                {
                    
                    while ((line = sr.ReadLine()) != null)
                    {
                        string word = line.Split(' ')[0].Trim();

                        if (word.Length >= 4 && word.Length<= 6 && !ignoreList.Keys.Contains(word))
                        {
                            words.Add(word);
                        }
                    }
                }
            }

            using (StreamWriter ignoreWriter = new StreamWriter(IGNORE_FILE_PATH,true))
            {
                using (StreamWriter outputWriter = new StreamWriter(OUTPUT_FILE_PATH))
                {
                    foreach (string word in words)
                    {
                        ignoreWriter.WriteLine(word);

                        WebClient client = new WebClient();
                        string result = client.DownloadString("http://dnsw.info/" + word +".io");

                        if (!result.Contains("Not available"))
                            outputWriter.WriteLine(word);
                    }

                }
            }

            Console.Write(words.Count);
            Console.ReadLine();
        }

        
    }
}
