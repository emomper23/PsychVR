/**
 * Copyright (C) 2014 Samsung Electronics Co., Ltd. All rights reserved.
 *
 * Mobile Communication Division,
 * IT & Mobile Communications, Samsung Electronics Co., Ltd.
 *
 * This software and its documentation are confidential and proprietary
 * information of Samsung Electronics Co., Ltd.  No part of the software and
 * documents may be copied, reproduced, transmitted, translated, or reduced to
 * any electronic medium or machine-readable form without the prior written
 * consent of Samsung Electronics.
 *
 * Samsung Electronics makes no representations with respect to the contents,
 * and assumes no responsibility for any errors that might appear in the
 * software and documents. This publication and the contents hereof are subject
 * to change without notice.
 */

package com.samsung.android.app.plugintracker;

import com.samsung.android.app.plugintracker.service.MyTrackerService;
import com.samsung.android.sdk.shealth.tracker.TrackerEventListener;
import com.samsung.android.sdk.shealth.tracker.TrackerTile;
import com.samsung.android.sdk.shealth.tracker.TrackerTileManager;

import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.res.Resources.NotFoundException;
import android.graphics.Color;
import android.util.Base64;
import android.util.Log;

import java.security.SecureRandom;
import java.util.Date;

public class MyTracker implements TrackerEventListener {

    private TrackerTileManager mTrackerTileManager;
    private static final String MY_TILE_ID = "hello_tile";
    private int mTemplate = TrackerTile.TRACKER_TILE_TYPE_1;

    private static final String SHARED_PREFERENCE_NAME = "tile_content";
    private static final String SHARED_PREFERENCE_CONTENT_VALUE_KEY = "content_value";
    private static final String SHARED_PREFERENCE_LOGIN_KEY = "log_in";
    private static final String VALIDATION_KEY = "validation_key";
    private static String VALIDATION_VALUE = "";

    static {
        byte[] validation_value_bytes = new byte[32];
        new SecureRandom().nextBytes(validation_value_bytes);
        VALIDATION_VALUE = Base64.encodeToString(validation_value_bytes, Base64.DEFAULT);
    }

    private int mTileContent;

    private static final String LOG_TAG = "PluginTracker";

    public MyTracker() {
        // An empty constructor should be created.
    }

    public MyTracker(Context context) {
        Log.d(LOG_TAG, "MyTracker");

        if (mTrackerTileManager == null) {
            try {
                mTrackerTileManager = new TrackerTileManager(context);
            } catch (IllegalArgumentException e) {
                Log.d(LOG_TAG, "MyTracker Constructor - IllegalArgumentException");
            }
        }
    }

    @Override
    public void onCreate(Context context, String trackerId) {
        Log.d(LOG_TAG, "onCreate(" + trackerId + ")");

        if (mTrackerTileManager == null) {
            try {
                mTrackerTileManager = new TrackerTileManager(context);
            } catch (IllegalArgumentException e) {
                Log.d(LOG_TAG, "MyTracker onCreate() - IllegalArgumentException");
            }
        }
    }

    @Override
    public void onSubscribed(Context context, String trackerId) {
        Log.d(LOG_TAG, "onSubscribed(" + trackerId + ")");

        SharedPreferences sp = context.getSharedPreferences(SHARED_PREFERENCE_NAME, Context.MODE_PRIVATE);
        boolean isLoggedIn = sp.getBoolean(SHARED_PREFERENCE_LOGIN_KEY, false);

        if (isLoggedIn) {
            updateTile(context, trackerId, MY_TILE_ID);
        } else {
            postDefaultTile(context, trackerId, MY_TILE_ID);
        }
    }

    @Override
    public void onUnsubscribed(Context context, String trackerId) {
        Log.d(LOG_TAG, "onUnsubscribed(" + trackerId + ")");

        SharedPreferences sp = context.getSharedPreferences(SHARED_PREFERENCE_NAME, Context.MODE_PRIVATE);
        sp.edit().putBoolean(SHARED_PREFERENCE_LOGIN_KEY, false).commit();
    }

    @Override
    public void onPaused(Context context, String trackerId) {
        Log.d(LOG_TAG, "onPaused(" + trackerId + ")");
    }

    @Override
    public void onTileRequested(Context context, String trackerId, String tileId) {
        Log.d(LOG_TAG, "onTileRequested(" + trackerId + ", " + tileId + ")");

        SharedPreferences sp = context.getSharedPreferences(SHARED_PREFERENCE_NAME, Context.MODE_PRIVATE);
        boolean isLoggedIn = sp.getBoolean(SHARED_PREFERENCE_LOGIN_KEY, false);

        if (isLoggedIn) {
            updateTile(context, trackerId, tileId);
        } else {
            postDefaultTile(context, trackerId, tileId);
        }
    }

    @Override
    public void onTileRemoved(Context context, String trackerId, String tileId) {
        Log.d(LOG_TAG, "onTileRemoved(" + trackerId + ", " + tileId + ")");
    }

