using AutoMapper;
using BankSystemProject.Data;
using BankSystemProject.Helpers;
using BankSystemProject.Model;
using BankSystemProject.Models.DTOs;
using BankSystemProject.Repositories.Interface;
using BankSystemProject.Shared.Enums;
using Mailjet.Client.Resources;
using Microsoft.EntityFrameworkCore;
using MimeKit.Encodings;
using System.Security.Claims;

namespace BankSystemProject.Repositories.Service
{
    public class CreditCardService : ICreditCard
    {
        private readonly Bank_DbContext _context;
        private readonly ILogger<CreditCardService> _logger;


        public CreditCardService(Bank_DbContext context, ILogger<CreditCardService> _logger)
        {
            _context = context;
            this._logger = _logger;
        }

        //public async Task<bool> CreateCreditCardAsync(Req_CreditCardDto creditCardDto, string UserID)
        //{

        //    try
        //    {
        //        // Map the DTO to the Entity
        //        var creditCardx = new CreditCard
        //        {
        //            CustomerAccountId = creditCardDto.CustomerAccountId,
        //            CardType = creditCardDto.CardType.ToString(),
        //            CreditLimit = creditCardDto.CreditLimit,
        //            //Balance = creditCardDto.Balance,
        //            ExpiryDate = creditCardDto.ExpiryDate,
        //            Status = creditCardDto.Status.ToString(),
        //            IsDeleted = creditCardDto.IsDeleted,
        //            CreatedAt = DateTime.Now,
        //            //PinCode = HashingHelper.HashString(creditCardDto.PinCode),
        //            CreatedBy = UserID,

        //        };

        //        // Add to the DbSet
        //        _context.CreditCards.Add(creditCardx);
        //        await _context.SaveChangesAsync();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception (optional)
        //        return false;
        //    }

        //}

        public async Task<bool> CreateCreditCardAsync(Req_CreditCardDto creditCardDto, string UserID)
        {
            try
            {
                // Check if the credit card already exists for the given CustomerAccountId and CardType
                var existingCreditCard = await _context.CreditCards
                    .FirstOrDefaultAsync(c => c.CustomerAccountId == creditCardDto.CustomerAccountId && c.CardType == creditCardDto.CardType.ToString());

                if (existingCreditCard != null)
                {
                    // Log the error if needed (e.g., if it's a duplicate entry)
                    // _logger.LogWarning("Duplicate credit card entry found for the same CustomerAccountId and CardType.");
                    return false; // You can also return a specific message or error code for duplication
                }

                var creditCard = new CreditCard
                {
                    CustomerAccountId = creditCardDto.CustomerAccountId,
                    CardType = creditCardDto.CardType.ToString(),
                    CreditLimit = creditCardDto.CreditLimit,
                    ExpiryDate = creditCardDto.ExpiryDate,
                    Status = enCreditCardStatus.Active.ToString(),
                    IsDeleted = false,
                    CreatedAt = DateTime.Now,
                    PinCode = Base64Helper.Encode(creditCardDto.PinCode),
                    CreatedBy = "7c7e8903-5b17-448f-86a7-20945ca509e8", // or hardcoded for testing
                };

                var customerAccount = await _context.CustomersAccounts.FindAsync(creditCardDto.CustomerAccountId);
                if (customerAccount == null)
                {
                    // Log the error for invalid CustomerAccountId if needed
                    // _logger.LogError($"CustomerAccountId {creditCardDto.CustomerAccountId} not found.");
                    return false;
                }

                _context.CreditCards.Add(creditCard);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception for better debugging
                // _logger.LogError(ex, "Error occurred while creating credit card.");
                return false;
            }
        }

