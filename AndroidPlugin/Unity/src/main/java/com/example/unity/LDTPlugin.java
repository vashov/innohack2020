package com.example.unity;

import android.Manifest;
import android.app.Activity;
import android.app.AlertDialog;
import android.app.Notification;
import android.app.PendingIntent;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.hardware.Sensor;
import android.hardware.SensorManager;
import android.location.Location;
import android.location.LocationManager;
import android.os.Build;
import android.util.Log;

import androidx.annotation.RequiresApi;
import androidx.core.app.ActivityCompat;
import androidx.core.content.ContextCompat;

import com.unity3d.player.UnityPlayerActivity;

public class LDTPlugin extends UnityPlayerActivity {
    private static final String LOGTAG = "LDT";
    private static final LDTPlugin instance = new LDTPlugin();
    public static LDTPlugin getInstance(){return instance;}

    boolean resultIsSet = false;
    Object result;

    public static Activity mainActivity;
    private static SensorManager sensorManager;
    public static String apiKey;
    public static int constructionId;

    private Intent intentGPSService;
    private Intent intentAccelerometerService;

    public interface AlertViewCallback{
        public void onButtonTapped(int id);
    }

    public int setIdConstr(int constructiD)
    {
        constructionId = constructiD;

        return 1;
    }


    public void setApiKey(String apiKeyUser)
    {
        apiKey = apiKeyUser;
    }


    private LDTPlugin()
    {
        Log.i(LOGTAG, "Created Plugin");
    }

