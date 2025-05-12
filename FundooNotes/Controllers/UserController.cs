using System.Security.Cryptography.Xml;
using BusinessLayer.Interface;
using DataAcessLayer.Entity;
using DataAcessLayer.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using Swashbuckle.AspNetCore.Swagger;

namespace FundooNotes.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    public readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;

    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegistrationModel users)
    {
        try
        {
            await _userService.AddUser(users);
            return Ok(new { message = "Registration Successful." });
        }
        catch (DuplicateUserException ex)
        {
            return Conflict(ex.Message); 
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during registration: {ex.Message}");
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginUser([FromBody] LoginRequestModel model)
    {
        try
        {
            var token = await _userService.LoginUser(model);
            if (token == null)
            {
                return BadRequest("Invalid User..");
            }
            return Ok(new {status=true,messege="Login sucessfull...", data=token});
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("forgetPassword")]
    public async Task<IActionResult> ForgotPassword(string email)
    {
        try
        {
            var result = await _userService.ForgotPassword(email);
            if (result)
                return Ok(new { Message = "Password reset link has been sent to your email." });

            return BadRequest(new { Message = "Failed to send password reset link." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [Authorize]
    [HttpPost("resetPassword")]
    public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
    {
        try
        {
            var result = await _userService.ResetPassword(model);
            if (result)
                return Ok(new { Message = "Your password has been successfully reset." });

            return BadRequest(new { Message = "Failed to reset password." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    //[Authorize]
    //[HttpPost("resetPassword")]
    //public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
    //{

    //    if (model.NewPassword != model.ConfirmPassword)
    //    {
    //        return BadRequest("Passwords do not match.");
    //    }

    //    var result = await _userService.ResetPassword(HttpContext,model);
    //    return result ? Ok("Password reset successful.") : BadRequest("Failed to reset password.");

    //}


    [HttpGet("GetAllUsers")]
    public async Task<ActionResult<User>> GetAllUsers()
    {
        await _userService.GetUsers();
        return Ok(await _userService.GetUsers());
    }

    [HttpGet("GetUserById")]
    public async Task<IActionResult> GetUserById(int id)
    {
        try
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                return BadRequest("Invalid User..");
            }

            return Ok(user);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("DeleteUser")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        try
        {
            var result = await _userService.DeleteUser(id);
            if (!result)
            {
                return BadRequest("Invalid User..");
            }

            return Ok("User deleted successfully.");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("UpdateUser")]
    public async Task<IActionResult> UpdateUser(RegistrationModel user)
    {
        return Ok(await _userService.UpdateUser(user));
    }




}
