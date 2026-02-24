using Microsoft.AspNetCore.Mvc;
using SimplePicPay.DTOs;
using SimplePicPay.Services;

namespace SimplePicPay.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WalletController : ControllerBase
{
    private readonly IWalletService _walletService;

    public WalletController(IWalletService walletService)
    {
        _walletService = walletService;
    }

    /// <summary>
    /// Cria uma nova carteira/usuário
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<WalletResponse>> CreateWallet([FromBody] CreateWalletRequest request)
    {
        try
        {
            var wallet = await _walletService.CreateWalletAsync(request);
            return CreatedAtAction(nameof(GetWallet), new { id = wallet.Id }, wallet);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Obtém uma carteira pelo ID
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<WalletResponse>> GetWallet(Guid id)
    {
        var wallet = await _walletService.GetWalletByIdAsync(id);
        if (wallet is null)
            return NotFound(new { error = "Carteira não encontrada." });

        return Ok(wallet);
    }

    /// <summary>
    /// Lista todas as carteiras
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<WalletResponse>>> GetAllWallets()
    {
        var wallets = await _walletService.GetAllWalletsAsync();
        return Ok(wallets);
    }

    /// <summary>
    /// Realiza um depósito na carteira
    /// </summary>
    [HttpPost("{id:guid}/deposit")]
    public async Task<ActionResult<WalletBalanceResponse>> Deposit(Guid id, [FromBody] DepositRequest request)
    {
        try
        {
            var result = await _walletService.DepositAsync(id, request.Amount);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Remove uma carteira
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteWallet(Guid id)
    {
        var deleted = await _walletService.DeleteWalletAsync(id);
        if (!deleted)
            return NotFound(new { error = "Carteira não encontrada." });

        return NoContent();
    }
}

