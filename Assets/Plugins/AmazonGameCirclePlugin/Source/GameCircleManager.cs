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
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// GameCircle manager.
/// </summary>
/// <remarks>
/// Helper script for managing native to unity messages
/// </remarks>

public class GameCircleManager : MonoBehaviour
{
	
	void Awake()
	{
		// Set the GameObject name to the class name for easy access from native code
		gameObject.name = this.GetType().ToString();
		DontDestroyOnLoad( this );
	}

	public void serviceReady( string empty )
	{		
		AGSClient.Log ("GameCircleManager - serviceReady");
		
		AGSClient.
			ServiceReady(empty);
	}
	
	public void serviceNotReady( string param )
	{
		AGSClient.Log ("GameCircleManager - serviceNotReady");

		AGSClient.
			ServiceNotReady( param );
	}
	
	public void playerAliasReceived( string json )
	{
		AGSClient.Log ("GameCircleManager - playerAliasReceived");
		
		AGSProfilesClient.
			PlayerAliasReceived( json );
	}
	
	public void playerAliasFailed( string json )
	{
		AGSClient.Log ("GameCircleManager - playerAliasFailed");	
		AGSProfilesClient.
			PlayerAliasFailed( json );
	}
	
	public void submitScoreFailed( string json )
	{
		AGSClient.Log ("GameCircleManager - submitScoreFailed");

		AGSLeaderboardsClient.
			SubmitScoreFailed( json );
	}

	public void submitScoreSucceeded( string json )
	{
		AGSClient.Log ("GameCircleManager - submitScoreSucceeded");
		AGSLeaderboardsClient.
			SubmitScoreSucceeded( json );
	}

	public void requestLeaderboardsFailed( string json )
	{
		AGSClient.Log ("GameCircleManager - requestLeaderboardsFailed");
		AGSLeaderboardsClient.
			RequestLeaderboardsFailed( json );
	}

	public void requestLeaderboardsSucceeded( string json )
	{	
		AGSClient.Log ("GameCircleManager - requestLeaderboardsSucceeded");
		AGSLeaderboardsClient.
			RequestLeaderboardsSucceeded(json);
	}

	public void requestLocalPlayerScoreFailed( string json )
	{
		AGSClient.Log ("GameCircleManager - requestLocalPlayerScoreFailed");
		AGSLeaderboardsClient.
			RequestLocalPlayerScoreFailed( json );
	}

	public void requestLocalPlayerScoreSucceeded( string json )
	{
		AGSClient.Log ("GameCircleManager - requestLocalPlayerScoreSucceeded");
		AGSLeaderboardsClient.
				RequestLocalPlayerScoreSucceeded(json);
	}

	public void updateAchievementSucceeded( string json )
	{
		AGSClient.Log ("GameCircleManager - updateAchievementSucceeded");
		AGSAchievementsClient.UpdateAchievementSucceeded( json );
	}
	
	public void updateAchievementFailed( string json )
	{
		AGSClient.Log ("GameCircleManager - updateAchievementsFailed");
		AGSAchievementsClient.
			UpdateAchievementFailed( json );
	}
	
	public void requestAchievementsSucceeded( string json )
	{	
		AGSClient.Log ("GameCircleManager - requestAchievementsSucceeded");

		AGSAchievementsClient.
			RequestAchievementsSucceeded( json );
	}
	
	public void requestAchievementsFailed( string json )
	{
		AGSClient.Log ("GameCircleManager -  requestAchievementsFailed");
		AGSAchievementsClient.
			RequestAchievementsFailed( json );
	}

	public void onNewCloudData( string empty ){
		AGSWhispersyncClient.OnNewCloudData();	
	}
	
	public void OnApplicationFocus(Boolean focusStatus){
		if(!AGSClient.IsServiceReady()){
			return;
		}
		
		if(!focusStatus){
			AGSClient.release();	
		}
	}
	
}

