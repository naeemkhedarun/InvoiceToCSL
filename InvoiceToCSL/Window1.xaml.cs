using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using InvoiceToCSL.Core;
using Microsoft.Win32;

namespace InvoiceToCSL
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1
    {
        public Window1()
        {
            InitializeComponent();
        }

        public String InputFileName { get; set; }
        public String InputFileDir { get; set; }

        private void browseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
                                                {
                                                    InitialDirectory =
                                                        Environment.GetFolderPath(
                                                        Environment.SpecialFolder.DesktopDirectory),
                                                    Title = "Please choose a file",
                                                    CheckFileExists = true,
                                                    Filter = "Text File|*.txt"
                                                };

            if ((bool) openFileDialog.ShowDialog())
            {
                string dir_path = Path.GetDirectoryName(openFileDialog.FileName);
                if (!string.IsNullOrEmpty(dir_path))
                {
                    InputFileName = openFileDialog.FileName;
                    InputFileDir = dir_path;
                    outputTextBox.Text += Environment.NewLine + "File Selected!";
                    convertFileButton.Visibility = Visibility.Visible;
                }
            }
        }

        private void convertFile_Click(object sender, RoutedEventArgs e)
        {
            outputTextBox.Text += Environment.NewLine + "Converting...";

            FileStream stream = new FileStream(InputFileName, FileMode.Open);
            InvoiceReader reader = new InvoiceReader(stream);
            IList<Receipt> receipts = reader.GetReceipts();

            FileStream magazineOutputStream =
                new FileStream(InputFileDir + "/" + reader.InvoiceMonth + "-" + reader.InvoiceNumber + " - Magazine" + ".csv",
                               FileMode.Create);
            TextWriter magazineTextWriter = new StreamWriter(magazineOutputStream);

            FileStream newspaperOutputStream =
                            new FileStream(InputFileDir + "/" + reader.InvoiceMonth + "-" + reader.InvoiceNumber + " - Newspaper" + ".csv",
                                           FileMode.Create);
            TextWriter newspaperTextWriter = new StreamWriter(newspaperOutputStream);


            Receipt previousNewspaper = receipts[0];
            Receipt previousMagazine = receipts[0];

            foreach (Receipt receipt in receipts)
            {
                if (receipt.Issue < 10000)
                {
                    if (!receipt.Distro.Equals(previousMagazine.Distro)) magazineTextWriter.WriteLine(",,,");
                    magazineTextWriter.WriteLine(receipt);
                    previousMagazine = receipt;

                }else
                {
                    if (!receipt.Distro.Equals(previousNewspaper.Distro)) newspaperTextWriter.WriteLine(",,,");
                    newspaperTextWriter.WriteLine(receipt);
                    previousNewspaper = receipt;
                }
            }

            magazineTextWriter.Close();
            magazineOutputStream.Close();

            newspaperTextWriter.Close();
            newspaperOutputStream.Close();

            outputTextBox.Text += Environment.NewLine + "Finished!";
        }
    }
}