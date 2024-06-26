using API.DTO;
using Application.Features.ProductFeatures.Commands;
using Application.Features.ProductFeatures.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Security.Jwt;

namespace API.Controllers.v1;

[ApiVersion("1.0")]
public class ProductController : BaseApiController
{
    /// <summary>
    /// Create a new product
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost, Authorize]
    public async Task<IActionResult> Create(CreateProductDto createProductDto)
    {
        try
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Split(" ")[1];
            var isvalid = TokenValidator.IsTokenValid(token);
            if (!isvalid) return Unauthorized("Token is not valid");
            var customerPhone = DecodeToken.DecodeJwt(token);
            var command = new CreateProductCommand
            {
                Name = createProductDto.Name,
                Barecode = createProductDto.Barecode,
                Description = createProductDto.Description,
                Rate = createProductDto.Rate,
                CustomerPhone = customerPhone
            };
            return Ok(await Mediator.Send(command));
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    /// <summary>
    /// Gets all Products
    /// </summary>
    /// <returns></returns>
    [HttpGet("GetAllProducts"), Authorize]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Split(" ")[1];
            var isvalid = TokenValidator.IsTokenValid(token);
            if (!isvalid) return Unauthorized("Token is not valid");
            var customerPhonenumber = DecodeToken.DecodeJwt(token);
            var products = await Mediator.Send(new GetAllProductsQuery(customerPhonenumber));
            if (products == null) return NotFound();
            var productsDto = Mapper.Map<List<GetAllProductDto>>(products);
            return Ok(productsDto);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message + e.Source);
        }
    }

    ///<summary>
    /// Updates the Product Entity based on Id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPut("[action]")]
    public async Task<IActionResult> Update(Guid id, UpdateProductCommand command)
    {
        try
        {
            if (id != command.Id)
                return NotFound("Product not found");
            return Ok(await Mediator.Send(command));
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    ///<summary>
    /// Deletes product Entity based on Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        return Ok(await Mediator.Send(new DeleteProductCommand { Id = id }));
    }
}