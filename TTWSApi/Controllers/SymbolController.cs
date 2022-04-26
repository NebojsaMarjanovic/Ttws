using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace TTWSApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SymbolController : ControllerBase
    {
        private readonly Symbol _symbol;

        public SymbolController(Symbol symbol)
        {
            _symbol = symbol;
        }

        [HttpGet("{isin}")]
        public async Task<string> GetSymbolsByISIN(string isin)
        {
            return await _symbol.GetByIsin(isin);
        }


    }
}
