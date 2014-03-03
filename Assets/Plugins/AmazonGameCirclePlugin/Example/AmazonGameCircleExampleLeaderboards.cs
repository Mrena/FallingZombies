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
using System.Collections.Generic;

/// <summary>
/// Amazon GameCircle implementation of example Leaderboard functionality.
/// </summary>
public class AmazonGameCircleExampleLeaderboards : AmazonGameCircleExampleBase {
    
    #region Leaderboard Variables
    // Key: Leaderboard ID, value: leaderboard submission status
    Dictionary<string,string> leaderboardsSubmissionStatus = new Dictionary<string, string>();
    // Key: Leaderboard ID, value: leaderboard submission message (any errors that occured)
    Dictionary<string,string> leaderboardsSubmissionStatusMessage = new Dictionary<string, string>();
    // Key: Leaderboard ID, value: leaderboard local score request status
    Dictionary<string,string> leaderboardsLocalScoreStatus = new Dictionary<string, string>();
    // Key: Leaderboard ID, value: leaderboard local score request message (any errors that occured)
    Dictionary<string,string> leaderboardsLocalScoreStatusMessage = new Dictionary<string, string>();
    // Key: Leaderboard ID, value: if the leaderboard is folded out.
    Dictionary<string,bool> leaderboardsFoldout = new Dictionary<string, bool>();
    // This menu uses a shared value for all leaderboard score submissions
    long leaderboardScoreValue = 0; 
    // Status messages for showing the status of leaderboard retrieval
    string requestLeaderboardsStatus = null;
    string requestLeaderboardsStatusMessage = null;
    // The list of retrieved leaderboards
    List<AGSLeaderboard> leaderboardList = null;
    // Are the leaderboards ready for this? This is used instead of
    // checking the leaderboard list for null, because if there are no
    // leaderboards available, the success callback will return null.
    bool leaderboardsReady = false;
    // The time the leaderboard list request began. This helps
    // show that this is an asynchronous operation.
    System.DateTime leaderboardsRequestTime;
    // The scope of leaderboard score requests.
    LeaderboardScope leaderboardScoreScope = LeaderboardScope.GlobalAllTime;
     // an invalid leaderboard used to test that GameCircle fails gracefully.
    AGSLeaderboard invalidLeaderboard;
    #endregion
    
    #region UI variables
    // define the regex once here to save on re-creating the regex
    // This regex is intended to add a newline after every second comma in a string.
    // See function addNewlineAfterSecondComma for full implementation of this regex.
    readonly System.Text.RegularExpressions.Regex addNewlineEverySecondCommaRegex = new System.Text.RegularExpressions.Regex(@"(,([^,]+),)");
    // The group number to use with the regex for keeping the text between commas.
    const int betweenCommaRegexGroup = 2;
    #endregion
	
    #region Local const strings
    // The title of this menu
    private const string leaderboardsMenuTitle = "Leaderboards";    
    // The label for the button that opens the leaderboards overal.
    private const string DisplayLeaderboardOverlayButtonLabel = "Leaderboards Overlay";
    // The label for the button that requests the list of leaderboards.
    private const string requestLeaderboardsButtonLabel = "Request Leaderboards";
    // The status message to display after the leaderboard list request has begun.
    private const string requestingLeaderboardsLabel = "Requesting Leaderboards...";
    // The status message to display if an error occurs retrieving the leaderboard list.
    private const string requestLeaderboardsFailedLabel = "Request Leaderboards failed with error:";
    // The status message to display if the leaderboard list request succeeded.
    private const string requestLeaderboardsSucceededLabel = "Available Leaderboards";
    // The message to display if there are no leaderboards in the list.
    private const string noLeaderboardsAvailableLabel = "No Leaderboards Available";
    // The label used to display leaderboard IDs.
    private const string leaderboardIDLabel = "Leaderboard \"{0}\"";
    // The label used for displaying the time elapsed since the leaderboard list was requested.
    private const string leaderboardRequestTimeLabel = "{0,5:N1} seconds";
    // The label to display the score for leaderboard score submission.
    private const string leaderboardScoreDisplayLabel = "{0} score units";
	// The label for the button used to submit a new leaderboard score.
    private const string submitLeaderboardButtonLabel = "Submit Score";
	// The status message to display if an error occurs submitting a score.
    private const string leaderboardFailed = "Leaderboard \"{0}\" failed with error:";
	// The status message to display if the leaderboard score submission succeeds.
    private const string leaderboardSucceeded = "Score uploaded to \"{0}\" successfully.";
	// The label for the button used to request the local player's score on a leaderboard.
    private const string requestLeaderboardScoreButtonLabel = "Request Score";
	// The label used to display the local player's score on the leaderboard.
    private const string leaderboardRankScoreLabel = "Rank {0} with score of {1,5:N1}";
	// The label to use if the local player score request failed.
    private const string leaderboardScoreFailed = "\"{0}\" score request failed with error:";
    #endregion
    
