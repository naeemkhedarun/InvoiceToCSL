using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace InvoiceToCSL.Core
{
    public class InvoiceReader
    {
        private const string INVOICE_REGEX =
            @"Invoice Month: (?<month>[a-zA-Z]+)[\n\r]+No (?<number>[0-9]+)";

        private const string RECIEPT_REGEX =
            @"(?<distro>.+)[ ]+(?<issue>[0-9]+) (?<date>[0-9]+/[0-9]+/[0-9]+) (?<quantity>[0-9]+) (?<dec1>[0-9]+\.[0-9]+) (?<dec2>[0-9]+\.[0-9]+)";

        public InvoiceReader(Stream inputStream)
        {
            InputStream = inputStream;
        }

        public Stream InputStream { get; set; }
        public int InvoiceNumber { get; set; }


        public string InvoiceMonth { get; set; }

        public IList<Receipt> GetReceipts()
        {
            List<Receipt> receipts = new List<Receipt>();

            //Take line by line and parse.
            String oldLine = String.Empty;
            using (StreamReader sr = new StreamReader(InputStream))
            {
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    //Parse
                    Regex receiptRegex = new Regex(RECIEPT_REGEX, RegexOptions.Compiled);
                    Match match = receiptRegex.Match(line);

                    if (match.Success)
                    {
                        String distro = match.Groups["distro"].Value;
                        int issue = Int32.Parse(match.Groups["issue"].Value);
                        DateTime date = DateTime.ParseExact(match.Groups["date"].Value, "dd/MM/yyyy", null);
                        int quantity = Int32.Parse(match.Groups["quantity"].Value);

                        receipts.Add(new Receipt(date, distro, issue, quantity));
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(InvoiceMonth))
                        {
                            Regex invoiceNumber = new Regex(INVOICE_REGEX, RegexOptions.Compiled);
                            string input = oldLine + Environment.NewLine + line;
                            Match invoiceMatch = invoiceNumber.Match(input);

                            if (invoiceMatch.Success)
                            {
                                InvoiceMonth = invoiceMatch.Groups["month"].Value;
                                InvoiceNumber = Int32.Parse(invoiceMatch.Groups["number"].Value);
                            }
                        }
                    }
                    oldLine = line;
                }
            }

            receipts.Sort();
            return receipts;
        }

//        private static void FillMissingDates(IList<Receipt> receipts)
//        {
//            Receipt previous = receipts[0];
//            Dictionary<int, Receipt> inserts = new Dictionary<int, Receipt>();
//
//            for (int i = 0; i < receipts.Count; i++)
//            {
//                Receipt receipt = receipts[i];
//                if (!receipt.Equals(previous) && !previous.Date.AddDays(1).Equals(receipt.Date))
//                {
//                    Receipt newReceipt = new Receipt(previous.Date.AddDays(1),
//                        receipt.Distro, 0, 0);
//                    inserts.Add(i, newReceipt);
//                }
//                previous = receipt;
//            }
//
//            foreach (KeyValuePair<int, Receipt> pair in inserts)
//            {
//                receipts.Insert(pair.Key, pair.Value);
//            }
//        }
    }
}