using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace BankingSystem
{
    class FileWriting
    {
        public static void writeOrAddToFile(string path,List<string> transactions)
        {
            CultureInfo.CurrentUICulture = new CultureInfo("el-GR");
            if (!File.Exists(path))
            {
                // Create a file to write to.
                string createText = string.Format("{0,-10} {1,-10} {2,-10} {3,-30} {4,15}\n", "Action", "From", "To", "Transaction Date", "Amount"); 
                File.WriteAllText(path, createText + Environment.NewLine);
            }


            using (FileStream bs = new FileStream(path, FileMode.Append))
            {
                using (StreamWriter sw = new StreamWriter(bs))
                {
                    //bw.Write(transactions)
                    foreach (string text in transactions)
                    {
                        sw.Write(text+ Environment.NewLine);
                    }
                    
                }
            }
        }
    }
}
