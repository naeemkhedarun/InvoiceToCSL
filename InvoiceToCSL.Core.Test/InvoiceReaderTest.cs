﻿using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InvoiceToCSL.Core.Test
{
    /// <summary>
    /// Summary description for InvoiceReaderTest
    /// </summary>
    [TestClass]
    public class InvoiceReaderTest
    {
        private readonly Stream singleReceiptStream = new MemoryStream(Encoding.Default.GetBytes("International Newsagent - Edinburgh 374 01/03/2008 3 3.50 10.50 "));
        private InvoiceReader invoiceReader;


        [TestMethod]
        public void InvoiceReader_ParseSingleReceipt_ReturnsOneReceipt()
        {
            invoiceReader = new InvoiceReader(singleReceiptStream);

            Assert.AreEqual(1, invoiceReader.GetReceipts().Count, "Failed to get correct number of receipts.");
        }

        [TestMethod]
        public void InvoiceReader_ParseSingleReceipt_CanReadDistro()
        {
            invoiceReader = new InvoiceReader(singleReceiptStream);
            
            Assert.AreEqual("International Newsagent - Edinburgh", invoiceReader.GetReceipts()[0].Distro, "Failed to read distro.");
        }

        [TestMethod]
        public void InvoiceReader_ParseSingleReceipt_CanReadDate()
        {
            invoiceReader = new InvoiceReader(singleReceiptStream);

            Assert.AreEqual(new DateTime(2008, 03, 01), invoiceReader.GetReceipts()[0].Date, "Failed to read date.");
        }

        [TestMethod]
        public void InvoiceReader_ParseSingleReceipt_CanReadIssue()
        {
            invoiceReader = new InvoiceReader(singleReceiptStream);

            Assert.AreEqual(374, invoiceReader.GetReceipts()[0].Issue, "Failed to read issue.");
        }

        [TestMethod]
        public void InvoiceReader_ParseSingleReceipt_CanReadQuantity()
        {
            invoiceReader = new InvoiceReader(singleReceiptStream);

            Assert.AreEqual(3, invoiceReader.GetReceipts()[0].Quantity, "Failed to read quantity.");
        }

        [TestMethod]
        public void InvoiceReader_ParseReceiptFile_ReturnsCorrectNumberOfReceipts()
        {
            invoiceReader = new InvoiceReader(new MemoryStream(Resource1.testinput));

            Assert.AreEqual(56, invoiceReader.GetReceipts().Count, "Failed to build correct receipts.");
        }

        [TestMethod]
        public void InvoiceReader_ParseReceiptFile_GetsInvoiceNumberAndMonthCorrectly()
        {
            invoiceReader = new InvoiceReader(new MemoryStream(Resource1.testinput));
            invoiceReader.GetReceipts();

            Assert.AreEqual(13289, invoiceReader.InvoiceNumber, "Failed to get invoice number.");
            Assert.AreEqual("March", invoiceReader.InvoiceMonth, "Failed to get invoice month.");
        }
    }
}