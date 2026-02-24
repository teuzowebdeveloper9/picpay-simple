namespace SimplePicPay.DTOs;

public record CreateTransferRequest(
    Guid SenderId,
    Guid ReceiverId,
    decimal Amount
);

public record TransferResponse(
    Guid TransferId,
    Guid SenderId,
    Guid ReceiverId,
    decimal Amount,
    DateTime TransferDate
);

