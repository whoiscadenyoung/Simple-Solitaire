public class Card
{
    // ranks are 1-13: ace, 2-10, jack, queen, king
    public int rank;
    // hearts, diamonds, clubs, spades
    public string suit;
    public string image;

    public Card(int newRank, string newSuit)
    {
        rank = newRank;
        suit = newSuit;
        image = rank.ToString() + "-" + suit;
    }

    public bool CompareTableau(Card bottomCard)
    {
        return (bottomCard.ReturnColor() != this.ReturnColor()) &&
            (this.rank + 1 == bottomCard.rank);
    }

    public string ReturnColor()
    {
        string color;
        if (this.suit == "hearts" || this.suit == "diamonds") color = "red";
        else color = "black";
        return color;
    }

    public bool CompareFoundation(Card bottomCard)
    {
        return this.rank - 1 == bottomCard.rank && this.suit == bottomCard.suit;
    }
}
