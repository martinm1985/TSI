using Crud.Data;
using Crud.DTOs;
using Crud.Models;
using Crud.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.IdentityModel.Protocols;
using System.Web;

namespace Crud.Services
{
    public class IdentityService : IIdentityService
    {

        private readonly UserManager<User> _userManager;
        private readonly JWTSettigs _jwtSetting;
        private readonly TokenValidationParameters _tokenValidationParameter;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public IdentityService (RoleManager<IdentityRole>  roleManager, IConfiguration configuration, UserManager<User> userManager, JWTSettigs jwtSetting, TokenValidationParameters tokenValidationParameter, ApplicationDbContext context, IMapper mapper )
        {
            _userManager = userManager;
            _jwtSetting = jwtSetting;
            _tokenValidationParameter = tokenValidationParameter;
            _context = context;
            _mapper = mapper;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<object> RegisterUser(string username, string email, string password, string name, string surname)
        {
            try
            {
                // this checks wheter or not it is an email, if not throws an error
                var addr = new System.Net.Mail.MailAddress(email);
            }
            catch
            {

                return new ResponseFailure
                {
                    Error = "A user must have a valid Email"
                };
            }

            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser != null)
            {
                return new ResponseFailure {
                    Error = "There's already a user with that Email"
                };
            }

            var newUser = new User
            {
                Email = email,
                UserName = username,
                Name = name,
                Surname = surname,

            };

            var createdUser = await _userManager.CreateAsync(newUser, password);

            if (!createdUser.Succeeded)
            {
                return new ResponseFailure
                {
                    Error = createdUser.Errors.Select(x => x.Description).FirstOrDefault()
                };
            }

            return await GetTokenResponse(newUser);
        }

        private async Task<ResponseRegistrationSuccess> GetTokenResponse(User user, RefreshToken refreshToken = null)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSetting.Secret);

            var roles = await _userManager.GetRolesAsync(user);


            var claimsIdentity = new ClaimsIdentity(claims: new[] {
                    new Claim(type: JwtRegisteredClaimNames.Sub, value: user.Id.ToString()),
                    new Claim(type: JwtRegisteredClaimNames.Email, value: user.Email),
                    new Claim(type: JwtRegisteredClaimNames.Jti, value: Guid.NewGuid().ToString()),
                    new Claim(type: ClaimTypes.NameIdentifier, value: user.UserName)
            });

            claimsIdentity.AddClaims(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), algorithm: SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(descriptor);
            var newrefreshToken = refreshToken;
            if (refreshToken == null) {
                newrefreshToken = new RefreshToken
                {
                    Token = Guid.NewGuid().ToString(),
                    JwtID = token.Id,
                    UserId = user.Id,
                    CreationDate = DateTime.UtcNow,
                    ExpiryDate = DateTime.UtcNow.AddMonths(6),
                };
                await _context.RefreshTokens.AddAsync(newrefreshToken);
                await _context.SaveChangesAsync();
            }  else
            {
                Guid tokenString = Guid.NewGuid();
                newrefreshToken.Token = tokenString.ToString();
                newrefreshToken.CreationDate = DateTime.UtcNow;
                newrefreshToken.ExpiryDate = DateTime.UtcNow.AddMonths(6);

                await _context.SaveChangesAsync();
            }

