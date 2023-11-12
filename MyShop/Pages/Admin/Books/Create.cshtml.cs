using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyShop.MyHelpers;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace MyShop.Pages.Admin.Books
{
    [RequireAuth(RequiredRole = "admin")]
    public class CreateModel : PageModel
    {

        [BindProperty]
        [Required(ErrorMessage = "The Title is required.")]
        [MaxLength(100, ErrorMessage = "The Title cannot exceed 100 characters.")]
        public string Title { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage = "The Authors are required.")]
        [MaxLength(255, ErrorMessage = "The Authors cannot exceed 255 characters.")]
        public string Authors { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage = "The ISBN is required.")]
        [MaxLength(100, ErrorMessage = "The ISBN cannot exceed 100 characters.")]
        public string Isbn { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage = "The Number of Pages is required.")]
        [Range(1, 10000, ErrorMessage = "The Number of Pages must be in a range from 1 to 10000.")]
        public int NumPages { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "The Price is required.")]
        public decimal Price { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "The Category is required.")]
        public string Category { get; set; } = "";

        [BindProperty]
        [MaxLength(1000, ErrorMessage = "The Description cannot exceed 1000 characters.")]
        public string? Description { get; set; } = "";//Description is an optional input

        [BindProperty]
        [Required(ErrorMessage = "The Image File is required.")]
        public IFormFile ImageFile { get; set; }

        public string errorMessage { get; set; } = "";
        public string successMessage { get; set; } = "";

        private IWebHostEnvironment webHostEnvironment;

        private readonly IConfiguration config;
        public CreateModel(IWebHostEnvironment env, IConfiguration configuration)
        {
            webHostEnvironment = env;
            config = configuration;
        }

        public void OnGet()
        {
        }

        public void OnPost()
        {
            if (!ModelState.IsValid)
            {
                errorMessage = "Data validation failed"; //Validate that all required data is entered
                return;
            }

            //check if the Description is null to give it an empty string value for delivery to the db
            if (Description == null) Description = "";

            //save the image file on the server
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(ImageFile.FileName);

            string imageFolder = webHostEnvironment.WebRootPath + "/images/books/";

            string imageFullPath = Path.Combine(imageFolder, newFileName);

            using (var stream = System.IO.File.Create(imageFullPath))
            {
                ImageFile.CopyTo(stream);
            }

            //save the new book to the database
            try
            {
                string connectionString = config.GetConnectionString("DefaultConnection");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO books " +
                    "(title, authors, isbn, num_pages, price, category, description, image_filename) VALUES " +
                    "(@title, @authors, @isbn, @num_pages, @price, @category, @description, @image_filename);";
                    
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@title", Title);
                        command.Parameters.AddWithValue("@authors", Authors);
                        command.Parameters.AddWithValue("@isbn", Isbn);
                        command.Parameters.AddWithValue("@num_pages", NumPages);
                        command.Parameters.AddWithValue("@price", Price);
                        command.Parameters.AddWithValue("@category", Category);
                        command.Parameters.AddWithValue("@description", Description);
                        command.Parameters.AddWithValue("@image_filename", newFileName);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
            successMessage = "Data saved correctly.";
            Response.Redirect("/Admin/Books/Index");
        }
    }
}
