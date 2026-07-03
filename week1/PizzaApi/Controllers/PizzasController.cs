using Microsoft.AspNetCore.Mvc;
using PizzaApi.Models;

namespace PizzaApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PizzasController : ControllerBase
{
    // In-memory "database" seeded with the modules's original two pizzas
    // plus one additional record required by the assignment (Id 3, Veggie Feast).
    public static List<Pizza> Pizzas { get; } = new()
    {
        new Pizza { Id = 1, Name = "Classic Italian", IsGlutenFree = false },
        new Pizza { Id = 2, Name = "Seafood Delight", IsGlutenFree = false },
        new Pizza { Id = 3, Name = "Veggie Feast", IsGlutenFree = true } // <-- additional record
    };

    // GET /Pizzas
    [HttpGet]
    public ActionResult<List<Pizza>> GetAll() => Ok(Pizzas);

    // GET /Pizzas/3
    [HttpGet("{id}")]
    public ActionResult<Pizza> Get(int id)
    {
        var pizza = Pizzas.FirstOrDefault(p => p.Id == id);
        if (pizza is null) return NotFound();
        return Ok(pizza);
    }

    // POST /Pizzas
    [HttpPost]
    public ActionResult<Pizza> Create(Pizza pizza)
    {
        pizza.Id = Pizzas.Count == 0 ? 1 : Pizzas.Max(p => p.Id) + 1;
        Pizzas.Add(pizza);
        return CreatedAtAction(nameof(Get), new { id = pizza.Id }, pizza);
    }

    // PUT /Pizzas/3
    [HttpPut("{id}")]
    public IActionResult Update(int id, Pizza input)
    {
        var index = Pizzas.FindIndex(p => p.Id == id);
        if (index == -1) return NotFound();
        input.Id = id;
        Pizzas[index] = input;
        return NoContent();
    }

    // DELETE /Pizzas/3
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var pizza = Pizzas.FirstOrDefault(p => p.Id == id);
        if (pizza is null) return NotFound();
        Pizzas.Remove(pizza);
        return NoContent();
    }
}
