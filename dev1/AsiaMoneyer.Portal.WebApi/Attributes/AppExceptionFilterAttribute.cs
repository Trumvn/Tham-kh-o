using AsiaMoneyer.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Filters;

namespace AsiaMoneyer.Portal.WebApi
{
    public class AppExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public IAutoMessagingAppService AutoMessagingAppService {get; set;}

        //Populate Exception message within constructor
        public AppExceptionFilterAttribute()
        {
            this.Mappings = new Dictionary<Type, HttpStatusCode>();
            this.Mappings.Add(typeof(ArgumentNullException), HttpStatusCode.BadRequest);
            this.Mappings.Add(typeof(ArgumentException), HttpStatusCode.BadRequest);
            this.Mappings.Add(typeof(DivideByZeroException), HttpStatusCode.BadRequest);

            this.AutoMessagingAppService = new MessagingInstantAppService(new GoogleEmailProvider(), new AutoMessagingMessageAppService(new AutoMessagingDatabindingHelperAppService()));
        }
        public IDictionary<Type, HttpStatusCode> Mappings
        {
            get;
            //Set is private to make it singleton
            private set;
        }

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext != null && actionExecutedContext.Exception != null)
            {
                var exception = actionExecutedContext.Exception;
                string type = exception.GetType().ToString();

                Dictionary<string, string> values = new Dictionary<string, string>();
                values.Add("{{binding_text_exception_message}}", exception.Message);
                values.Add("{{binding_text_exception_tracer}}", exception.StackTrace);

                this.AutoMessagingAppService.SendEmail(Commons.Constants.AUTO_EMAIL_TEMPLATE_SYSTEM_ERROR, values);

                // LookUp Mapping Dictionary to get exception type
                if (this.Mappings.ContainsKey(exception.GetType()))
                {
                    //Get Status code from Dictionary
                    var httpStatusCode = this.Mappings[exception.GetType()];
                    // Create Message Body with information
                    throw new HttpResponseException(new HttpResponseMessage(httpStatusCode)
                    {
                        Content = new StringContent("Method Access Exception " + actionExecutedContext.Exception.Message),
                        ReasonPhrase = actionExecutedContext.Exception.Message
                    });
                }
                else
                {
                    //Else part executes means there is not information in repository so it is some kind of anonymous exception
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent("System is failure to process request"),
                        ReasonPhrase = "System is failure to process request"
                    });
                }

            }
        }
    }
}