    public void  showAlertView(String[] strings, final AlertViewCallback callback)
    {
        if(strings.length < 3)
        {
            Log.i(LOGTAG, "Error - expected at least 3 strings, got" + strings.length);
        }
        DialogInterface.OnClickListener myClickListener = new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialogInterface, int id) {
                dialogInterface.dismiss();
                Log.i(LOGTAG, "Tapped: " + id);
                callback.onButtonTapped(id);
            }
        };
        AlertDialog alertDialog = new AlertDialog.Builder(mainActivity)
                .setTitle(strings[0])
                .setMessage(strings[1])
                .setCancelable(false)
                .create();
        alertDialog.setButton(AlertDialog.BUTTON_NEUTRAL, strings[2], myClickListener);
        if(strings.length > 3)
            alertDialog.setButton(AlertDialog.BUTTON_NEGATIVE, strings[3], myClickListener);
        if(strings.length > 4)
            alertDialog.setButton(AlertDialog.BUTTON_POSITIVE, strings[4], myClickListener);
        alertDialog.show();
    }

    public void setResult(Object result) {
        Log.w("result", "setResult");
        this.result = result;
        resultIsSet = true;
    }

    public Object getResult() {
        Log.w("result", "getResult");
        while(resultIsSet == false) {}
        return this.result;
    }

    public float[] getValuesAccel() {
        return AccelerometerService.getInstance().getValuesAccel();
    }
    public float[] getValuesAccelMotion() { return AccelerometerService.getInstance().getValuesAccelMotion(); }
    public float[] getValuesAccelGravity() { return AccelerometerService.getInstance().getValuesAccelGravity(); }
    public float[] getValuesLinAccel() { return AccelerometerService.getInstance().getValuesLinAccel(); }
    public float[] getValuesGravity() { return AccelerometerService.getInstance().getValuesGravity(); }

    public float[] getLocation()
    {
        float[] arraycoord = new float[2];

        arraycoord[0] = (float)GPSService.getInstance().getLatitude();
        arraycoord[1] = (float)GPSService.getInstance().getLongitude();
        return arraycoord;
    }

    public boolean getPermissionGeometria()
    {
        return checkLocationPermission();
    }

    //Start Services

    public int startGPSService()
    {
        intentGPSService = new Intent(mainActivity, GPSService.class);
        if(Build.VERSION.SDK_INT >= Build.VERSION_CODES.O)
        {
            mainActivity.startForegroundService(intentGPSService);
        }
        else{
            mainActivity.startService(intentGPSService);
        }
        return 1;
    }

    public int startAccelerometerService()
    {
        if(Build.VERSION.SDK_INT >= Build.VERSION_CODES.M)
        {
            intentAccelerometerService = new Intent(mainActivity, AccelerometerService.class);
            if(Build.VERSION.SDK_INT >= Build.VERSION_CODES.O)
            {
                mainActivity.startForegroundService(intentAccelerometerService);
            }
            else{
                mainActivity.startService(intentAccelerometerService);
            }
            return 1;
        }
        else{
            Log.i(LOGTAG, "Low API for startAccelerometerService");
            return 0;
        }
    }

    //end - Start Services

    //Stop Services

    public int stopGPSService()
    {
        GPSService.getInstance().stopUsingGPS();
        mainActivity.stopService(intentGPSService);
        return 1;
    }

    public int stopAccelerometerService()
    {
        if(Build.VERSION.SDK_INT >= Build.VERSION_CODES.M)
        {
            mainActivity.startService(intentAccelerometerService);
            return 1;
        }
        else{
            Log.i(LOGTAG, "Low API for startAccelerometerService");
            return 0;
        }
    }

    //end - Stop Services

    @Override
    public void onRequestPermissionsResult(int requestCode, String permissions[], int[] grantResults) {
        switch (requestCode) {
            case GPSService.MY_PERMISSIONS_REQUEST_LOCATION: {
                if (grantResults.length > 0 && grantResults[0] == PackageManager.PERMISSION_GRANTED) {
                    if(Build.VERSION.SDK_INT >= Build.VERSION_CODES.M)
                    {
                        if (mainActivity.checkSelfPermission(Manifest.permission. ACCESS_FINE_LOCATION) == PackageManager.PERMISSION_GRANTED ) {
                            GPSService.locationManager.requestLocationUpdates(LocationManager.GPS_PROVIDER, GPSService.MIN_TIME_BY_UPDATES, GPSService.MIN_TIME_BY_UPDATES, GPSService.getInstance());
                        }

                        if (mainActivity.checkSelfPermission(Manifest.permission. ACCESS_COARSE_LOCATION) == PackageManager.PERMISSION_GRANTED) {
                            GPSService.locationManager.requestLocationUpdates(LocationManager.NETWORK_PROVIDER,  GPSService.MIN_TIME_BY_UPDATES, GPSService.MIN_TIME_BY_UPDATES, GPSService.getInstance());
                        }
                    }

                } else {
                    //пермишен не был получее
                }
                return;
            }

        }
    }

    public static final int MY_PERMISSIONS_REQUEST_LOCATION = 777;
    public boolean checkLocationPermission() {
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.M) {
            if (mainActivity.checkSelfPermission(Manifest.permission.ACCESS_FINE_LOCATION) != PackageManager.PERMISSION_GRANTED &&
                    mainActivity.checkSelfPermission(Manifest.permission.ACCESS_COARSE_LOCATION) != PackageManager.PERMISSION_GRANTED) {
                if (mainActivity.shouldShowRequestPermissionRationale(Manifest.permission.ACCESS_FINE_LOCATION)) {
                    //показываем диалог
                    new AlertDialog.Builder(LDTPlugin.mainActivity)
                            .setTitle("Geoposition")
                            .setMessage("ALOOOOO")
                            .setPositiveButton("Ok", new DialogInterface.OnClickListener() {
                                @Override
                                public void onClick(DialogInterface dialogInterface, int i) {
                                    //Юзер одобрил
                                    mainActivity.requestPermissions(
                                            new String[]{Manifest.permission.ACCESS_FINE_LOCATION, Manifest.permission.ACCESS_COARSE_LOCATION}, MY_PERMISSIONS_REQUEST_LOCATION);
                                }
                            })
                            .create()
                            .show();
                } else {
                    //запрашиваем пермишен, уже не показывая диалогов с пояснениями
                    mainActivity.requestPermissions(new String[]{Manifest.permission.ACCESS_FINE_LOCATION, Manifest.permission.ACCESS_COARSE_LOCATION}, MY_PERMISSIONS_REQUEST_LOCATION);
                }
                return false;
            } else {
                return true;
            }
        } else {
            return true;
        }
    }
}
