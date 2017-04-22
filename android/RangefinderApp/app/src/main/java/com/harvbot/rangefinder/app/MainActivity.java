package com.harvbot.rangefinder.app;

import android.app.PendingIntent;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.content.res.XmlResourceParser;
import android.hardware.usb.UsbDevice;
import android.hardware.usb.UsbManager;
import android.support.v7.app.AppCompatActivity;
import android.content.BroadcastReceiver;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.ArrayAdapter;

import com.harvbot.rangefinder.RangefinderDriver;
import com.harvbot.rangefinder.RangefinderMeasurementListener;

import org.xmlpull.v1.XmlPullParserException;

import java.io.IOException;
import java.util.ArrayList;
import java.util.Iterator;

public class MainActivity extends AppCompatActivity {

    private RangefinderDriver mRangefinder;

    private UsbManager usbManager;

    private UsbDevice botDevice;

    private ArrayAdapter<String> listOfMeasurementAdapter;

    private ArrayList<String> listOfMeasurement = new ArrayList<String>();

    private static final String ACTION_USB_PERMISSION =
            "com.harvbot.rangefinder.USB_PERMISSION";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        this.usbManager = (UsbManager)this.getSystemService(Context.USB_SERVICE);

        this.findRangefinderDevice();

        this.listOfMeasurementAdapter=new ArrayAdapter<String>(this,
                android.R.layout.simple_list_item_1,
                listOfMeasurement);
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();

        try {
            this.stopMeasurement();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    public void onStartMeasurementClick(View view) {

        this.startMeasurement();
    }

    public void onStopMeasurementClick(View view) throws IOException {

        this.stopMeasurement();
    }

    private void startMeasurement() {

        if(this.botDevice != null) {
            PendingIntent mPermissionIntent = PendingIntent.getBroadcast(this, 0, new Intent(ACTION_USB_PERMISSION), 0);
            IntentFilter filter = new IntentFilter(ACTION_USB_PERMISSION);
            this.registerReceiver(mUsbReceiver, filter);

            usbManager.requestPermission(this.botDevice, mPermissionIntent);
        }
    }

    private void stopMeasurement() throws IOException {

        if(mRangefinder != null) {
            mRangefinder.turnOff();
            mRangefinder.close();
        }
    }

    private void findRangefinderDevice() {

        int productId = 0;
        int vendorId = 0;
        int eventType = -1;
        XmlResourceParser document = getResources().getXml(com.harvbot.rangefinder.app.R.xml.device_filter);

        while(eventType != XmlResourceParser.END_DOCUMENT)
        {
            String name = document.getText();

            try {
                if (document.getEventType() == XmlResourceParser.START_TAG) {
                    String s = document.getName();

                    if (s.equals("usb-device")) {
                        vendorId = Integer.parseInt(document.getAttributeValue(null, "vendor-id"));
                        productId = Integer.parseInt(document.getAttributeValue(null, "product-id"));
                        break;
                    }
                }

                eventType = document.next();
            } catch (XmlPullParserException e) {
                e.printStackTrace();
            } catch (IOException e) {
                e.printStackTrace();
            }
        }

        Iterator<UsbDevice> deviceIterator = usbManager.getDeviceList().values().iterator();

        while (deviceIterator.hasNext())
        {
            UsbDevice device = deviceIterator.next();

            // Find speicified divce.
            if(device.getVendorId()== vendorId && device.getProductId()==productId)
            {
                this.botDevice = device;
                break;
            }
        }

        if(this.botDevice != null) {
            // Request usb device permission.
            PendingIntent mPermissionIntent = PendingIntent.getBroadcast(this, 0, new Intent(ACTION_USB_PERMISSION), 0);
            IntentFilter filter = new IntentFilter(ACTION_USB_PERMISSION);
            this.registerReceiver(mUsbReceiver, filter);

            usbManager.requestPermission(this.botDevice, mPermissionIntent);
        }
    }

    private final RangefinderMeasurementListener mRangefinderListener = new RangefinderMeasurementListener() {

        @Override
        public void onMeasurement(boolean isError, double value) {
            if(!isError)
            {
                listOfMeasurementAdapter.add((new Double(value)).toString());
            }
            else
            {
                listOfMeasurementAdapter.add("Error during measurement");
            }
        }
    };

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

                            mRangefinder.setMeasurementListener(mRangefinderListener);

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
