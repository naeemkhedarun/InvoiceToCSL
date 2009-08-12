// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvoiceReader.cs" company="Khedan">
//   Naeem Khedarun
// </copyright>
// <summary>
//   Defines the InvoiceReader type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace InvoiceToCSL.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Parses the raw invoice and hydrates receipt objects.
    /// </summary>
    public class InvoiceReader
    {
        /// <summary>
        /// Regex for invoice header.
        /// </summary>
        private const string InvoiceRegex = @"Invoice Month: (?<month>[a-zA-Z]+)[\n\r]+No (?<number>[0-9]+)";

        /// <summary>
        /// Regex for receipt.
        /// </summary>
        private const string RecieptRegex = @"(?<distro>.+)[ ]+(?<issue>[0-9]+) (?<date>[0-9]+/[0-9]+/[0-9]+) (?<quantity>[0-9]+) (?<dec1>[0-9]+\.[0-9]+) (?<dec2>[0-9]+\.[0-9]+)";

        /// <summary>
        /// Initializes a new instance of the <see cref="InvoiceReader"/> class.
        /// </summary>
        /// <param name="inputStream">The input stream.</param>
        public InvoiceReader(Stream inputStream)
        {
            this.InputStream = inputStream;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvoiceReader"/> class.
        /// </summary>
        public InvoiceReader()
        {
        }

        /// <summary>
        /// Gets or sets the input stream.
        /// </summary>
        /// <value>The input stream.</value>
        public Stream InputStream { get; set; }

        /// <summary>
        /// Gets or sets the invoice number.
        /// </summary>
        /// <value>The invoice number.</value>
        public int InvoiceNumber { get; set; }

        /// <summary>
        /// Gets or sets the invoice month.
        /// </summary>
        /// <value>The invoice month.</value>
        public string InvoiceMonth { get; set; }

        /// <summary>
        /// Gets the receipts.
        /// </summary>
        /// <returns>Hydrated receipts.</returns>
        public List<Receipt> GetReceipts()
        {
            List<Receipt> receipts = new List<Receipt>();

            // Take line by line and parse.
            string oldLine = string.Empty;
            using (StreamReader sr = new StreamReader(this.InputStream))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    // Parse
                    Regex receiptRegex = new Regex(RecieptRegex, RegexOptions.Compiled);
                    Match match = receiptRegex.Match(line);

                    if (match.Success)
                    {
                        string distro = match.Groups["distro"].Value;
                        int issue = Int32.Parse(match.Groups["issue"].Value);
                        string rawDate = match.Groups["date"].Value;
                        int quantity = Int32.Parse(match.Groups["quantity"].Value);

                        receipts.Add(new Receipt(rawDate, distro, issue, quantity));
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(this.InvoiceMonth))
                        {
                            Regex invoiceNumber = new Regex(InvoiceRegex, RegexOptions.Compiled);
                            string input = oldLine + Environment.NewLine + line;
                            Match invoiceMatch = invoiceNumber.Match(input);

                            if (invoiceMatch.Success)
                            {
                                this.InvoiceMonth = invoiceMatch.Groups["month"].Value;
                                this.InvoiceNumber = Int32.Parse(invoiceMatch.Groups["number"].Value);
                            }
                        }
                    }
                    this.FinishedReadingLine(line.Length);
                    oldLine = line;
                }
            }

            receipts.Sort();
            return receipts;
        }

        /// <summary>
        /// Fills the missing dates.
        /// </summary>
        /// <param name="receipts">The receipts.</param>
        public void FillMissingDates(List<Receipt> receipts)
        {

            // Group by distro.
            IEnumerable<IGrouping<string, Receipt>> grouped = from r in receipts
                                     group r by r.Distro;

            List<Receipt> fullNewsPapers = new List<Receipt>();

            foreach (var group in grouped)
            {
                List<Receipt> distroReceipts = new List<Receipt>();
                distroReceipts.AddRange(new List<Receipt>(group).FindAll(receipt => receipt.Issue >= 10000));
                if (distroReceipts.Count == 0)
                {
                    continue;
                }

                List<int> days = new List<int>();
                List<int> missingDays = new List<int>();

//                DateTime date = DateTime.ParseExact(match.Groups["date"].Value, "dd/MM/yyyy", null);

                distroReceipts
                    .ForEach(currentReceipt => days.Add(DateTime.ParseExact(currentReceipt.Date, "dd/MM/yyyy", null).Day));

                for (int i = 1; i < 32; i++)
                {
                    if (!days.Contains(i))
                    {
                        missingDays.Add(i);
                    }
                }

                int month = DateTime.ParseExact(distroReceipts[0].Date, "dd/MM/yyyy", null).Month;
                int year = DateTime.ParseExact(distroReceipts[0].Date, "dd/MM/yyyy", null).Year;
                string distro = distroReceipts[0].Distro;

                foreach (int day in missingDays)
                {
                    distroReceipts.Add(new Receipt(string.Format("{0}/{1}/{2}", day, month, year), distro, 99999, 0));
                }

                fullNewsPapers.AddRange(distroReceipts);
            }

            List<Receipt> magazines = new List<Receipt>(receipts.FindAll(receipt => receipt.Issue < 10000));

            receipts.Clear();

            receipts.AddRange(fullNewsPapers);
            receipts.AddRange(magazines);

            receipts.Sort();
        }

        public event Action<long> FinishedReadingLine;
    }
}