    #region local const values (non-string)
    // The minimum and maximum values used for the leaderboard score slider.
    private const float leaderboardMinValue = -10000;
    private const float leaderboardMaxValue = 10000;
    #endregion
    
    #region constructor
    /// <summary>
    /// Initializes a new instance of the <see cref="AmazonGameCircleExampleLeaderboards"/> class.
    /// </summary>
    public AmazonGameCircleExampleLeaderboards() {
        // This is used to test the GameCircle plugin's behaviour with invalid leaderboard information.
        // This leaderboard ID should not be in your game's list, to test what happens with invalid leaderboard submissions.
        invalidLeaderboard = new AGSLeaderboard();
        invalidLeaderboard.id = "Invalid Leaderboard ID";
    }
    #endregion
    
    #region base class implementation
    /// <summary>
    /// The title of the menu.
    /// </summary>
    /// <returns>
    /// The title of the menu.
    /// </returns>
    public override string MenuTitle() {
        return leaderboardsMenuTitle;
    }
    /// <summary>
    /// Draws the menu. Note that this must be called from an OnGUI function.
    /// </summary>
    public override void DrawMenu() {      
        // this button will open the leaderboard overlay.
        if(GUILayout.Button(DisplayLeaderboardOverlayButtonLabel)) {
            AGSLeaderboardsClient.ShowLeaderboardsOverlay();
        }
        
        // If the leaderboard list has not been requested yet, display
        // a button that requests the leaderboard list.
        if(string.IsNullOrEmpty(requestLeaderboardsStatus)) {
            if(GUILayout.Button(requestLeaderboardsButtonLabel)) {
                RequestLeaderboards();   
            }
        }
        else {
            // once a request has been made for the list of leaderboards,
            // display the status message of that process.
            AmazonGameCircleExampleGUIHelpers.CenteredLabel(requestLeaderboardsStatus);
            if(!string.IsNullOrEmpty(requestLeaderboardsStatusMessage)) {
                AmazonGameCircleExampleGUIHelpers.CenteredLabel(requestLeaderboardsStatusMessage);
            }
            
            // If the leaderboards are not ready, display how long it has been since the request was put in.
            if(!leaderboardsReady) {
                AmazonGameCircleExampleGUIHelpers.CenteredLabel(string.Format(leaderboardRequestTimeLabel,(System.DateTime.Now - leaderboardsRequestTime).TotalSeconds));
            }
            else {
                // Once the leaderboard list request callback has been received,
                // display the leaderboards if available.
                if(null != leaderboardList && leaderboardList.Count > 0) {
                    foreach(AGSLeaderboard leaderboard in leaderboardList) {
                        DisplayLeaderboard(leaderboard);
                    }
                }
                // If the leaderboards are not available, display a message explaining that.
                else {
                    AmazonGameCircleExampleGUIHelpers.CenteredLabel(noLeaderboardsAvailableLabel);   
                }
                // display the invalid leaderboard (used to make sure GameCircle handles invalid data gracefully)
                if(null != invalidLeaderboard) {
                    DisplayLeaderboard(invalidLeaderboard);
                }
            }
        }  
    }
    #endregion
    
    
    #region UI functions
    /// <summary>
    /// Displays an individual leaderboard.
    /// </summary>
    /// <param name='leaderboard'>
    /// Leaderboard.
    /// </param>
    void DisplayLeaderboard(AGSLeaderboard leaderboard) {
        // Place a box around each leaderboard, to make it clear in the UI
        // what controls are for what leaderboard.
        GUILayout.BeginVertical(GUI.skin.box);
        // make sure this leaderboard is in the foldout dictionary.
        if(!leaderboardsFoldout.ContainsKey(leaderboard.id)) {
            leaderboardsFoldout.Add(leaderboard.id,false);
        }
        
        // Display a foldout for this leaderboard.
        // Foldouts keep the menu tidy.
        leaderboardsFoldout[leaderboard.id] = AmazonGameCircleExampleGUIHelpers.FoldoutWithLabel(leaderboardsFoldout[leaderboard.id],string.Format(leaderboardIDLabel,leaderboard.id));
        
        // If the foldout is open, display the leaderboard information.
        if(leaderboardsFoldout[leaderboard.id]) {
            // The controls for automatically word wrapping a label are not great,
            // so replacing every second comma in the string returned from the leaderboard's toString function
            // will allow for a cleaner display of each leaderboard's data
            AmazonGameCircleExampleGUIHelpers.AnchoredLabel(AddNewlineEverySecondComma(leaderboard.ToString()),TextAnchor.UpperCenter);
        
            // Display a centered slider, with the minimum value on the left, and maximum value on the right.
            // This lets the user select a value for the leaderboard's score.
            leaderboardScoreValue = (long)AmazonGameCircleExampleGUIHelpers.DisplayCenteredSlider(leaderboardScoreValue,leaderboardMinValue,leaderboardMaxValue,leaderboardScoreDisplayLabel);
            
            // If a leaderboard score submission is in progress, show its status.
            if(leaderboardsSubmissionStatus.ContainsKey(leaderboard.id) && !string.IsNullOrEmpty(leaderboardsSubmissionStatus[leaderboard.id])) {                
                
                AmazonGameCircleExampleGUIHelpers.CenteredLabel(leaderboardsSubmissionStatus[leaderboard.id]);   
                // Display any additional status message / error for this leaderboard submission.
                if(leaderboardsSubmissionStatusMessage.ContainsKey(leaderboard.id) && !string.IsNullOrEmpty(leaderboardsSubmissionStatusMessage[leaderboard.id])) {
                    AmazonGameCircleExampleGUIHelpers.CenteredLabel(leaderboardsSubmissionStatusMessage[leaderboard.id]);   
                }
            }
            
            // This button submits an update to the leaderboard's score to the GameCircle plugin.
            if(GUILayout.Button(submitLeaderboardButtonLabel)) {
                SubmitScoreToLeaderboard(leaderboard.id,leaderboardScoreValue);
            }
            
            // If a request has been made for the local user's score, display the status of that request.
            if(leaderboardsLocalScoreStatus.ContainsKey(leaderboard.id) && !string.IsNullOrEmpty(leaderboardsLocalScoreStatus[leaderboard.id])) {
                AmazonGameCircleExampleGUIHelpers.AnchoredLabel(leaderboardsLocalScoreStatus[leaderboard.id],TextAnchor.UpperCenter);
                // Display any additional information for the status message from the local score request.
                if(leaderboardsLocalScoreStatusMessage.ContainsKey(leaderboard.id) && !string.IsNullOrEmpty(leaderboardsLocalScoreStatusMessage[leaderboard.id])) {
                    AmazonGameCircleExampleGUIHelpers.AnchoredLabel(leaderboardsLocalScoreStatusMessage[leaderboard.id],TextAnchor.UpperCenter);
                }
            }
            
            // This button requests the local user's score.
            if(GUILayout.Button(requestLeaderboardScoreButtonLabel)) {
                RequestLocalPlayerScore(leaderboard.id);
            }
            
        }
        
        GUILayout.EndVertical();
    }
    
