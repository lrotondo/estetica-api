using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using dentist_panel_api.DTO;
using dentist_panel_api.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using static dentist_panel_api.Services.MessagesServices;

namespace dentist_panel_api.Services
{
    public class MessagesServices : ControllerBase
    {

        private readonly IConfiguration _configuration;

        public class Languaje
        {
            public string code { get; set; }
        }

        public class Parameter
        {
            public string type { get; set; }
            public string text { get; set; }
        }

        public class Component
        {
            public string type { get; set; }
            public Parameter[] parameters { get; set; }
        }

        public class Template
        {
            public string name { get; set; }
            public Languaje language { get; set; }
            public Component[] components { get; set; }
        }

        public class Text
        {
            public Boolean preview_url { get; set; }
            public string body { get; set; }
        }

        public class Message
        {
            public string messaging_product { get; set; }
            public string recipient_type { get; set; }
            public string to { get; set; }
            public string type { get; set; }
            public Template template { get; set; }
        }

        public class MessageText
        {
            public string messaging_product { get; set; }
            public string recipient_type { get; set; }
            public string to { get; set; }
            public string type { get; set; }
            public Text text { get; set; }
        }

        public MessagesServices(IConfiguration configuration)
        {
            this._configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

        }

        public ActionResult SendMessage(Appointment appointment)
        {
            var dentalToken = _configuration.GetValue<string>("DENTAL_TOKEN_WA_API");
            var codePhoneId = appointment.Owner.CodePhoneId;
            if (appointment == null)
            {
                appointment = new Appointment
                {
                    Patient = new Patient
                    {
                        Name = "Leo"
                    },
                    StartDate = DateTime.Now
                };
            }
            appointment.Patient.PhoneNumber = "54" + appointment.Patient.PhoneNumber;

            var client = new HttpClient();       


            /* var mensaje = appointment.Patient.Name + ", te recordamos que tenés un turno odontológico para el dia de hoy " +
                     $"a las {appointment.StartDate.ToString("HH:mm")} hs.";
             var message2 = new MessageText
             {
                 messaging_product = "whatsapp",
                 to = "541157694923",                
                 type = "text",
                 text = new Text
                 {
                     preview_url = false,
                     body = mensaje
                 }
             };*/

            var mensaje = new Message
            {
                messaging_product = "whatsapp",
                to = appointment.Patient.PhoneNumber,
                type = "template",
                template = new Template
                {
                    name = "appointment",
                    language = new Languaje
                    {
                        code = "es_AR"
                    },
                    components = new Component[]
                    {
                        new Component
                        {
                            type = "body",
                            parameters = new Parameter[]
                            {
                                new Parameter
                                {
                                    type = "text",
                                    text = appointment.Patient.Name
                                },
                                new Parameter
                                {
                                    type = "text",
                                    text = appointment.StartDate.ToString("HH:mm")
                                }
                            }
                        }
                    }
                }
            };
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {dentalToken}");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpContent bodyTemplate = new StringContent(JsonConvert.SerializeObject(mensaje), Encoding.UTF8, "application/json");
            var response = client.PostAsync($"https://graph.facebook.com/v15.0/{codePhoneId}/messages", bodyTemplate).Result;           
            return new OkObjectResult(response);
        }
    }
}
