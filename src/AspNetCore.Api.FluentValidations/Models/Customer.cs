namespace AspNetCore.Api.FluentValidations.Models;

public class Customer
{
    public int Id { get; set; }
    public string Surname { get; set; }
    public string Forename { get; set; }
    public string Photo { get; set; }
    public decimal Discount { get; set; }
    public bool IsPreferredCustomer { get; set; }
    public List<string> Address { get; set; }
    public Department Department { get; set; }
    public List<Order> Orders { get; set; }

}

public class Department
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class Order
{
    public double Total { get; set; }

    public string Cost { get; set; }
}
