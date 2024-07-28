using Humanizer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SchoolBookShop.Repositories.Services
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplictionDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private List<string> Extentions = new() { ".jpg", ".png" };


        public BookRepository(ApplictionDbContext context,IMapper mapper
            ,IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<ResponseDto> GetAllBookAsync()
        {
            var books= await _context.Books.ToListAsync();
            if(books is null || books.Count == 0 )
                return new ResponseDto { StatusCode = 404, Message = $"No found book in repository", IsSuccess = false };
            var model = _mapper.Map<List<BookDto>>(books);
            return new ResponseDto { Model= model ,IsSuccess=true,StatusCode=200};
        }

        public async Task<ResponseDto> GetBookAsync(int id)
        {
            var book =await _context.Books.FindAsync(id);

            if (book == null)
            {
                return new ResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = $"Not found Book With this Id : {id}"
                };
            }
            var model =  _mapper.Map<BookDto>(book);
            return new ResponseDto
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "",
                Model=model
            };
        }
        public async Task<ResponseDto> GetBookByPrice(int price=700)
        {
            var books=await _context.Books.Where(s=>s.Price<=price).ToListAsync();
            if (books is null||books.Count==0)
                return new ResponseDto { StatusCode = 404, Message = $"No found Book with this Price or less than it", IsSuccess = false };
            var model=_mapper.Map<List<BookDto>>(books);
            return new ResponseDto { 
            StatusCode=200,
            Model=model,
            IsSuccess=true,
            };
        }
        public async Task<ResponseDto> GetBookBySearch(string searchtext)
        {
            var books = await _context.Books.Where(s=>s.StudyYear.Contains(searchtext)||s.CompanyName.Contains(searchtext)
            ||s.Description.Contains(searchtext)||s.Subject.Contains(searchtext)).ToListAsync();
            if (books is null || books.Count == 0)
                return new ResponseDto { StatusCode = 404, Message = $"No found Books with your text search", IsSuccess = false };
            var model = _mapper.Map<List<BookDto>>(books);
            return new ResponseDto
            {
                StatusCode = 200,
                Model = model,
                IsSuccess = true,
            };
        }
        private async Task<ApplicationUser> GetCurrentUser()
        {
             var userclaim=_httpContextAccessor.HttpContext!.User;
            return await _userManager.FindByIdAsync(userclaim.Claims.FirstOrDefault(t => t.Type == "UserId").Value);
        }
        private async Task<ResponseEvalutePhotoDto> EvaluatePhtos(List<IFormFile> files)
        {
            var photopaths = new List<string>();
            var photos=new List<BookPhoto>();
            foreach (var photo in files)
            {

                if (!Extentions.Contains(Path.GetExtension(photo.FileName).ToLower()))
                {
                    return new ResponseEvalutePhotoDto
                    {
                        Message = $"Extention of {photo.FileName}  should be jpg or png",
                        IsSuccess = false,
                        StatusCode = 400,

                    };
                }
                if (photo.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await photo.CopyToAsync(memoryStream);
                        if (memoryStream.Length < 2097152)
                        {
                            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "bookimages");
                            string filename = $"{photo.FileName}";
                            string fullpath = Path.Combine(path, filename);
                            using (var stream = new FileStream(fullpath, FileMode.Create))
                            {
                                await photo.CopyToAsync(stream);
                            }
                            var newPhoto = new BookPhoto()
                            {
                                Data = memoryStream.ToArray(),
                                Description = photo.FileName,
                                FileExtension = Path.GetExtension(photo.FileName),
                                Size = photo.Length,

                            };
                           
                            photopaths.Add(fullpath);
                            photos.Add(newPhoto);
                        }
                        else
                        {
                            return new ResponseEvalutePhotoDto
                            {
                                Message = $"{photo.FileName} should be less than 2 MB",
                                IsSuccess = false,
                                StatusCode = 400,
                            };
                        }
                    }
                }
            }
            var scriptPath = Path.Combine(Directory.GetCurrentDirectory(), "Scripts", "evaluate_images.py");
            var startInfo = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = $"{scriptPath} {string.Join(" ", photopaths)}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            string result;
            string error;

            using (var process = Process.Start(startInfo))
            {
                result = await process.StandardOutput.ReadToEndAsync();
                error = await process.StandardError.ReadToEndAsync();
            }

            Debug.WriteLine($"Python script output: {result}");
            Debug.WriteLine($"Python script error: {error}");

            if (string.IsNullOrWhiteSpace(result))
            {
                return new ResponseEvalutePhotoDto
                {
                    Message = $"No output from Python script. Error: {error}",
                    IsSuccess = false,
                    StatusCode = 400
                };
            }

            JArray jsonResponse;
            try
            {
                jsonResponse = JArray.Parse(result);
            }
            catch (JsonReaderException ex)
            {
                Debug.WriteLine($"JSON parsing error: {ex.Message}");
                return new ResponseEvalutePhotoDto
                {
                    Message = $"JSON parsing error: {ex.Message}. Output: {result}",
                    IsSuccess = false,
                    StatusCode = 400
                };
            }

            int meanquality = 0;
            foreach (var item in jsonResponse)
            {
                meanquality += (int)item["quality_score"];
            }
            meanquality /= jsonResponse.Count();
            int final_score = meanquality;
            return new ResponseEvalutePhotoDto {
               IsSuccess=true,
               StatusCode=200,
               Score=final_score,
               photos=photos

            };
        }
        public async Task<ResponseDto> AddBookAsync(AddBookDto dto)
        {
           
            var user= await GetCurrentUser();
            if(user==null) {
                return new ResponseDto
                {
                    Message = $"This User Not Found",
                    IsSuccess = false,
                    StatusCode = 404,
                };
            }
            var book = _mapper.Map<Book>(dto);
            book.UserId = user.Id;
          
            var answer = await EvaluatePhtos(dto.Photo);
            if (!answer.IsSuccess)
                return new ResponseDto { Message =answer.Message,IsSuccess=answer.IsSuccess, StatusCode =answer.StatusCode};
            // Continue processing and adding the book to the database...  
                book.Rate = (int)answer.Score;
                 var photos =answer.photos ;
                await _context.Books.AddAsync(book);
                var response = _context.Entry(book);
                if (response.State == EntityState.Added)
                {
                     await _context.SaveChangesAsync();
                    foreach (var photo in photos)
                    {
                        photo.BookId = book.Id;
                        await _context.BookPhotos.AddAsync(photo);
                    }
                    await _context.SaveChangesAsync();
                    return new ResponseDto
                    {
                        Message = "",
                        IsSuccess = true,
                        StatusCode = 200,
                        Model = book
                    };
                }
                else
                {
                    return new ResponseDto
                    {
                        Message = "Book was not added",
                        IsSuccess = false,
                        StatusCode = 400
                    };
                }
            
        }
        public async Task<ResponseDto> UpdateBook(int id,UpdateDto dto )
        {
            var book =await _context.Books.FindAsync(id);
            if (book is null) return new ResponseDto { IsSuccess = false, StatusCode = 400, Message = "this book is not exist" };
            var model = _mapper.Map<Book>(dto);
            model.Id = id;
            model.Rate = book.Rate;
            model.BookPhotos = book.BookPhotos;
            model.UserId = book.UserId;
            _context.Entry(book).CurrentValues.SetValues(model);
            var result = _context.Entry(book);
            if(result.State==EntityState.Modified)
            {
                await _context.SaveChangesAsync();
                return new ResponseDto
                {
                    StatusCode=200,
                    IsSuccess=true,
                    Model=model
                };
            }
            return new ResponseDto
            {
                StatusCode = 400,
                IsSuccess = false,
                Message = "the process of update did not compelet"
            };
            
        }
        public async Task<ResponseDto> DeleteBook(int id)
        {
            var book=await _context.Books.FindAsync(id);
            if (book is null) return new ResponseDto { StatusCode = 400, IsSuccess = false, Message = "this Book is not exist" };
             _context.Books.Remove(book);
            var result = _context.Entry(book);
            if(result.State==EntityState.Deleted)
            {
                await _context.SaveChangesAsync();

                return new ResponseDto
                {
                    StatusCode = 200,
                    IsSuccess = true,
                    Model = book,
                    Message = "Book Delete Successfuly"
                };
            }
            return new ResponseDto
            {
                StatusCode = 400,
                IsSuccess = false,
                Message = "the process of Delete did not compelet"
            };
        }
    }
}
