using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Services;

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
        public IActionResult Login([FromBody] SystemAccount loginRequest)
        {
            var account = systemAccountService.Login(loginRequest.AccountEmail, loginRequest.AccountPassword);
            if (account == null)
            {
                return Unauthorized("Invalid email or password.");
            }
            return Ok(account);
        }
    }
}
