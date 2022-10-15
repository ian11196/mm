//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using WooCommerceNET.WooCommerce.v2;
//using WooCommerceNET;
//using WooCommerceNET.WooCommerce.v3.Extension;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;

//namespace InfinityLauncher.StaticClasses
//{   
//    public class Purchaser
//    {
//        public string Email = null;
//        public string FirstName = null;
//        public string LastName = null;
//        public string ProductID = null;
        
//        static RestAPI rest = new RestAPI("https://beyondinfinitystudio.com/wp-json/wc/v2/", "ck_43c99a8946eed48af9514c0807baca365f840f5a", "cs_79ad18a115ed30f38be881c5767e7538131ce493");
//        WCObject wc = new WCObject(rest);

//        Stripe.RequestOptions requestOptions = new Stripe.RequestOptions();

//        public async void ProcessOrder(int prodID, OrderBilling orderBilling)
//        {
//            var products = await wc.Product.GetAll();

//            Order order = new Order();

//            List<OrderLineItem> orderLineItem = new List<OrderLineItem>();

//            orderLineItem.Add(new OrderLineItem { product_id = (ulong)prodID, quantity = 1 });

//            order.line_items = orderLineItem;
//            order.status = "completed";
//            order.billing = orderBilling;
//            order.customer_id = ulong.Parse(UserData.ID);

//            var or = await wc.Order.Add(order);

//            OrderResponse orderResponse = new OrderResponse();
//            orderResponse.status = or.status;
//            orderResponse.id = or.id.ToString();
//            var jsonResponse = JsonConvert.SerializeObject(orderResponse);
//        }

//        public void DoPurchase(Stripe.TokenCardOptions Card)
//        {
//            try
//            {
//                Stripe.StripeConfiguration.ApiKey = "sk_test_qzXeMiYJ6Gd3qjgozQsAMsfX";

//                var optoken = new Stripe.TokenCreateOptions
//                {
//                    Card = Card
//                };

//                var service = new Stripe.TokenService();
//                Stripe.Token token = service.Create(optoken);
//                System.Diagnostics.Debug.WriteLine(token.StripeResponse);

//                var options = new Stripe.ChargeCreateOptions()
//                {
//                    Amount = Convert.ToInt32(3000),
//                    Currency = "usd",
//                    Description = "Testing",
//                    Source = token.Id
//                };

//                var charservice = new Stripe.ChargeService();
//                Stripe.Charge charge = charservice.Create(options);
//                if (charge.Paid)
//                    System.Diagnostics.Debug.WriteLine("paid");
//            }
//            catch(Exception e)
//            {
//                System.Diagnostics.Debug.WriteLine(e.Message);
//            }
//        }
//    }

//    public class OrderResponse
//    {
//        public string status;
//        public string id;
//    }
//}
