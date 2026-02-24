using Microsoft.AspNetCore.Mvc;
using SimplePicPay.DTOs;
using SimplePicPay.Services;

namespace SimplePicPay.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransferController : ControllerBase
{
    private readonly ITransferService _transferService;

    public TransferController(ITransferService transferService)
    {
        _transferService = transferService;
    }

    /// <summary>
    /// Realiza uma transferência entre carteiras
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<TransferResponse>> CreateTransfer([FromBody] CreateTransferRequest request)
    {
        try
        {
            var transfer = await _transferService.CreateTransferAsync(request);
            return CreatedAtAction(nameof(GetTransfer), new { id = transfer.TransferId }, transfer);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Obtém uma transferência pelo ID
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TransferResponse>> GetTransfer(Guid id)
    {
        var transfer = await _transferService.GetTransferByIdAsync(id);
        if (transfer is null)
            return NotFound(new { error = "Transferência não encontrada." });

        return Ok(transfer);
    }

    /// <summary>
    /// Lista todas as transferências
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TransferResponse>>> GetAllTransfers()
    {
        var transfers = await _transferService.GetAllTransfersAsync();
        return Ok(transfers);
    }

    /// <summary>
    /// Lista transferências de uma carteira específica
    /// </summary>
    [HttpGet("wallet/{walletId:guid}")]
    public async Task<ActionResult<IEnumerable<TransferResponse>>> GetTransfersByWallet(Guid walletId)
    {
        var transfers = await _transferService.GetTransfersByWalletIdAsync(walletId);
        return Ok(transfers);
    }
}

