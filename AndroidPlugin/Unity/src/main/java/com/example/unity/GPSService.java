package com.example.unity;

import android.Manifest;
import android.app.AlertDialog;
import android.app.IntentService;
import android.app.Notification;
import android.app.NotificationChannel;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.app.Service;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.hardware.Sensor;
import android.hardware.SensorManager;
import android.location.Location;
import android.location.LocationListener;
import android.location.LocationManager;
import android.os.Build;
import android.os.Bundle;
import android.os.IBinder;
import android.util.Log;

import androidx.annotation.Nullable;
import androidx.core.app.ActivityCompat;
import androidx.core.content.ContextCompat;

import org.apache.hc.client5.http.classic.HttpClient;
import org.apache.hc.client5.http.classic.methods.HttpPost;
import org.apache.hc.client5.http.impl.classic.HttpClientBuilder;
import org.apache.hc.core5.http.HttpResponse;
import org.apache.hc.core5.http.io.entity.StringEntity;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.BufferedWriter;
import java.io.DataOutputStream;
import java.io.IOException;
import java.io.OutputStream;
import java.io.OutputStreamWriter;
import java.net.HttpURLConnection;
import java.net.MalformedURLException;
import java.net.ProtocolException;
import java.net.URL;
import java.net.URLEncoder;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.Date;
import java.util.Timer;
import java.util.TimerTask;

public class GPSService extends Service  implements LocationListener {
    final String LOG_TAG = "GPSService Log";
    public static final String CHANNEL_ID = "GPSService";

    private static GPSService instance;

    public static GPSService getInstance(){return instance;}

    // flag for GPS status
    boolean isGPSEnabled = false;

    // flag for network status
    boolean isNetworkEnabled = false;

    public boolean canGetLocation = false;

    Location location; // location
    double latitude; // latitude
    double longitude; // longitude

    // The minimum distance to change Updates in meters
    public static final long MIN_DISTANCE_CHANGE_FOR_UPDATES = 1;

    // The minimum time between updates in milliseconds
    public static final long MIN_TIME_BY_UPDATES = 5000;

    // Declaring a Location Manager
    public static LocationManager locationManager;

    public Location getLocation() {
        try {
            Log.d(LOG_TAG, "ENTER");
            locationManager = (LocationManager) LDTPlugin.mainActivity.getSystemService(LOCATION_SERVICE);

            // getting GPS status
            isGPSEnabled = locationManager.isProviderEnabled(LocationManager.GPS_PROVIDER);

            // getting network status
            isNetworkEnabled = locationManager.isProviderEnabled(LocationManager.NETWORK_PROVIDER);

            if (!isGPSEnabled && !isNetworkEnabled) {
                // no network provider is enabled
            } else {
                this.canGetLocation = true;

                // if GPS Enabled get lat/long using GPS Services
                if (isGPSEnabled) {
                    Log.i(LOG_TAG, "from gps");
                    if (location == null) {
                        if(Build.VERSION.SDK_INT >= Build.VERSION_CODES.M)
                        {
                            if (LDTPlugin.mainActivity.checkSelfPermission(Manifest.permission.ACCESS_FINE_LOCATION) != PackageManager.PERMISSION_GRANTED && LDTPlugin.mainActivity.checkSelfPermission(Manifest.permission.ACCESS_COARSE_LOCATION) != PackageManager.PERMISSION_GRANTED) {
                                if(!checkLocationPermission()) return null;
                            }
                        }
                        locationManager.requestLocationUpdates(LocationManager.GPS_PROVIDER, MIN_TIME_BY_UPDATES, MIN_DISTANCE_CHANGE_FOR_UPDATES, getInstance());
                        Log.d(LOG_TAG, "GPS Enabled");
                        location = locationManager.getLastKnownLocation(LocationManager.GPS_PROVIDER);
                        if (location != null) {
                            latitude = location.getLatitude();
                            longitude = location.getLongitude();
                        }
                    }
                }
                // second get location from Network Provider
                if (isNetworkEnabled) {
                    Log.i(LOG_TAG, "from net");
                    locationManager.requestLocationUpdates(LocationManager.NETWORK_PROVIDER, MIN_TIME_BY_UPDATES, MIN_DISTANCE_CHANGE_FOR_UPDATES, getInstance());
                    Log.d(LOG_TAG, "Network");
                    if (locationManager != null) {
                        location = locationManager.getLastKnownLocation(LocationManager.NETWORK_PROVIDER);
                        if (location != null) {
                            latitude = location.getLatitude();
                            longitude = location.getLongitude();
                        }
                    }
                }
            }

        } catch (Exception e) {
            e.printStackTrace();
        }

        return location;
    }

    public void onCreate() {
        super.onCreate();
        Log.d(LOG_TAG, "onCreate");
        instance = this;
        getLocation();
    }

