
module.exports = function (mongoose) {

    var FriendsSchema = new mongoose.Schema({
            username : {type : String, unique : true}
    });

    var Friends = mongoose.model("Friends",FriendsSchema);

    var AccountSchema = new mongoose.Schema({
        email_address: { type: String, unique: true },
        username: { type: String, unique: true },
        password: { type: String },
        presence: { type: String, default: "offline" },
        friends: [Friends],
        friendInvites: [Friends],
        name: {
            first: { type: String },
            last: { type: String }
        },
        birthday: {
            day: { type: Number, min: 1, max: 31, required: false },
            month: { type: Number, min: 1, max: 31, required: false },
            year: { type: Number }
        },
        sex: { type: String },
        city: { type: String },
        country : { type : String},
        photoData : { type : String },
        biography : { type : String }
    });

   var Account = mongoose.model('Account', AccountSchema);
   
   this.login = function (_username,_password,response) {
       
       try {

           Account.findOne({ username: _username, password: _password }, function(err, doc) {
               if (err == null && doc != null) {
                   console.log("Login success");
                   response.end("true");
               } else {
                   console.log("Login error "+ err);
                   response.end("false");
               }
           });

       } catch (e) {
           
           console.log("Error : " + e);
           response.end("false");
       }
   };


   this.register = function (_username,_password,_email_address,response) {
       console.log("In register function");
       try {

           var user = new Account({
               username: _username,
               password: _password,
               email_address: _email_address
           });

           user.save(function (err) {
               if (err) {
                   console.log("Error : " + err);
                   response.end("false");
               } else {
                   console.log("Success!");
                   response.end("true");
               }
               console.log("In save function");
           });
 
       } catch (e) {
           response.end("false");
           console.log("Error : "+e);
       }
   };


   this.resetPassword = function (email_address,response) {


       response.end();
   };

   this.setPresence = function (_username,_presense,response) {

       Account.findOne({ username : _username }, function (err,doc) {
           if (err == null && doc != null) {
               doc.presence = _presense;
               doc.save(function(err) {
                   if (err == null) {
                       
                       response.end("true");
                       
                   } else {
                       
                       console.log("Error : "+err);
                       response.end("false");
                   }
               });
           } else {
               
               console.log("Error : "+err);
               response.end("false");
               
           }
       });

   };

   this.setInvite = function (inviter,invitee,response) {

       Account.findOne({username : invitee}, function(err,doc) {
           if (err == null && doc != null) {
               var hasAlreadyInvited = false;
               doc.friendInvites.forEach(function(friendInvite) {
                   if (inviter == friendInvite) {
                       hasAlreadyInvited = true;
                       
                   }
                   
               });
               var isAlreadyFriend = false;
               doc.friends.forEach(function(friend) {
                   if (inviter == friend) {
                       isAlreadyFriend = true;
                      
                   }
               });


               if (!hasAlreadyInvited && !isAlreadyFriend) {
                   doc.friendInvites.push({ username: inviter });
                   doc.save(function(err) {
                       if (err != null) {
                           response.end("true");
                       } else {
                           console.log("Error : " + err);
                           response.end("false");
                       }
                   });
               }

           } else {
               console.log("Error : "+err);
               response.end("false");
           }
       });

   };

   this.acceptInvite = function(invitee,inviter,response) {

       Account.findOne({username : invitee},function(err,doc) {
           if (err == null && doc != null) {
               for (var i = 0; i < doc.friendInvites.length; i++) {
                   if (doc.friendInvites[i].username == inviter) {
                       doc.friends.push({ username: inviter });
                       doc.friendInvites[i].remove();
                       doc.save(function(err) {
                           if (err == null) {
                               response.end("true");
                           } else {
                               console.log("Error : " + err);
                               response.end("false");
                           }

                       });
                   }
               }
           } else {
               console.log("Error : "+err);
               response.end("false");
           }
       });

   };
    
    
   this.declineInvite = function(invitee,inviter,response) {

       Account.findOne({username : invitee},function(err,doc) {
           if (err == null && doc != null) {
               for (var i = 0; i < doc.friendInvites.length; i++) {
                   if (doc.friendInvites[i].username == inviter) {
                       doc.friendInvites[i].remove();
                       doc.save(function(err) {
                           if (err == null) {
                               response.end("true");
                           } else {
                               console.log("Error : " + err);
                               response.end("false");
                           }
                       });
                   }
               }
           } else {
               console.log("Error : "+err);
               response.end("false");
           }
       });

   };

    this.removeFriend = function(_username,friend,response) {

        Account.findOne({username : _username},function(err,doc) {
            if (err == null && doc != null) {
                for (var i = 0; i < doc.friends.length; i++) {
                    if (doc.friends[i].username == friend) {
                        doc.friends[i].remove();
                        doc.save(function(err) {
                            if (err != null) {
                                console.log("Error : " + err);
                                response.end("false");
                            }
                        });
                    }
                }
            } else {
                console.log("Error : "+err);
                response.end("false");
            }
        });
        
        Account.findOne({username : friend},function(err,doc) {
            if (err == null && doc != null) {
                for (var i = 0; i < doc.friends.length; i++) {
                    if (doc.friends[i].username == username) {
                        doc.friends[i].remove();
                        doc.save(function(err) {
                            if (err != null) {
                                console.log("Error : " + err);
                                response.end("false");
                            } else {
                                response.end("true");
                            }
                        });
                    }
                }
            } else {
                console.log("Error : "+err);
                response.end("false");
            }
        });

    };

    this.getUserFriends = function(_username,response) {

        Account.findOne({username : _username},function(err,doc) {
            if (err == null && doc != null) {
                console.log("friends about to be returned "+response);
                response.end(JSON.stringify(doc.friends));
            } else {
                console.log("An error occured while trying to get the user's friends list");
                console.log("Error : " + err.toString());
                response.end("false");
            }
        });


    };
    
    this.getUserFriendsInvites = function (_username, response) {

        Account.findOne({ username: _username }, function (err, doc) {
            if (err == null && doc != null) {
                console.log("friends invites about to be returned "+response);
                response.end(JSON.stringify(doc.friendInvites));
            } else {
                console.log("An error occured while trying to get the user's friends invite list");
                console.log("Error : " + err.toString());
                response.end("false");
            }
        });
    };


    this.getUserProfile = function (_username, response) {

        var userProfileInfo = {
            "firstName" : "TestName" ,
            "lastName" : "TestLastName",
            "birthday" : "TestBirthday",
            "sex" : "TestSex",
            "city" : "TestCity",
            "country" : "TestCountry",
            "biography" : "TestBiography",
            "photoData" : "TestPhoto"
        };

        Account.findOne({ username : _username},function(err,doc) {
            if (err == null && doc != null) {

                userProfileInfo.firstName = doc.name.first;
                userProfileInfo.lastName = doc.name.last;
                userProfileInfo.birthday += doc.birthday.day;
                userProfileInfo.birthday += doc.birthday.month;
                userProfileInfo.birthday += doc.birthday.year;
                userProfileInfo.sex = doc.sex;
                userProfileInfo.city = doc.city;
                userProfileInfo.country = doc.country;
                userProfileInfo.photoData = doc.photoData;

                response.end(JSON.stringify(userProfileInfo));

            } else {
                console.log("Error getting user profile");
                console.log("Error : " + err.toString());
                response.end("false");
            }
        });

    };

    return this;
};