    /// <summary>
    /// Adds a newline after every second comma in a string.
    /// It is difficult to decipher the intent of a regex from its definition,
    /// Containing the behavior of the regex within these functions makes the intent clear.
    /// </summary>
    /// <returns>
    /// a string similar to the passed in string, but with a newline after every third comma.
    /// </returns>
    /// <param name='stringToChange'>
    /// String to change.
    /// </param>
    string AddNewlineEverySecondComma(string stringToChange) {
        return addNewlineEverySecondCommaRegex.Replace(    stringToChange, 
                                                        (regexMatchEvaluator) => string.Concat(",",regexMatchEvaluator.Groups[betweenCommaRegexGroup].Value,",\n"));
    }
    #endregion
    
    #region GameCircle plugin functions
    /// <summary>
    /// Requests the list of leaderboards from the GameCircle plugin.
    /// </summary>
    void RequestLeaderboards() {
        // Start the clock, to track the progress of this async operation.
        leaderboardsRequestTime = System.DateTime.Now;
        // Subscribe to the events to receive the leaderboard list.
        SubscribeToLeaderboardRequestEvents();
        // Request the leaderboard list from the GameCircle plugin.
        AGSLeaderboardsClient.RequestLeaderboards();   
        // Set the status message to show this process has begun.
        requestLeaderboardsStatus = requestingLeaderboardsLabel;
    }
	
