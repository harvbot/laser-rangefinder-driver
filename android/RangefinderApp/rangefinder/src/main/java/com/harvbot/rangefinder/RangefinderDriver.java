package com.harvbot.rangefinder;

import android.hardware.usb.UsbDevice;
import android.hardware.usb.UsbDeviceConnection;
import android.hardware.usb.UsbManager;

import com.hoho.android.usbserial.driver.Ch34xSerialDriver;
import com.hoho.android.usbserial.driver.UsbSerialDriver;
import com.hoho.android.usbserial.driver.UsbSerialPort;

import java.io.IOException;
import java.util.ArrayList;
import java.util.Timer;
import java.util.TimerTask;

public class RangefinderDriver {

    /// <summary>
    /// Laser rangefinder baud rate.
    /// </summary>
    public static final int BaudRate = 9600;

    public static final int Timeout = 1000;

    /// <summary>
    /// Stores default laser address.
    /// </summary>
    public static final int DefaultAddr = (byte)0x80;

    /// <summary>
    /// Continuouse measurement command header.
    /// </summary>
    private static final byte[] ContinuousMeasureCommand = new byte[] { RangefinderDriver.DefaultAddr, 0x06, 0x03, 0x77 };

    /// <summary>
    /// Turn off command header.
    /// </summary>
    private static final byte[] TurnOffCommand = new byte[] { RangefinderDriver.DefaultAddr, 0x04, 0x02, 0x7A };

    /// <summary>
    /// Stores serial port instance.
    /// </summary>
    private UsbSerialDriver serial;

    /// <summary>
    /// Stores serial port instance.
    /// </summary>
    private UsbSerialPort serialPort;

    private UsbDeviceConnection usbDeviceConnection;

    /// <summary>
    /// Stores timer instance.
    /// </summary>
    private Timer timer;

    private ArrayList<RangefinderMeasurementListener> listeners;

    private UsbManager usbManager;

    public RangefinderDriver(UsbManager usbManager, UsbDevice device)
    {
        this.usbManager = usbManager;

        this.usbDeviceConnection = usbManager.openDevice(device);
        this.serial =  new Ch34xSerialDriver (device);
        this.serialPort = this.serial.getPorts().get(0);
        this.timer = new Timer();

        this.listeners = new ArrayList<RangefinderMeasurementListener>();
    }

    /// <summary>
    /// Opens rangefinder to start measurement.
    /// </summary>
    public void open() throws IOException
    {
        this.serialPort.open(this.usbDeviceConnection);
    }

    /// <summary>
    /// Closes rangefinder.
    /// </summary>
    public void close() throws IOException
    {
        this.serialPort.close();
    }

    public void start() throws IOException
    {
        this.serialPort.write(this.ContinuousMeasureCommand, RangefinderDriver.Timeout);

        TimerTask timerTask = new TimerTask()
        {
            @Override
            public void run()
            {
                try {
                    readMeasurement();
                } catch (IOException e) {
                    e.printStackTrace();
                }
            }
        };

        this.timer.schedule(timerTask, 1000, 1000);
    }

    public  void stop() throws IOException
    {
        this.timer.cancel();
    }

    /// <summary>
    /// Turns of rangefinder.
    /// </summary>
    /// <returns><c>true</c> if rangefinder was successfully turned off; otherwise, <c>false</c>.</returns>
    public boolean turnOff() throws IOException
    {
        this.timer.cancel();
        this.serialPort.write(this.TurnOffCommand, RangefinderDriver.Timeout);

        return true;
    }

    public void setMeasurementListener(RangefinderMeasurementListener listener)
    {
        this.listeners.add(listener);
    }

    /// <summary>
    /// Sends measurement.
    /// </summary>
    private void sendMeasurement() throws IOException {
        this.serialPort.write(this.ContinuousMeasureCommand, RangefinderDriver.Timeout);
    }

    /// <summary>
    /// Reads measurement.
    /// </summary>
    private void readMeasurement() throws IOException
    {
        byte[] buffer = new byte[11];
        int read = this.serialPort.read(buffer, RangefinderDriver.Timeout);

        if (read == buffer.length)
        {
            if (buffer[0] == RangefinderDriver.DefaultAddr &&
                    buffer[1] == 0x06 && buffer[2] == -0x7D && buffer[6] == 0x2E)
            {
                double value = 0;

                value += (buffer[3] - 48) * 100;
                value += (buffer[4] - 48) * 10;
                value += (buffer[5] - 48);

                value += (buffer[7] - 48) * 0.1;
                value += (buffer[8] - 48) * 0.01;
                value += (buffer[9] - 48) * 0.001;

                this.onMeasurement(false, value);
            }
            else
            {
                this.onMeasurement(true, -1);
            }
        }
        else
        {
            this.onMeasurement(true, -1);
        }
    }

    private void onMeasurement(boolean isError, double value)
    {
        for(int i=0;i<this.listeners.size(); i++)
        {
            this.listeners.get(i).onMeasurement(isError, value);
        }
    }
}
