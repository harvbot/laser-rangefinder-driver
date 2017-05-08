using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Harvbot.Rangefinder.Driver;

namespace Harvbot.Rangefinder.TestApp
{
    public partial class MainForm : Form
    {
        private RangefinderDriver driver;

        public MainForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var ports = SerialPort.GetPortNames();
            foreach (var port in ports)
            {
                this.CbPorts.Items.Add(port);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            this.DisposeDriver();
            base.OnClosed(e);
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            if (this.CbPorts.SelectedItem != null)
            {
                this.InitializeDriver(this.CbPorts.SelectedItem.ToString());
            }
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            this.DisposeDriver();
        }

        private void InitializeDriver(string comPort)
        {
            this.DisposeDriver();

            this.driver = new RangefinderDriver(comPort);

            this.driver.Measurement += Driver_Measurement;

            this.driver.Open();
            this.driver.StartContinuousMeasurement();
        }

        private void DisposeDriver()
        {
            if (this.driver != null)
            {
                this.driver.Measurement -= Driver_Measurement;
                this.driver.Dispose();
            }
        }

        private void Driver_Measurement(object sender, RangefinderMeasurementArgs e)
        {
            if (!this.IsDisposed)
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new EventHandler<RangefinderMeasurementArgs>(this.Driver_Measurement), new object[] { sender, e });
                }
                else
                {
                    if (e.Success)
                    {
                        this.LsMeasurement.Items.Add(e.Range.ToString());
                    }
                    else
                    {
                        this.LsMeasurement.Items.Add("Error");
                    }
                }
            }
        }
    }
}
