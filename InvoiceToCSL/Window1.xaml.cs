using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using InvoiceToCSL.Core;
using Microsoft.Win32;

namespace InvoiceToCSL
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : IProgressWindow
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

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += WorkerOnDoWork;

            worker.RunWorkerAsync(this);
        }

        private void WorkerOnDoWork(object sender, DoWorkEventArgs args)
        {
            IProgressWindow window = args.Argument as IProgressWindow;
            FileStream stream = new FileStream(InputFileName, FileMode.Open);

            long overallLength = stream.Length;

            InvoiceReader reader = new InvoiceReader(stream);
            reader.FinishedReadingLine += lineLength =>
                                              {
                                                  window.UpdateProgress(((double)lineLength / (double)overallLength)); 
                                              };

            List<Receipt> receipts = reader.GetReceipts();

            reader.FillMissingDates(receipts);

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
                    if (!receipt.Distro.Equals(previousMagazine.Distro))
                    {
                        magazineTextWriter.WriteLine(",,,");
                    }

                    magazineTextWriter.WriteLine(receipt);
                    previousMagazine = receipt;

                }
                else
                {
                    if (!receipt.Distro.Equals(previousNewspaper.Distro))
                    {
                        newspaperTextWriter.WriteLine(",,,");
                    }

                    newspaperTextWriter.WriteLine(receipt);
                    previousNewspaper = receipt;
                }
            }

            magazineTextWriter.Close();
            magazineOutputStream.Close();

            newspaperTextWriter.Close();
            newspaperOutputStream.Close();

            window.WorkDone();

        }

        public void UpdateProgress(double progress)
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback) delegate { this.progressBar.SetValue(ProgressBar.ValueProperty, (double)this.progressBar.GetValue(ProgressBar.ValueProperty) + progress); }, null);
        }

        public void WorkDone()
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate
                                                                                               {
                                                                                                   outputTextBox.Text += Environment.NewLine + "Finished!";
                                                                                                   progressBar.SetValue(ProgressBar.ValueProperty, 1.0);
                                                                                               }, null);
        }
    }

    public interface IProgressWindow
    {
        void UpdateProgress(double progress);
        void WorkDone();
    }
}