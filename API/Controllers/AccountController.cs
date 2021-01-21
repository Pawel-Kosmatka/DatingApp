﻿using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public AccountController(DataContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await IsUserExisting(registerDto.UserName)) return BadRequest("User name is taken");

            var user = MapToAppUserAndHashPassword(registerDto);

            await AddAppUserToDatabase(user);

            return new UserDto(user.UserName, _tokenService.CreateToken(user), user.Photos.FirstOrDefault(p => p.IsMain)?.Url);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await GetUserFromDatabase(loginDto.UserName);

            if (user == null) return Unauthorized("Invalid user name");

            if (!IsPasswordValid(user, loginDto)) return Unauthorized("Invalid password");

            return new UserDto(UserName: user.UserName, _tokenService.CreateToken(user), user.Photos.FirstOrDefault(p => p.IsMain)?.Url);
        }

        private async Task<AppUser> GetUserFromDatabase(string userName)
        {
            return await _context.Users.Include(u => u.Photos).SingleOrDefaultAsync(u => u.UserName == userName.ToLower());
        }

        private static bool IsPasswordValid(AppUser user, LoginDto loginDto)
        {
            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return false;
            }

            return true;
        }

        private async Task<bool> IsUserExisting(string username)
        {
            return await _context.Users.AnyAsync(u => u.UserName == username.ToLower());
        }

        private static AppUser MapToAppUserAndHashPassword(RegisterDto registerDto)
        {
            using var hmac = new HMACSHA512();
            return new AppUser
            {
                UserName = registerDto.UserName.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };
        }

        private async Task AddAppUserToDatabase(AppUser user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}