    Timer timer;
    TimerTask timerTask;
    int seconds = 10;
    boolean isSetLoopRequest = false;
    public void setLoopRequest()
    {
        isSetLoopRequest = true;
        timer = new Timer();
        Date dateNow = new Date();
        SimpleDateFormat formatForDateNow = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss");
        HttpClient httpClient = HttpClientBuilder.create().build();
        timerTask = new TimerTask()
        {
            @Override
            public void run()
            {
                try {
                    String timeStamp =  formatForDateNow.format(dateNow);
                    HttpPost request = new HttpPost("https://bittech.xyz/api/gps/add");
                    JSONObject jsonObj = new JSONObject();
                    JSONArray list1Json = new JSONArray();
                    jsonObj.put("latitude", getLatitude());
                    jsonObj.put("longitude", getLongitude());

                    jsonObj.put("date", timeStamp);
                    jsonObj.put("constructionId", LDTPlugin.constructionId);
                    list1Json.put(jsonObj);
                    StringEntity params = new StringEntity(list1Json.toString());
                    request.addHeader("Content-Type", "application/json");
                    request.addHeader("api-key", LDTPlugin.apiKey);
                    request.setEntity(params);
                    Log.i(LOG_TAG, list1Json.toString());
                    Log.i(LOG_TAG, params.toString());
                    HttpResponse response = httpClient.execute(request);
                    Log.i(LOG_TAG, list1Json.toString());
                    Log.i(LOG_TAG, String.valueOf(response.getCode()));

                  //  conn.disconnect();
                } catch (JSONException | MalformedURLException e) {
                    e.printStackTrace();
                    Log.i(LOG_TAG , "sasasdasdasdasd");
                } catch (ProtocolException e) {
                    e.printStackTrace();
                    Log.i(LOG_TAG , "ssssssssssssss");
                } catch (IOException e) {
                    e.printStackTrace();
                    Log.i(LOG_TAG , "zzzzzzzzzzzzzzzzz");
                }
            }
        };
        //Set the schedule function and rate
        timer.schedule(timerTask, 0, 30000);
    }


    @Override
    public IBinder onBind(Intent intent) {
        // TODO Написать код для метода
        return null;
    }

    @Override
    public void onLocationChanged(Location location) {
        Log.d(LOG_TAG, "onLocationChanged");
        this.location = location;
        if(!isSetLoopRequest && timerTask == null)setLoopRequest();
        //do something
    }
    @Override
    public void onStatusChanged(String provider, int status, Bundle extras) {
        // TODO Написать код для метода

    }
    @Override
    public void onProviderEnabled(String provider) {
        // TODO Написать код для метода

    }
    @Override
    public void onProviderDisabled(String provider) {
        // TODO Написать код для метода

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
        startForeground(1333, notification);

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

    public double getLatitude() {
        if (location != null) {
            latitude = location.getLatitude();
        }
        return latitude;
    }

    public double getLongitude() {
        if (location != null) {
            longitude = location.getLongitude();
        }
        return longitude;
    }

    public void stopUsingGPS() {
        if (locationManager != null) {
            locationManager.removeUpdates(getInstance());
        }

        if(timer != null)
        {
            timer.cancel();
        }

        if(timerTask != null)
        {
            timerTask.cancel();
            timerTask = null;
        }
        isSetLoopRequest = false;
    }

    public boolean canGetLocation() {
        return this.canGetLocation;
    }

    public static final int MY_PERMISSIONS_REQUEST_LOCATION = 777;

    public boolean checkLocationPermission() {
        if(Build.VERSION.SDK_INT >= Build.VERSION_CODES.M)
        {
            if (LDTPlugin.mainActivity.checkSelfPermission(Manifest.permission. ACCESS_FINE_LOCATION) != PackageManager.PERMISSION_GRANTED &&
                    LDTPlugin.mainActivity.checkSelfPermission(Manifest.permission. ACCESS_COARSE_LOCATION) != PackageManager.PERMISSION_GRANTED) {
                if (LDTPlugin.mainActivity.shouldShowRequestPermissionRationale(Manifest.permission. ACCESS_FINE_LOCATION) ) {
                    //показываем диалог
                    new AlertDialog.Builder(LDTPlugin.mainActivity)
                            .setTitle("Geoposition")
                            .setMessage("ALOOOOO")
                            .setPositiveButton("Ok", new DialogInterface.OnClickListener() {
                                @Override
                                public void onClick(DialogInterface dialogInterface, int i) {
                                    //Юзер одобрил
                                    LDTPlugin.mainActivity.requestPermissions(
                                            new String[]{Manifest.permission.ACCESS_FINE_LOCATION, Manifest.permission. ACCESS_COARSE_LOCATION}, MY_PERMISSIONS_REQUEST_LOCATION);
                                }
                            })
                            .create()
                            .show();
                } else {
                    //запрашиваем пермишен, уже не показывая диалогов с пояснениями
                    LDTPlugin.mainActivity.requestPermissions(new String[]{Manifest.permission. ACCESS_FINE_LOCATION, Manifest.permission. ACCESS_COARSE_LOCATION}, MY_PERMISSIONS_REQUEST_LOCATION);
                }
                return false;
            } else {
                return true;
            }
        }
        else{
            return true;
        }
    }
}