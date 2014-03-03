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
using System.ComponentModel;

/// <summary>
/// AGS profile.
/// </summary>
public class AGSProfile{
    /// <summary>
    /// player alias.
    /// </summary>
    public readonly string alias;
    /// <summary>
    /// The player identifier.
    /// </summary>
    public readonly string playerId;
    
    #region public static functions
    /// <summary>
    /// creates a new instance of the <see cref="AGSProfile"/> class from a hashtable
    /// </summary>
    /// <returns>
    /// A <see cref="AGSProfile"/> class created from the passed in hashtable.
    /// </returns>
    /// <param name='hashtable'>
    /// hashtable.
    /// </param>
    public static AGSProfile fromHashtable( Hashtable profileDataAsHashtable ){
        if(null == profileDataAsHashtable) { 
            return null;
        }
        return new AGSProfile(getStringValue(profileDataAsHashtable,"alias"),
                                    getStringValue(profileDataAsHashtable,"playerId"));
    }
    #endregion
    
    #region private static functions
    /// <summary>
    /// Gets the string value.
    /// </summary>
    /// <returns>
    /// The string value.
    /// </returns>
    /// <param name='hashtable'>
    /// hashtable.
    /// </param>
    /// <param name='key'>
    /// Key.
    /// </param>
    private static String getStringValue(Hashtable hashtable, String key){
        if(null == hashtable) {
            return null;
        }
        if(hashtable.Contains(key)){
            return hashtable[key].ToString();
        }
        return null;
    }        
    #endregion
    
    #region private constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="AGSProfile"/> class.
    /// This constructor is private because this class should only be 
    /// instantiated through fromHashtable
    /// </summary>
    private AGSProfile() {
        alias = null;
        playerId = null;
        AGSClient.LogGameCircleError("AGSProfile was instantiated without valid playerId and alias information.");
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="AGSProfile"/> class.
    /// This constructor is private because this class should only be 
    /// instantiated through fromHashtable
    /// </summary>
    /// <param name='alias'>
    /// Alias.
    /// </param>
    /// <param name='playerId'>
    /// Player identifier.
    /// </param>
    private AGSProfile(string alias, string playerId) {
        this.alias = alias;
        this.playerId = playerId;
    }
    #endregion
            
    #region overrides
    /// <summary>
    /// Returns a <see cref="System.String"/> that represents the current <see cref="AGSProfile"/>.
    /// </summary>
    /// <returns>
    /// A <see cref="System.String"/> that represents the current <see cref="AGSProfile"/>.
    /// </returns>
    public override string ToString(){
        return string.Format( "alias: {0}, playerId: {1}",
            alias, playerId);
    }
    #endregion
    
}
