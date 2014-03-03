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
using System.Collections;

/// <summary>
/// Amazon GameCircle example implementation of profile retrieval.
/// </summary>
public class AmazonGameCircleExampleProfiles : AmazonGameCircleExampleBase {
    
    #region Local variables
	// These strings are updated once
	// player profile begins retrieval.
    string playerProfileStatus = null;
    string playerProfileStatusMessage = null;
	// the player profile information.
    AGSProfile playerProfile = null;
    #endregion
    
    #region Local const strings
    // The title of this menu
    private const string profileMenuTitle = "User Profile";
    // UI labels for profile retrieval callbacks.
    private const string playerAliasReceivedLabel = "Retrieved local player data";
    private const string playerAliasFailedLabel = "Failed to retrieve local player data";
    // label for the button that begins profile retrieval
    private const string playerAliasRetrieveButtonLabel = "Retrieve local player data";
    // label for displaying player profile information
    private const string playerProfileLabel = "ID {0} : Alias {1}";
    // label for displaying that player profile retrieval has begun.
    private const string playerAliasRetrievingLabel = "Retrieving local player data...";
	// displaying "null" instead of an empty string looks nicer in the UI
    private const string nullAsString = "null";
    #endregion
        
    #region base class implementation
    /// <summary>
    /// The title of the menu.
    /// </summary>
    /// <returns>
    /// The title of the menu.
    /// </returns>
    public override string MenuTitle() {
        return profileMenuTitle;   
    }
    /// <summary>
    /// Draws the GameCircle Profiles Menu. Note that this must be called from an OnGUI function.
    /// </summary>
    public override void DrawMenu() {     
        // Once the playerProfileStatus string is not null, profile retrieval has begun.
        if(string.IsNullOrEmpty(playerProfileStatus)) {
            // This button begins the player profile retrieval process.
            if(GUILayout.Button(playerAliasRetrieveButtonLabel)) {
                RequestLocalPlayerData();    
            }
        }
        else {
            AmazonGameCircleExampleGUIHelpers.CenteredLabel(playerProfileStatus);
			// If there is a status / error message, display it.
            if(!string.IsNullOrEmpty(playerProfileStatusMessage)) {
                AmazonGameCircleExampleGUIHelpers.CenteredLabel(playerProfileStatusMessage);    
            }
			// player profile has been received, display it.
            if(null != playerProfile) {
            	// When the profile information is null (for guest accounts), 
				// displaying "null" looks nicer than an empty string
                string playerId = !string.IsNullOrEmpty(playerProfile.playerId) ? playerProfile.playerId : nullAsString;
                string alias = !string.IsNullOrEmpty(playerProfile.alias) ? playerProfile.alias : nullAsString;
                 
                AmazonGameCircleExampleGUIHelpers.CenteredLabel(string.Format(playerProfileLabel,playerId,alias));    
            }
        }
    }
    #endregion
       
    #region GameCircle plugin functions
    /// <summary>
    /// Requests the local player data from the GameCircle plugin.
    /// </summary>
    void RequestLocalPlayerData() {
        // Need to subscribe to callback messages to receive the player profile from GameCircle.
        SubscribeToProfileEvents();
		// Request the player's profile from the GameCircle plugin
        AGSProfilesClient.RequestLocalPlayerProfile();
        
        // update the menu to show that the retrieval process has begun.
        playerProfileStatus = playerAliasRetrievingLabel;
    }        
    
    /// <summary>
    /// Subscribes to GameCircle profile events.
    /// </summary>
    void SubscribeToProfileEvents() {
        AGSProfilesClient.PlayerAliasReceivedEvent += PlayerAliasReceived;
        AGSProfilesClient.PlayerAliasFailedEvent += PlayerAliasFailed;   
    }
    
    /// <summary>
    /// Unsubscribes from GameCircle profile events.
    /// </summary>
    void UnsubscribeFromProfileEvents() {
        AGSProfilesClient.PlayerAliasReceivedEvent -= PlayerAliasReceived;
        AGSProfilesClient.PlayerAliasFailedEvent -= PlayerAliasFailed;          
    }
    #endregion
    
    #region Callbacks
    /// <summary>
    /// Callback for receiving player profile information.
    /// </summary>
    /// <param name='profile'>
    /// GameCircle profile information
    /// </param>
    private void PlayerAliasReceived(AGSProfile profile) {
        // Update the menu information to show the received player profile.
        playerProfileStatus = playerAliasReceivedLabel;
        playerProfileStatusMessage = null;
        playerProfile = profile;
        
        // no longer need to subscribe after a callback has occured.
        UnsubscribeFromProfileEvents();
    }
    
    /// <summary>
    /// Callback for handling errors attempting to retrieve the local player's profile.
    /// </summary>
    /// <param name='errorMessage'>
    /// Error message.
    /// </param>
    private void PlayerAliasFailed(string errorMessage) {
        playerProfileStatus = playerAliasFailedLabel;
        playerProfileStatusMessage = errorMessage;
        
        // no longer need to subscribe after a callback has occured.
        UnsubscribeFromProfileEvents();
    }
    #endregion
    
}
