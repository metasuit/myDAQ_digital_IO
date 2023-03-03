/******************************************************************************
*
* Example program:
*   WriteDigChan
*
* Category:
*   DO
*
* Description:
*   This example demonstrates how to write values to a digital output channel.
*
* Instructions for running:
*   1.  Select the channel parameters on the DAQ device to be written.Note: You
*       must specify exactly 8 lines in the channel string box.
*   2.  Use the checkboxes to select a value to write.
*
* Steps:
*   1.  Create a new task and a digital output channel.
*   2.  Create a DigitalSingleChannelWriter and call the
*       WriteSingleSampleMultiLine method to write the data to the channel.
*   3.  Dispose the Task object to clean-up any resources associated with the
*       task.
*   4.  Handle any DaqExceptions, if they occur.
*
* I/O Connections Overview:
*   Make sure your signal output terminals match the Lines text box. In this
*   case wire the item to receive the signal to the specified eight digital
*   lines on your DAQ Device.  For more information on the input and output
*   terminals for your device, open the NI-DAQmx Help, and refer to the NI-DAQmx
*   Device Terminals and Device Considerations books in the table of contents.
*
* Microsoft Windows Vista User Account Control
*   Running certain applications on Microsoft Windows Vista requires
*   administrator privileges, 
*   because the application name contains keywords such as setup, update, or
*   install. To avoid this problem, 
*   you must add an additional manifest to the application that specifies the
*   privileges required to run 
*   the application. Some Measurement Studio NI-DAQmx examples for Visual Studio
*   include these keywords. 
*   Therefore, all examples for Visual Studio are shipped with an additional
*   manifest file that you must 
*   embed in the example executable. The manifest file is named
*   [ExampleName].exe.manifest, where [ExampleName] 
*   is the NI-provided example name. For information on how to embed the manifest
*   file, refer to http://msdn2.microsoft.com/en-us/library/bb756929.aspx.Note: 
*   The manifest file is not provided with examples for Visual Studio .NET 2003.
*
******************************************************************************/

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using NationalInstruments.DAQmx;
using System.Threading;

namespace NationalInstruments.Examples.WriteDigChan
{
    /// <summary>
    /// Summary description for Mainform.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.CheckBox bit0CheckBox;
        private System.Windows.Forms.CheckBox bit1CheckBox;
        private System.Windows.Forms.CheckBox bit7CheckBox;
        private System.Windows.Forms.Label dataToWriteLabel;
        private System.Windows.Forms.CheckBox bit2CheckBox;
        private System.Windows.Forms.Label channelParamsLabel;
        private System.Windows.Forms.Label bit0Label;
        private System.Windows.Forms.Label bit1Label;
        private System.Windows.Forms.Label bit2Label;
        private System.Windows.Forms.Label bit7Label;
        private System.Windows.Forms.Label warningLabel;
        private System.Windows.Forms.Button writeButton;
        private System.Windows.Forms.ComboBox physicalChannelComboBox;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private Button StopButton;
        private Button WriteOnce;
        private NumericUpDown delay;
        private BackgroundWorker _worker;

