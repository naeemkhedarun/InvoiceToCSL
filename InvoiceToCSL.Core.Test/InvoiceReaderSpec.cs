using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using InvoiceToCSL.Core.Test.BDD;
using MbUnit.Framework;

namespace InvoiceToCSL.Core.Test
{
    public class invoice_reader_context : ContextSpecification<InvoiceReader>
    {
        protected override InvoiceReader create_sut()
        {
            return new InvoiceReader();
        }

        protected override void because()
        {
        }

        protected override void establish_context()
        {
            this.sut = this.create_sut();
        }
    }

    [TestFixture]
    public class when_the_invoice_reader_is_asked_to_fill_in_the_missing_dates : invoice_reader_context
    {
        private List<Receipt> receipts;

        protected override void establish_context()
        {
            base.establish_context();

            this.receipts = new List<Receipt>
                                {
                                    new Receipt(new DateTime(2009, 01, 01), "Distro", 1, 1),
                                    new Receipt(new DateTime(2009, 01, 09), "Distro", 1, 1),
                                    new Receipt(new DateTime(2009, 01, 10), "Distro", 1, 1),
                                    new Receipt(new DateTime(2009, 01, 12), "Distro", 1, 1),
                                    new Receipt(new DateTime(2009, 01, 13), "Distro", 1, 1),
                                    new Receipt(new DateTime(2009, 01, 19), "Distro", 1, 1),
                                    new Receipt(new DateTime(2009, 01, 25), "Distro", 1, 1),
                                };

        }

        protected override void because()
        {
            this.sut.FillMissingDates(receipts);
        }

        [Test]
        public void should_have_thirty_one_entries()
        {
            this.receipts.Count.should_be_equal_to(31);
        }

        [Test]
        public void should_have_one_entry_per_day()
        {
            int day = 1;
            new List<Receipt>(this.receipts)
                .ForEach(receipt => receipt.Date.Day.should_be_equal_to(day++));
        }
    }

    [TestFixture]
    public class when_the_invoice_reader_is_asked_to_fill_in_the_missing_dates_for_multiple_distros : invoice_reader_context
    {
        private List<Receipt> receipts;

        protected override void establish_context()
        {
            base.establish_context();

            this.receipts = new List<Receipt>
                                {
                                    new Receipt(new DateTime(2009, 01, 01), "Distro", 1, 1),
                                    new Receipt(new DateTime(2009, 01, 09), "Distro", 1, 1),
                                    new Receipt(new DateTime(2009, 01, 10), "Distro", 1, 1),
                                    new Receipt(new DateTime(2009, 01, 12), "Distro2", 1, 1),
                                    new Receipt(new DateTime(2009, 01, 13), "Distro2", 1, 1),
                                    new Receipt(new DateTime(2009, 01, 19), "Distro2", 1, 1),
                                    new Receipt(new DateTime(2009, 01, 25), "Distro2", 1, 1),
                                };

        }

        protected override void because()
        {
            this.sut.FillMissingDates(receipts);
        }

        [Test]
        public void should_have_thirty_one_entries()
        {
            this.receipts.Count.should_be_equal_to(62);
        }
    }
}
