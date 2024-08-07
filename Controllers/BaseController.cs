using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OnlineLearningPlatform.Context.Identity;

namespace OnlineLearningPlatform.Controllers;

public class BaseController : Controller {
    private readonly UserManager<ApplicationUser> _userManager;

    public BaseController(UserManager<ApplicationUser> userManager) {
        _userManager = userManager;
    }

    // Override the method with the same access modifier
    public override void OnActionExecuting(ActionExecutingContext context) {
        base.OnActionExecuting(context);

        if (User.Identity != null && User.Identity.IsAuthenticated) {
            var user = _userManager.GetUserAsync(User).Result;
            if (user != null) {
                // Convert the byte array to a base64-encoded string
                var base64String = user.ProfilePictureUrl == null
                    ? null
                    : Convert.ToBase64String(user.ProfilePictureUrl);

                ViewData["ProfilePictureUrl"] = base64String;
                ViewData["FirstName"] = user.FirstName;
                ViewData["LastName"] = user.LastName;
            }
        }
    }
}