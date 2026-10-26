
using System;
using System.Collections.Generic;


// SECTION 01: BOOKS AND DELEGATES

public class Book
{
    public string ISBN { get; set; }
    public string Title { get; set; }
    public string[] Authors { get; set; }
    public DateTime PublicationDate { get; set; }
    public decimal Price { get; set; }

    public Book(
        string _ISBN,
        string _Title,
        string[] _Authors,
        DateTime _PublicationDate,
        decimal _Price)
    {
        ISBN = _ISBN;
        Title = _Title;
        Authors = _Authors;
        PublicationDate = _PublicationDate;
        Price = _Price;
    }

    public override string ToString()
    {
        return $"Title: {Title}, ISBN: {ISBN}, Price: {Price:C}, " +
               $"Published: {PublicationDate.ToShortDateString()}";
    }
}


// User-defined delegate with the same signature as Book functions.
public delegate string BookDelegate(Book book);


public static class BookFunctions
{
    // Methods that return book information.
    public static string GetTitle(Book book)
    {
        return book.Title;
    }

    public static string GetAuthors(Book book)
    {
        return string.Join(", ", book.Authors);
    }

    public static string GetPrice(Book book)
    {
        return book.Price.ToString("C");
    }

    public static string GetISBN(Book book)
    {
        return book.ISBN;
    }

    public static string GetPublicationDate(Book book)
    {
        return book.PublicationDate.ToShortDateString();
    }


    // ProcessBooks using a user-defined delegate.
    public static void ProcessBooks(
        List<Book> books,
        BookDelegate function)
    {
        foreach (Book book in books)
        {
            Console.WriteLine(function(book));
        }
    }


    // ProcessBooks using the built-in Func delegate.
    public static void ProcessBooks(
        List<Book> books,
        Func<Book, string> function)
    {
        foreach (Book book in books)
        {
            Console.WriteLine(function(book));
        }
    }
}



// SECTION 02: ORDER PROCESSING SYSTEM

// Part 1: User-defined delegate.
public delegate decimal PriceCalculator(Order order);


public class Order
{
    public int Id { get; set; }
    public string CustomerName { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }

    public Order(
        int id,
        string customerName,
        decimal price,
        int quantity)
    {
        Id = id;
        CustomerName = customerName;
        Price = price;
        Quantity = quantity;
    }
}


public class OrderService
{
    // Part 5: Event for completed orders.
    public event Action<Order>? OrderProcessed;

    // Bonus: The caller supplies the pricing strategy.
    public decimal CalculatePrice(
        Order order,
        Func<Order, decimal> pricingStrategy)
    {
        return pricingStrategy(order);
    }

    
    public void ProcessOrder(Order order)
    {
        Console.WriteLine($"Processing order {order.Id}...");

        OrderProcessed?.Invoke(order);
    }
}


internal class Program
{
    // Part 1: User-defined delegate methods.
    static decimal CalculateTotal(Order order)
    {
        return order.Price * order.Quantity;
    }

    static decimal CalculateTotalWithDiscount(Order order)
    {
        return (order.Price * order.Quantity) - 10m;
    }

    static decimal CalculateOrderPrice(
        Order order,
        PriceCalculator calculator)
    {
        return calculator(order);
    }


    // Part 2: Func<Order, decimal>.
    static decimal CalculateOrderPrice(
        Order order,
        Func<Order, decimal> calculator)
    {
        return calculator(order);
    }


    // Part 3: Predicate<Order>.
    static bool ValidateOrder(
        Order order,
        Predicate<Order> validationRule)
    {
        return validationRule(order);
    }


    // Part 4: Action<Order>.
    static void ProcessOrder(
        Order order,
        Action<Order> action)
    {
        action(order);
    }


    // Event handlers.
    static void Handler1(Order order)
    {
        Console.WriteLine(
            $"Handler 1: Order {order.Id} completed.");
    }

    static void Handler2(Order order)
    {
        Console.WriteLine(
            $"Handler 2: Confirmation sent to {order.CustomerName}.");
    }

    static void Handler3(Order order)
    {
        Console.WriteLine(
            $"Handler 3: Audit recorded for order {order.Id}.");
    }


