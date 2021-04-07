using BookManagement.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagement.Api.Services
{
    public class BookService : IBookService
    {
        private readonly List<Book> _books;
        
        public BookService()
        {
            var taroGomi = new Author
            {
                Id = new Guid("d08a13f9-756c-4f25-8e5f-6cb7475e5246")
            };

            var jamesSACorey = new Author
            {
                Id = new Guid("2f775d9a-835e-4674-9b68-8ad29ccba6d6")
            };

            var noAuthor = new Author
            {
                Id = Guid.Empty
            };

            _books = new List<Book>()
            {
                new Book(new Guid("48de5cca-0266-40fc-a34b-22aaaff258f0"), "Everyone Poops", 
                    "Both a matter-of-fact, educational guide and a hilarious romp through poop territory.", 
                    taroGomi, 9781929132140, "Kane/Miller Book Publishers", DateTime.Parse("01/28/1993"), 28),
                new Book(new Guid("af5370ae-33f8-48b9-b5a0-eeb81190181b"), "Leviathan Wakes (Expanse Series #1)",
                    "Humanity has colonized the solar system — Mars, the Moon, the Asteroid Belt and beyond — but the stars are still out of our reach./n/nJim Holden is XO of an ice miner making runs from the rings of Saturn to the mining stations of the Belt. When he and his crew stumble upon a derelict ship, the Scopuli, they find themselves in possession of a secret they never wanted. A secret that someone is willing to kill for — and kill on a scale unfathomable to Jim and his crew. War is brewing in the system unless he can find out who left the ship and why./n/nDetective Miller is looking for a girl. One girl in a system of billions, but her parents have money and money talks. When the trail leads him to the Scopuli and rebel sympathizer Holden, he realizes that this girl may be the key to everything./n/nHolden and Miller must thread the needle between the Earth government, the Outer Planet revolutionaries, and secretive corporations — and the odds are against them. But out in the Belt, the rules are different, and one small ship can change the fate of the universe.", 
                    jamesSACorey, 9780316129084, "Orbit", DateTime.Parse("06/15/2011"), 592),
                new Book(new Guid("85ce8816-5fbf-443a-b512-b37ede78911a"), "Caliban's War (Expanse Series #2)",
                    "We are not alone./n/nOn Ganymede, breadbasket of the outer planets, a Martian marine watches as her platoon is slaughtered by a monstrous supersoldier. On Earth, a high-level politician struggles to prevent interplanetary war from reigniting. And on Venus, an alien protomolecule has overrun the planet, wreaking massive, mysterious changes and threatening to spread out into the solar system./n/nIn the vast wilderness of space, James Holden and the crew of the Rocinante have been keeping the peace for the Outer Planets Alliance.When they agree to help a scientist search war - torn Ganymede for a missing child, the future of humanity rests on whether a single ship can prevent an alien invasion that may have already begun. . .",
                    jamesSACorey, 9780316129060, "Orbit", DateTime.Parse("06/26/2012"), 624),
                new Book(new Guid("b4d706b1-5c58-478e-8158-44ab50759af7"), "Abaddon's Gate (Expanse Series #3)",
                    "For generations, the solar system — Mars, the Moon, the Asteroid Belt — was humanity's great frontier. Until now. The alien artifact working through its program under the clouds of Venus has appeared in Uranus's orbit, where it has built a massive gate that leads to a starless dark./n/nJim Holden and the crew of the Rocinante are part of a vast flotilla of scientific and military ships going out to examine the artifact. But behind the scenes, a complex plot is unfolding, with the destruction of Holden at its core. As the emissaries of the human race try to find whether the gate is an opportunity or a threat, the greatest danger is the one they brought with them./n/nAbaddon's Gate is a breakneck science fiction adventure following the critically acclaimed Caliban's War.",
                    jamesSACorey, 9780316129077, "Orbit", DateTime.Parse("06/04/2013"), 566),

            };
        }

        public Book GetBookById(Guid id)
        {
            return _books.FirstOrDefault(
                b => b.Id == id);
        }

        public Task<Book> GetBookByIdAsync(Guid id)
        {
            return Task.FromResult(
                _books.FirstOrDefault(b => b.Id == id));
        }

        public Task<IEnumerable<Book>> GetBooksAsync()
        {
            return Task.FromResult(
                _books.AsEnumerable());
        }
    }

    public interface IBookService
    {
        Book GetBookById(Guid id);
        Task<Book> GetBookByIdAsync(Guid id);
        Task<IEnumerable<Book>> GetBooksAsync();
    }
}
