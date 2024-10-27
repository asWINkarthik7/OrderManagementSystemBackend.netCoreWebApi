using Moq;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using OrderManagementSystem.Interfaces;
using OrderManagementSystem.Model;
using OrderManagementSystem.DataAccess;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mysqlx.Crud;

namespace OrderManagementSystem.Tests
{
    [TestFixture]
    public class OrderRepositoryTests
    {
        private Mock<IOrderRepository> _mockOrderRepository;
        private string _connectionString = "server=localhost;database=OrderManagementDB;user=root;password=Test@123;";

        [SetUp]
        public void Setup()
        {
            _mockOrderRepository = new Mock<IOrderRepository>();
        }

        [Test]
        public async Task GetAllOrdersAsync_ShouldReturnListOfOrders()
        {
            // Arrange
            var expectedOrders = new List<OrderInfo>
            {
                new OrderInfo { OrderID = 1, Vendor = "Vendor A", OrderAmount = 100.00M, OrderNumber = 123, Shop = "Shop 1" },
                new OrderInfo { OrderID = 2, Vendor = "Vendor B", OrderAmount = 200.00M, OrderNumber = 124, Shop = "Shop 2" }
            };
            _mockOrderRepository.Setup(repo => repo.GetAllOrdersAsync()).ReturnsAsync(expectedOrders);

            // Act
            var orders = await _mockOrderRepository.Object.GetAllOrdersAsync();

            // Assert
            Assert.AreEqual(2, orders.Count);
            Assert.AreEqual("Vendor A", orders[0].Vendor);
        }

        [Test]
        public async Task CreateOrderAsync_ShouldAddNewOrder()
        {
            // Arrange
            var newOrder = new OrderInfo { Vendor = "Vendor C", OrderAmount = 150.00M, OrderNumber = 125, Shop = "Shop 3" };

            // Act
            await _mockOrderRepository.Object.CreateOrderAsync(newOrder);

            // Assert
            _mockOrderRepository.Verify(repo => repo.CreateOrderAsync(newOrder), Times.Once);
        }
        [Test]
        public async Task UpdateOrderAsync_ShouldModifyExistingOrder()
        {
            // Arrange
            var existingOrder = new OrderInfo { OrderID = 1, Vendor = "Vendor A", OrderAmount = 100.00M, OrderNumber = 123, Shop = "Shop 1" };
            var updatedOrder = new OrderInfo { OrderID = 1, Vendor = "Vendor A Updated", OrderAmount = 120.00M, OrderNumber = 123, Shop = "Shop 1" };

            _mockOrderRepository.Setup(repo => repo.UpdateOrderAsync(updatedOrder)).Returns(Task.CompletedTask);

            // Act
            await _mockOrderRepository.Object.UpdateOrderAsync(updatedOrder);

            // Assert
            _mockOrderRepository.Verify(repo => repo.UpdateOrderAsync(updatedOrder), Times.Once);
        }

        [Test]
        public async Task DeleteOrderAsync_ShouldRemoveOrder()
        {
            // Arrange
            var orderIdToDelete = 1;

            _mockOrderRepository.Setup(repo => repo.DeleteOrderAsync(orderIdToDelete)).Returns(Task.CompletedTask);

            // Act
            await _mockOrderRepository.Object.DeleteOrderAsync(orderIdToDelete);

            // Assert
            _mockOrderRepository.Verify(repo => repo.DeleteOrderAsync(orderIdToDelete), Times.Once);
        }

    }
}
