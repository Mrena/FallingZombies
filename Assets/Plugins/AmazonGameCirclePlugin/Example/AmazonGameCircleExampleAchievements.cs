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
/// Amazon GameCircle example implementation of Achievement functionality.
/// </summary>
public class AmazonGameCircleExampleAchievements : AmazonGameCircleExampleBase {
    
    #region Achievements Variables
    // Key : Achievement ID, value: achievement submission status
    Dictionary<string,string> achievementsSubmissionStatus = new Dictionary<string, string>();
    // Key: Achievement ID, value: achievement submission message (any errors that occured)
    Dictionary<string,string> achievementsSubmissionStatusMessage = new Dictionary<string, string>();
    // Key: Achievement ID, value: if the achievement is folded out.
    Dictionary<string,bool> achievementsFoldout = new Dictionary<string, bool>();
    // Status messages for showing the status of achievement retrieval
    string requestAchievementsStatus = null;
    string requestAchievementsStatusMessage = null;
    // the list of achievements received from the GameCircle plugin.
    List<AGSAchievement> achievementList = null;
    // have the achievements been received from the GameCircle plugin?
    // used in case a null list was received from the success callback.
    bool achievementsReady = false;
    // 
    // What time achievements were requested at.
    // Used to show a timer to show the menu is active, and is just waiting on a callback.
    System.DateTime achievementsRequestTime;
    // used to test that GameCircle fails gracefully with invalid achievement information
    AGSAchievement invalidAchievement;
    #endregion
    
    #region UI variables
    // define the regex once here to save on re-creating the regex
	// This regex is intended to add a newline after every third comma in a string.
    // See function addNewlineAfterThirdComma for full implementation of this regex.
    readonly System.Text.RegularExpressions.Regex addNewlineEveryThirdCommaRegex = new System.Text.RegularExpressions.Regex(@"(,([^,]+,[^,]+),)");
    // The group number to use with the regex for keeping the text between commas.
    const int betweenCommaRegexGroup = 2;
    #endregion
    
    #region Local const strings
    // The title of this menu
    private const string achievementsMenuTitle = "Achievements";
    // The label for the button that opens the achievements overlay
    private const string displayAchievementOverlayButtonLabel = "Achievements Overlay";
    // The label for showing individual achievements in the UI. {0} is the achievement ID
    private const string achievementProgressLabel = "Achievement \"{0}\"";
    // The label for the button to submit any progress updates for achievements.
    private const string submitAchievementButtonLabel = "Submit Achievement Progress";
    // Labels for showing the status of achievement progress submission after it completes.
    private const string achievementFailedLabel = "Achievement \"{0}\" failed with error:";
    private const string achievementSucceededLabel = "Achievement \"{0}\" uploaded successfully.";
    // displays the achievement progress as a percentage value.
    private const string achievementPercent = "{0}%";
    // The label for the button to request achievement information.
    private const string requestAchievementsButtonLabel = "Request Achievements";
    // The status message to display that achievement retrieval has begun.
    private const string requestingAchievementsLabel = "Requesting Achievements...";
    // The label to display if achievement retrieval fails.
    private const string requestAchievementsFailedLabel = "Request Achievements failed with error:";
    // The label to display if achievement retrieval succeeds.
    private const string requestAchievementsSucceededLabel = "Available Achievements";
    // The label to display if the list of retrieved achievements was empty.
    private const string noAchievementsAvailableLabel = "No Achievements Available";
    // The label to display how long it has been since achievements were requested.
    private const string achievementRequestTimeLabel = "{0,5:N1} seconds";
    // The label to display that information submission has began for an achievement.    
    private const string submittingInformationString = "Submitting Achievement..."; 
	// If the update achievements callback is triggered with a null achievement ID, this error is displayed.
	private const string updateAchievementsReturnedMissingAchievementId = "AmazonGameCircleExampleAchievements received GameCircle plugin callback with invalid achievement ID.";
	// If the error string passed in to the update achievements callback is null or empty, this is the error it is replaced with.
	private const string noErrorMessageReceived = "MISSING ERROR STRING";	
    #endregion
    
