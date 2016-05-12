using AsiaMoneyer.Client.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaMoneyer.Client
{
    public class SubscriptionAppService : AppServiceBase, ISubscriptionAppService
    {
        public bool CreateSubscription(Dtos.SubscriptionDto subscriptionDto)
        {
            try
            {
                Entities.Subscription subscription = new Entities.Subscription()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = subscriptionDto.UserId,
                    ProductId = subscriptionDto.ProductId,
                    SubscriptionTypeId = (int)Commons.Constants.SUBSCIPTION_TYPE.MONTH,
                    ValidDate = DateTime.Now,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedDate = DateTime.Now
                };

                this.UnitOfWork.SubscriptionRepository.Add(subscription);

                this.UnitOfWork.Save(subscription.UserId);
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                StringBuilder stb = new StringBuilder();
                foreach (var eve in e.EntityValidationErrors)
                {                                        
                    stb.AppendLine(String.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        stb.AppendLine(String.Format("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage));
                    }
                }

                throw new Exception(stb.ToString());
            }

            return true;
        }

        public SubscriptionDto GetUserCurrentSubscription(string userId)
        {
            Entities.Subscription subscription = this.UnitOfWork.SubscriptionRepository.GetUserCurrentSubscription(userId);
            SubscriptionDto subscriptionDto = AutoMapper.Mapper.Map<Entities.Subscription, SubscriptionDto>(subscription);

            SubscriptionTypeDto subscriptionTypeDto = AutoMapper.Mapper.Map<Entities.SubscriptionType, SubscriptionTypeDto>(subscription.SubscriptionType);
            subscriptionDto.SubscriptionType = subscriptionTypeDto;

            // check next invoice
            InvoiceDto nextNnvoiceDto = this.GetSubscriptionNextInvoice(subscription.Id, userId);

            if (nextNnvoiceDto != null)
            {
                subscriptionDto.NextInvoiceId = nextNnvoiceDto.Id;
            }

            subscriptionDto.CanUpgrade = this.CanUpgradeSubscription(subscription.Id);
            return subscriptionDto;
        }

        public List<ProductDto> GetUserUpgradeProducts(string userId)
        {
            List<ProductDto> productDtos = null;
            Entities.Subscription subscription = this.UnitOfWork.SubscriptionRepository.GetUserCurrentSubscription(userId);
            if(subscription != null)
            {
                List<Entities.Product> products = null;
                if(subscription.Product != null)
                {
                    int currentSubcriptionProductUpgradeLevel = subscription.Product.UpgradeLevel;
                    products = this.UnitOfWork.ProductRepository.GetAvailableUpgradeProductsFromLevel(currentSubcriptionProductUpgradeLevel);
                }

                productDtos = AutoMapper.Mapper.Map<List<Entities.Product>, List<ProductDto>>(products);

                foreach(Entities.Product product in products)
                {
                    List<ProductPriceDto> priceDtos = AutoMapper.Mapper.Map<List<Entities.ProductPrice>, List<ProductPriceDto>>(product.ProductPrices.ToList());
                    ProductDto productDto = productDtos.Where(x => x.Id == product.Id).FirstOrDefault();
                    productDto.ProductPrices = priceDtos;
                }
            }
            return productDtos;
        }

        public bool CreateStartSubscriptionForNewUser(string userId)
        {
            Entities.Product product = this.UnitOfWork.ProductRepository.GetAvailavleStartLevelProduct();

            var subscriptionDto = new SubscriptionDto()
            {
                UserId = userId,
                ProductId = (product != null?product.Id:null),
            };

            return this.CreateSubscription(subscriptionDto);
        }

        public InvoiceDto RegisterNewSubscription(string productId, string priceId, string userId)
        {
            InvoiceDto invoiceDto = null;
            try
            {
                Entities.ProductPrice price = this.UnitOfWork.ProductPriceRepository.Get(priceId);

                Entities.Subscription subscription = new Entities.Subscription()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = userId,
                    ProductId = productId,
                    SubscriptionTypeId = price.SubscriptionTypeId,
                    ValidDate = DateTime.Now,                    
                    IsActive = false,
                    IsDeleted = false,
                    CreatedDate = DateTime.Now
                };

                this.UnitOfWork.SubscriptionRepository.Add(subscription);

                // create invoice for payment
                Entities.Invoice invoice = new Entities.Invoice()
                {
                    Id = Guid.NewGuid().ToString(),
                    SubscriptionId = subscription.Id,
                    ProductPriceId = priceId,
                    CreatedDate = DateTime.Now,
                    Price = price.Price,
                    Discount = 0,
                    Amount = 0,
                    InvoiceDate =DateTime.Now,
                    IsCompleted = false,
                    Quality = 1,
                    UserId = userId,
                    IsDeleted = false,
                };

                invoice.Amount = invoice.Price * invoice.Quality - invoice.Discount;

                this.UnitOfWork.InvoiceRepository.Add(invoice);

                this.UnitOfWork.Save(subscription.UserId);

                invoiceDto = AutoMapper.Mapper.Map<Entities.Invoice, Dtos.InvoiceDto>(invoice);
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                StringBuilder stb = new StringBuilder();
                foreach (var eve in e.EntityValidationErrors)
                {
                    stb.AppendLine(String.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        stb.AppendLine(String.Format("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage));
                    }
                }

                throw new Exception(stb.ToString());
            }
            return invoiceDto;
        }

        public InvoiceDto ProceedPurchaseWithPaymentMethod(string invoiceId, int paymentMethodId)
        {
            InvoiceDto invoiceDto = null;
            try
            {
                Entities.Invoice invoice = this.UnitOfWork.InvoiceRepository.Get(invoiceId);

                invoice.PaymentMethodId = paymentMethodId;

                this.UnitOfWork.InvoiceRepository.Update(invoice, x=>x.PaymentMethodId);

                this.UnitOfWork.Save(this.UserId);

                invoiceDto = AutoMapper.Mapper.Map<Entities.Invoice, Dtos.InvoiceDto>(invoice);

            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                StringBuilder stb = new StringBuilder();
                foreach (var eve in e.EntityValidationErrors)
                {
                    stb.AppendLine(String.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        stb.AppendLine(String.Format("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage));
                    }
                }

                throw new Exception(stb.ToString());
            }
            return invoiceDto;
        }

        public void PaymentSuccess(PaymentObjectDto paymentObjectDto)
        {
            try
            {
                Entities.Invoice invoice = this.UnitOfWork.InvoiceRepository.Get(paymentObjectDto.InvoiceId);

                invoice.PaidAmount = paymentObjectDto.PaidAmount;
                invoice.PaymentDate = paymentObjectDto.PaymentDate;
                invoice.TransactionId = paymentObjectDto.PriceId;

                this.UnitOfWork.InvoiceRepository.Update(invoice, x => x.PaymentDate, x => x.PaidAmount, x => x.TransactionId);

                // subscription
                Entities.Subscription subscription = this.UnitOfWork.SubscriptionRepository.Get(invoice.SubscriptionId);
                // update latest valid subscription

                this.UnitOfWork.SubscriptionRepository.UpdateLastSubscriptionsByUser(subscription.UserId);

                subscription.IsActive = true;
                subscription.ExpiredDate = subscription.ValidDate.Value.AddMonths(subscription.SubscriptionType.MonthValue);
                subscription.NextInvoiceDate = subscription.ExpiredDate;

                this.UnitOfWork.SubscriptionRepository.Update(subscription, x => x.IsActive, x => x.ExpiredDate, x=>x.NextInvoiceDate);

                this.UnitOfWork.Save(subscription.UserId);

            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                StringBuilder stb = new StringBuilder();
                foreach (var eve in e.EntityValidationErrors)
                {
                    stb.AppendLine(String.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        stb.AppendLine(String.Format("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage));
                    }
                }

                throw new Exception(stb.ToString());
            }
        }

        public InvoiceDto GetSubscriptionNextInvoice(string subscriptionId, string userId)
        {
            InvoiceDto invoiceDto = null;
            try
            {
                if (String.IsNullOrEmpty(subscriptionId))
                {
                    throw new System.ArgumentNullException("subscriptionId");
                }

                Entities.Subscription subscription = this.UnitOfWork.SubscriptionRepository.Get(subscriptionId);

                if(subscription == null)
                {
                    throw new System.NullReferenceException(String.Format("Null subscription with id: {0}", subscriptionId));
                }
                Entities.Invoice invoice = null;

                if (subscription.NextInvoiceDate.HasValue)
                {
                    invoice = this.UnitOfWork.InvoiceRepository.GetSubscriptionInvoiceByInvoiceDate(subscription.Id, subscription.NextInvoiceDate.Value);

                    if (invoice == null)
                    {
                        // create invoice for payment
                        invoice = new Entities.Invoice()
                        {
                            Id = Guid.NewGuid().ToString(),
                            SubscriptionId = subscriptionId,
                            CreatedDate = DateTime.Now,
                            Price = 0,
                            Discount = 0,
                            Amount = 0,
                            InvoiceDate = subscription.NextInvoiceDate,
                            IsCompleted = false,
                            Quality = 1,
                            UserId = userId,
                            IsDeleted = false,
                        };

                        this.UnitOfWork.InvoiceRepository.Add(invoice);
                        this.UnitOfWork.Save(this.UserId);
                    }

                    invoiceDto = AutoMapper.Mapper.Map<Entities.Invoice, Dtos.InvoiceDto>(invoice);

                }

            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                StringBuilder stb = new StringBuilder();
                foreach (var eve in e.EntityValidationErrors)
                {
                    stb.AppendLine(String.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        stb.AppendLine(String.Format("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage));
                    }
                }

                throw new Exception(stb.ToString());
            }

            return invoiceDto;
        }

        private bool CanUpgradeSubscription(string subscriptionId)
        {
            if (String.IsNullOrEmpty(subscriptionId))
            {
                throw new System.ArgumentNullException("subscriptionId");
            }

            Entities.Subscription subscription = this.UnitOfWork.SubscriptionRepository.Get(subscriptionId);

            if(subscription == null)
            {
                throw new System.NullReferenceException("Subscription can not be null.");
            }

            int maxUpgradeLevel = this.UnitOfWork.ProductRepository.GetMaxUpgradeLevel();            

            return subscription.Product.UpgradeLevel < maxUpgradeLevel;
        }
    }
}
