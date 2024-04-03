using ClientManager.Core.Domain;
using ClientManager.Core.Processor;
using Microsoft.AspNetCore.Mvc;

namespace ClientManager.ASP.NETCoreWebAPIProject.Controllers;

[ApiController]
[Route("[controller]")]
public class ClientsController : ControllerBase
{
    private readonly IClientRequestProcessor _clientRequestProcessor;

    public ClientsController(IClientRequestProcessor clientRequestProcessor)
    {
        _clientRequestProcessor = clientRequestProcessor;
    }

    [HttpPost]
    public ActionResult<Client> CreateClient([FromBody] ClientRequest client)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = _clientRequestProcessor.CreateClient(client);
        if (result.IsSuccess)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpGet("{clientId}")]
    public ActionResult<Client> GetClient(int clientId)
    {
        var result = _clientRequestProcessor.GetClient(clientId);
        if (result.IsSuccess)
            return Ok(result);
        return NotFound(result);
    }

    [HttpPut("{clientId}")]
    public ActionResult<Client> UpdateClient(int clientId, [FromBody] ClientRequest client)
    {
        if (clientId != client.Id)
            return BadRequest("Client ID mismatch.");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var getResult = _clientRequestProcessor.GetClient(clientId);
        if (!getResult.IsSuccess)
            return NotFound();

        var result = _clientRequestProcessor.UpdateClient(client);
        if (result.IsSuccess)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpDelete("{clientId}")]
    public ActionResult<Client> DeleteClient(int clientId)
    {
        var getResult = _clientRequestProcessor.GetClient(clientId);
        if (!getResult.IsSuccess)
            return NotFound();

        var result = _clientRequestProcessor.DeleteClient(clientId);
        if (result.IsSuccess)
            return Ok(result);
        return NotFound(result);
    }

    [HttpGet]
    public ActionResult<IEnumerable<Client>> GetAllClients()
    {
        var clients = _clientRequestProcessor.GetAllClients();
        if (clients != null)
            return Ok(clients);
        return NotFound();
    }
}