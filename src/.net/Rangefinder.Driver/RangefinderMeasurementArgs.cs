using System;

namespace Harvbot.Rangefinder.Driver
{
    /// <summary>
    /// Represents measurement event args.
    /// </summary>
    public class RangefinderMeasurementArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RangefinderMeasurementArgs"/> class.
        /// </summary>
        /// <param name="success">The value indicating whether measurement was successfull.</param>
        /// <param name="range">The measurement value.</param>
        public RangefinderMeasurementArgs(bool success, decimal range)
        {
            this.Success = success;
            this.Range = range;
        }

        /// <summary>
        /// Gets the measurement value.
        /// </summary>
        public decimal Range { get; private set; }

        /// <summary>
        /// Gets the value indicating whether measurement was successfull.
        /// </summary>
        public bool Success { get; private set; }
    }
}
