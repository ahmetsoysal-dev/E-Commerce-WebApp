using E_Commerce_WebApp.Models.MVVM;
using System.Net;
using System.Net.Mail;

namespace E_Commerce_WebApp.Models.Concrete
{
    public class UserRepository
    {
        Context context = new Context();

        public bool LoginControl(string Email)
        {
            // ORM = ado.net (select,insert,update,delete)
            // select * from Users where Email = 'sedat@hotmail.com'

            // ORM = entityframeworkcore
            // =>  lamdda expression
            User? result = context.Users.FirstOrDefault(u => u.Email == Email);
            if (result == null) return false;
            return true;
        }

        public bool Add(User user)
        {
            try
            {
                user.Active = true;
                context.Users?.Add(user);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string LoginControl(User user)
        {
            // context.Users üzerinden veritabanındaki her bir kullanıcı kaydını geçici olarak u adıyla ele alır
            //bu kayıtların email ve şifre alanlarını, formdan gelen user nesnesindeki bilgilerle karşılaştırır
            //Eşleşme bulunursa, bu kayıt usr adlı değişkene atanır
            User? usr = context.Users.FirstOrDefault(u => u.Email == user.Email && u.Password == user.Password);

            if (usr == null)
            {
                //login/şifre yanlış
                return "error";
            }
            else
            {
                //login/şifre doğru
                if (usr.IsAdmin)
                {
                    return usr.NameSurname!;
                }
                else
                {
                    return usr.Email!;
                }
            }
        }

        public static User Get(string email)
        {
            using (Context context = new Context())
            {
                User? user = context.Users?.FirstOrDefault(u => u.Email == email);
                return user;
            }
        }

        public User Get(int? id)
        {
            User? user = context.Users?.FirstOrDefault(u => u.UserID == id);
            return user;
        }

        public bool Update(User user)
        {
            try
            {
                var dbUser = context.Users.FirstOrDefault(u => u.UserID == user.UserID);
                if (dbUser == null)
                    return false;

                dbUser.NameSurname = user.NameSurname;
                dbUser.Email = user.Email;
                dbUser.Telephone = user.Telephone;
                dbUser.InvoicesAddress = user.InvoicesAddress;

                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void Send_Email(string OrderGroupGUID)

        {

            SmtpClient client = new SmtpClient("Host - 192.168.35.123");


            client.UseDefaultCredentials = false;

            client.Credentials = new NetworkCredential("user - sedat", "pass - 123.");

            MailMessage mailMessage = new MailMessage();

            mailMessage.From = new MailAddress("emailfrom - iakademi");

            mailMessage.To.Add("recipient@gmail.com");

            mailMessage.Body = "body";
        }
    }
}
