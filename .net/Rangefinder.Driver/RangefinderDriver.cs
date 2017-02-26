using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Timers;

namespace Harvbot.Rangefinder.Driver
{
    /// <summary>
    /// Represents rangefinder driver.
    /// </summary>
    public class RangefinderDriver: IDisposable
    {
        #region Constants

        /// <summary>
        /// Laser rangefinder baud rate.
        /// </summary>
        public const int BaudRate = 9600;

        /// <summary>
        /// Stores default laser address.
        /// </summary>
        public const int DefaultAddr = 0x80;

        /// <summary>
        /// Continuouse measurement command header.
        /// </summary>
        private readonly byte[] ContinuousMeasureCommand = new byte[] { RangefinderDriver.DefaultAddr, 0x06, 0x03, 0x77 };

        /// <summary>
        /// Turn off command header.
        /// </summary>
        private readonly byte[] TurnOffCommand = new byte[] { RangefinderDriver.DefaultAddr, 0x04, 0x02, 0x7A };

        #endregion

        #region Fields

        /// <summary>
        /// Stores value indicating whether driver was disposed.
        /// </summary>
        private bool isDisposed;

        /// <summary>
        /// Stores serial port instance.
        /// </summary>
        private SerialPort serial;

        /// <summary>
        /// Stores timer instance.
        /// </summary>
        private Timer timer;

        #endregion

        /// <summary>
        /// Occurs when measurement was done.
        /// </summary>
        public event EventHandler<RangefinderMeasurementArgs> Measurement;

        /// <summary>
        /// Initializes a new instance of the <see cref="RangefinderDriver"/> class.
        /// </summary>
        /// <param name="comNum">The COM number.</param>
        public RangefinderDriver(string comNum)
        {
            if (string.IsNullOrEmpty(comNum))
            {
                throw new ArgumentNullException("comNumber");
            }

            this.serial = new SerialPort(comNum, RangefinderDriver.BaudRate);
            this.timer = new Timer();
            this.timer.Interval = 1000;
            this.timer.Elapsed += Timer_Elapsed;
        }

        ~RangefinderDriver()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Opens rangefinder to start measurement.
        /// </summary>
        public void Open()
        {
            this.serial.Open();
        }

        /// <summary>
        /// Closes rangefinder.
        /// </summary>
        public void Close()
        {
            this.serial.Close();
        }

        /// <summary>
        /// Starts continuous measurement.
        /// </summary>
        /// <returns><c>true</c> if measurement was successfully started; otherwise, <c>false</c>.</returns>
        public bool StartContinuousMeasurement()
        {
            this.serial.Write(this.ContinuousMeasureCommand, 0, this.ContinuousMeasureCommand.Length);
            this.timer.Start();

            return true;
        }

        /// <summary>
        /// Turns of rangefinder.
        /// </summary>
        /// <returns><c>true</c> if rangefinder was successfully turned off; otherwise, <c>false</c>.</returns>
        public bool TurnOff()
        {
            this.timer.Stop();
            this.serial.Write(this.TurnOffCommand, 0, this.TurnOffCommand.Length);

            return true;
        }

        /// <summary>
        /// Releases all resources used by current instance.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (!this.isDisposed)
            {
                this.TurnOff();

                this.serial.Dispose();
                this.timer.Dispose();
                this.isDisposed = true;
            }
        }

        /// <summary>
        /// Raises the <see cref="Measurement"/> event.
        /// </summary>
        /// <param name="success">The value indicating whether measurement was successfull.</param>
        /// <param name="value">The measurement value.</param>
        protected void OnRangefinderDriver(bool success, decimal value)
        {
            if (this.Measurement != null)
            {
                this.Measurement(this, new RangefinderMeasurementArgs(success, value));
            }
        }

        /// <summary>
        /// Sends measurement.
        /// </summary>
        private void SendMeasurement()
        {
            this.serial.Write(this.ContinuousMeasureCommand, 0, ContinuousMeasureCommand.Length);
            this.timer.Start();
        }

        /// <summary>
        /// Reads measurement.
        /// </summary>
        private void ReadMeasurement()
        {
            byte[] buffer = new byte[11];
            var read = this.serial.Read(buffer, 0, buffer.Length);

            if (read == buffer.Length)
            {
                if (buffer[0] == RangefinderDriver.DefaultAddr && 
                    buffer[1] == 0x06 && buffer[2] == 0x83 && buffer[6] == 0x2E)
                {
                    decimal value = 0;

                    value += (buffer[3] - 48) * 100;
                    value += (buffer[4] - 48) * 10;
                    value += (buffer[5] - 48);

                    value += (buffer[7] - 48) * 0.1M;
                    value += (buffer[8] - 48) * 0.01M;
                    value += (buffer[9] - 48) * 0.001M;

                    this.OnRangefinderDriver(true, value);
                }
                else
                {
                    this.OnRangefinderDriver(false, -1);
                }
            }
            else
            {
                this.OnRangefinderDriver(false, -1);
            }
        }

        /// <summary>
        /// Handles timer elapsed event.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.ReadMeasurement();
        }
    }
}
