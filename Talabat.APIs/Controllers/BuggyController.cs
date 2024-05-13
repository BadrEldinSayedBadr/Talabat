using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.Repository.Data.Context;

namespace Talabat.APIs.Controllers
{
    public class BuggyController : ApiBaseController
    {
        private readonly StoreDbContext _context;

        public BuggyController(StoreDbContext context) 
        {
            _context = context;
        }

 
        [HttpGet("NotFound")]
        public ActionResult GetNotFoundRequest()
        {
            var product = _context.Products.Find(100);
            if (product is null)
                return NotFound(new ApiErrorResponse(404));

            return Ok(product);
        }


        [HttpGet("BadRequest")]
        public ActionResult GetBadRequest()
        {
            return BadRequest(new ApiErrorResponse(400));
        }


        [HttpGet("ServerError")]
        public ActionResult ServerErrorRequest()
        {
            var product = _context.Products.Find(100);

            var productToReturn = product.ToString();
            return Ok(productToReturn);
        }


        [HttpGet("BadRequest/{id}")]
        public ActionResult GetBadRequest(int id)
        {
            return Ok();
        }
    }
}
