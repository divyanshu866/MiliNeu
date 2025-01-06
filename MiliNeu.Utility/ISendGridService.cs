using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiliNeu.Utility
{
    public interface ISendGridService
    {
        public Task SendEmailAsync(string username, string toEmail, string subject, string message);
    }
}
