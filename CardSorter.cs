class CardSorter
{

    public class Card
    {
        public string Suit { get; set; }
        public string Value { get; set; }
    }
    private Dictionary<string, int> cardValues = new Dictionary<string, int>()
    {
        {"2",1},
        {"3",2},
        {"4",3},
        {"5",4},
        {"6",5},
        {"7",6},
        {"8",7},
        {"9",8},
        {"J",9},
        {"Q",10},
        {"K",11},
        {"A",12},
        {"Hearts",10},
        {"Diamonds",20},
        {"Clubs",30},
        {"Spades",40}
    };

    public List<Card> sortCards(List<Card> unsortedCards)
    {
        // For values, assume: 2 < 3 < 4 < 5 < 6 < 7 < 8 < 9 < J < Q < K < A
        // For suits, assume: Hearts < Diamonds < Clubs < Spades

        var sorted = false;
        while (!sorted)
        {
            // assume true until proven otherwise
            sorted = true;
            for (int i = 1; i < unsortedCards.Count; ++i)
            {
                Card leftCard = unsortedCards[i - 1];
                Card rightCard = unsortedCards[i];
                int leftCardWeight = getCardWeight(leftCard.Value, leftCard.Suit);
                int rightCardWeight = getCardWeight(rightCard.Value, rightCard.Suit);
                if (leftCardWeight > rightCardWeight) {
                    // need to switch the cards around
                    // assign left slot to right card
                    unsortedCards[i - 1] = rightCard;
                    // assign right slot to left card
                    unsortedCards[i] = leftCard;
                    // because of the switch we're not sure of the full state of the cards
                    // need to set sorted = false
                    sorted = false;
                }
            }
        }
		
		return unsortedCards;
    }

    public int getCardWeight(string val, string suit)
    {
        int cardValueWeight = cardValues[val];
        int cardSuitWeight = cardValues[suit];

        // giving each card a weighted value makes it much easier to sort
        return cardValueWeight + cardSuitWeight;
    }
}