using Knjizara.Data;
using Knjizara.Models.Authentication;
using Knjizara.Models.Books;
using Microsoft.AspNetCore.Mvc;
using PayPal.Api;
using System.Linq;
using System.Web.Http;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;

namespace Knjizara.Controllers
{
    public class PayPalPaymentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PayPalPaymentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult BuyBook(int id)
        {
            APIContext apiContext = Models.PayPal.Configuration.GetAPIContext();
            Book? book = _context.Books.FirstOrDefault(b => b.Id == id);
            try
            {
                string payerId = Request.Query["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    string baseURI = Request.Scheme + "://" + Request.Host +
                                "/Paypal/PaymentWithPayPal?";

                    var guid = Convert.ToString((new Random()).Next(100000));

                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid, book ?? new Book());

                    var links = createdPayment.links.GetEnumerator();

                    string paypalRedirectUrl = string.Empty;

                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;

                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    HttpContext.Session.SetString("guid", createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    var guid = Request.Query["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, HttpContext.Session.GetString("guid") ?? string.Empty);
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("Failure");
                    }
                }
            }
            catch
            {
                return View("Failure");
            }
            return View("Success");
        }

        [HttpGet]
        public ActionResult PayLateFee(Guid id)
        {
            APIContext apiContext = Models.PayPal.Configuration.GetAPIContext();
            AppUser? user = _context.Users.FirstOrDefault(u => u.Id == id);
            try
            {
                string payerId = Request.Query["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    string baseURI = Request.Scheme + "://" + Request.Host +
                                "/Paypal/PaymentWithPayPal?";

                    var guid = Convert.ToString((new Random()).Next(100000));

                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid, user ?? new AppUser());

                    var links = createdPayment.links.GetEnumerator();

                    string paypalRedirectUrl = string.Empty;

                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;

                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    HttpContext.Session.SetString("guid", createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    var guid = Request.Query["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, HttpContext.Session.GetString("guid") ?? string.Empty);
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("Failure");
                    }
                }
            }
            catch
            {
                return View("Failure");
            }
            return View("Success");
        }

        private Payment? payment;

        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            this.payment = new Payment() { id = paymentId };
            return this.payment.Execute(apiContext, paymentExecution);
        }

        private Payment CreatePayment(APIContext apiContext, string redirectUrl, Book book)
        {
            var itemList = new ItemList() { items = new List<Item>() };

            itemList.items.Add(new Item()
            {
                name = book.Title,
                currency = "EUR",
                price = book.PriceForBuying.ToString("0.##"),
                quantity = "1",
                sku = "sku"
            });

            var payer = new Payer() { payment_method = "paypal" };

            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl,
                return_url = redirectUrl
            };

            var amount = new Amount()
            {
                currency = "EUR",
                total = itemList.items[0].price
            };

            var transactionList = new List<Transaction>();

            transactionList.Add(new Transaction()
            {
                description = $"Transaction description:",
                invoice_number = "invoice number",
                amount = amount,
                item_list = itemList
            });

            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            return this.payment.Create(apiContext);
        }

        private Payment CreatePayment(APIContext apiContext, string redirectUrl, AppUser user)
        {
            var itemList = new ItemList() { items = new List<Item>() };

            itemList.items.Add(new Item()
            {
                name = user.UserName,
                currency = "EUR",
                price = user.LateFee.ToString("0.##"),
                quantity = "1",
                sku = "sku"
            });

            var payer = new Payer() { payment_method = "paypal" };

            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl,
                return_url = redirectUrl
            };

            var amount = new Amount()
            {
                currency = "EUR",
                total = itemList.items[0].price
            };

            var transactionList = new List<Transaction>();

            transactionList.Add(new Transaction()
            {
                description = $"Transaction description:",
                invoice_number = "invoice number",
                amount = amount,
                item_list = itemList
            });

            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            return this.payment.Create(apiContext);
        }
    }
}
