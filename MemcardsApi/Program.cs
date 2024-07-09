

using MemcardsApi.Services;
using MemcardsApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IDbService, InMemoryDB>();
var app = builder.Build();



app.MapGet("/decks", (IDbService service) =>
{
  return TypedResults.Ok(service.GetDecks());
});

app.MapGet("decks/{deckId}", (string deckId, IDbService service) =>
{
  var deck = service.GetDeck(deckId);
  if (deck == null)
  {
    return Results.NotFound();
  }
  return TypedResults.Ok(deck);
});

app.MapPut("/decks/{deckId}", (string deckId, DeckName deckName, IDbService service) =>
{
  var existingDeck = service.UpdateDeck(deckId, deckName);
  if (existingDeck == null)
  {
    return Results.NotFound();
  }
  return TypedResults.Ok(existingDeck);
});

app.MapDelete("/decks/{deckId}", (string deckId, IDbService service) =>
{
  var deck = service.DeleteDeck(deckId);
  if (deck.Equals(0))
  {
    return Results.NotFound();
  }
  return Results.NoContent();
});

app.MapPost("/decks/{deckId}/flashcards", (string deckId, Flashcard flashcard, IDbService service) =>
{
  var createdFlashcard = service.CreateFlashcard(deckId, flashcard);
  if (createdFlashcard == null)
  {
    return Results.NotFound();
  }

  return Results.Created("", createdFlashcard);

});



app.Run();


public partial class Program { }