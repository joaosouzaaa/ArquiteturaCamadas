using Bogus;

namespace TestBuilders.BaseBuilders
{
    public abstract class BuilderBase
    {
        protected static int GenerateRandomNumber() => new Random().Next();

        protected static string GenerateRandomWord() => new Faker().Random.Word();
    }
}