            return new ResponseRegistrationSuccess
            {
                User =  _mapper.Map<UserData>(
                await _context.Usuarios
                .Where(c => c.Id == user.Id)
                .Include(s => s.Creador)
                .Include(c => c.Creador.Categoria1)
                .Include(c => c.Creador.Categoria2)
                .Include(c => c.Creador.TiposDeSuscripciones)
                .FirstOrDefaultAsync()
                ),
                Token = tokenHandler.WriteToken(token),
                RefreshToken = newrefreshToken.Token,
            };
        }

        public async Task<object> Login(string email, string password)
        {

            try
            {
                // this checks wheter or not it is an email, if not throws an error
                var addr = new System.Net.Mail.MailAddress(email);
            }
            catch
            {

                return new ResponseFailure
                {
                    Error = "Email not valid"
                };
            }

            var user = await _userManager.FindByEmailAsync(email);

            var lockoutDate = await _userManager.GetLockoutEndDateAsync(user);

            if (lockoutDate != null && DateTimeOffset.Compare((DateTimeOffset)lockoutDate, DateTimeOffset.UtcNow) > 0)
            {
                return new ResponseFailure
                {
                    Error = "Usuario bloqueado"
                };
            }

            if (user == null)
            {
                return new ResponseFailure
                {
                    Error = "Email/Password combination not correct"
                };
            }

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, password);

            if (!isPasswordCorrect)
            {
                return new ResponseFailure
                {
                    Error =  "Email/Password combination not correct"
                };
            }

            return await GetTokenResponse(user);
        }

        private ClaimsPrincipal GetPrincipalFromToken (string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {

                var principal = tokenHandler.ValidateToken(token, _tokenValidationParameter, out var validatedToken);
                if (!IsValidJWTWithSecurityAlgorithm(validatedToken))
                {
                    return null;
                }
                return principal;

            } catch
            {
                return null;
            }
        }

        private bool IsValidJWTWithSecurityAlgorithm(SecurityToken validatedToken ) {
            return (validatedToken is JwtSecurityToken jwtSecurityToken && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task<object> Refresh(string refreshToken, string token)
        {
            var validatedToken = GetPrincipalFromToken(token);

            var  refreshObject = await _context.RefreshTokens.FindAsync(refreshToken);

            if (refreshObject == null)
            {
                return new ResponseFailure
                {
                    Error = "Refresh token invalid"
                };
            }

            if (validatedToken == null)
            {
                return new ResponseFailure
                {
                    Error = "Invalid Token"
                };
            }

            var expiryDateUnix = long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDateUtc = new DateTime(year: 1970, month: 1, day: 1, hour: 0, minute: 0, second: 0, DateTimeKind.Utc).AddSeconds(expiryDateUnix);

            if (expiryDateUtc > DateTime.UtcNow)
            {
                return new ResponseFailure
                {
                    Error = "The token hansn't Expire"
                };
            }

            var user = await _userManager.FindByIdAsync(refreshObject.UserId);

            return await GetTokenResponse(user);
        }

        public async Task<UserData> GetUserInfo(ClaimsPrincipal claim) {

            var user = await _userManager.GetUserAsync(claim);

            if (user == null) {
                return null;
            }
            _context.Entry(user).Reference(p => p.Creador).Load();

            return _mapper.Map<UserData>(user);
        }

        public async Task<UserData> GetUserInfoById(string id) {

            var user = await _userManager.FindByIdAsync(id);

            if (user == null) {
                return null;
            }
            _context.Entry(user).Reference(p => p.Creador).Load();


            return _mapper.Map<UserData>(user);
        }

        public async Task<UserData?> MakeAdmin(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) {
                return null;
            }

            user.isAdministrador = true;
            await _userManager.UpdateAsync(user);

            return await this.GetUserInfoById(userId);

        }

        public async Task<UserData?> RemoveAdmin(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) {
                return null;
            }

            user.isAdministrador = false;
            await _userManager.UpdateAsync(user);

            return await this.GetUserInfoById(userId);

        }

        public async Task ForgotPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) {
                return;
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            var webApp = _configuration.GetSection("WebApp")["Url"];
            var apiKey = Environment.GetEnvironmentVariable("EMAIL__API_KEY");
            if (apiKey != null)
            {
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress("creadoreu@gmail.com", "CreadoresUY");
                var subject = "Recuperación de contraseña";
                var to = new EmailAddress(email, user.Name);
                var plainTextContent = $"Para restaurar tu contraseña <a href='{webApp}reset_password?user={HttpUtility.UrlEncode(user.Id)}&code={HttpUtility.UrlEncode(code)}'>presiona aquí</a>.";
                var htmlContent = $"Para restaurar tu contraseña <a href='{webApp}reset_password?user={HttpUtility.UrlEncode(user.Id)}&code={HttpUtility.UrlEncode(code)}'>presiona aquí</a>.";
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
            }
            return;

        }
    }
}
