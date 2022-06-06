using grease_ranker_api.DTOs;
using grease_ranker_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace grease_ranker_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class McDonaldsController : ControllerBase
    {
        private readonly McDonaldsService _mcDonaldsService;

        public McDonaldsController(McDonaldsService mcDonaldsService)
        {
            this._mcDonaldsService = mcDonaldsService;
        }

        [HttpGet]
        public async Task<IEnumerable<Product>> Get()
        {
            return await _mcDonaldsService.GetProducts();
        }

        [HttpGet("update")]
        public OkResult Update()
        {
            _mcDonaldsService.UpdateProducts();
            return Ok();
        }
    }
}
