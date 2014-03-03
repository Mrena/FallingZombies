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
using UnityEngine;
using System;
using System.Collections.Generic;
#if UNITY_IOS
using System.Runtime.InteropServices;
#endif

/// <summary>
/// WhispersyncClient used for interacting with synchronized game objects
/// </summary>
public class AGSWhispersyncClient : MonoBehaviour
{
    private static AmazonJavaWrapper javaObject;

#if UNITY_ANDROID
    private static readonly string PROXY_CLASS_NAME = "com.amazon.ags.api.unity.WhispersyncClientProxyImpl"; 
#elif UNITY_IOS
    // For iOS, a JavaObject cannot be used, so a static instance of the game map is kept instead.
    static AGSGameDataMap gameDataMapInstance = null;
    
     [DllImport ("__Internal")]
     private static extern void _AmazonGCWSSynchronize();
    
     [DllImport ("__Internal")]
     private static extern void _AmazonGCWSFlush();
#endif
    
    /// <summary>
    /// Event called when new data is available
    /// </summary>
    public static event Action OnNewCloudDataEvent;

    static AGSWhispersyncClient()
    {
#if UNITY_ANDROID    
        javaObject = new AmazonJavaWrapper(); 

        using( var PluginClass = new AndroidJavaClass( PROXY_CLASS_NAME ) ){
            if (PluginClass.GetRawClass() == IntPtr.Zero)
            {
                AGSClient.LogGameCircleWarning("No java class " + PROXY_CLASS_NAME + " present, can't use AGSWhispersyncClient" );
                return;
            }        
            javaObject.setAndroidJavaObject(PluginClass.CallStatic<AndroidJavaObject>( "getInstance" ));
        }
#endif
    }
    
     /// <summary>
     /// gets the root game datamap 
     /// </summary>
     /// <returns>Game datamap</returns>
    public static AGSGameDataMap GetGameData( )
    {
#if UNITY_ANDROID
        AndroidJavaObject jo = javaObject.Call<AndroidJavaObject>( "getGameData" );
        if(jo != null){
            return new AGSGameDataMap(new AmazonJavaWrapper(jo));
        }
        return null;
#elif UNITY_IOS
        if(null == gameDataMapInstance) {
            gameDataMapInstance = new AGSGameDataMap();   
        }
        return gameDataMapInstance;
#else
        return null;
#endif
    }
    
     /// <summary>
     /// Manually triggers a background thread to synchronize in-memory game data with local storage and the cloud.
     /// </summary>
    public static void Synchronize(){
#if UNITY_ANDROID            
        javaObject.Call( "synchronize" );    
#elif UNITY_IOS && !UNITY_EDITOR
        _AmazonGCWSSynchronize();
#endif
    }

     /// <summary>
     /// Manually triggers a background thread to write in-memory game data to only the local storage.
     /// </summary>    
    public static void Flush(){
#if UNITY_ANDROID                    
        javaObject.Call( "flush" );
#elif UNITY_IOS && !UNITY_EDITOR
        _AmazonGCWSFlush();
#endif
    }

    /// <summary>
    ///  callback method for native code to communicate events back to unity
    /// </summary>            
    public static void OnNewCloudData()
    {
        if( OnNewCloudDataEvent != null )
        {        
            OnNewCloudDataEvent(  );
        }
    }

}
