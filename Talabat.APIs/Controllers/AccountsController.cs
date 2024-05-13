using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services;
using Talabat.Service;

namespace Talabat.APIs.Controllers
{

    public class AccountsController : ApiBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountsController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }





        [HttpPost]
        public async Task<ActionResult<AppUserDto>> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            
            if(user is null )
                return Unauthorized(new ApiErrorResponse(401));

            var Result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (!Result.Succeeded)
                return Unauthorized(new ApiErrorResponse(401));

            return Ok(new AppUserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = _tokenService.GenerateToken(user)

            });
        }





        [HttpPost]
        public async Task<ActionResult<AppUserDto>> Register(RegisterDto model)
        {

            if (CheckEmailExist(model.Email).Result.Value)
                return BadRequest(new ApiValidationErrorResponse()
                {
                    Errors = new string[] { "This Email is Already Exist" }
                });

            var user = new AppUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.Email.Split('@')[0],
                PhoneNumber = model.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(new ApiErrorResponse(400));

            return Ok(new AppUserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token =  _tokenService.GenerateToken(user)
            });
        }





        //[Authorize(AuthenticationSchemes = "Bearer")]
        //[Authorize(JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "Bearer")]
        [HttpGet]
        public async Task<ActionResult<AppUser>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var user = await _userManager.FindByEmailAsync(email);

            return Ok(new AppUserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token =  _tokenService.GenerateToken(user)
            });
        }






        [Authorize(Policy = "Bearer")]
        [HttpGet]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var user = await _userManager.FindUserWithAddressByEmail(User);

            var address = _mapper.Map<AddressDto>(user.Address);

            return Ok(address);
        }




        [Authorize(Policy = "Bearer")]
        [HttpPut]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto UpdatedAddress)
        {
            var address = _mapper.Map<AddressDto, Address>(UpdatedAddress);

            var user = await _userManager.FindUserWithAddressByEmail(User);

            //address.Id = user.OrderAddress.Id;
            user.Address = address;

            var result = await _userManager.UpdateAsync(user);

            if(!result.Succeeded)
                return BadRequest(new ApiErrorResponse(400));

            return Ok(UpdatedAddress);
        }



        [HttpGet]
        public async Task<ActionResult<bool>> CheckEmailExist(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }




    }
}