    static void Main()
    {
        
        // SECTION 01: BOOKS AND DELEGATES
        
        Console.WriteLine("========== SECTION 01 ==========");

        List<Book> books = new List<Book>
        {
            new Book(
                "ISBN001",
                "C# Basics",
                new string[] { "Ahmed Ali", "Mona Samir" },
                new DateTime(2024, 5, 10),
                250m),

            new Book(
                "ISBN002",
                "OOP in C#",
                new string[] { "Sara Mohamed" },
                new DateTime(2025, 2, 15),
                300m)
        };


        Console.WriteLine("\n1. User-defined delegate: GetTitle");

        BookFunctions.ProcessBooks(
            books,
            (BookDelegate)BookFunctions.GetTitle);


        Console.WriteLine("\n2. Built-in Func: GetAuthors");

        Func<Book, string> authorsFunction =
            BookFunctions.GetAuthors;

        BookFunctions.ProcessBooks(books, authorsFunction);


        Console.WriteLine("\n3. Built-in Func: GetPrice");

        Func<Book, string> priceFunction =
            BookFunctions.GetPrice;

        BookFunctions.ProcessBooks(books, priceFunction);


        Console.WriteLine("\n4. Anonymous Method: GetISBN");

        Func<Book, string> isbnFunction = delegate (Book book)
        {
            return book.ISBN;
        };

        BookFunctions.ProcessBooks(books, isbnFunction);


        Console.WriteLine("\n5. Lambda Expression: GetPublicationDate");

        Func<Book, string> publicationDateFunction =
            book => book.PublicationDate.ToShortDateString();

        BookFunctions.ProcessBooks(books, publicationDateFunction);


        Console.WriteLine("\n6. Book ToString");

        foreach (Book book in books)
        {
            Console.WriteLine(book.ToString());
        }


        // SECTION 02: ORDER PROCESSING
        
        Console.WriteLine("\n========== SECTION 02 ==========");

        Order order = new Order(101, "Esraa", 100m, 3);


        // Part 1: User-defined delegate.
        Console.WriteLine("\n1. User-defined Delegate");

        decimal total = CalculateOrderPrice(
            order,
            (PriceCalculator)CalculateTotal);

        decimal discountedTotal = CalculateOrderPrice(
            order,
            (PriceCalculator)CalculateTotalWithDiscount);

        Console.WriteLine($"Normal total: {total}");
        Console.WriteLine($"Total after fixed discount: {discountedTotal}");


        // Part 2: Func<> with lambdas.
        Console.WriteLine("\n2. Func<Order, decimal>");

        Func<Order, decimal> normalPricing =
            x => x.Price * x.Quantity;

        Func<Order, decimal> discount10 =
            x => x.Price * x.Quantity * 0.90m;

        Console.WriteLine(
            $"Normal price: {CalculateOrderPrice(order, normalPricing)}");

        Console.WriteLine(
            $"10% discount: {CalculateOrderPrice(order, discount10)}");


        // Part 3: Predicate<> validation.
        Console.WriteLine("\n3. Predicate<Order>");

        Predicate<Order> validQuantity =
            x => x.Quantity > 0;

        Predicate<Order> validPrice =
            x => x.Price > 0;

        Predicate<Order> hasCustomerName =
            x => !string.IsNullOrWhiteSpace(x.CustomerName);

        Console.WriteLine(
            $"Valid quantity: {ValidateOrder(order, validQuantity)}");

        Console.WriteLine(
            $"Valid price: {ValidateOrder(order, validPrice)}");

        Console.WriteLine(
            $"Has customer name: {ValidateOrder(order, hasCustomerName)}");

        bool isValid =
            ValidateOrder(order, validQuantity) &&
            ValidateOrder(order, validPrice) &&
            ValidateOrder(order, hasCustomerName);

        Console.WriteLine($"Is order valid? {isValid}");


        // Part 4: Action<>.
        Console.WriteLine("\n4. Action<Order>");

        Action<Order> printOrder = x =>
        {
            Console.WriteLine(
                $"Order ID: {x.Id}, Customer: {x.CustomerName}, " +
                $"Price: {x.Price}, Quantity: {x.Quantity}");
        };

        Action<Order> sendConfirmation = x =>
        {
            Console.WriteLine(
                $"Confirmation sent for order {x.Id}.");
        };

        Action<Order> writeAudit = x =>
        {
            Console.WriteLine(
                $"Audit: Order {x.Id} was processed.");
        };

        ProcessOrder(order, printOrder);
        ProcessOrder(order, sendConfirmation);
        ProcessOrder(order, writeAudit);


        // Part 5 and Part 6: Events and subscriptions.
        Console.WriteLine("\n5. Events and Multicast Delegates");

        OrderService orderService = new OrderService();

        orderService.OrderProcessed += Handler1;
        orderService.OrderProcessed += Handler2;
        orderService.OrderProcessed += Handler3;

        orderService.ProcessOrder(order);


        Console.WriteLine("\n6. Unsubscribe Handler1");

        orderService.OrderProcessed -= Handler1;

        orderService.ProcessOrder(order);


        // Part 7: Final application flow.
        Console.WriteLine("\n7. Final Application");

        if (isValid)
        {
            Func<Order, decimal> pricingStrategy =
                x => x.Price * x.Quantity * 0.80m;

            decimal finalPrice =
                orderService.CalculatePrice(order, pricingStrategy);

            Console.WriteLine($"Final price after 20% discount: {finalPrice}");

            orderService.ProcessOrder(order);
        }
        else
        {
            Console.WriteLine("Order is invalid.");
        }


        // Bonus: Pricing strategies supplied at runtime.
        Console.WriteLine("\n8. Bonus: Pricing Strategies");

        Func<Order, decimal> normal =
            x => x.Price * x.Quantity;

        Func<Order, decimal> discount20 =
            x => x.Price * x.Quantity * 0.80m;

        Func<Order, decimal> vipDiscount =
            x => x.Price * x.Quantity * 0.75m;

        Console.WriteLine(
            $"Normal: {orderService.CalculatePrice(order, normal)}");

        Console.WriteLine(
            $"10% discount: {orderService.CalculatePrice(order, discount10)}");

        Console.WriteLine(
            $"20% discount: {orderService.CalculatePrice(order, discount20)}");

        Console.WriteLine(
            $"VIP discount: {orderService.CalculatePrice(order, vipDiscount)}");
    }
}
