using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace PdfParsingiTextSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(ExtractTextFromPdf("/Users/katiestone/Downloads/Mixtureinstructiontest.pdf"));
        }

        public static string ExtractTextFromPdf(string path)
        {
            using (PdfReader reader = new PdfReader(path))
            {
                StringBuilder text = new StringBuilder();

                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    text.Append(PdfTextExtractor.GetTextFromPage(reader, i));
                }

                string result;
                result =  text.ToString();
                
                Regex rx = new(@"(\d+) (.+) \$(\d+.\d+) (\d{8})( \w*|\s)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                Regex rxDrugName = new(@"Mixture Instructions for (.+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                Regex rxNumber = new(@"Tx: (.+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                Regex rxInstrution = new(@"Instructions(?:\s|\n)*(\d(?:.|\n)*)Page", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                MatchCollection matches = rx.Matches(result);
                Match nameMatches = rxDrugName.Match(result);
                Match numberMatches = rxNumber.Match(result);
                Match instrutionMatch = rxInstrution.Match(result);

                Console.WriteLine("Drug Info");
                foreach (var match in matches)
                {
                    var m = (Match)match;
                    Console.WriteLine(m.Groups[1]);
                    Console.WriteLine(m.Groups[2]);
                    Console.WriteLine(m.Groups[3]);
                    Console.WriteLine(m.Groups[4]);
                    Console.WriteLine(m.Groups[5]);
                }
                 
                Console.WriteLine("Name");
                Console.WriteLine(nameMatches.Groups[1]);
                Console.WriteLine("RX Number");
                Console.WriteLine(numberMatches.Groups[1]);
                Console.WriteLine("Instructions");
                Console.WriteLine(instrutionMatch.Groups[1]);


                return text.ToString();

            }
        }
    }
}
