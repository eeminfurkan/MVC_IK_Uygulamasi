// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using MVC_IK_Uygulamasi.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;

namespace MVC_IK_Uygulamasi.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly PersonelServisi _personelServisi; // <-- 1. ADIM: Servisi ekleyin


        public RegisterModel(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            PersonelServisi personelServisi) // <-- 2. ADIM: Constructor'a parametre olarak ekleyin
        
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _personelServisi = personelServisi; // <-- 3. ADIM: Atamayı yapın

        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        public SelectList PersonelListesi { get; set; } // Personel listesini tutacak


        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }


            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            // --- YENİ EKLENECEK ALAN ---
            [Required(ErrorMessage = "Lütfen bir personel seçin.")]
            [Display(Name = "Bağlanacak Personel")]
            public int PersonelId { get; set; } // Seçilen personelin ID'sini tutacak
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            // --- YENİ EKLENECEK KOD ---
            // Sayfa ilk yüklendiğinde atanmamış personelleri çekip dropdown için hazırlıyoruz.
            var atanmamisPersoneller = await _personelServisi.GetirAtanmamisPersonelleriAsync();
            PersonelListesi = new SelectList(atanmamisPersoneller, "Id", "Ad");
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            // Eğer ModelState geçersiz olursa ve form geri dönerse,
            // Personel listesinin boş kalmaması için listeyi burada yeniden dolduruyoruz.
            var atanmamisPersoneller = await _personelServisi.GetirAtanmamisPersonelleriAsync();
            PersonelListesi = new SelectList(atanmamisPersoneller, "Id", "Ad");

            if (ModelState.IsValid)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    // 1. Yeni kullanıcıya "Personel" rolünü atıyoruz.
                    await _userManager.AddToRoleAsync(user, "Personel");

                    // 2. Formdan gelen PersonelId ile ilgili personeli buluyoruz.
                    var personel = await _personelServisi.PersonelBulAsync(Input.PersonelId);
                    if (personel != null)
                    {
                        // 3. Personelin UserId'sini yeni oluşturulan kullanıcının Id'si ile güncelliyoruz.
                        personel.UserId = user.Id;
                        await _personelServisi.PersonelGuncelleAsync(personel);
                    }

                    // Normalde kayıt olan kullanıcıyı sisteme login eder.
                    // Ama admin yeni bir kullanıcı oluşturduğunda login olmasını istemeyiz.
                    // Bu yüzden direkt olarak admin paneline yönlendireceğiz.
                    if (User.IsInRole("Admin"))
                    {
                        // Admini, kullanıcı listesinin olduğu sayfaya geri yönlendiriyoruz.
                        return RedirectToAction("Index", "Admin", new { area = "" });
                    }

                    // Eğer kayıt olan kişi admin değilse (bu senaryoda pek mümkün değil ama genel bir yapı için durabilir)
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // Eğer bir hata varsa, formu ve doğrulama hatalarını tekrar göster.
            return Page();
        }

        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }
}
