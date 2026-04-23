using E_Commerce_WebApp.Models.MVVM;

namespace E_Commerce_WebApp.Models.Concrete
{
    public class OrderRepository
    {
        Context context = new Context();

        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public string? MyCart { get; set; }
        public decimal UnitPrice { get; set; }
        public string? ProductName { get; set; }
        public string? PhotoPath { get; set; }
        public int Kdv { get; set; }
        public string? Email { get; set; }


        public bool AddToMyCart(string id)
        {
            //bu metod bittiğinde hala false ise ürün sepete eklendi.
            bool exists = false;
            if (MyCart == "")
            {
                MyCart = id + "=" + Quantity; //10=1
            }
            else
            {
                string[] MyCartArray = MyCart.Split('&');
                //MyCart = 10=1&20=1&30=1
                //10=1 MyCartArray[0]
                //20=1 MyCartArray[1]
                //30=1 MyCartArray[2]

                for (int i = 0; i < MyCartArray.Length; i++)
                {
                    string[] item = MyCartArray[i].Split('=');
                    //item[0] 10 ProductID
                    //item[1] 1  Quantity
                    if (item[0] == id)
                    {
                        int quantity = Convert.ToInt32(item[1]);
                        quantity += Quantity;
                        MyCartArray[i] = id + "=" + quantity;
                        exists = true;
                        break;
                    }
                }
                if (exists == false)
                {
                    MyCart = MyCart + "&" + id.ToString() + "=1";
                }
            }
            return exists;
        }

        public void DeleteFromMyCart(string id)
        {
            string[] MyCartArray = MyCart.Split('&');
            List<string> newList = new List<string>();

            for (int i = 0; i < MyCartArray.Length; i++)
            {
                string[] items = MyCartArray[i].Split('=');
                string ProductID = items[0];

                if (ProductID != id)
                {
                    newList.Add(MyCartArray[i]);
                }
            }

            MyCart = string.Join("&", newList);
        }

        public List<OrderRepository> SelectMyCart()
        {
            List<OrderRepository> list = new List<OrderRepository>();

            if (string.IsNullOrEmpty(MyCart)) return list; // ← boşsa direkt dön

            string[] MyCartArray = MyCart.Split('&');

            for (int i = 0; i < MyCartArray.Length; i++)
            {
                if (string.IsNullOrEmpty(MyCartArray[i])) continue; // ← boş eleman atla

                string[] items = MyCartArray[i].Split('=');
                if (items.Length < 2) continue; // ← format hatalıysa atla

                int ProductID = Convert.ToInt32(items[0]);
                int Quantity = Convert.ToInt32(items[1]);

                Product? product = context.Products?.FirstOrDefault(p => p.ProductID == ProductID);
                if (product == null) continue; // ← ürün bulunamazsa atla

                OrderRepository orderRepository = new OrderRepository();
                orderRepository.ProductID = product.ProductID;
                orderRepository.Quantity = Quantity;
                orderRepository.UnitPrice = Convert.ToDecimal(product.UnitPrice);
                orderRepository.ProductName = product.ProductName;
                orderRepository.PhotoPath = product.PhotoPath;
                orderRepository.Kdv = product.Kdv;
                list.Add(orderRepository);
            }

            return list;
        }

        public string Add(string email)
        {
            List<OrderRepository> list = SelectMyCart();

            DateTime OrderDate = DateTime.Now;
            string OrderGroupGUID = DateTime.Now.ToString().Replace(":", "").Replace(" ", "").Replace(".", "");

            foreach (var item in list)
            {
                Order order = new Order();

                order.OrderDate = OrderDate;
                order.OrderGroupGUID = OrderGroupGUID;
                order.UserID = context.Users.FirstOrDefault(u => u.Email == email).UserID;
                order.ProductID = item.ProductID;
                order.Quantity = item.Quantity;

                context.Orders.Add(order);
                context.SaveChanges();
            }
            return OrderGroupGUID;
        }

        public List<Vw_MyOrder> GetMyOrders(string Email)
        {
            int UserID = context.Users.FirstOrDefault(u => u.Email == Email).UserID;

            List<Vw_MyOrder> myOrders = context.Vw_MyOrders.Where(o => o.UserID == UserID).ToList();

            return myOrders;
        }

        public void UpdateQuantity(string id, int change)
        {
            string[] MyCartArray = MyCart.Split('&');

            for (int i = 0; i < MyCartArray.Length; i++)
            {
                string[] item = MyCartArray[i].Split('=');
                if (item[0] == id)
                {
                    int newQty = Convert.ToInt32(item[1]) + change;

                    if (newQty <= 0)
                    {
                        DeleteFromMyCart(id);
                    }
                    else
                    {
                        MyCartArray[i] = id + "=" + newQty;
                        MyCart = string.Join("&", MyCartArray);
                    }
                    break;
                }
            }
        }
    }
}
