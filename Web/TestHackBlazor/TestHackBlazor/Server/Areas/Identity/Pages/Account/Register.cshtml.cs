﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TestHackBlazor.Server.Data;
using TestHackBlazor.Server.Entities;

namespace TestHackBlazor.Server.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        [BindProperty]
        public List<SelectListItem> Professions { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "{0} - обязательное поле.")]
            [Display(Name = "Имя")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "{0} - обязательное поле.")]
            [Display(Name = "Фамилия")]
            public string SecondName { get; set; }

            [Display(Name = "Отчество")]
            public string Patronymic { get; set; }

            [Required(ErrorMessage = "{0} - обязательное поле.")]
            [EmailAddress(ErrorMessage = "Невалидная почта.")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "{0} - обязательное поле.")]
            [Phone(ErrorMessage = "Невалидный номер телефона.")]
            [Display(Name = "Номер телефона")]
            public string PhoneNumber { get; set; }

            [Required(ErrorMessage = "{0} - обязательное поле.")]
            [Display(Name = "Профессия")]
            public long ProfessionId { get; set; }

            [Required(ErrorMessage ="{0} - обязательное поле.")]
            [StringLength(100, ErrorMessage = "{0} должен быть длиной от {2} до {1} символов.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Пароль")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Подтвердите пароль")]
            [Compare("Password", ErrorMessage = "Пароли не совпадают.")]
            public string ConfirmPassword { get; set; }

            [Display(Name = "Cогласие на сбор и обработку персональных данных.")]
            [Range(typeof(bool), "true", "true", ErrorMessage = "Обязательное соглашение сервиса.")]
            public bool TermsAndConditions { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;

            Professions = await LoadProfessions();

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser 
                { 
                    FirstName = Input.FirstName,
                    SecondName = Input.SecondName,
                    Patronymic = Input.Patronymic,
                    PhoneNumber = Input.PhoneNumber,
                    UserName = Input.Email, 
                    Email = Input.Email ,
                    ProfessionId = Input.ProfessionId
                };

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            Professions = await LoadProfessions();

            return Page();
        }

        private async Task<List<SelectListItem>> LoadProfessions()
        {
            var profs = await _context.Professions
                .AsNoTracking()
                .OrderBy(p => p.Id)
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Title
                })
                .ToListAsync();

            var otherProf = profs.First();
            profs.Remove(otherProf);
            profs = profs.OrderBy(p => p.Text).ToList();
            profs.Add(otherProf);

            return profs;
        }
    }
}
