using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OnlineLearningPlatform.Context.Identity;
using OnlineLearningPlatform.ViewModels.Account;

namespace OnlineLearningPlatform.Controllers;

public class AccountController : BaseController
{
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IMapper _mapper;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountController(
        SignInManager<ApplicationUser> signInManager,
        IMapper mapper,
        UserManager<ApplicationUser> userManager,
        IWebHostEnvironment hostingEnvironment
    )
        : base(userManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
        _hostingEnvironment = hostingEnvironment;
    }

    // GET: /Account/Register
    [AllowAnonymous]
    public IActionResult Register()
    {
        // Check if the user is already logged in
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }

    // POST: /Account/Register
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (EmailExists(model.Email))
        {
            // Return a JSON response indicating that the email is already registered
            return Json(new { emailExists = true });
        }

        if (ModelState.IsValid)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Student"); // Default role for new users

                await _signInManager.SignInAsync(user, false);
                return Json(new { success = true });
            }

            AddErrors(result);
        }

        // If we got this far, something failed, redisplay form
        return View(model);
    }

    // GET: /Account/Login
    [AllowAnonymous]
    public IActionResult Login()
    {
        // Check if the user is already logged in
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }

    // POST: /Account/Login
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!EmailExists(model.Email))
        {
            ModelState.AddModelError("Email", "Email Address Not Exist.");
        }

        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(
                model.Email,
                model.Password,
                model.RememberMe,
                false
            );

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            if (result.IsLockedOut)
            {
                return View("Lockout");
            }
            // ModelState.AddModelError("", "");
            // ModelState.AddModelError("Password", "InCorrect Password.");

            return View(model);
        }

        // If we got this far, something failed, redisplay form
        return View(model);
    }

    // POST: /Account/Logout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        // Check if the user is already logged in
        if (User.Identity?.IsAuthenticated == true)
        {
            await _signInManager.SignOutAsync();
        }

        return RedirectToAction("Index", "Home");
    }

    // GET: /Account/Profile
    [Authorize]
    public async Task<IActionResult> Profile()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return RedirectToAction("Index", "Home");

        var model = new ProfileViewModel
        {
            FirstName = user.FirstName,
            Email = user.Email ?? "",
            LastName = user.LastName,
            ProfilePictureUrl = null, // Set to null initially
            ProfilePictureBase64 =
                user.ProfilePictureUrl == null
                    ? null
                    : Convert.ToBase64String(user.ProfilePictureUrl)
        };

        return View(model);
    }

    // POST: /Account/Profile
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Profile(ProfileViewModel model, IFormFile? profilePictureUrl)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return RedirectToAction("Index", "Home");

        if (profilePictureUrl != null)
            using (var memoryStream = new MemoryStream())
            {
                await profilePictureUrl.CopyToAsync(memoryStream);

                // Upload the file if less than 2 MB
                if (memoryStream.Length < 2097152)
                {
                    user.ProfilePictureUrl = memoryStream.ToArray();
                }
                else
                {
                    ModelState.AddModelError("File", "The file is too large.");
                    return View(model);
                }
            }

        user.FirstName = model.FirstName;
        user.LastName = model.LastName;

        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
            return RedirectToAction("Profile");

        foreach (var error in result.Errors)
            ModelState.AddModelError(string.Empty, error.Description);

        return View(model);
    }

    #region Helpers

    // Method to check if the email exists in the database
    private bool EmailExists(string email)
    {
        // Replace this with your actual logic to check if the email exists
        var user = _userManager.FindByEmailAsync(email).Result;
        return user != null;
    }

    private void AddErrors(IdentityResult result)
    {
        foreach (var error in result.Errors)
            ModelState.AddModelError(string.Empty, error.Description);
    }

    private IActionResult RedirectToLocal(string returnUrl)
    {
        if (Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);
        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    #endregion
}
