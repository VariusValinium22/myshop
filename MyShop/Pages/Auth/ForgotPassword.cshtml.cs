using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyShop.MyHelpers;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace MyShop.Pages.Auth
{
    [RequireNoAuth]
    public class ForgotPasswordModel : PageModel
    {
        [BindProperty, Required(ErrorMessage = "The Email is Required."), EmailAddress]
        public string Email { get; set; } = "";

        public string errorMessage = "";
        public string successMessage = "";

        private readonly IConfiguration config;
        public ForgotPasswordModel(IConfiguration configuration)
        {
            config = configuration;
        }

        public void OnGet()
        {
        }

        public void OnPost()
        {
            if (!ModelState.IsValid)
            {
                errorMessage = "Data Validation Failed.";
                return;
            }

            // 1. create token 2. save token in the db 3. send token by email to the user
            try
            {
                string connectionString = config.GetConnectionString("DefaultConnection");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM users WHERE email=@email";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@email", Email);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string firstname = reader.GetString(1);
                                string lastname = reader.GetString(2);

                                //create a token
                                string token = Guid.NewGuid().ToString();
                                //save the token to the db using this method we will create later
                                SaveToken(Email, token);
                                //send the token by email to the user
                                string resetUrl = Url.PageLink("/Auth/ResetPassword") + "?token=" + token;
                                string username = firstname + " " + lastname;
                                string subject = "Password Reset";
                                string message = "Dear " + username + ",\n\n" +
                                    "You can reset your password using the following link:\n\n" +
                                    resetUrl + "\n\n" + "Best Regards.";

                                EmailSender.SendEmail(Email, username, subject, message);
                            }
                            else
                            {
                                errorMessage = "We have no user with the email address provided";
                                return;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
            successMessage = "Please check your email and click on the reset password link";
        }
        private void SaveToken(string email, string token)
        {
            try
            {
                string connectionString = config.GetConnectionString("DefaultConnection");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "DELETE FROM password_resets WHERE email=@email";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {   //inject this parameter into the sql query
                        command.Parameters.AddWithValue("@email", email);
                        command.ExecuteNonQuery();
                    }

                    sql = "INSERT INTO password_resets (email, token) VALUES (@email, @token)";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {   //inject these parameters into the sql query
                        command.Parameters.AddWithValue("@email", email);
                        command.Parameters.AddWithValue("@token", token);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
