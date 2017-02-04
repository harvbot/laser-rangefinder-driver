package com.harvbot.rangefinderapp;

import android.app.PendingIntent;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.hardware.usb.UsbDevice;
import android.hardware.usb.UsbManager;
import android.support.v7.app.AppCompatActivity;
import android.content.BroadcastReceiver;
import android.os.Bundle;
import android.util.Log;

import com.harvbot.rangefinder.RangefinderDriver;

import java.io.IOException;
import java.util.Iterator;

public class MainActivity extends AppCompatActivity {

    private RangefinderDriver mRangefinder;

    private UsbManager usbManager;

    private static final String ACTION_USB_PERMISSION =
            "com.harvbot.rangefinder.USB_PERMISSION";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        this.usbManager = (UsbManager)this.getSystemService(Context.USB_SERVICE);

        this.findRangefinderDevice();
    }

    private void findRangefinderDevice()
    {
        Iterator<UsbDevice> deviceIterator = usbManager.getDeviceList().values().iterator();

        while (deviceIterator.hasNext())
        {
            UsbDevice device = deviceIterator.next();

            if(device.getVendorId()==3034 && device.getProductId()==33142)
            {
                PendingIntent mPermissionIntent = PendingIntent.getBroadcast(this, 0, new Intent(ACTION_USB_PERMISSION), 0);
                IntentFilter filter = new IntentFilter(ACTION_USB_PERMISSION);
                this.registerReceiver(mUsbReceiver, filter);

                usbManager.requestPermission(device, mPermissionIntent);
                break;
            }
        }
    }

    private final BroadcastReceiver mUsbReceiver = new BroadcastReceiver() {

        public void onReceive(Context context, Intent intent) {
            String action = intent.getAction();
            if (ACTION_USB_PERMISSION.equals(action)) {
                synchronized (this) {
                    UsbDevice device = (UsbDevice)intent.getParcelableExtra(UsbManager.EXTRA_DEVICE);

                    if (intent.getBooleanExtra(UsbManager.EXTRA_PERMISSION_GRANTED, false)) {
                        if(device != null)
                        {
                            //call method to set up device communication
                            mRangefinder = new RangefinderDriver(usbManager, device);

                            if(mRangefinder != null) {
                                try {

                                    mRangefinder.open();

                                    mRangefinder.start();

                                } catch (IOException e) {
                                    e.printStackTrace();
                                }
                            }
                        }
                    }
                    else {
                        Log.d("TAG", "permission denied for the device " + device);
                    }
                }
            }
        }
    };
}
