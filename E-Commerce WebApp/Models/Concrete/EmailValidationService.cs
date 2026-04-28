using DnsClient;

namespace E_Commerce_WebApp.Models.Concrete
{
    public class EmailValidationService
    {
        public async Task<bool> IsEmailDomainValidAsync(string email)
        {
            try
            {
                var domain = email.Split('@')[1];
                var lookup = new LookupClient();
                var result = await lookup.QueryAsync(domain, QueryType.MX);
                return result.Answers.MxRecords().Any();
            }
            catch
            {
                return false;
            }
        }
    }
}