using AsiaMoneyer.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using AsiaMoneyer.Client.Dtos;

namespace AsiaMoneyer.WebApi.Controllers
{
    [RoutePrefix("api/billing")]
    [Authorize]
    public class BillingController : BaseApiController
    {
        Client.IClientAppService _clientAppService;
        Client.ISubscriptionAppService _subscriptionAppService;
        Client.IBillingAppService _billingAppService;

        public BillingController(Client.IClientAppService clientAppService, Client.ISubscriptionAppService subscriptionAppService, Client.IBillingAppService billingAppService)
        {
            this._clientAppService = clientAppService;
            this._subscriptionAppService = subscriptionAppService;
            this._billingAppService = billingAppService;
        }

        [HttpPost]
        public IHttpActionResult PlaceOrder(ProductPriceDto productPriceDto)
        {
            try
            {
                this._subscriptionAppService.UserId = User.Identity.GetUserId();
                InvoiceDto invoiceDto = this._subscriptionAppService.RegisterNewSubscription(productPriceDto.ProductId, productPriceDto.Id, this._subscriptionAppService.UserId);
                return Ok(invoiceDto);
            }
            catch (Exception ex)
            {
                return new HttpActionResult(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public IHttpActionResult Purchase(PaymentObjectDto paymentObjectDto)
        {
            try
            {
                this._subscriptionAppService.UserId = User.Identity.GetUserId();
                InvoiceDto invoiceDto = this._subscriptionAppService.ProceedPurchaseWithPaymentMethod(paymentObjectDto.InvoiceId, paymentObjectDto.PaymentMethodId);
                return Ok(invoiceDto);
            }
            catch (Exception ex)
            {
                return new HttpActionResult(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public IHttpActionResult PaymentSuccess(PaymentObjectDto paymentObjectDto)
        {
            try
            {
                // Test Onlye
                paymentObjectDto.PriceId = Guid.NewGuid().ToString();
                paymentObjectDto.PaidAmount = 5;
                paymentObjectDto.PaymentDate = DateTime.Now;

                this._subscriptionAppService.UserId = User.Identity.GetUserId();
                this._subscriptionAppService.PaymentSuccess(paymentObjectDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return new HttpActionResult(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        
        [HttpPost]
        public IHttpActionResult GetPaymentMethods()
        {
            try
            {
                this._billingAppService.UserId = User.Identity.GetUserId();
                var paymentMethods = this._billingAppService.GetPaymentMethods();
                return Ok(paymentMethods);
            }
            catch (Exception ex)
            {
                return new HttpActionResult(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public IHttpActionResult GetInvoiceProductPrices(PaymentObjectDto paymentObjectDto)
        {
            try
            {
                this._billingAppService.UserId = User.Identity.GetUserId();
                var subscription = this._billingAppService.GetInvoiceProductPricesInSubscription(paymentObjectDto);
                return Ok(subscription);
            }
            catch (Exception ex)
            {
                return new HttpActionResult(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        
        [HttpPost]
        public IHttpActionResult PayInvoice(PaymentObjectDto paymentObjectDto)
        {
            try
            {
                this._billingAppService.UserId = User.Identity.GetUserId();
                var invoice = this._billingAppService.UpdateInvoiceProductPrice(paymentObjectDto);
                return Ok(invoice);
            }
            catch (Exception ex)
            {
                return new HttpActionResult(System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
