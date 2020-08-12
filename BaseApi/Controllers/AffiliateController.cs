using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseApi.Models.JsonApi;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Repository.InterFace;

namespace BaseApi.Controllers
{

    public class AffiliateController : BaseController
    {
        private readonly IUnitOfWork _uow;

        public AffiliateController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IActionResult> GetAffiliateUsers(string email = null)
        {
            try
            {
                var affiliates = email == null ? await _uow.AffiliateRepo.GetAsync() : await _uow.AffiliateRepo.GetAsync(d => d.Email == email);
                var data = new JsonResultContent<IEnumerable<Tb_Affiliates>>(true, JsonStatusCode.Success, affiliates);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new JsonResultContent(false, JsonStatusCode.Error));
            }
        }

    }
}
