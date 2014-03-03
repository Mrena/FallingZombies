/**
 * © 2012-2013 Amazon Digital Services, Inc. All rights reserved.
 *
 * Licensed under the Apache License, Version 2.0 (the "License"). You may not use this file except in compliance with the License. A copy
 * of the License is located at
 *
 * http://aws.amazon.com/apache2.0/
 *
 * or in the "license" file accompanying this file. This file is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
 */
using AmazonCommon;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_IOS
using System.Runtime.InteropServices;
#endif

/// <summary>
/// enum for the available leaderboard scopes
/// </summary>
public enum LeaderboardScope{
    GlobalAllTime,
    GlobalWeek,
    GlobalDay,
    FriendsAllTime
}

/// <summary>
/// Client used to submit and read leaderboards for the current logged in or guest player
/// </summary>
public class AGSLeaderboardsClient : MonoBehaviour{
    
    private static AmazonJavaWrapper JavaObject;
    
#if UNITY_ANDROID
    private static readonly string PROXY_CLASS_NAME = "com.amazon.ags.api.unity.LeaderboardsClientProxyImpl"; 
#elif UNITY_IOS
     [DllImport ("__Internal")]
     private static extern void _AmazonGameCircleShowLeaderboardsOverlay(bool isAnimated);
    
     [DllImport ("__Internal")]
     private static extern void _AmazonGameCircleSubmitScore(string leaderboardId, long score);
    
     [DllImport ("__Internal")]
     private static extern void _AmazonGameCircleRequestLeaderboards();
    
     [DllImport ("__Internal")]
     private static extern void _AmazonGameCircleRequestLocalPlayerScore( string leaderboardId, int scope);
#endif
    
    /// <summary>
    /// Event called when a score submission fails
    /// </summary>
    /// <param name="leaderboardId">the id of the leaderboard that failed to update</param>
    /// <param name="failureReason">a string indicating the failure reason</param>
    
    public static event Action<string,string> SubmitScoreFailedEvent;

    /// <summary>
    /// Event called when a score submission succeeds
    /// </summary>
    /// <param name="leaderboardId">the id of the leaderboard that a score has been submitted to</param>
    /// <param name="failureReason">a string indicating the failure reason</param>    
    public static event Action<string> SubmitScoreSucceededEvent;
    
    
    /// <summary>
    /// Event called when a request for all game leaderboards fails
    /// </summary>
    /// <param name="failureReason">a string indicating the failure reason</param>    
    public static event Action<string> RequestLeaderboardsFailedEvent;

    /// <summary>
    /// Event called when a request for all game leaderboards succeeds
    /// </summary>
    /// <param name="leaderboardList">list of leaderboards for this game</param>    
    public static event Action<List<AGSLeaderboard>> RequestLeaderboardsSucceededEvent;
    
    /// <summary>
    /// Event called when a score request fails
    /// </summary>
    /// <param name="leaderboardId">the id of the leaderboard for the failed request</param>
    /// <param name="failureReason">a string indicating the failure reason</param>    
    public static event Action<string,string> RequestLocalPlayerScoreFailedEvent;
    
    /// <summary>
    /// Event called when a score request succeeds
    /// </summary>
    /// <param name="leaderboardId">the id of the leaderboard for the score request</param>
    /// <param name="rank">player's rank in leaderboard</param>        
    /// <param name="score">player's score in leaderboard</param>        
    public static event Action<string,int,long> RequestLocalPlayerScoreSucceededEvent;
    
    /// <summary>
    /// Initializes the <see cref="AGSLeaderboardsClient"/> class.
    /// </summary>
    static AGSLeaderboardsClient(){    
#if UNITY_ANDROID
        JavaObject = new AmazonJavaWrapper(); 
        using( var PluginClass = new AndroidJavaClass( PROXY_CLASS_NAME ) ){
            
            if (PluginClass.GetRawClass() == IntPtr.Zero)
            {
                AGSClient.LogGameCircleWarning("No java class " + PROXY_CLASS_NAME + " present, can't use AGSLeaderboardsClient" );
                return;
            }
            JavaObject.setAndroidJavaObject(PluginClass.CallStatic<AndroidJavaObject>( "getInstance" ));
        }
#endif

    }

    /// <summary>
    /// submit a score to leaderboard
    /// </summary>
    /// <remarks>
    /// SubmitScoreSuccess or SubmitScoreFailure events will be called if they are registered
    /// </remarks>
    /// <param name="leaderboardId">the id of the leaderboard for the score request</param>
    /// <param name="score">player score</param>
    public static void SubmitScore( string leaderboardId, long score ){
#if UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
        // GameCircle only functions on device.
#elif UNITY_ANDROID      
        JavaObject.Call( "submitScore", leaderboardId, score );
#elif UNITY_IOS
        _AmazonGameCircleSubmitScore(leaderboardId, score);
#else
        if( SubmitScoreFailedEvent != null ){
            SubmitScoreFailedEvent( leaderboardId, "PLATFORM_NOT_SUPPORTED" );
        }        
#endif
    }


    /// <summary>
    /// show leaderboard in GameCircle overlay
    /// </summary>
    public static void ShowLeaderboardsOverlay(){
#if UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
        // GameCircle only functions on device.
#elif UNITY_ANDROID      
        JavaObject.Call( "showLeaderboardsOverlay" );
#elif UNITY_IOS
        _AmazonGameCircleShowLeaderboardsOverlay(true);
#endif
    }

