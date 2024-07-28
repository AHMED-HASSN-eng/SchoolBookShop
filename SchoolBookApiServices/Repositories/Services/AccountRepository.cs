namespace SchoolBookShop.Repositories.Services
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _autoMapper;
        private readonly JwtOptions _jwtOptions;

       
        public AccountRepository(UserManager<ApplicationUser> userManager,JwtOptions jwtOptions,
            RoleManager<IdentityRole> roleManager,IMapper autoMapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _autoMapper = autoMapper;
            _jwtOptions =jwtOptions;
        }

       

        public async Task<string> AddRoleAsync(AddRolesModel addrole)
        {
            var user = await _userManager.FindByIdAsync(addrole.UserId);
            if (user == null)
                return "User Is Not Found";
            if (await _userManager.IsInRoleAsync(user, addrole.Role))
                return "User Already Assigned To This Role";
            if (await _roleManager.FindByNameAsync(addrole.Role) is null)
                return $"Role {addrole.Role} does not exist";
            var result=await _userManager.AddToRoleAsync(user, addrole.Role);
            if (result.Succeeded)
                return string.Empty;
            return "somthing wrong";
        }

        public async Task<JwtSecurityToken> CreateTokenAsync(ApplicationUser authModel)
        {
            var userclaim = await _userManager.GetClaimsAsync(authModel);
            var userroles=await _userManager.GetRolesAsync(authModel);
            var claimroles = new List<Claim>();
            foreach (var role in userroles)
            {
                claimroles.Add(new Claim("role", role));
            }
            var claimss = new[]
            {
                new Claim(ClaimTypes.Name,authModel.UserName),
               new Claim(ClaimTypes.Email,authModel.Email),
               new Claim("UserId",authModel.Id)
            }
            .Union(claimroles)
            .Union(userclaim);
           
            var symmetricsecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
            var signingcredentia = new SigningCredentials(symmetricsecurityKey, SecurityAlgorithms.HmacSha256);
            var jwtsecuritytoken = new JwtSecurityToken
            (
                issuer: _jwtOptions.Issure,
                audience: _jwtOptions.Audience,
                claims: claimss,
                signingCredentials: signingcredentia,
                expires: DateTime.Now.AddDays(_jwtOptions.Duration)
            ); 
            return jwtsecuritytoken;
        }
        public async Task<Authinformation> LoginAsync(LoginDto dto)
        {
            
            var user= await _userManager.FindByEmailAsync(dto.Email);
            if (user is null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                return new Authinformation { Message = "This Email Or Password Is Wrong" };
           var userroles=await _userManager.GetRolesAsync(user);
            var securitytoken = await CreateTokenAsync(user);
            return new Authinformation
            {
                Email = dto.Email,
                UserName = user.UserName,
                IsAuthenticated = true,
                ExpireDate = securitytoken.ValidTo,
                Roles = userroles.ToList(),
                Token = new JwtSecurityTokenHandler().WriteToken(securitytoken)
            };
        }

        public async Task<Authinformation> RegisterAsync(RegisterDto dto)
        {
            if (await _userManager.FindByEmailAsync(dto.Email) is not null)
                return new Authinformation { Message = "This Email Is Used Before" };
            if (await _userManager.FindByNameAsync(dto.UserName) is not null)
                return new Authinformation { Message = "This UserName Is Used Before" };
            var user = new ApplicationUser
            {
                UserName = dto.UserName,
                Email = dto.Email,
                FirstName = dto.FirstName,
                Address=dto.Address,
                LastName = dto.LastName,
                PhoneNumber = dto.Phone,
            };
            //var user = _autoMapper.Map<ApplicationUser>(dto);
            var result=await _userManager.CreateAsync(user,dto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Empty;
                foreach(var error in result.Errors)
                {
                    errors += $"{error.Description},";
                }
                return new Authinformation { Message = errors };
            }
            await _userManager.AddToRoleAsync(user, "User");
            var securitytoken = await CreateTokenAsync(user);
            return new Authinformation
            {
                UserName = dto.UserName,
                Email = dto.Email,
                IsAuthenticated = true,
                ExpireDate = securitytoken.ValidTo,
                Roles = new List<string> { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(securitytoken)
            };
        }
    }
}
