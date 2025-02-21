using System;

public class ComputerResponse
{
    private static readonly string[] Phrases = new[]
    {
        "Привет!",
        "Как дела?",
        "Что нового?",
        "Пока!",
        "Хорошего дня!"
    };

    private static readonly Random Random = new Random();

    public static string GetRandomPhrase()
    {
        return Phrases[Random.Next(Phrases.Length)];
    }
}