// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Receipt.cs" company="Khedan">
//   Naeem Khedarun
// </copyright>
// <summary>
//   Defines the Receipt type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace InvoiceToCSL.Core
{
    using System;

    /// <summary>
    /// Receipt Entity
    /// </summary>
    public class Receipt : IComparable<Receipt>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Receipt"/> class.
        /// </summary>
        /// <param name="date">The receipt date.</param>
        /// <param name="distro">The distro.</param>
        /// <param name="issue">The issue.</param>
        /// <param name="quantity">The quantity.</param>
        public Receipt(string date, string distro, int issue, int quantity)
        {
            this.Date = date;
            this.Distro = distro;
            this.Issue = issue;
            this.Quantity = quantity;

            this.Distro = this.Distro.Replace(',', '-');
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Receipt"/> class.
        /// </summary>
        public Receipt()
        {
        }

        /// <summary>
        /// Gets or sets the distro.
        /// </summary>
        /// <value>The distro.</value>
        public string Distro { get; set; }

        /// <summary>
        /// Gets or sets the issue.
        /// </summary>
        /// <value>The issue.</value>
        public int Issue { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The receipt date.</value>
        public string Date { get; set; }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        /// <value>The quantity.</value>
        public int Quantity { get; set; }

        #region IComparable<Receipt> Members

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings:
        /// Value
        /// Meaning
        /// Less than zero
        /// This object is less than the <paramref name="other"/> parameter.
        /// Zero
        /// This object is equal to <paramref name="other"/>.
        /// Greater than zero
        /// This object is greater than <paramref name="other"/>.
        /// </returns>
        public int CompareTo(Receipt other)
        {
            int to = this.Distro.CompareTo(other.Distro);
            
            return to == 0 
                    ? this.Date.CompareTo(other.Date) 
                    : to;
        }

        #endregion

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(
                "{0}, {1}, {2}, {3}", 
                this.Distro, 
                this.Issue, 
                this.Date, 
                this.Quantity);
        }
    }
}