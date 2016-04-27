
package com.samsung.android.app.plugintracker;

import com.samsung.android.sdk.shealth.tracker.TrackerTileManager;

import android.app.Activity;
import android.graphics.Color;
import android.os.Bundle;
import android.text.Spannable;
import android.text.SpannableStringBuilder;
import android.text.style.ForegroundColorSpan;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;

import java.util.ArrayList;

public class MainActivity extends Activity {
    
    private MyTracker myTracker;
    private TextView mText;
    private static final String TRACKER_ID = "tracker.test";
    private static final String LOG_TAG = "PluginTracker";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        myTracker = new MyTracker(this);

        mText = (TextView) findViewById(R.id.state_message);
        updateDescriptionText();

        // Button to remove a posted tile
        ((Button) findViewById(R.id.remove1)).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                try {
                    TrackerTileManager mTrackerTileManager = new TrackerTileManager(v.getContext());
                    if (mTrackerTileManager != null) {
                        ArrayList<String> postedTileIds = mTrackerTileManager.getPostedTrackerTileIds(TRACKER_ID);

                        if (postedTileIds != null && postedTileIds.size() > 0) {
                            for (String tileId : postedTileIds) {
                                myTracker.removeTile(v.getContext(), TRACKER_ID, tileId);
                            }
                        }
                    }
                } catch (IllegalArgumentException e) {
                    Log.d(LOG_TAG, "MainActivity onCreate() IllegalArgumentException");
                }

                updateDescriptionText();
            }
        });
    }

    @Override
    protected void onResume() {
        super.onResume();
        updateDescriptionText();
    }

    // Update TextView with posted tile information list
    private void updateDescriptionText() {
        try {
            TrackerTileManager mTrackerTileManager = new TrackerTileManager(this);

            if (mTrackerTileManager != null) {

                ArrayList<String> list = mTrackerTileManager.getPostedTrackerTileIds(TRACKER_ID);

                SpannableStringBuilder tileListText = new SpannableStringBuilder();
                tileListText.append("Posted tiles id : [");

                int start = tileListText.toString().length();
                for (String tileId : list) {
                    tileListText.append(tileId + ", ");
                }
                int end = tileListText.toString().length();

                tileListText.append("]");
                tileListText.setSpan(new ForegroundColorSpan(Color.RED), start, end,
                        Spannable.SPAN_EXCLUSIVE_EXCLUSIVE);
                mText.setText(tileListText);
            }
        } catch (IllegalArgumentException e) {
            Log.d(LOG_TAG, "MainActivity updateDescriptionText() IllegalArgumentException");
        }
    }
}
