module.exports = function (self,mongoose) {
    var Account = require("../models/Account")(mongoose);

    self.routes['/account/login/:username?/:password?'] = function (request,response) {

        if (request.param("username") && request.param("password")) {
            Account.login(request.param("username"), request.param("password"),response);
        } else {
            console.log("Username or password has not been set " + request.param("username") + " " + request.param("password"));
            response.end("false");
        }

    };

    self.routes['/account/register/:username?/:password?/:email_address?'] = function (request, response) {
        console.log("request : "+request," response : "+response);
        if (request.param("username") && request.param("password") && request.param("email_address")) {
            Account.register(request.param("username"), request.param("password"), request.param("email_address"),response);
        } else {
            console.log("Username, password or email address has not been set " + request.param("username") + " " + request.param("password") + " " + request.param("email_address"));
            response.end("false");
        }

    };

    self.routes['/account/resetPassword/:email_address?'] = function (request,response) {
       
        if (request.param("email_address")) {
            Account.resetPassword(request.param("email_address"),response);
        } else {
            console.log("Email address has not been set " + request.param("email_address"));
            response.end("false");
        }
    };
    

    self.routes['/account/setPresence/:username?/:presence?'] = function (request,response) {

        if (request.param("presence") && request.param("username")) {
            Account.setPresence(request.param("username"),request.param("presense"),response);
        } else {
            console.log("username or presense is not set " + request.param("username") +" "+ request.param("presense"));
            response.end("false");
        }

    };
    


    self.routes['/account/:inviter?/invites/:invitee?'] = function (request,response) {

        if (request.param("inviter") && request.param("invitee")) {
            Account.setInvite(request.param("inviter"),request.param("invitee"),response);
        } else {
            console.log("inviter or invitee is not set " + request.param("inviter") + " " + request.param("invitee"));
            response.end("false");
        }

    };

    self.routes['/account/:invitee?/accepts/:inviter?'] = function(request,response) {
        if (request.param("invitee") && request.param("inviter")) {
            Account.acceptInvite(request.param("invitee"),request.param("inviter"),response);
        } else {
            console.log("invitee or inviter is not set " + request.param("invitee") + " " + request.param("inviter"));
            response.end("false");
        }
    };
    
    self.routes['/account/:invitee?/declines/:inviter?'] = function (request, response) {
        if (request.param("invitee") && request.param("inviter")) {
            Account.declineInvite(request.param("invitee"), request.param("inviter"), response);
        } else {
            console.log("invitee or inviter is not set " + request.param("invitee") + " " + request.param("inviter"));
            response.end("false");
        }
    };

    self.routes['/account/:username?/removeFriend/:friend?'] = function (request, response) {
        
        if (request.param("username") && request.param("friend")) {
            Account.removeFriend(request.param("username"),request.param("friend"),response);
        } else {
            console.log("username or friend is not set " + request.param("username") + " " + request.param("friend"));
            response.end("false");
        }

    };

    self.routes['/account/friends/:username?'] = function (request, response) {

        if (request.param("username")) {
            Account.getUserFriends(request.param("username"),response);
        } else {
            console.log("username is not set " + request.param("username"));
            response.end("false");
        }


    };
    

    self.routes['/account/friends/invites/:username?'] = function (request, response) {

        if (request.param("username")) {
            Account.getUserFriendsInvite(request.param("username"),response);
        } else {
            console.log("username is not set " + request.param("username"));
            response.end("false");
        }


    };

    self.routes['/account/profile/:username?'] = function(request,response) {

        if (request.param("username")) {
            Account.getUserProfile(request.param("username"),response);
        } else {
            console.log("username is not set " + request.param("username"));
            response.end("false");
        }


    };

};