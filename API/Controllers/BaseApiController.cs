using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Security.Jwt;

namespace API.Controllers;
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    private IMediator _mediator;
    private IMapper _mapper;
    private ITokenGenerator _tokenGenerator;
    protected ITokenValidator _tokenValidator;
    protected IDecodeToken _decodeToken;
    protected IDecodeToken DecodeToken => _decodeToken??= HttpContext.RequestServices.GetService<IDecodeToken>();
    protected ITokenValidator TokenValidator => _tokenValidator??= HttpContext.RequestServices.GetService<ITokenValidator>();
    protected ITokenGenerator TokenGenerator => _tokenGenerator??= HttpContext.RequestServices.GetService<TokenGeneration>();
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
    protected IMapper Mapper => _mapper??= HttpContext.RequestServices.GetService<IMapper>();
}