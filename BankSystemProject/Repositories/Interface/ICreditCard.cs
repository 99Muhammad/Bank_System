using BankSystemProject.Data;
using BankSystemProject.Model;
using BankSystemProject.Models.DTOs;

namespace BankSystemProject.Repositories.Interface
{
    public interface ICreditCard
    {
        Task<bool> CreateCreditCardAsync(Req_CreditCardDto creditCard,string UserID);
        Task<bool> UpdateCreditCardAsync(int creditCardId, Req_UserUpdateCreditCardDto updateCardDto);
        Task<bool> UpdateCreditCardByAdminAsync(int creditCardId, Req_AdminUpdateCreditCard updateCardDto);
        Task<List<Res_GetCreditCardInfoDto>> GetAllCardsAsync(bool IncludeDelete);
        Task<Res_GetCreditCardInfoDto> GetCreditCardByIdAsync(int CreditID);
        Task<bool> DeleteCreditCardAsync(int CreditCardID);



    }
}
