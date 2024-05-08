﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bookAppApi.Entities;
using bookAppApi.Model;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Data;

namespace bookAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json", "application/xml")]
    [Consumes("application/json", "application/xml")]
    public class UserController : ControllerBase
    {
        private readonly databaseContext _dbContext;

        public UserController(databaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/User
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var users = _dbContext.User
                .Select(u => new User
                {
                    id = u.Id,
                    username = u.Username,
                    password = u.Password,
                    level = u.Level,
                    image = u.Image

                })
                .ToList();

            if (users.Any())
            {
                return Ok(users);
            }
            else
            {
                return NotFound();
            }
        }

        // GET: api/User/{id}
        [HttpGet("Get/{id}")]
        public IActionResult GetById(int id)
        {
            Entities.Users entityUser = _dbContext.User.FirstOrDefault(u => u.Id == id);

            if (entityUser != null)
            {
                Model.User modelUser = new Model.User
                {
                    id = entityUser.Id,
                    username = entityUser.Username,
                    password = entityUser.Password,
                    level = entityUser.Level,
                    image = entityUser.Image,
                };

                return Ok(modelUser);
            }
            else
            {
                return NotFound();
            }
        }

        // POST: api/User
        [HttpPost("Create")]
        public IActionResult Create([FromBody] Model.User userModel)
        {
            if (ModelState.IsValid)
            {
                Entities.Users newUser = new Entities.Users()
                {
                    Username = userModel.username,
                    Password = userModel.password,
                    Level = userModel.level,
                    Image = userModel.image
                };

                _dbContext.User.Add(newUser); 
                try
                {
                    _dbContext.SaveChanges();
                    return StatusCode(201, "User created successfully.");
                }
                catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2627)
                {
                    return StatusCode(406, $"Username '{userModel.username}' already exists.");
                }
                catch (DbUpdateException ex)
                {
                    // Handle other database update exceptions
                    return StatusCode(500, "Error creating user. Please try again later.");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        // PUT: api/User/{id}
        [HttpPut("Update/{id}")]
        public IActionResult Update(int id, [FromBody] Model.User userModel)
        {
            if (ModelState.IsValid)
            {
                Entities.Users existingUser = _dbContext.User.FirstOrDefault(u => u.Id == id);

                if (existingUser != null)
                {
                    existingUser.Username = userModel.username;
                    existingUser.Password = userModel.password;
                    existingUser.Level = userModel.level;
                    existingUser.Image = userModel.image;

                    _dbContext.SaveChanges();

                    return Ok("User updated successfully.");
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // DELETE: api/User/{id}
        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            Entities.Users user = _dbContext.User.FirstOrDefault(u => u.Id == id);

            if (user != null)
            {
                _dbContext.User.Remove(user);
                _dbContext.SaveChanges();
                return StatusCode(201, "User deleted successfully.");
            }
            else
            {
                return NotFound();
            }
        }
    }
}