    /// <summary>
    /// request all leaderboards for this game
    /// </summary>
    /// <remarks>
    /// RequestLeaderboardSucceeded or RequestLeaderboardFailed events will be called if they are registered
    /// </remarks>
    public static void RequestLeaderboards(){
#if UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
        // GameCircle only functions on device.
#elif UNITY_ANDROID      
        JavaObject.Call( "requestLeaderboards" );
#elif UNITY_IOS
        _AmazonGameCircleRequestLeaderboards();
#else
        if( RequestLeaderboardsFailedEvent != null ){
            RequestLeaderboardsFailedEvent( "PLATFORM_NOT_SUPPORTED" );
        }        
#endif        
    }


    /// <summary>
    /// request current player's score for a given leaderboad and scope
    /// </summary>
    /// <remarks>
    /// RequestLocalPlayerScoreSucceededEvent or RequestLocalPlayerScoreFailedEvent events will be called if they are registered
    /// </remarks>
    /// <param name="leaderboardId">the id of the leaderboard for the score request</param>
    /// <param name="scope">enum value of leaderboard scope</param>
    public static void RequestLocalPlayerScore( string leaderboardId, LeaderboardScope scope ){
#if UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
        // GameCircle only functions on device.
#elif UNITY_ANDROID      
        JavaObject.Call( "requestLocalPlayerScore", leaderboardId, (int)scope );
#elif UNITY_IOS
        _AmazonGameCircleRequestLocalPlayerScore(leaderboardId, (int)scope);
#else
        if( RequestLocalPlayerScoreFailedEvent != null ){
            RequestLocalPlayerScoreFailedEvent( leaderboardId, "PLATFORM_NOT_SUPPORTED" );
        }    
#endif        
    }

    /// <summary>
    ///  callback method for native code to communicate events back to unity
    /// </summary>
    public static void SubmitScoreFailed( string json ){
        if( SubmitScoreFailedEvent != null ){
            var ht = json.hashtableFromJson();
            string leaderboardId = GetStringFromHashtable(ht,"leaderboardId");
            string error = GetStringFromHashtable(ht,"error");
            SubmitScoreFailedEvent( leaderboardId, error );
        }
    }

    /// <summary>
    ///  callback method for native code to communicate events back to unity
    /// </summary>
    public static void SubmitScoreSucceeded( string json ){
        if( SubmitScoreSucceededEvent != null ){
            var ht = json.hashtableFromJson();
            string leaderboardId = GetStringFromHashtable(ht,"leaderboardId");
            SubmitScoreSucceededEvent(leaderboardId);
        }
    }

    /// <summary>
    ///  callback method for native code to communicate events back to unity
    /// </summary>
    public static void RequestLeaderboardsFailed( string json ){
        if( RequestLeaderboardsFailedEvent != null ){
            var ht = json.hashtableFromJson();
            string error = GetStringFromHashtable(ht,"error");
            RequestLeaderboardsFailedEvent( error );
        }
    }

    /// <summary>
    ///  callback method for native code to communicate events back to unity
    /// </summary>
    public static void RequestLeaderboardsSucceeded( string json ){
        if( RequestLeaderboardsSucceededEvent != null ){
            var leaderboards = new List<AGSLeaderboard>();
            var arrayList = json.arrayListFromJson();
            foreach( Hashtable ht in arrayList ){
                leaderboards.Add( AGSLeaderboard.fromHashtable( ht ) );
            }
            
            RequestLeaderboardsSucceededEvent( leaderboards );
        }
    }

    /// <summary>
    ///  callback method for native code to communicate events back to unity
    /// </summary>
    public static void RequestLocalPlayerScoreFailed( string json ){
        if( RequestLocalPlayerScoreFailedEvent != null ){
            var ht = json.hashtableFromJson();
            string leaderboardId = GetStringFromHashtable(ht,"leaderboardId");
            string error = GetStringFromHashtable(ht,"error");
            RequestLocalPlayerScoreFailedEvent( leaderboardId, error );
        }
    }

    /// <summary>
    ///  callback method for native code to communicate events back to unity
    /// </summary>
    public static void RequestLocalPlayerScoreSucceeded( string json ){
        if( RequestLocalPlayerScoreSucceededEvent != null ){
                var ht = json.hashtableFromJson();
                
                int rank = 0;
                long score = 0;
                string leaderboardId = null;
                try{
                
                    if(ht.Contains("leaderboardId")){
                        leaderboardId = ht["leaderboardId"].ToString();
                    }                
                
                    if(ht.Contains("rank")){
                        rank = int.Parse(ht["rank"].ToString());
                    }
                
                    if(ht.Contains("score")){
                        score = long.Parse(ht["score"].ToString());
                    }
                }catch(FormatException e){
                    AGSClient.Log ("unable to parse score " + e.Message);
                }
                        
                RequestLocalPlayerScoreSucceededEvent( leaderboardId, rank, score );
        }
    }
    
    /// <summary>
    /// Gets the string from hashtable.
    /// </summary>
    /// <returns>
    /// The string from hashtable.
    /// </returns>
    /// <param name='ht'>
    /// Ht.
    /// </param>
    /// <param name='key'>
    /// Key.
    /// </param>
    private static string GetStringFromHashtable(Hashtable ht, string key){
        string val = null;
        if(ht.Contains(key)){
            val = ht[key].ToString();    
        }
        return val;
    }

    
}