    /// <summary>
    /// Submits a score to a leaderboard.
    /// </summary>
    /// <param name='leaderboardId'>
    /// Leaderboard identifier.
    /// </param>
    /// <param name='scoreValue'>
    /// Score value.
    /// </param>
    void SubmitScoreToLeaderboard(string leaderboardId, long scoreValue) {
        // Subscribe to the events to receive the score submission status.
        SubscribeToScoreSubmissionEvents();
        // Submit the score update to GameCircle plugin.
        AGSLeaderboardsClient.SubmitScore(leaderboardId,scoreValue); 
    }
    
    /// <summary>
    /// Requests the local player's score.
    /// </summary>
    /// <param name='leaderboardId'>
    /// Leaderboard to request the score for.
    /// </param>
    void RequestLocalPlayerScore(string leaderboardId) {
        // Subscribe to the events to receive the local player's score.
        SubscribeToLocalPlayerScoreRequestEvents();
        // Request the local player's score from the GameCircle plugin.
        AGSLeaderboardsClient.RequestLocalPlayerScore(leaderboardId,leaderboardScoreScope);
    }
    #endregion
    
    #region Callback helper functions
    /// <summary>
    /// Subscribes to leaderboard request events.
    /// </summary>
    private void SubscribeToLeaderboardRequestEvents() {   
        AGSLeaderboardsClient.RequestLeaderboardsFailedEvent += RequestLeaderboardsFailed;
        AGSLeaderboardsClient.RequestLeaderboardsSucceededEvent += RequestLeaderboardsSucceeded;
    }
    
    /// <summary>
    /// Unsubscribes from leaderboard request events.
    /// </summary>
    private void UnsubscribeFromLeaderboardRequestEvents() {   
        AGSLeaderboardsClient.RequestLeaderboardsFailedEvent -= RequestLeaderboardsFailed;
        AGSLeaderboardsClient.RequestLeaderboardsSucceededEvent -= RequestLeaderboardsSucceeded;
    }
    
    /// <summary>
    /// Subscribes to score submission events.
    /// </summary>
    private void SubscribeToScoreSubmissionEvents() {   
        AGSLeaderboardsClient.SubmitScoreFailedEvent += SubmitScoreFailed;
        AGSLeaderboardsClient.SubmitScoreSucceededEvent += SubmitScoreSucceeded;
    }
    
    /// <summary>
    /// Unsubscribes from score submission events.
    /// </summary>
    private void UnsubscribeFromScoreSubmissionEvents() {   
        AGSLeaderboardsClient.SubmitScoreFailedEvent -= SubmitScoreFailed;
        AGSLeaderboardsClient.SubmitScoreSucceededEvent -= SubmitScoreSucceeded;
    }
    
    /// <summary>
    /// Subscribes to local player score request events.
    /// </summary>
    private void SubscribeToLocalPlayerScoreRequestEvents() {   
        AGSLeaderboardsClient.RequestLocalPlayerScoreFailedEvent += RequestLocalPlayerScoreFailed;
        AGSLeaderboardsClient.RequestLocalPlayerScoreSucceededEvent += RequestLocalPlayerScoreSucceeded;
    }
    
    /// <summary>
    /// Unsubscribes from local player score request events.
    /// </summary>
    private void UnsubscribeFromLocalPlayerScoreRequestEvents() {   
        AGSLeaderboardsClient.RequestLocalPlayerScoreFailedEvent -= RequestLocalPlayerScoreFailed;
        AGSLeaderboardsClient.RequestLocalPlayerScoreSucceededEvent -= RequestLocalPlayerScoreSucceeded;
    }
    #endregion
    
    #region Callbacks    
    /// <summary>
    /// Callback for when the leaderboard list request fails.
    /// </summary>
    /// <param name='error'>
    /// Error message.
    /// </param>
    private void RequestLeaderboardsFailed(string error) {
        // Update the status message to show the error.
        requestLeaderboardsStatus = requestLeaderboardsFailedLabel;
        requestLeaderboardsStatusMessage = error;
        // Once the callback is received, these events do not need to be subscribed to.
        UnsubscribeFromLeaderboardRequestEvents();
    }
    
