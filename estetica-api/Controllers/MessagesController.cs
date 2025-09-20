using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using dentist_panel_api.DTO;
using dentist_panel_api.Entities;
using dentist_panel_api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace dentist_panel_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        public MessagesServices messagesServices;
        private readonly UserServices _userServices;

        public MessagesController(MessagesServices messagesServices)
        {            
            this.messagesServices = messagesServices;
        }
       
        [HttpPost()]
        public async Task<ActionResult> SendMessage()
        {
            //ApplicationUser user = await _userServices.GetCurrentUser(this.User);
            return messagesServices.SendMessage(null);           
        }        
      
    }
}
