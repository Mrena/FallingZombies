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
using System.Collections.Generic;


/// <summary>
/// AGS score.
/// </summary>
public class AGSScore{
	public string playerAlias;
	public int rank;
	public string scoreString;
	public long scoreValue;
	
	
	public static AGSScore fromHashtable( Hashtable ht ){
		var score = new AGSScore();
		score.playerAlias = ht["playerAlias"].ToString();
		score.rank = int.Parse( ht["rank"].ToString() );
		score.scoreString = ht["scoreString"].ToString();
		score.scoreValue = long.Parse( ht["scoreValue"].ToString() );
		
		return score;
	}
	
	
	public static List<AGSScore> fromArrayList( ArrayList list ){
		var scores = new List<AGSScore>();
		
		foreach( Hashtable ht in list ){
			scores.Add( AGSScore.fromHashtable( ht ) );
		}
		
		return scores;
	}
	
	
	public override string ToString(){
		return string.Format( "playerAlias: {0}, rank: {1}, scoreString: {2}", playerAlias, rank, scoreString );
	}
	
}