        public MainForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            _worker = new BackgroundWorker();
            _worker.WorkerSupportsCancellation = true;
            _worker.DoWork += Worker_DoWork;
            _worker.RunWorkerCompleted += Worker_RunWorkerCompleted;

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            physicalChannelComboBox.Items.AddRange(DaqSystem.Local.GetPhysicalChannels(PhysicalChannelTypes.DOLine, PhysicalChannelAccess.External));
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataToWriteLabel = new System.Windows.Forms.Label();
            this.writeButton = new System.Windows.Forms.Button();
            this.channelParamsLabel = new System.Windows.Forms.Label();
            this.bit0CheckBox = new System.Windows.Forms.CheckBox();
            this.bit1CheckBox = new System.Windows.Forms.CheckBox();
            this.bit7CheckBox = new System.Windows.Forms.CheckBox();
            this.bit2CheckBox = new System.Windows.Forms.CheckBox();
            this.bit0Label = new System.Windows.Forms.Label();
            this.bit1Label = new System.Windows.Forms.Label();
            this.bit2Label = new System.Windows.Forms.Label();
            this.bit7Label = new System.Windows.Forms.Label();
            this.warningLabel = new System.Windows.Forms.Label();
            this.physicalChannelComboBox = new System.Windows.Forms.ComboBox();
            this.StopButton = new System.Windows.Forms.Button();
            this.WriteOnce = new System.Windows.Forms.Button();
            this.delay = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.delay)).BeginInit();
            this.SuspendLayout();
            // 
            // dataToWriteLabel
            // 
            this.dataToWriteLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.dataToWriteLabel.Location = new System.Drawing.Point(54, 164);
            this.dataToWriteLabel.Name = "dataToWriteLabel";
            this.dataToWriteLabel.Size = new System.Drawing.Size(204, 23);
            this.dataToWriteLabel.TabIndex = 4;
            this.dataToWriteLabel.Text = "Data to Write";
            // 
            // writeButton
            // 
            this.writeButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.writeButton.Location = new System.Drawing.Point(30, 281);
            this.writeButton.Name = "writeButton";
            this.writeButton.Size = new System.Drawing.Size(128, 35);
            this.writeButton.TabIndex = 0;
            this.writeButton.Text = "&Write";
            this.writeButton.Click += new System.EventHandler(this.WriteButton_Click);
            // 
            // channelParamsLabel
            // 
            this.channelParamsLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.channelParamsLabel.Location = new System.Drawing.Point(26, 12);
            this.channelParamsLabel.Name = "channelParamsLabel";
            this.channelParamsLabel.Size = new System.Drawing.Size(179, 23);
            this.channelParamsLabel.TabIndex = 1;
            this.channelParamsLabel.Text = "Lines";
            // 
            // bit0CheckBox
            // 
            this.bit0CheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bit0CheckBox.Location = new System.Drawing.Point(58, 199);
            this.bit0CheckBox.Name = "bit0CheckBox";
            this.bit0CheckBox.Size = new System.Drawing.Size(25, 23);
            this.bit0CheckBox.TabIndex = 6;
            this.bit0CheckBox.Text = "Line0";
            // 
            // bit1CheckBox
            // 
            this.bit1CheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bit1CheckBox.Location = new System.Drawing.Point(89, 199);
            this.bit1CheckBox.Name = "bit1CheckBox";
            this.bit1CheckBox.Size = new System.Drawing.Size(26, 23);
            this.bit1CheckBox.TabIndex = 8;
            this.bit1CheckBox.Text = "Line1";
            // 
            // bit7CheckBox
            // 
            this.bit7CheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bit7CheckBox.Location = new System.Drawing.Point(238, 199);
            this.bit7CheckBox.Name = "bit7CheckBox";
            this.bit7CheckBox.Size = new System.Drawing.Size(26, 23);
            this.bit7CheckBox.TabIndex = 20;
            this.bit7CheckBox.Text = "Line7";
            // 
            // bit2CheckBox
            // 
            this.bit2CheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bit2CheckBox.Location = new System.Drawing.Point(121, 199);
            this.bit2CheckBox.Name = "bit2CheckBox";
            this.bit2CheckBox.Size = new System.Drawing.Size(26, 23);
            this.bit2CheckBox.TabIndex = 10;
            this.bit2CheckBox.Text = "Line2";
            // 
            // bit0Label
            // 
            this.bit0Label.Location = new System.Drawing.Point(58, 234);
            this.bit0Label.Name = "bit0Label";
            this.bit0Label.Size = new System.Drawing.Size(25, 23);
            this.bit0Label.TabIndex = 5;
            this.bit0Label.Text = "0";
            // 
            // bit1Label
            // 
            this.bit1Label.Location = new System.Drawing.Point(89, 234);
            this.bit1Label.Name = "bit1Label";
            this.bit1Label.Size = new System.Drawing.Size(26, 23);
            this.bit1Label.TabIndex = 7;
            this.bit1Label.Text = "1";
            // 
            // bit2Label
            // 
            this.bit2Label.Location = new System.Drawing.Point(121, 234);
            this.bit2Label.Name = "bit2Label";
            this.bit2Label.Size = new System.Drawing.Size(26, 23);
            this.bit2Label.TabIndex = 9;
            this.bit2Label.Text = "2";
            // 
            // bit7Label
            // 
            this.bit7Label.Location = new System.Drawing.Point(212, 234);
            this.bit7Label.Name = "bit7Label";
            this.bit7Label.Size = new System.Drawing.Size(79, 23);
            this.bit7Label.TabIndex = 19;
            this.bit7Label.Text = "ON/OFF";
            // 
            // warningLabel
            // 
            this.warningLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.warningLabel.Location = new System.Drawing.Point(26, 82);
            this.warningLabel.Name = "warningLabel";
            this.warningLabel.Size = new System.Drawing.Size(307, 47);
            this.warningLabel.TabIndex = 3;
            this.warningLabel.Text = "You must specify eight lines in the channel string";
            this.warningLabel.Click += new System.EventHandler(this.warningLabel_Click);
            // 
            // physicalChannelComboBox
            // 
            this.physicalChannelComboBox.Location = new System.Drawing.Point(26, 35);
            this.physicalChannelComboBox.Name = "physicalChannelComboBox";
            this.physicalChannelComboBox.Size = new System.Drawing.Size(294, 28);
            this.physicalChannelComboBox.TabIndex = 2;
            this.physicalChannelComboBox.Text = "myDAQ2/Port0/line0:7";
            // 
            // StopButton
            // 
            this.StopButton.Location = new System.Drawing.Point(179, 281);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(141, 35);
            this.StopButton.TabIndex = 21;
            this.StopButton.Text = "Stop";
            this.StopButton.UseVisualStyleBackColor = true;
            this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // WriteOnce
            // 
            this.WriteOnce.Location = new System.Drawing.Point(194, 157);
            this.WriteOnce.Name = "WriteOnce";
            this.WriteOnce.Size = new System.Drawing.Size(109, 34);
            this.WriteOnce.TabIndex = 22;
            this.WriteOnce.Text = "Write Once";
            this.WriteOnce.UseVisualStyleBackColor = true;
            this.WriteOnce.Click += new System.EventHandler(this.WriteOnce_Click);
            // 
            // delay
            // 
            this.delay.Location = new System.Drawing.Point(183, 343);
            this.delay.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.delay.Name = "delay";
            this.delay.Size = new System.Drawing.Size(120, 26);
            this.delay.TabIndex = 23;
            this.delay.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.delay.ValueChanged += new System.EventHandler(this.delay_ValueChanged);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(8, 19);
            this.ClientSize = new System.Drawing.Size(365, 392);
            this.Controls.Add(this.delay);
            this.Controls.Add(this.WriteOnce);
            this.Controls.Add(this.StopButton);
            this.Controls.Add(this.physicalChannelComboBox);
            this.Controls.Add(this.warningLabel);
            this.Controls.Add(this.bit7Label);
            this.Controls.Add(this.bit2Label);
            this.Controls.Add(this.bit1Label);
            this.Controls.Add(this.bit0Label);
            this.Controls.Add(this.bit0CheckBox);
            this.Controls.Add(this.bit1CheckBox);
            this.Controls.Add(this.bit7CheckBox);
            this.Controls.Add(this.channelParamsLabel);
            this.Controls.Add(this.writeButton);
            this.Controls.Add(this.dataToWriteLabel);
            this.Controls.Add(this.bit2CheckBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Write Dig Channel";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.delay)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.DoEvents();
            Application.Run(new MainForm());
        }



        private void WriteButton_Click(object sender, System.EventArgs e)
        {
            //Start Async worker
            if (!_worker.IsBusy)
            {
                _worker.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show("The operation is already in progress.");
            }

        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            //Check if the worker thread is currently running
            if (_worker.IsBusy)
            {
                //If it is, cancel the asynchonous operation
                _worker.CancelAsync();
            }
        }

        private async void Worker_DoWork(object sender, DoWorkEventArgs e)
        //this method is executed on the worker thread
        {
            //Loop indefinitely until the asynchronous operation is cancelled
            while (true)
            {
                //check if cancellation has been requested
                if (_worker.CancellationPending)
                {
                    //set the cancel property of the event arguments and break out of the loop
                    e.Cancel = true;
                    break;
                }
                Cursor.Current = Cursors.WaitCursor;
                try
                {
                    // create a new task for digital write operations
                    using (Task digitalWriteTask = new Task())
                    {
                        // Invoke the creation of a new digital channel for the specified physical channel
                        Invoke((Action)(() =>
                        {
                            digitalWriteTask.DOChannels.CreateChannel(physicalChannelComboBox.Text, "",
                            ChannelLineGrouping.OneChannelForAllLines);
                        }));



                        bool[] dataArray = new bool[8];
                        dataArray[7] = true;

                        for (int i = 0; i < 8; i++)
                        {

                            ///*
                            //inelegant sol(?) for incrementation
                            if (i == 0) { }
                            else if (i == 1)
                            {
                                dataArray[0] = true;
                            }
                            else
                            {
                                for (int j = 0; j < 3; j++)
                                {
                                    if (dataArray[j] == false)
                                    {
                                        for (int k = 0; k < j + 1; k++)
                                        {
                                            if (dataArray[k] == false) { dataArray[k] = true; }
                                            else { dataArray[k] = false; }
                                        }
                                        break;

                                    }
                                }
                            }
                            // Console.WriteLine("[{0}]", string.Join(", ", dataArray));

                            //Get a DigitalSingleChannelWriter for the stream of the digital write task
                            DigitalSingleChannelWriter writer2 = new DigitalSingleChannelWriter(digitalWriteTask.Stream);
                            //write the current state of the dataArray to the digital channel
                            writer2.WriteSingleSampleMultiLine(true, dataArray);
                            //*/

                            /*
                            dataArray[i] = true;
                            DigitalSingleChannelWriter writer = new DigitalSingleChannelWriter(digitalWriteTask.Stream);
                            writer.WriteSingleSampleMultiLine(true, dataArray);
                            dataArray[i] = false;
                            */

                            //Sleep = proof of concept --> ist ineffiziente Lösung (CPU macht nix in der Zeit), chatGPT nach besserer Lösung fragen
                            int sleeptime = (int)delay.Value;
                            Thread.Sleep(sleeptime);
                            
                        }
                        //Max' Code End

                        //Original Code
                        /*

                        bool[] dataArray = new bool[8];
                        dataArray[0] = bit0CheckBox.Checked;
                        dataArray[1] = bit1CheckBox.Checked;
                        dataArray[2] = bit2CheckBox.Checked;
                        dataArray[3] = bit3CheckBox.Checked;
                        dataArray[4] = bit4CheckBox.Checked;
                        dataArray[5] = bit5CheckBox.Checked;
                        dataArray[6] = bit6CheckBox.Checked;
                        dataArray[7] = bit7CheckBox.Checked;
                        DigitalSingleChannelWriter writer = new DigitalSingleChannelWriter(digitalWriteTask.Stream);
                        writer.WriteSingleSampleMultiLine(true, dataArray);

                        */
                    }
                }
                catch (DaqException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }

        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        // this method is called when the worker has completed its task or has been cancelled
        {
            if (e.Cancelled)
            {
                Console.WriteLine("The Worker was cancelled");
            }
            else if (e.Error != null)
            {
                Console.WriteLine("There was an error running the task");
            }
            else
            {
                Console.WriteLine("The task was completed successfully");
            }
        }

        private void warningLabel_Click(object sender, EventArgs e)
        {

        }

        private void WriteOnce_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                // create a new task for digital write operations
                using (Task digitalWriteTask = new Task())
                {
                    digitalWriteTask.DOChannels.CreateChannel(physicalChannelComboBox.Text, "",
                            ChannelLineGrouping.OneChannelForAllLines);

                    bool[] dataArray = new bool[8];
                    dataArray[0] = bit0CheckBox.Checked;
                    dataArray[1] = bit1CheckBox.Checked;
                    dataArray[2] = bit2CheckBox.Checked;
                    dataArray[7] = bit7CheckBox.Checked;
                    DigitalSingleChannelWriter writer = new DigitalSingleChannelWriter(digitalWriteTask.Stream);
                    writer.WriteSingleSampleMultiLine(true, dataArray);
                }
            }
            catch (DaqException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

        }

        private void delay_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
