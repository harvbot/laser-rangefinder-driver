package com.harvbot.rangefinder;

public abstract class RangefinderMeasurementListener {

    public abstract void onMeasurement(boolean isError, double value);
}
