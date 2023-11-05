using EmailBox_Application.Services;
using EmailBox_Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailBox_Application.Interfaces
{
    public interface IConfirmationCodeServices
    {
        returnStatus GenerateConfirmationCode(UserRequestByUser Model, int length);
        returnStatus VerifyConfirmationCode(UserRequestWithCode Model);
    }
}
