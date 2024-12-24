using BankSystemProject.Model;
using BankSystemProject.Models.DTOs;
using BankSystemProject.Repositories.Interface;
using BankSystemProject.Repositories.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BankSystemProject.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CreditCardController : ControllerBase
    {
            private readonly ICreditCard _ICreditCard;

            public CreditCardController(ICreditCard _ICreditCard)
            {
                this._ICreditCard = _ICreditCard;
            }

        [HttpPost]
      //  [Authorize(Roles = "SystemAdministrator,CreditCardOfficer,Teller")]
        public async Task<IActionResult> CreateCreditCard([FromForm] Req_CreditCardDto creditCard)
            {
            
            // Get UserId from the JWT token
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 

            if (creditCard == null)
                {  
                    return BadRequest("Credit card information is required.");
                }

                if ( creditCard.CreditLimit <= 0)
                {
                    return BadRequest("Invalid card data.");
                }

                var result = await _ICreditCard.CreateCreditCardAsync(creditCard,userId);

            if (result)
            {
                return CreatedAtAction(nameof(CreateCreditCard), new { message = "Credit Card Created Successfully", creditCard });
            }
           
                return StatusCode(500, "Internal server error while creating credit card.");
            }

        [Authorize(Roles ="Customer")]
        [HttpPut("Customer/{creditCardId}")]
        public async Task<IActionResult> UpdateCreditCardById(int creditCardId, [FromForm] Req_UserUpdateCreditCardDto updateCardDto)
        {
            if (updateCardDto == null)
            {
                return BadRequest("Invalid data.");
            }

            var result = await _ICreditCard.UpdateCreditCardAsync(creditCardId, updateCardDto);

            if (!result)
            {
                return NotFound("Credit card not found.");
            }

            return Ok(new { message = "Credit card updated successfully." });
        }

        
        [HttpPut("Admin/{creditCardId}")]
        [Authorize(Roles = "SystemAdministrator,CreditCardOfficer,Teller")]

        public async Task<IActionResult> UpdateCreditCardById(int creditCardId, [FromForm] Req_AdminUpdateCreditCard updateCardDto)
        {
            if (updateCardDto == null)
            {
                return BadRequest("Invalid data.");
            }

            var result = await _ICreditCard.UpdateCreditCardByAdminAsync(creditCardId, updateCardDto);

            if (!result)
            {
                return NotFound("Credit card not found.");
            }

            return Ok(new { message = "Credit card updated successfully." });
        }

        [Authorize(Roles = "SystemAdministrator,CreditCardOfficer,Teller")]
        [HttpGet]
        public async Task<IActionResult> GetAllCardsInfo()
        {
            var cards = await _ICreditCard.GetAllCardsAsync(false);

            if (cards == null || !cards.Any())
            {
                return NotFound("No credit cards found.");
            }

            return Ok(cards);
        }

        [Authorize(Roles = "SystemAdministrator,CreditCardOfficer")]
        [HttpGet]
        public async Task<IActionResult> GetAllDeletedCardsInfo()
        {
            var cards = await _ICreditCard.GetAllCardsAsync(true);

            if (cards == null || !cards.Any())
            {
                return NotFound("No credit cards found.");
            }

            return Ok(cards);
        }

       [Authorize(Roles = "SystemAdministrator,CreditCardOfficer,Teller,Customer")]
        [HttpGet("{creditCardID}")]
        public async Task<IActionResult> GetCreditCardById(int creditCardID)
        {
            var creditCard = await _ICreditCard.GetCreditCardByIdAsync(creditCardID);

            if (creditCard == null)
            {
                return NotFound(new { Message = "Credit Card not found." });
            }

            return Ok(creditCard);
        }


        [Authorize(Roles = "SystemAdministrator,CreditCardOfficer")]
        [HttpDelete("{creditCardID}")]
        public async Task<IActionResult> DeleteCreditCard(int creditCardID)
        {
            var result = await _ICreditCard.DeleteCreditCardAsync(creditCardID);

            if (!result)
            {
                return NotFound(new { Message = "Credit Card not found or already deleted." });
            }

            return Ok(new { Message = "Credit Card successfully soft deleted." });
        }
    }
}
