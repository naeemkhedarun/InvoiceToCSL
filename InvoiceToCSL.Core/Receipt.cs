using System;

namespace InvoiceToCSL.Core
{
    public class Receipt : IComparable<Receipt>
    {
        public Receipt(DateTime date, string distro, int issue, int quantity)
        {
            Date = date;
            Distro = distro;
            Issue = issue;
            Quantity = quantity;

            Distro = Distro.Replace(',', '-');
        }

        public Receipt()
        {
        }

        public string Distro { get; set; }
        public int Issue { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }

        #region IComparable<Receipt> Members

        public int CompareTo(Receipt other)
        {
            int to = Distro.CompareTo(other.Distro);
            if (to == 0)
            {
                return Issue.CompareTo(other.Issue);
            }
            return to;
        }

        #endregion

        public override string ToString()
        {
            return Distro + ", " + Issue + ", " + Date + ", " + Quantity;
        }
    }
}