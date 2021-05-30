using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PDRProvBackEnd.DTOModels;
using PDRProvBackEnd.Models;
using PDRProvBackEnd.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDRProvBackEnd.Services
{

    public class MessageContactService : IMessageContact
    {

        private IConfiguration _configuration;
        private readonly ILogger<MessageContactService> _log;
        private IPDRGlobal<Models.MessageContact> _pawaRepo;
        public MessageContactService(IPDRGlobal<MessageContact> pawaRepo, IConfiguration configuration, ILogger<MessageContactService> logger)
        {
            this._configuration = configuration;
            this._log = logger;
            this._pawaRepo = pawaRepo;
        }

        #region Create Message contact
        public async Task<MessageContactModel> CreateMessageContact(MessageContactModel messageContact)
        {
                using (var db = _pawaRepo.PDRContext)
                {
                    db.Add(messageContact);
                    db.SaveChanges();
                }
            
            return messageContact;
        }
        #endregion

        #region Edit message contact Id
        public async Task<MessageContact> EditMessageContact(MessageContactModel messageContact)
        {
            MessageContact message = new MessageContact();
            using (var db = _pawaRepo.PDRContext)
            {
                message = db.MessageContacts.FirstOrDefault(x => x.Id.ToString() == messageContact.Id);
                if (message.Title!= null)
                {
                    message.Title = messageContact.Title;
                    message.Body = messageContact.Body;
                    message.TypeContact = messageContact.TypeContact;
                    message.IsRead = messageContact.IsRead;

                    db.SaveChanges();
                }
            }
            return message;
        }
        #endregion

        #region list mesagge contact 
        public async Task<List<MessageContact>> ListMessageContact()
        {

            List<MessageContact> messageContacts = _pawaRepo.PDRContext.Set<MessageContact>().ToList();
            return messageContacts;
        }
        #endregion

        #region list mesagge contact by Id 
        public async Task<MessageContact> MessageContact(string Id)
        {
            MessageContact message = _pawaRepo.PDRContext.MessageContacts.FirstOrDefault(x => x.Id.ToString() == Id);
            return message;
        }
        #endregion
    }
}
