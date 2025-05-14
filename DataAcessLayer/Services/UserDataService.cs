using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DataAcessLayer.Context;
using DataAcessLayer.Entity;
using DataAcessLayer.Exceptions;
using DataAcessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ModelLayer;

namespace DataAcessLayer.Services
{
    public class UserDataService : IUserDataService
    {
        private readonly UserDBContext _userContext;
        private readonly IConfiguration _configuration;
        private readonly IEmailDataService _emailService;


        public UserDataService(UserDBContext userDBContext, IConfiguration configuration, IEmailDataService emailService)
        {
            _userContext = userDBContext;
            _configuration = configuration;
            _emailService = emailService;

        }

        public async Task AddUser(RegistrationModel users)
        {
            var isUserDuplicate = await _userContext.Users.AnyAsync(e => e.Email == users.Email);
            if (isUserDuplicate)
            {
                throw new DuplicateUserException("User already exists. Please log in.");
            }

            User user = new User
            {
                FirstName = users.FirstName,
                LastName = users.LastName,
                Email = users.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(users.Password)
            };

            _userContext.Users.Add(user);
            await _userContext.SaveChangesAsync();
        }


        public async Task<bool> DeleteUser(int id)
        {
            var user = await _userContext.Users.Include(u => u.Notes).FirstOrDefaultAsync(u => u.UserID == id);
            if (user == null)
            {
                throw new Exception("User Not Available");

            }
            _userContext.Users.Remove(user);
            await _userContext.SaveChangesAsync();
            return true;
        }



        public async Task<User> GetUserById(int id)
        {
            var user = await _userContext.Users.Include(u => u.Notes).FirstOrDefaultAsync(u => u.UserID == id);
            if (user == null)
            {
                throw new Exception("User Not Available");

            }
            return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _userContext.Users.ToListAsync();
        }

        public async Task<TokenResponse> LoginUser(LoginRequestModel model)
        {
            var isExistUser = await _userContext.Users.FirstOrDefaultAsync(e => e.Email == model.Email);
            if (isExistUser != null)
            {
                var isPasswordMatch = BCrypt.Net.BCrypt.Verify(model.Password, isExistUser.Password);
                if (isPasswordMatch)
                {
                    var Issuer = _configuration["Jwt:Issuer"];
                    var Audience = _configuration["Jwt:Audience"];
                    var Key = _configuration["Jwt:Key"];
                    var TokenValiditiInMin = _configuration["Jwt:TokenValidityInMin"];
                    var TokenExpiryInMin = DateTime.UtcNow.AddMinutes(Convert.ToDouble(TokenValiditiInMin + 1));

                    var TokenDiscriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {

                          new Claim(ClaimTypes.NameIdentifier, isExistUser.UserID.ToString()),
                            new Claim(ClaimTypes.Email, isExistUser.Email),

                        }),
                        Expires = TokenExpiryInMin,
                        Issuer = Issuer,
                        Audience = Audience,
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key)),
                        SecurityAlgorithms.HmacSha256Signature)
                    };
                    var TokenHandler = new JwtSecurityTokenHandler();
                    var Token = TokenHandler.CreateToken(TokenDiscriptor);

                    var TokenString = TokenHandler.WriteToken(Token);

                    return new TokenResponse
                    {
                        UserID = isExistUser.UserID,
                        Token = TokenString,
                        Email = isExistUser.Email,
                        TokenExpiry = TokenExpiryInMin
                    };
                }
                else
                {
                    throw new Exception("Invalid Password");
                }
            }
            else
            {
                throw new Exception("User Not Found");
            }
        }
        
        public async Task<bool> ForgotPassword(string email)
        {
            var isExistUser = await _userContext.Users.FirstOrDefaultAsync(e => e.Email == email);
            if (isExistUser == null)
            {
                throw new Exception("Email does not exist");
            }

            var Issuer = _configuration["Jwt:Issuer"];
            var Audience = _configuration["Jwt:Audience"];
            var Key = _configuration["Jwt:EmailKey"];
            var TokenValiditiInMin = _configuration["Jwt:TokenValidityInMin"];
            var TokenExpiryInMin = DateTime.UtcNow.AddMinutes(Convert.ToDouble(TokenValiditiInMin + 1));

            var TokenDiscriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {

                          new Claim(ClaimTypes.NameIdentifier, isExistUser.UserID.ToString()),
                            new Claim(ClaimTypes.Email, isExistUser.Email),

                }),
                Expires = TokenExpiryInMin,
                Issuer = Issuer,
                Audience = Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key)),
                SecurityAlgorithms.HmacSha256Signature)
            };
            var TokenHandler = new JwtSecurityTokenHandler();
            var Token = TokenHandler.CreateToken(TokenDiscriptor);

            var TokenString = TokenHandler.WriteToken(Token);

            // Send Reset Email
            var resetLink = $"http://localhost:4200/res?token={TokenString}";
            await _emailService.SendEmail(isExistUser.Email, "Password Reset Request", $"Click here to reset your password: {resetLink}");

            return true;
        }

        

        public async Task<bool> ResetPassword(ResetPasswordModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Token))
                {
                    throw new Exception("Reset token is missing or empty.");
                }

                if (model.NewPassword != model.ConfirmPassword)
                {
                    throw new Exception("Passwords do not match.");
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = _configuration["Jwt:EmailKey"];

                // Validate Token
                var claimsPrincipal = tokenHandler.ValidateToken(model.Token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"],
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var email = claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value;
                if (string.IsNullOrEmpty(email))
                {
                    throw new Exception("Invalid token.");
                }

                var isExistUser = await _userContext.Users.FirstOrDefaultAsync(e => e.Email == email);
                if (isExistUser == null)
                {
                    throw new Exception("User not found.");
                }

                // Hash and update password, then mark entity as modified
                isExistUser.Password = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
                _userContext.Entry(isExistUser).State = EntityState.Modified;
                await _userContext.SaveChangesAsync();
                return true;
            }
            catch (ArgumentException)
            {
                throw new Exception("Reset token is not in a valid format.");
            }
            catch (SecurityTokenException)
            {
                throw new Exception("Invalid or expired token.");
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}");
            }
        }



        public async Task<User> UpdateUser(RegistrationModel userModel)
        {
            var user = await _userContext.Users.FirstOrDefaultAsync(u => u.Email == userModel.Email);
            if (user == null)
            {
                throw new Exception("User Not Available");
            }

            user.FirstName = userModel.FirstName;
            user.LastName = userModel.LastName;
            user.Password = BCrypt.Net.BCrypt.HashPassword(userModel.Password);

            _userContext.Users.Update(user);
            await _userContext.SaveChangesAsync();
            return user;
        }
    }
}

       