    private void postDefaultTile(Context context, String trackerId, String tileId) {
        TrackerTile myTrackerTile = null;

        if (tileId == null) {
            tileId = MY_TILE_ID;
        }

        try {
            // Create Intent to do an action
            // when the tracker tile is clicked
            Intent launchIntent = new Intent(context, MainActivity.class);

            // Create Intent to do an action
            // when the button on this tile is clicked
            Intent serviceIntent = new Intent(context, MyTrackerService.class);
            serviceIntent.putExtra(SHARED_PREFERENCE_LOGIN_KEY, true);

            SharedPreferences sp = context.getSharedPreferences(SHARED_PREFERENCE_NAME, Context.MODE_PRIVATE);
            String validationKey = sp.getString(VALIDATION_KEY, "");
            if (validationKey.isEmpty()) {
                validationKey = VALIDATION_VALUE;
                sp.edit().putString(VALIDATION_KEY, validationKey).commit();
            }

            serviceIntent.putExtra(VALIDATION_KEY, validationKey);

            // Set template
            mTemplate = TrackerTile.TRACKER_TILE_TYPE_1;

            // Create TrackerTile and set each values and intents
            myTrackerTile = new TrackerTile(context, trackerId, tileId, mTemplate);

            // Set Title
            myTrackerTile
                    .setTitle(R.string.tracker_test_display_name)
                    // Set Icon resource
                    .setIcon(R.drawable.tracker_test_icon_big_1)
                    // Set content color
                    .setContentColor(Color.parseColor("#7CB342"))
                    // Set content intent
                    .setContentIntent(TrackerTile.INTENT_TYPE_ACTIVITY, launchIntent)
                    // Set button intent
                    .setButtonIntent("START", TrackerTile.INTENT_TYPE_SERVICE, serviceIntent);

            if (mTrackerTileManager != null) {
                mTrackerTileManager.post(myTrackerTile);
            }

        } catch (IllegalArgumentException e) {
            Log.d(LOG_TAG, "MyTracker postDefaultTile(" + trackerId + ", " + tileId + ") IllegalArgumentException");
        } catch (NotFoundException e) {
            Log.d(LOG_TAG, "MyTracker postDefaultTile(" + trackerId + ", " + tileId + ") NotFoundException");
        }
    }

    public void updateTile(Context context, String trackerId, String tileId) {
        Log.d(LOG_TAG, "updateTile(" + trackerId + ", " + tileId + ")");

        SharedPreferences sp = context.getSharedPreferences(SHARED_PREFERENCE_NAME, Context.MODE_PRIVATE);
        mTileContent = sp.getInt(SHARED_PREFERENCE_CONTENT_VALUE_KEY, 0);

        TrackerTile myTrackerTile = null;

        if (tileId == null) {
            tileId = MY_TILE_ID;
        }

        try {
            // Create Intent to do an action
            // when the tracker tile is clicked
            Intent launchIntent = new Intent(context, MainActivity.class);

            // Create Intent to do an action
            // when the button on this tile is clicked
            Intent serviceIntent = new Intent(context, MyTrackerService.class);

            String validationKey = sp.getString(VALIDATION_KEY, "");
            if (validationKey.isEmpty()) {
                validationKey = VALIDATION_VALUE;
                sp.edit().putString(VALIDATION_KEY, validationKey).commit();
            }

            serviceIntent.putExtra(VALIDATION_KEY, validationKey);

            // Set template
            mTemplate = TrackerTile.TRACKER_TILE_TYPE_3;

            // Create TrackerTile and set each values and intents
            myTrackerTile = new TrackerTile(context, trackerId, tileId, mTemplate);

            // Set Title
            myTrackerTile
                    .setTitle(R.string.tracker_test_display_name)
                    // Set Icon resource
                    .setIcon(R.drawable.tracker_test_icon_big)
                    // Set content value
                    .setContentValue(String.valueOf(mTileContent))
                    // Set content unit
                    .setContentUnit("LBS")
                    // Set Date text
                    .setDate(new Date())
                    // Set content color
                    .setContentColor(Color.parseColor("#7CB342"))
                    // Set content intent
                    .setContentIntent(TrackerTile.INTENT_TYPE_ACTIVITY, launchIntent)
                    // Set button intent
                    .setButtonIntent("UPDATE", TrackerTile.INTENT_TYPE_SERVICE, serviceIntent);

            if (mTrackerTileManager != null) {
                mTrackerTileManager.post(myTrackerTile);
            }

        } catch (IllegalArgumentException e) {
            Log.d(LOG_TAG, "MyTracker updateTile(" + trackerId + ", " + tileId + ") IllegalArgumentException");
        } catch (NotFoundException e) {
            Log.d(LOG_TAG, "MyTracker updateTile(" + trackerId + ", " + tileId + ") NotFoundException");
        }
    }

    public void removeTile(Context context, String trackerId, String tileId) {
        Log.d(LOG_TAG, "removeTile(" + trackerId + ", " + tileId + ")");

        try {
            if (mTrackerTileManager != null) {
                mTrackerTileManager.remove(trackerId, tileId);
            }
        } catch (IllegalArgumentException e) {
            Log.d(LOG_TAG, "MyTracker removeTile(" + trackerId + ", " + tileId + ") IllegalArgumentException");
        }
    }
}
