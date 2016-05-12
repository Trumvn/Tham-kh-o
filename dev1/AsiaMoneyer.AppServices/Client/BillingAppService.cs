using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Client
{
    public class BillingAppService : AppServiceBase, IBillingAppService
    {
        public List<Dtos.PaymentMethodDto> GetPaymentMethods()
        {
            List<Entities.PaymentMethod> paymentMethods = this.UnitOfWork.PaymentMethodRepository.GetAvailablePaymentMethods();
            List<Dtos.PaymentMethodDto> paymentMethodDtos = AutoMapper.Mapper.Map<List<Entities.PaymentMethod>, List<Dtos.PaymentMethodDto>>(paymentMethods);

            return paymentMethodDtos;
        }

        public Dtos.SubscriptionDto GetInvoiceProductPricesInSubscription(Dtos.PaymentObjectDto paymentObjectDto)
        {
            if (paymentObjectDto == null)
            {
                throw new System.NullReferenceException("PaymentObjectDto cannot be null.");
            }

            if (String.IsNullOrEmpty(paymentObjectDto.InvoiceId))
            {
                throw new System.NullReferenceException("PaymentObjectDto.InvoiceId cannot be null.");
            }

            Entities.Invoice invoice = this.UnitOfWork.InvoiceRepository.Get(paymentObjectDto.InvoiceId);

            if (invoice == null)
            {
                throw new System.NullReferenceException("Invoice cannot be null.");
            }

            if (invoice.Subscription == null)
            {
                throw new System.NullReferenceException("Invoice.Subscription cannot be null.");
            }

            Dtos.SubscriptionDto subscriptionDto = AutoMapper.Mapper.Map<Entities.Subscription, Dtos.SubscriptionDto>(invoice.Subscription);

            List<Entities.ProductPrice> prices = this.UnitOfWork.ProductPriceRepository.GetProductPricesFromProductId(invoice.Subscription.ProductId);
            List<Dtos.ProductPriceDto> priceDtos = AutoMapper.Mapper.Map<List<Entities.ProductPrice>, List<Dtos.ProductPriceDto>>(prices);
            subscriptionDto.Product.ProductPrices = priceDtos;
            subscriptionDto.InvoiceId = invoice.Id;

            return subscriptionDto;
        }

        public Dtos.InvoiceDto UpdateInvoiceProductPrice(Dtos.PaymentObjectDto paymentObjectDto)
        {
            if (paymentObjectDto == null)
            {
                throw new System.NullReferenceException("PaymentObjectDto cannot be null.");
            }

            if (String.IsNullOrEmpty(paymentObjectDto.InvoiceId))
            {
                throw new System.NullReferenceException("PaymentObjectDto.InvoiceId cannot be null.");
            }

            if (String.IsNullOrEmpty(paymentObjectDto.PriceId))
            {
                throw new System.NullReferenceException("PaymentObjectDto.PriceId cannot be null.");
            }

            Entities.Invoice invoice = this.UnitOfWork.InvoiceRepository.Get(paymentObjectDto.InvoiceId);

            if (invoice == null)
            {
                throw new System.NullReferenceException("Invoice is not found.");
            }

            invoice.ProductPriceId = paymentObjectDto.PriceId;

            this.UnitOfWork.InvoiceRepository.Update(invoice, x => x.ProductPriceId);

            Dtos.InvoiceDto invoiceDto = AutoMapper.Mapper.Map<Entities.Invoice, Dtos.InvoiceDto>(invoice);

            return invoiceDto;
        }
    }
}
