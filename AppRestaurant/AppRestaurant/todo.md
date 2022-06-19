adminPage.js
	[] To implement: FIND ME
		[] Update for user (only change password and nickname) - only for users, NO ADMIN SHOULD BE 
		[] Add to home page, change nickname.
		[] Add to login page, change password

AutheController.cs
	[] Use 'Claim' instead of 'Session'

DishController.cs [line:54]
	[] Maybe change [HttpPost] to [HttpPatch] on update requests

Create.cshtml
	[] Dish creation. Admin may select to upload a file from his own PC or put a url to image source.
    // A way to choose images from client PC and send them to server images' folder
    // Image is URL will change between simple input field and upload button.
    // ImageId could be bound to the ID of Dish.