    /// <summary>
    /// Callback for when the leaderboard list request succeeds.
    /// </summary>
    /// <param name='leaderboards'>
    /// Leaderboard list.
    /// </param>
    private void RequestLeaderboardsSucceeded(List<AGSLeaderboard> leaderboards) {
        // Update the status message to show the request has succeeded.
        requestLeaderboardsStatus = requestLeaderboardsSucceededLabel;
        // Store the list of leaderboards.
        leaderboardList = leaderboards;
        // Mark the leaderboards as ready for use. 
        // This bool is used instead of checking if the leaderboard list is null,
        // because the passed in leaderboard list can be null. 
        // In that case, the menu should update to show that leaderboards were received, but empty.
        leaderboardsReady = true;
        // Once the callback is received, these events do not need to be subscribed to.
        UnsubscribeFromLeaderboardRequestEvents();
    }
    
    /// <summary>
    /// Callback for when score submission fails.
    /// </summary>
    /// <param name='leaderboardId'>
    /// Leaderboard identifier.
    /// </param>
    /// <param name='error'>
    /// Error message.
    /// </param>
    private void SubmitScoreFailed(string leaderboardId, string error) {
        // Make sure the leaderboard submission status dictionaries have this leaderboard key added.
        if(!leaderboardsSubmissionStatus.ContainsKey(leaderboardId)) {
            leaderboardsSubmissionStatus.Add(leaderboardId,null);   
        }
        if(!leaderboardsSubmissionStatusMessage.ContainsKey(leaderboardId)) {
            leaderboardsSubmissionStatusMessage.Add(leaderboardId,null);   
        }
        // Update the leaderboard submission status message to show this error.
        leaderboardsSubmissionStatus[leaderboardId] = string.Format(leaderboardFailed, leaderboardId);
        leaderboardsSubmissionStatusMessage[leaderboardId] = error;
        // Once the callback is received, these events do not need to be subscribed to.
        UnsubscribeFromScoreSubmissionEvents();
    }
    
    /// <summary>
    /// Callback for when score submission succeeds.
    /// </summary>
    /// <param name='leaderboardId'>
    /// Leaderboard identifier.
    /// </param>
    private void SubmitScoreSucceeded(string leaderboardId) {
        // Make sure the leaderboard submission status dictionaries have this leaderboard key added.
        if(!leaderboardsSubmissionStatus.ContainsKey(leaderboardId)) {
            leaderboardsSubmissionStatus.Add(leaderboardId,null);   
        }
        // Update the leaderboard submission status message to show this has succeeded.
        leaderboardsSubmissionStatus[leaderboardId] = string.Format(leaderboardSucceeded, leaderboardId);  
        // Once the callback is received, these events do not need to be subscribed to.
        UnsubscribeFromScoreSubmissionEvents();
    }
    
    /// <summary>
    /// Callback for when the request for the local player's score has failed.
    /// </summary>
    /// <param name='leaderboardId'>
    /// Leaderboard identifier.
    /// </param>
    /// <param name='error'>
    /// Error message.
    /// </param>
    private void RequestLocalPlayerScoreFailed(string leaderboardId, string error) {
        // Make sure the local score request status dictionaries have this leaderboard key.
        if(!leaderboardsLocalScoreStatus.ContainsKey(leaderboardId)) {
            leaderboardsLocalScoreStatus.Add(leaderboardId,null);   
        }
        if(!leaderboardsLocalScoreStatusMessage.ContainsKey(leaderboardId)) {
            leaderboardsLocalScoreStatusMessage.Add(leaderboardId,null);   
        }
        // Update the local score request status message to show the error that occured.
        leaderboardsLocalScoreStatus[leaderboardId] = string.Format(leaderboardScoreFailed, leaderboardId);
        leaderboardsLocalScoreStatusMessage[leaderboardId] = error;
        // Once the callback is received, these events do not need to be subscribed to.
        UnsubscribeFromLocalPlayerScoreRequestEvents();
    }
    
    private void RequestLocalPlayerScoreSucceeded(string leaderboardId, int rank, long score) {
        // Make sure the local score request status dictionary has this leaderboard key.
        if(!leaderboardsLocalScoreStatus.ContainsKey(leaderboardId)) {
            leaderboardsLocalScoreStatus.Add(leaderboardId,null);   
        }
        // Update the local score request status to show that the local score request has succeeded,
        // and update the message with the local score and rank.
        leaderboardsLocalScoreStatus[leaderboardId] = string.Format(leaderboardRankScoreLabel, rank, score);  
        // Once the callback is received, these events do not need to be subscribed to.
        UnsubscribeFromLocalPlayerScoreRequestEvents();
    }
    #endregion
}
