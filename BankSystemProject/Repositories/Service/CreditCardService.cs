using AutoMapper;
using BankSystemProject.Data;
using BankSystemProject.Helpers;
using BankSystemProject.Model;
using BankSystemProject.Models.DTOs;
using BankSystemProject.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using MimeKit.Encodings;

namespace BankSystemProject.Repositories.Service
{
    public class CreditCardService : ICreditCard
    {
        private readonly Bank_DbContext _context;


        public CreditCardService(Bank_DbContext context)
        {
            _context = context;

        }

        public async Task<bool> CreateCreditCardAsync(Req_CreditCardDto creditCardDto)
        {
            try
            {
                // Map the DTO to the Entity
                var creditCardx = new CreditCard
                {
                    CustomerAccountId = creditCardDto.CustomerAccountId,
                    CardType = creditCardDto.CardType.ToString(),
                    CreditLimit = creditCardDto.CreditLimit,
                    //Balance = creditCardDto.Balance,
                    ExpiryDate = creditCardDto.ExpiryDate,
                    Status = creditCardDto.Status.ToString(),
                    // IsDeleted = creditCardDto.IsDeleted,
                    CreatedAt = DateTime.Now,
                    PinCode = HashingHelper.HashString(creditCardDto.PinCode),

                };

                // Add to the DbSet
                _context.CreditCards.Add(creditCardx);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return false;
            }

        }

        public async Task<bool> UpdateCreditCardAsync(int creditCardId, Req_UserUpdateCreditCardDto updateCardDto)
        {
            var card = await _context.CreditCards
                                     .FirstOrDefaultAsync(c => c.CreditCardId == creditCardId);

            if (card == null) return false;

            card.CardType = updateCardDto.CardType.ToString();
            // card.CreditLimit = updateCardDto.CreditLimit;
            // card.Balance = updateCardDto.Balance;
            // card.ExpiryDate = updateCardDto.ExpiryDate;
            //card.Status = updateCardDto.Status;
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

            _context.CreditCards.Update(card);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Res_GetCreditCardInfoDto>> GetAllCardsAsync()
        {
            var cards = await _context.CreditCards
                .Include(u => u.CustomerAccount)
                .ThenInclude(u => u.User)
              .Where(c => !c.IsDeleted)
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
              .ToListAsync();

            return cards;
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
