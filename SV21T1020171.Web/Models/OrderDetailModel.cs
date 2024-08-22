using SV21T1020171.DomainModels;

public class OrderDetailModel
{
    public Order Order { get; set; }
    public List<OrderDetail> Details { get; set; }
}