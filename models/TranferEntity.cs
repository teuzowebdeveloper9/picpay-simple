namespace SimplePicPay.Models;

public class TranferEntity
{
    public Guid TransferId { get; set; }
    public Guid SenderId { get; set; }
    public Guid ReceiverId { get; set;}
    public decimal Amount { get; set; }
    public DateTime TransferDate { get; set; } = DateTime.UtcNow;



    public TranferEntity(Guid transferId, Guid senderId, Guid receiverId, decimal amount)
    {
        TransferId = transferId;
        SenderId = senderId;
        ReceiverId = receiverId;
        Amount = amount;
        TransferDate = DateTime.UtcNow;
    }
    
}