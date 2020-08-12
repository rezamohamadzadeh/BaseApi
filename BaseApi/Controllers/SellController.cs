using BaseApi.Models.JsonApi;
using BaseApi.Models.Sell;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Repository.InterFace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseApi.Controllers
{
    public class SellController : BaseController
    {
        private readonly IUnitOfWork _uow;

        public SellController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task<IActionResult> GetAffiliateSells(string code, string email, string filterDate)
        {
            try
            {
                if (code == null || filterDate == null || email == null)
                    return BadRequest(new JsonResultContent(false, JsonStatusCode.Error));

                IEnumerable<Tb_Sell> sells = null;

                if (filterDate == null)
                    sells = await _uow.SellRepo.GetAsync(d => d.AffiliateCode == code);
                else
                {
                    switch (filterDate)
                    {
                        case "All":
                            sells = await _uow.SellRepo.GetAsync(d => d.AffiliateCode == code);
                            break;
                        case "Monthly":
                            sells = await _uow.SellRepo.GetAsync(d => d.AffiliateCode == code && d.CreateAt > DateTime.Now.AddMonths(-1));
                            break;
                        case "Weekly":
                            sells = await _uow.SellRepo.GetAsync(d => d.AffiliateCode == code && d.CreateAt > DateTime.Now.AddDays(-7));
                            break;
                        default:
                            break;
                    }
                }
                var registeredCount = sells.Count() == 0 ? 0 : sells.Count(d => d.PayStatus == PayStatus.Registered);
                var sell = sells.Count() == 0 ? 0 : sells.Count(d => d.PayStatus == PayStatus.Registered);
                var sumSell = sells.Count() == 0 ? 0 : sells.Where(m => m.PayStatus != PayStatus.Registered).Sum(d => d.Price);
                var report = new AffiliateReportDto()
                {
                    AffiliateCode = code,
                    RegisteredCount = registeredCount,
                    SumSell = sumSell,
                    AffiliateEmail = email
                };
                var data = new JsonResultContent<AffiliateReportDto>(true, JsonStatusCode.Success, report);
                return Ok(data);
            }
            catch (Exception)
            {
                return BadRequest(new JsonResultContent(false, JsonStatusCode.Error));
            }
        }
    }
}
