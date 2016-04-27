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

package com.samsung.android.app.plugintracker.service;

import com.samsung.android.app.plugintracker.MyTracker;
import com.samsung.android.sdk.shealth.tracker.TrackerTileManager;

import android.app.IntentService;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.util.Log;

/**
 * This class receives event from SHealth
 * 
 * @hide This class contains internal implementation for SHealth application
 */
final public class MyTrackerService extends IntentService {

    private static final String LOG_TAG = "PluginTracker";

    private static final String SHARED_PREFERENCE_NAME = "tile_content";
    private static final String SHARED_PREFERENCE_CONTENT_VALUE_KEY = "content_value";
    private static final String SHARED_PREFERENCE_LOGIN_KEY = "log_in";
    private static final String VALIDATION_KEY = "validation_key";

    /**
     * Constructs a TrackerTileService object.
     */
    public MyTrackerService() {
        super(LOG_TAG);
    }

    @Override
    protected void onHandleIntent(Intent intent) {

        if (intent == null) {
            return;
        }

        String trackerId = intent.getStringExtra(TrackerTileManager.EXTRA_TRACKER_ID);
        if (trackerId == null) {
            return;
        }
        Log.d(LOG_TAG, "tracker id : " + trackerId);

        String tileId = intent.getStringExtra(TrackerTileManager.EXTRA_TILE_ID);
        if (tileId == null) {
            return;
        }
        Log.d(LOG_TAG, "tile id : " + tileId);
        
        String validationValue = intent.getStringExtra(VALIDATION_KEY);
        SharedPreferences sp = getSharedPreferences(SHARED_PREFERENCE_NAME, Context.MODE_PRIVATE);
        String validationSavedValue = sp.getString(VALIDATION_KEY, "");

        if (validationValue.isEmpty() || !validationValue.equals(validationSavedValue)) {
            Log.d(LOG_TAG, "invalid validation value");
            return;
        }

        boolean isLogInRequest = intent.getBooleanExtra(SHARED_PREFERENCE_LOGIN_KEY, false);
        Log.d(LOG_TAG, "log in request : " + isLogInRequest);

        if (isLogInRequest) {
            sp.edit().putBoolean(SHARED_PREFERENCE_LOGIN_KEY, true).commit();

        } else {
            int tileContent = sp.getInt(SHARED_PREFERENCE_CONTENT_VALUE_KEY, 0) + 1;
            Log.d(LOG_TAG, "content value : " + String.valueOf(tileContent));
            sp.edit().putInt(SHARED_PREFERENCE_CONTENT_VALUE_KEY, tileContent).apply();
        }

        MyTracker tracker = new MyTracker(this);
        tracker.updateTile(this, trackerId, tileId);
    }

    @Override
    public void onDestroy() {
        super.onDestroy();
    }
}
