using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;

namespace StaticFactoryCoupling
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestShippingByStore_Seven_1_Order_Family_2_Orders()
        {
            //arrange
            var target = new ShipService();

            var orders = new List<Order>
            {
                new Order{ StoreType= StoreType.Seven, Id=1},
                new Order{ StoreType= StoreType.Family, Id=2},
                new Order{ StoreType= StoreType.Family, Id=3},
            };

            var fakeSeven = Substitute.For<IStoreService>();
            SimpleFactory.SevenService = fakeSeven;
            var fakeFamily = Substitute.For<IStoreService>();
            SimpleFactory.FamilyService = fakeFamily;

            //act
            target.ShippingByStore(orders);

            //todo, assert
            //ShipService should invoke SevenService once and FamilyService twice
            fakeFamily.ReceivedWithAnyArgs(2);
            fakeSeven.ReceivedWithAnyArgs(1);
        }
    }

    public enum StoreType
    {
        /// <summary>
        /// 7-11
        /// </summary>
        Seven = 0,

        /// <summary>
        /// 全家
        /// </summary>
        Family = 1
    }

    public class ShipService
    {
        public void ShippingByStore(List<Order> orders)
        {
            foreach (var order in orders)
            {
                // simple factory pattern implementation
                IStoreService storeService = SimpleFactory.GetStoreService(order);
                storeService.Ship(order);
            }
        }
    }

    internal class SimpleFactory
    {
        //private static IStoreService sevenService = new SevenService();
        //private static IStoreService familyService = new FamilyService();
        private static IStoreService _sevenService;

        private static IStoreService _familyService;

        internal static IStoreService SevenService
        {
            get => _sevenService ?? new SevenService();
            set => _sevenService = value;
        }

        internal static IStoreService FamilyService
        {
            get => _familyService ?? new FamilyService();
            set => _familyService = value;
        }

        internal static IStoreService GetStoreService(Order order)
        {
            if (order.StoreType == StoreType.Family)
            {
                return _familyService;
            }
            else
            {
                return _sevenService;
            }
        }
    }

    public class Order
    {
        public StoreType StoreType { get; set; }
        public int Id { get; set; }
        public int Amount { get; set; }
    }

    internal class SevenService : IStoreService
    {
        public void Ship(Order order)
        {
            // seven web service
            //var client = new HttpClient();
            //client.PostAsync("http://api.seven.com/Order", order, new JsonMediaTypeFormatter());
        }
    }

    internal class FamilyService : IStoreService
    {
        public void Ship(Order order)
        {
            // family web service
            //var client = new HttpClient();
            //client.PostAsync("http://api.family.com/Order", order, new JsonMediaTypeFormatter());
        }
    }

    public interface IStoreService
    {
        void Ship(Order order);
    }
}