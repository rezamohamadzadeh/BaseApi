using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BaseApi.Models;
using BaseApi.Models.JsonApi;
using BaseApi.Models.Sell;
using BaseApi.Utility.GeneratePdfFile;
using Common.Images;
using DAL.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Repository.InterFace;

namespace BaseApi.Controllers
{

    public class AffiliateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IWebHostEnvironment _hostEnvironment;

        public AffiliateController(IUnitOfWork uow, IWebHostEnvironment hostEnvironment)
        {
            _uow = uow;
            _hostEnvironment = hostEnvironment;
        }

        /// <summary>
        /// Get Affiliates Sell Report
        /// </summary>
        /// <param name="filterDate"></param>
        /// <param name="userType"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<IActionResult> GetAffiliateUsers(string filterDate, string userType, string email = null)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(filterDate) ||
                    string.IsNullOrWhiteSpace(userType))
                    return BadRequest(new JsonResultContent(false, JsonStatusCode.Error));

                List<AffiliateReportDto> list = new List<AffiliateReportDto>();
                IEnumerable<Tb_Sell> sells = null;


                var affiliates = userType.Contains("Admin") ? await _uow.AffiliateRepo.GetAsync() : await _uow.AffiliateRepo.GetAsync(d => d.Email == email);
                foreach (var affiliate in affiliates)
                {
                    if (userType.Contains("Admin"))
                    {
                        switch (filterDate)
                        {
                            case "All":
                                sells = await _uow.SellRepo.GetAsync(d => d.AffiliateCode == affiliate.Code);
                                break;
                            case "Monthly":
                                sells = await _uow.SellRepo.GetAsync(d => d.AffiliateCode == affiliate.Code && d.CreateAt > DateTime.Now.AddMonths(-1));
                                break;
                            case "Weekly":
                                sells = await _uow.SellRepo.GetAsync(d => d.AffiliateCode == affiliate.Code && d.CreateAt > DateTime.Now.AddDays(-7));
                                break;
                            default:
                                break;
                        }
                    }
                    else
                        sells = await _uow.SellRepo.GetAsync(d => d.AffiliateCode == affiliate.Code);

                    var registeredCount = sells.Count() == 0 ? 0 : sells.Count(d => d.PayStatus == PayStatus.Registered);
                    var sell = sells.Count() == 0 ? 0 : sells.Count(d => d.PayStatus == PayStatus.Registered);
                    var sumSell = sells.Count() == 0 ? 0 : sells.Where(m => m.PayStatus != PayStatus.Registered).Sum(d => d.Price);
                    var report = new AffiliateReportDto()
                    {
                        AffiliateCode = affiliate.Code,
                        RegisteredCount = registeredCount,
                        SumSell = sumSell,
                        AffiliateEmail = affiliate.Email
                    };

                    list.Add(report);
                }
                var pdfFile = CreatePdf<AffiliateReportDto>.createReport(_hostEnvironment.WebRootPath, list, true, filterDate + " Sell Report", "AffiliateCode", "AffiliateEmail", "RegisteredCount").GenerateAsByteArray();

                return Ok(new JsonResultContent<byte[]>(true, JsonStatusCode.Success, pdfFile));
            }
            catch (Exception ex)
            {
                return BadRequest(new JsonResultContent(false, JsonStatusCode.Error));
            }
        }

        /// <summary>
        /// Change sell report pdf file header
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        [Produces("multipart/form-data")]
        public async Task<IActionResult> ChangeAffiliateSellReportFileHeader()
        {
            try
            {
                var model = Request.Form.Files[0];

                if (model == null)
                    return BadRequest(new JsonResultContent(false, JsonStatusCode.Warning));

                string fileName = "AffiliateSellReportHeader.Png";

                Upload uploader = new Upload();
                Delete delete = new Delete();
                string deletPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                delete.DeleteImage(deletPath);


                string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);


                string DirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                await uploader.UploadImage(savePath, DirectoryPath, model);
                return Ok();


            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Get all affiliate users for admin in bot
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> GetAffiliates()
        {
            try
            {
                var affiliates = await _uow.AffiliateRepo.GetAffiliates();
                return Ok(new JsonResultContent<IEnumerable<Tb_Affiliates>>(true, JsonStatusCode.Success, affiliates));
            }
            catch (Exception e)
            {
                return BadRequest(new JsonResultContent(false, JsonStatusCode.Error));
            }
        }

        /// <summary>
        /// Get Affiliate Sells by affiliatecode for admin in bot
        /// </summary>
        /// <param name="affiliateCode"></param>
        /// <returns></returns>
        public async Task<IActionResult> GetAffiliateSells(string affiliateCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(affiliateCode))
                    return NotFound(new JsonResultContent(false, JsonStatusCode.Error));


                var affiliateSells = await _uow.SellRepo.GetAsync(d => d.AffiliateCode == affiliateCode);
                var pdfFile = CreatePdf<Tb_Sell>.createReport(_hostEnvironment.WebRootPath, affiliateSells.ToList(), false, "AffiliateSell", "AffiliateCode", "FullName", "Phone", "Country", "City", "ProductName", "PayStatus").GenerateAsByteArray();

                return Ok(new JsonResultContent<byte[]>(true, JsonStatusCode.Success, pdfFile));
            }
            catch (Exception e)
            {
                return BadRequest(new JsonResultContent(false, JsonStatusCode.Error));
            }
        }

    }
}