    #region local const values (non-string)
    // The achievement system should be able to handle negative values, this helps test that.
    private const float achievementMinValue = -200;
    // achievements are "complete" at 100.0f. A max value of 200 allows us to test and make sure achievement submission does not fail over 100%
    private const float achievementMaxValue = 200;
    #endregion
    
    #region constructor
    /// <summary>
    /// Initializes a new instance of the <see cref="AmazonGameCircleExampleAchievements"/> class.
    /// </summary>
    public AmazonGameCircleExampleAchievements() {
        // This is used to test the GameCircle plugin's behaviour with invalid achievement information.
        // This achievement ID should not be in your game's list, to test what happens with invalid achievements.
        invalidAchievement = new AGSAchievement();
        invalidAchievement.title = "Invalid Achievement Title";
        invalidAchievement.id = "Invalid Achievement ID"; 
        invalidAchievement.progress = 0;
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
        return achievementsMenuTitle;
    }
    
    /// <summary>
    /// Draws the Achievements menu. Note that this must be called from an OnGUI function.
    /// </summary>
    public override void DrawMenu() {     
		// This button will open the achievements overlay.
        if(GUILayout.Button(displayAchievementOverlayButtonLabel)) {
            AGSAchievementsClient.ShowAchievementsOverlay();
        }
        
		// If achievement information retrieval has not begun,
		// display a button to begin the functionality.
        if(string.IsNullOrEmpty(requestAchievementsStatus)) {
			// This button will begin retrieval of achievement information.
            if(GUILayout.Button(requestAchievementsButtonLabel)) {
                RequestAchievements();
            }
        }
        else {
            // once a request has been made for the list of achievements,
            // display the status message of that process.
            AmazonGameCircleExampleGUIHelpers.CenteredLabel(requestAchievementsStatus);
            if(!string.IsNullOrEmpty(requestAchievementsStatusMessage)) {
                AmazonGameCircleExampleGUIHelpers.CenteredLabel(requestAchievementsStatusMessage);
            }
            
            // If the achievements are not ready, display how long it has been since the request was put in
			// to make it clear this is an asynchronous operation.
            if(!achievementsReady) {
                AmazonGameCircleExampleGUIHelpers.CenteredLabel(string.Format(achievementRequestTimeLabel,(System.DateTime.Now - achievementsRequestTime).TotalSeconds));
            }
            // once achievement retrieval is successful, display the list of achievements.
            else {
                // If the achievement list was not empty, display it.
                if(null != achievementList && achievementList.Count > 0) {
                    foreach(AGSAchievement achievement in achievementList) {
                        DisplayAchievement(achievement);
                    }
                }
                else {
                    // You will only see this message if there is no list of achievements.
                    // This happens in two cases:
                    //      You have not added any achievements to your project on the GameCircle website.
                    //      You only have draft achievements, and the user you are testing with cannot see draft achievements.
                    AmazonGameCircleExampleGUIHelpers.CenteredLabel(noAchievementsAvailableLabel);   
                }
                // display an "invalid" achievement to ensure that GameCircle handles invalid data properly.
                if(null != invalidAchievement) {
                    DisplayAchievement(invalidAchievement);   
                }
            }
        }
    }
    #endregion
    