        public async Task<bool> UpdateCreditCardAsync(int creditCardId, Req_UserUpdateCreditCardDto updateCardDto)
        {
            var card = await _context.CreditCards
                                     .FirstOrDefaultAsync(c => c.CreditCardId == creditCardId);

            if (card == null) return false;

            card.CardType = updateCardDto.CardType.ToString();
            card.PinCode = HashingHelper.HashString(updateCardDto.PinCode);

            _context.CreditCards.Update(card);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateCreditCardByAdminAsync(int creditCardId, Req_AdminUpdateCreditCard updateCardDto)
        {
            var card = await _context.CreditCards
                                     .FirstOrDefaultAsync(c => c.CreditCardId == creditCardId);

            if (card == null) return false;
            
            card.CardType = updateCardDto.CardType.ToString();
            card.CreditLimit = updateCardDto.CreditLimit;
            //card.Balance = updateCardDto.Balance;
            card.ExpiryDate = updateCardDto.ExpiryDate;
            card.Status = updateCardDto.Status.ToString();
            //card.PinCode = HashingHelper.HashString(updateCardDto.PinCode);
            card.IsDeleted = updateCardDto.IsDeleted;
            _context.CreditCards.Update(card);
            await _context.SaveChangesAsync();

            return true;
        }

        //public async Task<List<Res_GetCreditCardInfoDto>> GetAllCardsAsync()
        //{
        //    var cards = await _context.CreditCards
        //        .Include(u => u.CustomersAccounts)
        //        .ThenInclude(u => u.User)
        //      .Where(c => !c.IsDeleted)
        //      .Select(c => new Res_GetCreditCardInfoDto
        //      {
        //          CreditCardId = c.CreditCardId,
        //          CardHolderName = c.CustomersAccounts.User.FullName,
        //          CardType = c.CardType,
        //          CreditLimit = c.CreditLimit,
        //          Balance = c.CustomersAccounts.Balance,
        //          ExpiryDate = c.ExpiryDate,
        //          Status = c.Status,
        //          CreatedAt = c.CreatedAt
        //      })
        //      .ToListAsync();

        //    return cards;
        //}

        public async Task<List<Res_GetCreditCardInfoDto>> GetAllCardsAsync(bool includeDeleted)
        {
            var query = _context.CreditCards
            .Where(c => includeDeleted ? c.IsDeleted : !c.IsDeleted) 
            .Include(c => c.CustomerAccount);  

            var creditCards = await query.ToListAsync();

            return await query.Select(c => new Res_GetCreditCardInfoDto
            {
                CreditCardId = c.CreditCardId,
                CardHolderName = c.CustomerAccount.User.FullName,
                CardType = c.CardType,
                CreditLimit = c.CreditLimit,
                Balance = c.CustomerAccount.Balance,
                ExpiryDate = c.ExpiryDate,
                Status = c.Status,
                CreatedAt = c.CreatedAt
            }).ToListAsync();
        }

        public async Task<Res_GetCreditCardInfoDto> GetCreditCardByIdAsync(int CreditID)
        {

            var creditCard = await _context.CreditCards
            .Where(c => c.CreditCardId == CreditID && !c.IsDeleted)
            .Include(u => u.CustomerAccount)
            .ThenInclude(u => u.User)
            .Select(c => new Res_GetCreditCardInfoDto
            {
                 CreditCardId = c.CreditCardId,
                CardHolderName = c.CustomerAccount.User.FullName,
                CardType = c.CardType,
                CreditLimit = c.CreditLimit,
                Balance = c.CustomerAccount.Balance,
                ExpiryDate = c.ExpiryDate,
                Status = c.Status,
                CreatedAt = c.CreatedAt
            })
            .FirstOrDefaultAsync();

            if (creditCard == null)
            {
                throw new KeyNotFoundException($"No credit card found with ID: {CreditID}");
            }
            return creditCard;

        }

        public async Task<bool> DeleteCreditCardAsync(int CreditCardID)
        {
            
            var creditCard = await _context.CreditCards
                .FirstOrDefaultAsync(c => c.CreditCardId == CreditCardID && !c.IsDeleted);

            if (creditCard == null)
            {
                return false; 
            }

            creditCard.IsDeleted = true;

            await _context.SaveChangesAsync();

            return true; 
        }
    }
}
