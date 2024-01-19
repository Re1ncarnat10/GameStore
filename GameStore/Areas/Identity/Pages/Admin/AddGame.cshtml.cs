using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

public class AddGameModel : PageModel
{
    private readonly ApplicationDbContext _db;

    [BindProperty]
    public Game InputGame { get; set; }

    public AddGameModel(ApplicationDbContext db)
    {
        _db = db;
        InputGame = new Game(); 
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            TempData["StatusMessage"] = "There was an error with your submission. Please correct the errors and try again.";
            return Page();
        }

        _db.Games.Add(InputGame);
        var created = await _db.SaveChangesAsync();

        if (created > 0)
        {
            TempData["StatusMessage"] = "Game added successfully.";
        }
        else
        {
            TempData["StatusMessage"] = "An error occurred while adding the game.";
        }

        return RedirectToPage("/Admin/AddGame");
    }
}
