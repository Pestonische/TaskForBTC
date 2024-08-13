using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TaskForBTC.Services.IServices
{
    //Интерфейс, реализующий отправку сообщений на почту к пользователю
    public interface IEmailService
    {
        public Task SendEmailAsync(string email, string subject, string message);
    }
}
