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
/// AGS leaderboard.
/// </summary>
public class AGSLeaderboard{
	public string name;
	public string id;
	public string displayText;
	public string scoreFormat;
	public List<AGSScore> scores = new List<AGSScore>();
	
	public static AGSLeaderboard fromHashtable( Hashtable ht ){
		var board = new AGSLeaderboard();
		board.name = ht["name"].ToString();
		board.id = ht["id"].ToString();
		board.displayText = ht["displayText"].ToString();
		board.scoreFormat = ht["scoreFormat"].ToString();
		
		// handle scores if we have them
		if( ht.ContainsKey( "scores" ) && ht["scores"] is ArrayList )
		{
			var scoresList = ht["scores"] as ArrayList;
			board.scores = AGSScore.fromArrayList( scoresList );
		}
		
		return board;
	}
	
	
	public override string ToString(){
		return string.Format( "name: {0}, id: {1}, displayText: {2}, scoreFormat: {3}", name, id, displayText, scoreFormat );
	}
	
}
