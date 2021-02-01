using JinnDev.Order.Models;
using JinnDev.OrderNoDI;
using System.Collections.Generic;

namespace Shipping.ConsoleNoDi
{
    class Program
    {
        // This is an "Entry Point", which could be an Event, API Endpoint, or Main()
        static void Main()
        {
            // The "Entry Point" gets a Service
            OrderService service = new OrderService();

            // The "Entry Point" uses the Service to Command or Query
            OrderModel result = service.CalculateOrder(1, new List<int>());

            var lawl = service.CreateOrder(result.OrderId, new PaymentInfoModel());

            // Output for fun:
            System.Console.WriteLine("Order Total: ${0}, Order Created: {1}", result.NetTotal, lawl);
            System.Console.ReadKey();
        }
    }
}