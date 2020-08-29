# oktaapientitlements
APIs to get assigned roles/entitlements

There are 4 Endpoints for the entitlements API…

Step 1 - Get all the apps for an org:
 
Entitlement Endpoint:  /api/v1/apps/
Okta Endpoint:  /api/v1/apps/
Additional logic:  Filter unnecessary fields
Return: name, id
Output:
[
   {
      "name":"boxnet",
      "id":"0oa4sr2wxpsdfSRD0x7"
   },
   {
      "name":"google",
      "id":"0oa4ssy6sfdrFY0x7"
   },
   {
      "name":"salesforce",
      "id":"0oa4stkc15sfdql0x7"
   },
…
]
Step 2 - Get all users associated with an appId:
 
Entitlement Endpoint:  /api/v1/users/{appId}/
Okta Endpoint:  /api/v1/apps/{appId}/users/
Additional logic:  Filter unnecessary fields
Return: credentials.userName, id
Output:
[
   {
      "name":"aaa@okta.com",
      "id":"00u4s6fnwxBpDC1rm0x7"
   },
   {
      "name":"aaa@oktapreview.com",
  	"id":"00u4s6fnx1nXSDqUW0x7"
   },
   {
      "name":"somone@gmail.com",
      "id":"00u43ersNs1sj0x7"
   },
   {
      "name":"bbbb@oktatest.net",
      "id":"00u4syoyumftyf9It0x7"
   },
…
] 
Step 3a - Get role info associated with an user for an app:
 
Entitlement Endpoint:  /api/v1/role/{appId}/{userId}/
Okta Endpoint:  /api/v1/apps/{appId}/users/{userId}/
Returns: Id, Role, Profile, Title  
Additional logic:  Filter unnecessary fields
Output:
[
   {
      "id":"00u4ss3ossdfafsj0x7",
      "role":"-- No Role --",
      "profile":"Chatter Free User",
      "title":null
   }
]
 
Step 3b - Get all roles assigned to any user for an app:
 
Entitlement Endpoint:  /api/v1/roles/{appId}/
Okta Endpoint:  /api/v1/apps/{appId}/users/  &  /api/v1/apps/{appId}/users/{userId}/
Additional logic:  Loop through all users in the app for assigned roles, filter unnecessary fields
Returns: Id, Role, Profile, Title  
Output:
[
 {
      "id":null,
      "role":"-- No Role --",
      "profile":"Chatter Free User",
      "title":"VP, Employee Benefits"
   },
   {
      "id":null,
      "role":"-- No Role --",
      "profile":"Chatter Free User",
      "title":"Sales Engineer"
   }
…
]
 

