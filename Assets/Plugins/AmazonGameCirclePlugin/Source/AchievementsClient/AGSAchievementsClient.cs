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
/// Client used to submit and read achievements for the current logged in or guest player
/// </summary>
public class AGSAchievementsClient : MonoBehaviour{
	
	private static AmazonJavaWrapper JavaObject;
    
#if UNITY_ANDROID
	private static readonly string PROXY_CLASS_NAME = "com.amazon.ags.api.unity.AchievementsClientProxyImpl"; 
#elif UNITY_IOS
     [DllImport ("__Internal")]
     private static extern void _AmazonGameCircleShowAchievementsOverlay(bool isAnimated);
     [DllImport ("__Internal")]
     private static extern void _AmazonGameCircleUpdateAchievementProgress(string achievementId, float progress);
     [DllImport ("__Internal")]
     private static extern void _AmazonGameCircleRequestAchievements();
    
#endif
    
	
	/// <summary>
	/// Event called when a request to update achievements fails
	/// </summary>
	/// <param name="achievementId">the id of the achievement that failed to update</param>
	/// <param name="failureReason">a string indicating the failure reason</param>
	public static event Action<string,string> UpdateAchievementFailedEvent;
	
	/// <summary>
	/// Event called when a request to update achievements succeeds
	/// </summary>
	/// <param name="achievementId">the id of the achievement that has been updated</param>
	public static event Action<string> UpdateAchievementSucceededEvent;
	
	/// <summary>
	/// Event called when a request to get all achievements succeeds
	/// </summary>
	/// <param name="achievementsList"></param>	
	public static event Action<List<AGSAchievement>> RequestAchievementsSucceededEvent;
	

	/// <summary>
	/// Event called when a request to get all achievements has failed
	/// </summary>
	/// <param name="failureReason">a string indicating the reason for the request failure</param>	
	public static event Action<string> RequestAchievementsFailedEvent;

	
	static AGSAchievementsClient(){
#if UNITY_ANDROID
		
		JavaObject = new AmazonJavaWrapper(); 
		using( var PluginClass = new AndroidJavaClass( PROXY_CLASS_NAME ) ){
			if (PluginClass.GetRawClass() == IntPtr.Zero)
            {
                AGSClient.LogGameCircleWarning(string.Format("No java class {0} present, can't use AGSAchievementsClient",PROXY_CLASS_NAME ));
				return;
            }
			JavaObject.setAndroidJavaObject(PluginClass.CallStatic<AndroidJavaObject>( "getInstance" ));
		}
#endif

	}
	
	/// <summary>
	/// updates an achievement
	/// </summary>
	/// <remarks>
	/// If a value outside of range is submitted, it is capped at 100 or 0.
    /// If submitted value is less than the stored value, the update is ignored.
	/// </remarks>
	/// <param name="achievementId">the id of the achievement to update</param>
	/// <param name="percentComplete">a float between 0.0f and 100.0f</param>	
	public static void UpdateAchievementProgress( string achievementId, float progress ){
#if UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
        // GameCircle only functions on device.
#elif UNITY_ANDROID		
		JavaObject.Call( "updateAchievementProgress", achievementId, progress );	
#elif UNITY_IOS
        _AmazonGameCircleUpdateAchievementProgress(achievementId,progress);
#else
		if( UpdateAchievementFailedEvent != null ){
			UpdateAchievementFailedEvent( achievementId, "PLATFORM_NOT_SUPPORTED" );
		}
#endif
	}

	
	/// <summary>
	///  requests a list of all achievements
	/// </summary>
	/// <remarks>
	/// Registered updateAchievementSucceededEvents will recieve response
    /// The list returned in the response includes all achievements for the game.
    /// Each Achievement object in the list includes the current players
    /// progress toward the Achievement.
	/// </remarks>
	public static void RequestAchievements(){
#if UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
        // GameCircle only functions on device.
#elif UNITY_ANDROID      
		JavaObject.Call( "requestAchievements" );
#elif UNITY_IOS
        _AmazonGameCircleRequestAchievements();
#else
		if(RequestAchievementsFailedEvent != null){
			RequestAchievementsFailedEvent("PLATFORM_NOT_SUPPORTED");
		}
#endif
	}

	/// <summary>
	///  shows the Amazon GameCircle Overlay
	/// </summary>
	public static void ShowAchievementsOverlay(){
#if UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
        // GameCircle only functions on device.
#elif UNITY_ANDROID      
		JavaObject.Call( "showAchievementsOverlay" );
#elif UNITY_IOS
        _AmazonGameCircleShowAchievementsOverlay(true);
#endif
	}
	
	/**
	 * callback method for native code
	 **/
	public static void RequestAchievementsSucceeded( string json ){
		if( RequestAchievementsSucceededEvent != null ){
			var Achievements = new List<AGSAchievement>();
			var list = json.arrayListFromJson();
			foreach( Hashtable ht in list ){
				Achievements.Add( AGSAchievement.fromHashtable( ht ) );
			}
			RequestAchievementsSucceededEvent( Achievements );
		}
	}
	
	/// <summary>
	///  callback method for native code to communicate events back to unity
	/// </summary>
	public static void UpdateAchievementFailed( string json ){
		if( UpdateAchievementFailedEvent != null ){
			var ht = json.hashtableFromJson();
			string achievementId = GetStringFromHashtable(ht,"achievementId");
			string error = GetStringFromHashtable(ht,"error");
			UpdateAchievementFailedEvent( achievementId, error );
		}
	}

	/// <summary>
	///  callback method for native code to communicate events back to unity
	/// </summary>
	public static void UpdateAchievementSucceeded( string json ){
		if( UpdateAchievementSucceededEvent != null ){
			var ht = json.hashtableFromJson();
			string AchievementId = GetStringFromHashtable(ht,"achievementId");
			UpdateAchievementSucceededEvent(AchievementId);
		}
	}
	
	/// <summary>
	///  callback method for native code to communicate events back to unity
	/// </summary>
	public static void RequestAchievementsFailed( string json ){
		if( RequestAchievementsFailedEvent != null ){
			var ht = json.hashtableFromJson();
			string error = GetStringFromHashtable(ht,"error");
			RequestAchievementsFailedEvent(error);
		}
	}
	
	private static string GetStringFromHashtable(Hashtable ht, string key){
        if(null == ht) {
            return null;
        }
        if(null == key) {
            return null;                
        }
        string val = null;
        if(ht.Contains(key)){
            val = ht[key].ToString();	
        }
        return val;
	}
}
