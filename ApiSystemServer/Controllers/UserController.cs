﻿using ApiSystemServer.Model;
using AutoMapper;
using Domain.Entity;
using Domain.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;
using Services.Validations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiSystemServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("Bearer")]

    public class UserController : ControllerBase
    {
        #region Declarações
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        #endregion

        #region Construtor
        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }
        #endregion

        #region InserirUsuario
        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        [EnableCors("AllowOrigin")]
        public async Task<IActionResult> Put(User user)
        {
            HttpContext.Response.ContentType = "application/json";

            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse("error", ModelState));
            }

            try
            {
                bool retun = await _userService.CreateUserAsync(user, new ModelStateWrapper(ModelState));

                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse("error", ModelState));
                }

                return Ok(new ApiResponse("OK", "Usuário cadastrado com sucesso"));


            }
            catch (Exception e)
            {
                return StatusCode(500, "Erro interno do servidor" + e.Message);
            }

        }
        #endregion

        #region Retornar Usuario por Id
        [HttpGet("{id}", Name = "GetUser")]
        [Authorize(Roles = "CRIAR_USUARIO")]

        public async Task<IActionResult> Get(int id)
        {
            HttpContext.Response.ContentType = "application/json";
            IEnumerable<UserViewModel> userGet;
            try
            {
                var usuarios = await _userService.GetUserAsync(id, new ModelStateWrapper(ModelState));

                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse("error", ModelState));
                }

                userGet = _mapper.Map<IEnumerable<UserViewModel>>(usuarios);
                return Ok(userGet);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Erro interno do servidor " + e.Message);
            }

        }
        #endregion

        #region Retorna todos os Usuarios
        [HttpGet]
        [Authorize(Roles = "CRIAR_USUARIO")]
        public async Task<IActionResult> Get()
        {
            HttpContext.Response.ContentType = "application/json";
            IEnumerable<UserViewModel> userGet;
            try
            {
                var usuarios = await _userService.GetUserAsync(0, new ModelStateWrapper(ModelState));

                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse("error", ModelState));
                }

                userGet = _mapper.Map<IEnumerable<UserViewModel>>(usuarios);
                return Ok(userGet);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Erro interno do servidor " + e.Message);
            }

        }
        #endregion

        #region Editar Usuario
        [HttpPost]
        [Route("Edit")]
        [Authorize(Roles = "CRIAR_USUARIO")]
        public async Task<IActionResult> Edit(User user)
        {
            HttpContext.Response.ContentType = "application/json";
            UserViewModel userGet;
            try
            {
                var usuarios = await _userService.EditUserAsync(user, new ModelStateWrapper(ModelState));

                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse("error", ModelState));
                }

                userGet = _mapper.Map<UserViewModel>(usuarios);
                return Ok(userGet);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Erro interno do servidor " + e.Message);
            }

        }
        #endregion      

    }
}