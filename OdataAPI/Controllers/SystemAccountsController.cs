using BusinessObjects;
using DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.IdentityModel.Tokens;
using Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OdataAPI.Controllers
{
    public class SystemAccountsController : ODataController
    {
        private readonly ISystemAccountService systemAccountService;

        public SystemAccountsController(ISystemAccountService service)
        {
            systemAccountService = service;
        }

        [EnableQuery]
        [Authorize("AdminOnly")]
        public ActionResult<IEnumerable<SystemAccount>> Get()
        {
            var accounts = systemAccountService.GetSystemAccounts();
            return Ok(accounts);
        }

        [EnableQuery]
        public ActionResult<SystemAccount> Get([FromRoute] short key)
        {
            var account = systemAccountService.GetSystemAccountById(key);
            if (account == null)
            {
                return NotFound();
            }
            return Ok(account);
        }

        [HttpPost]
        public IActionResult Post([FromBody] SystemAccount systemAccount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            systemAccountService.CreateSystemAccount(systemAccount);
            return Created(systemAccount);
        }

        [HttpPut("/odata/SystemAccounts/{id}")]
        public IActionResult Put([FromRoute] short id, [FromBody] SystemAccount systemAccount)
        {
            try
            {
                systemAccount.AccountId = id;
                systemAccountService.UpdateSystemAccount(systemAccount);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("/odata/SystemAccounts/{id}")]
        public IActionResult Delete([FromRoute] short id)
        {
            try
            {
                short accountId = short.Parse(User.FindFirst("AccountId")?.Value);
                if (accountId == id)
                {
                    throw new InvalidOperationException("Cannot delete current account.");
                }
                systemAccountService.RemoveSystemAccount(id);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginRequestDTO loginDTO)
        {
            var account = systemAccountService.Login(loginDTO.Email, loginDTO.Password);
            if (account == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            //Generate JWT Token
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, account.AccountEmail),
                new Claim("Role", account.AccountRole.ToString()),
                new Claim("AccountId", account.AccountId.ToString()),
                new Claim("AccountName", account.AccountName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var preparedToken = new JwtSecurityToken(
                issuer: configuration["JWT:Issuer"],
                audience: configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            var token = new JwtSecurityTokenHandler().WriteToken(preparedToken);
            var role = account.AccountRole.ToString(); //0:Admin 1:Staff 2:Manager
            var accountId = account.AccountId.ToString();
            return Ok(new
            {
                token,
                role,
                accountId
            });
        }

        [Authorize]
        [HttpGet("CurrentUser")]
        public IActionResult GetCurrentUser()
        {
            // Lấy thông tin từ claims của JWT token
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var role = User.FindFirst("Role")?.Value;
            var accountId = User.FindFirst("AccountId")?.Value;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(role) || string.IsNullOrEmpty(accountId))
            {
                return Unauthorized("User not authenticated.");
            }

            return Ok(new
            {
                Email = email,
                Role = role,
                AccountId = accountId
            });
        }
    }
}
