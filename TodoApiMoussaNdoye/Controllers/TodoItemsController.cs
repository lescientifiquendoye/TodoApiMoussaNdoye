using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApiMoussaNdoye.Models;
using TodoApiMoussaNdoye.Services;

namespace TodoApiMoussaNdoye.Controllers;

[ApiController]
[Route("api/todoitems")]
public class TodoItemsController : ControllerBase
{
    private readonly TodosService _service;

    public TodoItemsController(TodosService service) => _service = service;

    private static TodoItemDTO ToDto(TodoItem item) =>
        new() { Id = item.Id, Name = item.Name, IsComplete = item.IsComplete };

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetAll()
    {
        var items = await _service.GetAsync();
        return Ok(items.Select(ToDto));
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<TodoItemDTO>> GetById(string id)
    {
        var item = await _service.GetAsync(id);
        if (item is null) return NotFound();
        return Ok(ToDto(item));
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<TodoItemDTO>> Create(TodoItemDTO dto)
    {
        var item = new TodoItem { Name = dto.Name, IsComplete = dto.IsComplete };
        await _service.CreateAsync(item);
        return CreatedAtAction(nameof(GetById), new { id = item.Id }, ToDto(item));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Update(string id, TodoItemDTO dto)
    {
        var existing = await _service.GetAsync(id);
        if (existing is null) return NotFound();

        existing.Name = dto.Name;
        existing.IsComplete = dto.IsComplete;
        await _service.UpdateAsync(id, existing);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Delete(string id)
    {
        var existing = await _service.GetAsync(id);
        if (existing is null) return NotFound();

        await _service.RemoveAsync(id);
        return NoContent();
    }
}
