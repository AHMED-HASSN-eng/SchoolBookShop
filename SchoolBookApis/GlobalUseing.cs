global using Microsoft.EntityFrameworkCore;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using SchoolBookShop.Data;
global using SchoolBookShop.Options;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.IdentityModel.Tokens;
global using System.Text;
global using SchoolBookShop.Authentication;
global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;
global using SchoolBookShop.Repositories.Services;
global using Microsoft.Extensions.Options;
global using Microsoft.AspNetCore.Mvc;
global using SchoolBookShop.Model;
global using SchoolBookShop.Dtos.AccountDto;
global using SchoolBookShop.Repositories.Interfaces;
global using Humanizer;
global using SchoolBookShop.Models;
global using System.ComponentModel.DataAnnotations.Schema;
global using AutoMapper;
global using SchoolBookShop.Dtos.BookDto;
global using SchoolBookShop.Dtos.HelperDto;
global using Microsoft.AspNetCore.Identity;
global using System.Diagnostics;
global using CartItem = SchoolBookShop.Models.CartItem;
global using Cart = SchoolBookShop.Models.Cart;
global using SchoolBookShop.Dtos.Cart;
global using OrderItem = SchoolBookShop.Models.OrderItem;
global using SchoolBookShop.Dtos.Order;
global using Microsoft.AspNetCore.Authorization;