    #region UI functions
    /// <summary>
    /// Displays an individual achievement.
    /// </summary>
    /// <param name='achievement'>
    /// The achievement to display.
    /// </param>
    void DisplayAchievement(AGSAchievement achievement) {
        // Place a box around each achievement, to make it clear in the UI
        // what controls are for what achievement.
        GUILayout.BeginVertical(GUI.skin.box);
        
        // If this achievement has not been added to the foldout dictionary, add it.
        if(!achievementsFoldout.ContainsKey(achievement.id)) {
            achievementsFoldout.Add(achievement.id,false);
        }
        
        // Display a foldout for this achievement.
        // Foldouts keep the menu tidy.
        achievementsFoldout[achievement.id] = AmazonGameCircleExampleGUIHelpers.FoldoutWithLabel(achievementsFoldout[achievement.id],string.Format(achievementProgressLabel,achievement.id));
        
        // If the foldout is open, display the achievement information.
        if(achievementsFoldout[achievement.id]) {
            // The controls for automatically word wrapping a label are not great,
            // so replacing every third comma in the string returned from the achievement's toString function
            // will allow for a cleaner display of each achievement's data
            AmazonGameCircleExampleGUIHelpers.AnchoredLabel(AddNewlineEveryThirdComma(achievement.ToString()),TextAnchor.UpperCenter);
            
            // if this achievement has no pending progress submissions, display information to submit an update.
            if(!achievementsSubmissionStatus.ContainsKey(achievement.id) || string.IsNullOrEmpty(achievementsSubmissionStatus[achievement.id])) {
                
                // Display a centered slider, with the minimum value on the left, and maximum value on the right.
                // This lets the user select a value for the achievement's progress.
                achievement.progress = AmazonGameCircleExampleGUIHelpers.DisplayCenteredSlider(achievement.progress,achievementMinValue,achievementMaxValue,achievementPercent);
                
                // This button submits an update to the achievement's progress to the GameCircle plugin.
                if(GUILayout.Button(submitAchievementButtonLabel)) {
                    SubmitAchievement(achievement.id,achievement.progress);
                }
            }
            else {
                // If the achievement update is in the process of being submitted, display the submission status.
                AmazonGameCircleExampleGUIHelpers.CenteredLabel(achievementsSubmissionStatus[achievement.id]);
                
                if(achievementsSubmissionStatusMessage.ContainsKey(achievement.id) && !string.IsNullOrEmpty(achievementsSubmissionStatusMessage[achievement.id])) {
                    AmazonGameCircleExampleGUIHelpers.CenteredLabel(achievementsSubmissionStatusMessage[achievement.id]);
                }
            } 
        }
        GUILayout.EndVertical();
    }
    
    /// <summary>
    /// Adds a newline after every third comma in a string.
    /// It is difficult to decipher the intent of a regex from its definition,
    /// Containing the behavior of the regex within these functions makes the intent clear.
    /// </summary>
    /// <returns>
    /// a string similar to the passed in string, but with a newline after every third comma.
    /// </returns>
    /// <param name='stringToChange'>
    /// String to change.
    /// </param>
    string AddNewlineEveryThirdComma(string stringToChange) {
        return addNewlineEveryThirdCommaRegex.Replace(    stringToChange, 
                                                        (regexMatchEvaluator) => string.Concat(",",regexMatchEvaluator.Groups[betweenCommaRegexGroup].Value,",\n"));
    }
    #endregion
    
    #region GameCircle plugin functions
    /// <summary>
    /// Requests the list of Achievements from the GameCircle plugin.
    /// </summary>
    void RequestAchievements() {
        // start the clock, to track the progress of this async operation
        achievementsRequestTime = System.DateTime.Now;
        // subscribe to the events to receive the achievement list.
        SubscribeToAchievementRequestEvents();
        // request the achievement list from the GameCircle plugin.
        AGSAchievementsClient.RequestAchievements();
        // set the request status message to show that achievement retrieval has begun.
        requestAchievementsStatus = requestingAchievementsLabel;
    }
    
    /// <summary>
    /// Submits an achievement progress update to the GameCircle plugin.
    /// </summary>
    /// <param name='achievementId'>
    /// Achievement identifier.
    /// </param>
    /// <param name='progress'>
    /// Progress. At 100, achievement is unlocked.
    /// </param>
    void SubmitAchievement(string achievementId, float progress) {
        // Subscribe to the events to receive the achievement submission status message
        SubscribeToSubmitAchievementEvents();
        // Submit the achievement update to the GameCircle plugin.
        AGSAchievementsClient.UpdateAchievementProgress(achievementId,progress);  
        // Make sure the submission status dictionary has a key for this achievement ID.
        if(!achievementsSubmissionStatus.ContainsKey(achievementId)) {
            achievementsSubmissionStatus.Add(achievementId,null);   
        }
        // Update the status of this achievement to show submission has begun.
        achievementsSubmissionStatus[achievementId] = string.Format(submittingInformationString);
    }
    #endregion
    
    #region Callback helper functions
    /// <summary>
    /// Subscribes to achievement request events.
    /// </summary>
    private void SubscribeToAchievementRequestEvents() {
        AGSAchievementsClient.RequestAchievementsFailedEvent += RequestAchievementsFailed;
        AGSAchievementsClient.RequestAchievementsSucceededEvent += RequestAchievementsSucceeded;        
    }
    
