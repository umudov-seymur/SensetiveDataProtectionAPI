using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class DataController : ControllerBase
{
    private readonly IEncryptionService _encryptionService;

    public DataController(IEncryptionService encryptionService)
    {
        _encryptionService = encryptionService;
    }

    [Authorize]
    [HttpPost("encrypt")]
    public IActionResult Encrypt([FromBody] string plainText)
    {
        if (string.IsNullOrWhiteSpace(plainText))
            return BadRequest("Plain text cannot be empty.");

        var encryptedData = _encryptionService.EncryptData(plainText);
        return Ok(new { EncryptedData = encryptedData });
    }

    [Authorize]
    [HttpPost("decrypt")]
    public IActionResult Decrypt([FromBody] string cipherText)
    {
        if (string.IsNullOrWhiteSpace(cipherText))
            return BadRequest("Cipher text cannot be empty.");

        try
        {
            var decryptedData = _encryptionService.DecryptData(cipherText);
            return Ok(new { DecryptedData = decryptedData });
        }
        catch
        {
            return BadRequest("Invalid cipher text format.");
        }
    }
}