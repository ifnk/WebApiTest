﻿namespace WebApiTest.Api.Dto
{
    public class UserUpdateDto
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public bool Status { get; set; } = true;
    }
}