    /// <summary>
    /// Unsubscribes from achievement request events.
    /// </summary>
    private void UnsubscribeFromAchievementRequestEvents() {
        AGSAchievementsClient.RequestAchievementsFailedEvent -= RequestAchievementsFailed;
        AGSAchievementsClient.RequestAchievementsSucceededEvent -= RequestAchievementsSucceeded;        
    }
    
    /// <summary>
    /// Subscribes to achievement submission events.
    /// </summary>
    private void SubscribeToSubmitAchievementEvents() {
        AGSAchievementsClient.UpdateAchievementFailedEvent += UpdateAchievementsFailed;
        AGSAchievementsClient.UpdateAchievementSucceededEvent += UpdateAchievementsSucceeded;        
    }
    
    /// <summary>
    /// Unsubscribes from achievement submission events.
    /// </summary>
    private void UnsubscribeFromSubmitAchievementEvents() {
        AGSAchievementsClient.UpdateAchievementFailedEvent -= UpdateAchievementsFailed;
        AGSAchievementsClient.UpdateAchievementSucceededEvent -= UpdateAchievementsSucceeded;
    }
    #endregion

    #region Callbacks
    /// <summary>
    /// Callback received when the request for the list of achievements has failed.
    /// </summary>
    /// <param name='error'>
    /// The error message.
    /// </param>
    private void RequestAchievementsFailed(string error) {
        // Update the status to show the failure message, and the error.
        requestAchievementsStatus = requestAchievementsFailedLabel;
        requestAchievementsStatusMessage = error;
        // Once the callback is received, these events do not need to be subscribed to.
        UnsubscribeFromAchievementRequestEvents();
    }
    
    /// <summary>
    /// Callback received when the request for the list of achievements has succeeded.
    /// </summary>
    /// <param name='achievements'>
    /// The list of achievements.
    /// </param>
    private void RequestAchievementsSucceeded(List<AGSAchievement> achievements) {
        // Update the status message to success.
        requestAchievementsStatus = requestAchievementsSucceededLabel;
        // Store the list of achievements locally.
        achievementList = achievements;
        // Mark the achievements as ready for use. 
        // This bool is used instead of checking if the achievement list is null,
        // because the passed in achievement list can be null. 
        // In that case, the menu should update to show that achievements were received, but empty.
        achievementsReady = true;
        // Once the callback is received, these events do not need to be subscribed to.
        UnsubscribeFromAchievementRequestEvents();
    }
    
    /// <summary>
    /// Callback received when the submission of an achievement update has failed.
    /// </summary>
    /// <param name='achievementId'>
    /// Achievement identifier.
    /// </param>
    /// <param name='error'>
    /// The error message.
    /// </param>
    private void UpdateAchievementsFailed(string achievementId, string error) {
		if(string.IsNullOrEmpty(achievementId)) {
			Debug.LogError(updateAchievementsReturnedMissingAchievementId);	
			return;
		}
		if(string.IsNullOrEmpty(error)) {
			error = noErrorMessageReceived;	
		}
        // make sure that keys exist in the dictionaries tracking achievement submission status.
        if(!achievementsSubmissionStatus.ContainsKey(achievementId)) {
            achievementsSubmissionStatus.Add(achievementId,null);   
        }
        if(!achievementsSubmissionStatusMessage.ContainsKey(achievementId)) {
            achievementsSubmissionStatusMessage.Add(achievementId,null);   
        }
        // Update the achievement submission status to show the error.
        achievementsSubmissionStatus[achievementId] = string.Format(achievementFailedLabel, achievementId);
        achievementsSubmissionStatusMessage[achievementId] = error;
    }
    
    /// <summary>
    /// Callback received when the submission of an achievement update has succeeded.
    /// </summary>
    /// <param name='achievementId'>
    /// Achievement identifier.
    /// </param>
    private void UpdateAchievementsSucceeded(string achievementId) {
        // make sure that keys exist in the dictionaries tracking achievement submission status.
        if(!achievementsSubmissionStatus.ContainsKey(achievementId)) {
            achievementsSubmissionStatus.Add(achievementId,null);   
        }
        // Update the status message to show that achievement submission was successful.
        achievementsSubmissionStatus[achievementId] = string.Format(achievementSucceededLabel, achievementId);  
    }
    #endregion
    
}
