package com.example.unity;

import android.app.Notification;
import android.app.NotificationChannel;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.app.Service;
import android.content.Intent;
import android.hardware.Sensor;
import android.hardware.SensorEvent;
import android.hardware.SensorEventListener;
import android.hardware.SensorManager;
import android.os.Build;
import android.os.IBinder;
import android.util.Log;

import java.util.Timer;
import java.util.TimerTask;

public class AccelerometerService extends Service implements SensorEventListener {
    final String LOG_TAG = "AccelService Log";
    public static final String CHANNEL_ID = "AccelerometerService";

    private static AccelerometerService instance;
    public static AccelerometerService getInstance(){return instance;}

    private SensorManager sensorManager;
    Sensor sensorAccel;
    Sensor sensorLinAccel;
    Sensor sensorGravity;

    float[] valuesAccel = new float[3];
    float[] valuesAccelMotion = new float[3];
    float[] valuesAccelGravity = new float[3];
    float[] valuesLinAccel = new float[3];
    float[] valuesGravity = new float[3];

    public float[] getValuesAccel()
    {
        return valuesAccel;
    }

    public float[] getValuesAccelMotion()
    {
        return valuesAccelMotion;
    }

    public float[] getValuesAccelGravity()
    {
        return valuesAccelGravity;
    }

    public float[] getValuesLinAccel()
    {
        return valuesLinAccel;
    }

    public float[] getValuesGravity()
    {
        return valuesGravity;
    }

    public AccelerometerService() { }

    public void onCreate() {
        super.onCreate();
        Log.d(LOG_TAG, "onCreate");

        instance = this;
        sensorManager=(SensorManager) getSystemService(SENSOR_SERVICE);
        sensorLinAccel = sensorManager.getDefaultSensor(Sensor.TYPE_LINEAR_ACCELERATION);
        sensorAccel = sensorManager.getDefaultSensor(Sensor.TYPE_ACCELEROMETER);
        sensorGravity = sensorManager.getDefaultSensor(Sensor.TYPE_GRAVITY);

        sensorManager.registerListener(this, sensorAccel, SensorManager.SENSOR_DELAY_NORMAL);
        sensorManager.registerListener(this, sensorLinAccel, SensorManager.SENSOR_DELAY_NORMAL);
        sensorManager.registerListener(this, sensorGravity, SensorManager.SENSOR_DELAY_NORMAL);
    }

    public int onStartCommand(Intent intent, int flags, int startId) {
        Log.d(LOG_TAG, "onStartCommand");
        createNotificationChannel();
        Intent notificationIntent = new Intent(this, LDTPlugin.class);
        PendingIntent pendingIntent = PendingIntent.getActivity(this, 0, intent, 0);

        Notification notification = new Notification.Builder(this)
                .setContentTitle("asd")
                .setContentText("asd")
                .setContentIntent(pendingIntent)
                .setTicker("asd")
                .build();
        startForeground(1334, notification);
        return Service.START_STICKY;
    }

    private void createNotificationChannel() {
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
            NotificationChannel serviceChannel = new NotificationChannel(
                    CHANNEL_ID,
                    "Foreground Service Channel",
                    NotificationManager.IMPORTANCE_DEFAULT
            );

            NotificationManager manager = getSystemService(NotificationManager.class);
            manager.createNotificationChannel(serviceChannel);
        }
    }

    public void onDestroy() {
        super.onDestroy();
        Log.d(LOG_TAG, "onDestroy");
    }

    public IBinder onBind(Intent intent) {
        Log.d(LOG_TAG, "onBind");
        return null;
    }

    @Override
    public void onSensorChanged(SensorEvent event) {
        Log.i(LOG_TAG, "SENSOR onSensorChanged");
        switch (event.sensor.getType()) {
            case Sensor.TYPE_ACCELEROMETER:
                for(int i = 0; i < 3; i++)
                {
                    valuesAccel[i] = event.values[i];
                    valuesAccelGravity[i] = (float) (0.1 * event.values[i] + 0.9 * valuesAccelGravity[i]);
                    valuesAccelMotion[i] = event.values[i] - valuesAccelGravity[i];
                }
                break;
            case Sensor.TYPE_LINEAR_ACCELERATION:
                for(int i = 0; i < 3; i++) valuesLinAccel[i] = event.values[i];
                break;
            case Sensor.TYPE_GRAVITY:
                for(int i = 0; i < 3; i++) valuesGravity[i] = event.values[i];
                break;
        }
    }

    @Override
    public void onAccuracyChanged(Sensor sensor, int accuracy) {

    }
}