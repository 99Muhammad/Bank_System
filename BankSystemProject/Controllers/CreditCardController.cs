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
    [Route("api/[controller]")]
    [ApiController]
    public class CreditCardController : ControllerBase
    {
            private readonly ICreditCard _ICreditCard;

            public CreditCardController(ICreditCard _ICreditCard)
            {
                this._ICreditCard = _ICreditCard;
            }

        [HttpPost("/CreateCreditCard")]
        //[Authorize(Roles ="Employee")]
        public async Task<IActionResult> CreateCreditCard([FromForm] Req_CreditCardDto creditCard)
            {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get UserId from the JWT token

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

        //[Authorize(Roles ="Customer")]
        [HttpPut("/UpdateCreditCard/{creditCardId}")]
        public async Task<IActionResult> UpdateCreditCard(int creditCardId, [FromForm] Req_UserUpdateCreditCardDto updateCardDto)
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

        //[Authorize(Roles ="Employee")]
        [HttpPut("/UpdateCreditCardByAdmin/{creditCardId}")]
        public async Task<IActionResult> UpdateCreditCardByAdminAsync(int creditCardId, [FromForm] Req_AdminUpdateCreditCard updateCardDto)
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

        //[Authorize(Roles ="Employee")]
        [HttpGet("GetAllCardsInfo")]
        public async Task<IActionResult> GetAllCardsInfo()
        {
            var cards = await _ICreditCard.GetAllCardsAsync(false);

            if (cards == null || !cards.Any())
            {
                return NotFound("No credit cards found.");
            }

            return Ok(cards);
        }

        //[Authorize(Roles ="Employee")]
        [HttpGet("GetAllDeletedCardsInfo")]
        public async Task<IActionResult> GetAllDeletedCardsInfo()
        {
            var cards = await _ICreditCard.GetAllCardsAsync(true);

            if (cards == null || !cards.Any())
            {
                return NotFound("No credit cards found.");
            }

            return Ok(cards);
        }

        //[Authorize(Roles ="Customer")]
        [HttpGet("GetCreditCardById/{creditCardID}")]
        public async Task<IActionResult> GetCreditCardById(int creditCardID)
        {
            var creditCard = await _ICreditCard.GetCreditCardByIdAsync(creditCardID);

            if (creditCard == null)
            {
                return NotFound(new { Message = "Credit Card not found." });
            }

            return Ok(creditCard);
        }

        //[Authorize(Roles ="Employee")]
        [HttpDelete("DeleteCreditCard/{creditCardID}")]
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
