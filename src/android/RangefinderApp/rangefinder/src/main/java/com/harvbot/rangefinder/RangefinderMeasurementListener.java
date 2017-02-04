package com.harvbot.rangefinder;

public interface RangefinderMeasurementListener {

    void onMeasurement(boolean isError, double value);
}
