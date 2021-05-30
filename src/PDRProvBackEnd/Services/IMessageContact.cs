using PDRProvBackEnd.DTOModels;
using PDRProvBackEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDRProvBackEnd.Services
{   
   public interface IMessageContact
    {
        Task<MessageContact> MessageContact(string Id);
        Task<MessageContactModel> CreateMessageContact(DTOModels.MessageContactModel messageContact);
        Task<List<MessageContact>> ListMessageContact();
        Task<MessageContact> EditMessageContact(MessageContactModel messageContact);
    }
}
