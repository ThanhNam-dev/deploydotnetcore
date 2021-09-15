using EMa.API.Entity;
using EMa.Data.DataContext;
using EMa.Data.Entities;
using EMa.Data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace EMa.API.Controllers
{
    [ApiController]
    [Route("")]
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly DataDbContext _context;

        private string accountSid = "AC1b8cf251bf9d24497abf3fe3d32bf237";
        private string authToken = "7943dca7f9a1268b88a74502bf7df1d8";

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            IConfiguration configuration, DataDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _context = context;

        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<object> Login([FromBody] LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByNameAsync(model.PhoneNumber);

                if (existingUser == null)
                {
                    return BadRequest(new AuthResult()
                    {
                        Errors = new List<string>() {
                                "Invalid login request"
                            },
                        Success = false
                    });
                }

                var isCorrect = await _userManager.CheckPasswordAsync(existingUser, model.Password);
                if (!isCorrect)
                {
                    return BadRequest(new AuthResult()
                    {
                        Errors = new List<string>() {
                                "Invalid login request"
                            },
                        Success = false
                    });
                }
                var _check = existingUser.PhoneNumberConfirmed;

                if (_check == false)
                {
                    String _phoneNumber = existingUser.PhoneNumber.Substring(1);
                    _phoneNumber = "+84" + _phoneNumber;
                    TwilioClient.Init(accountSid, authToken);

                    var message = MessageResource.Create(
                        body: "Mã xác nhận của bạn là " + existingUser.Code.ToString(),
                        from: new Twilio.Types.PhoneNumber("+17348905251"),
                        to: new Twilio.Types.PhoneNumber(_phoneNumber)
                    );
                    Content(message.Sid);
                    return Ok(new AuthResult()
                    {
                        Errors = new List<string>() {
                                "Mã xác nhận của bạn là " + existingUser.Code.ToString(),
                            },
                        Success = false
                    });
                }
                var jwtToken = GenerateJwtToken(existingUser.PhoneNumber, existingUser);
                return Ok(new AuthResult()
                {
                    Success = true,
                    Token = jwtToken.Result
                });

                
            }

            return BadRequest();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<object> Register([FromBody] RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                if (model.PhoneNumber.Length != 10 && model.PhoneNumber.Substring(0, 1) == "0")
                {
                    return BadRequest(new AuthResult()
                    {
                        Errors = new List<string>() {
                                "PhoneNumber must be of length 10 and must start from 0"
                            },
                        Success = false
                    });
                }
                var phoneNumberExists = await _userManager.FindByNameAsync(model.PhoneNumber);

                if (phoneNumberExists != null)
                {
                    if (phoneNumberExists.PhoneNumberConfirmed == false)
                    {
                        String _phoneNumber = model.PhoneNumber.Substring(1);
                        _phoneNumber = "+84" + _phoneNumber;
                        TwilioClient.Init(accountSid, authToken);

                        var message = MessageResource.Create(
                            body: "Mã xác nhận của bạn là " + phoneNumberExists.Code.ToString(),
                            from: new Twilio.Types.PhoneNumber("+17348905251"),
                            to: new Twilio.Types.PhoneNumber(_phoneNumber)
                        );
                        Content(message.Sid);
                        return Ok(new AuthResult()
                        {
                            Errors = new List<string>() {
                                "Mã xác nhận của bạn là " + phoneNumberExists.Code.ToString(),
                            },
                            Success = true
                        });
                    } else
					{
                        return Ok(new AuthResult()
                        {
                            Errors = new List<string>() {
                                "PhoneNumber already in use"
                            },
                            Success = false
                        });
                    }
                    
                }
                
                    var user = new AppUser
                    {
                        ChildName = model.ChildName,
                        UserName = model.PhoneNumber,
                        PhoneNumber = model.PhoneNumber
                    };
                    var newId = Guid.NewGuid();
                    Random _code = new Random();
                    var newUser = new AppUser() { Id = newId,ChildName = user.ChildName, UserName = user.UserName, PhoneNumber = user.PhoneNumber, Code = _code.Next(100000, 999999) };
                    var result = await _userManager.CreateAsync(newUser, model.Password);

                    if (result.Succeeded)
                    {
                    var _existingPhoneNumber = await _userManager.FindByIdAsync(newId.ToString());
                    String _phoneNumber = _existingPhoneNumber.PhoneNumber.Substring(1);
                    _phoneNumber = "+84" + _phoneNumber;
                    TwilioClient.Init(accountSid, authToken);

                    var message = MessageResource.Create(
                        body: "Mã xác nhận của bạn là " + _existingPhoneNumber.Code.ToString(),
                        from: new Twilio.Types.PhoneNumber("+17348905251"),
                        to: new Twilio.Types.PhoneNumber(_phoneNumber)
                    );
                    Content(message.Sid);
                    return Ok(new AuthResult()
                    {
                        Errors = new List<string>() {
                                "Mã xác nhận của bạn là " + _existingPhoneNumber.Code.ToString(),
                            },
                        Success = true
                    });
                }
                else
                {
                    return BadRequest(new AuthResult()
                    {
                        Errors = result.Errors.Select(x => x.Description).ToList(),
                        Success = false
                    });
                }

                
            }

            return BadRequest();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Confirm")]
        public async Task<IActionResult> Confirm([FromBody] UserConfirm user)
        {
            if (ModelState.IsValid)
            {
                var existingPhoneNumber = await _userManager.FindByNameAsync(user.PhoneNumber);
                if (existingPhoneNumber == null)
                {
                    return Ok(new AuthResult()
                    {
                        Errors = new List<string>() {
                                "PhoneNumber not already in use"
                            },
                        Success = false
                    });
                }
                if (existingPhoneNumber.Code != user.Code)
                {
                    return Ok(new AuthResult()
                    {
                        Errors = new List<string>() {
                                "Mã xác nhận không đúng"
                            },
                        Success = false
                    });
                }
                else
                {



                    existingPhoneNumber.PhoneNumberConfirmed = true;
                    await _userManager.UpdateAsync(existingPhoneNumber);
                    var jwtToken = GenerateJwtToken(existingPhoneNumber.PhoneNumber,existingPhoneNumber);

                    return Ok(new AuthResult()
                    {
                        Success = true,
                        Token = jwtToken.Result
                    });
                }

            }

            return BadRequest(new AuthResult()
            {
                Errors = new List<string>() {
                        "Invalid payload"
                    },
                Success = false
            });
        }

        private Task<string> GenerateJwtToken(string phoneNumber, AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim("id", user.Id.ToString()),
                new Claim("phone", phoneNumber.ToString()),
                new Claim("childName", user.ChildName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddYears(Convert.ToInt32(_configuration["JwtExpireDays"]));

            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }

        
    }
}
