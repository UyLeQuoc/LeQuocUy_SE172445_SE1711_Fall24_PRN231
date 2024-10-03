using BusinessObjects;
using DTO;
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
    [Route("odata/SystemAccounts")]
    public class SystemAccountController : ODataController
    {
        private readonly ISystemAccountService systemAccountService;

        public SystemAccountController(ISystemAccountService service)
        {
            this.systemAccountService = service;
        }

        [EnableQuery]
        [HttpGet]
        public ActionResult<IQueryable<SystemAccount>> Get()
        {
            var accounts = systemAccountService.GetSystemAccounts().AsQueryable();
            return Ok(accounts);
        }

        [EnableQuery]
        [HttpGet("{id}")]
        public ActionResult<SystemAccount> Get([FromRoute] short id)
        {
            var account = systemAccountService.GetSystemAccountById(id);
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

        [HttpPut("{id}")]
        public IActionResult Put([FromRoute] short id, [FromBody] SystemAccount systemAccount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            systemAccountService.UpdateSystemAccount(systemAccount);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] short id)
        {
            systemAccountService.RemoveSystemAccount(id);
            return NoContent();
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginRequestDTO loginDTO)
        {
            var account = systemAccountService.Login(loginDTO.Email, loginDTO.Password);
            if (account == null)
            {
                return Unauthorized("Invalid email or password.");
            }
            account.NewsArticles = null;

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
    }
}
