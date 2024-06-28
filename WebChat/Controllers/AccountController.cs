using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebChat.Models;
using WebChat.Models.View;

namespace WebChat.Controllers
{
    public class AccountController : Controller
	{
		private readonly UserManager<TUser> _userManager;
		public AccountController(UserManager<TUser> userManager) 
		{
			_userManager = userManager;
		}
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginForm form)
		{
			if (!ModelState.IsValid)
			{
				return View(form);
			}
			var user=await _userManager.FindByNameAsync(form.Nickname);
			if (user == null)
			{
				ModelState.AddModelError("User", "Wrong password or nickname don't found");
				return View();
			}
			var check = await _userManager.CheckPasswordAsync(user, form.Password);
			if (!check)
			{
				ModelState.AddModelError("User", "Wrong password or nickname don't found");
				return View();
			}
			var identity=new ClaimsIdentity(IdentityConstants.ApplicationScheme);
			identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.RowKey));
			identity.AddClaim(new Claim(ClaimTypes.Name,user.Nickname));
			await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, new ClaimsPrincipal(identity));
			return Redirect("/");
		}
		public IActionResult Register()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Register(RegisterForm form)
		{
			if (!ModelState.IsValid)
			{
				return View(form);
			}
			var user = new TUser()
			{
				Nickname = form.Nickname
			};
			var res=await _userManager.CreateAsync(user,form.Password);
			if (!res.Succeeded)
			{
				ModelState.AddModelError(
					nameof(form.Password),
					string.Join("; ", res.Errors.ToList().Select(e => e.Description))
					);
				return View(form);
			}
			return await Login(new LoginForm()
			{
				Nickname=form.Nickname,
				Password=form.Password,
			});
		}
		public IActionResult LogOut()
		{
			HttpContext.SignOutAsync();
			return RedirectToAction("Login");
		}
	}
}
