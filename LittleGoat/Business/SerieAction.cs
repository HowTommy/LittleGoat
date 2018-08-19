namespace LittleGoat.Business
{
    public enum SerieAction
    {
        PlayerSwitchedCards1And2 = 1,
        PlayerSwitchedCards1And3 = 2,
        PlayerSwitchedCards1And4 = 3,
        PlayerSwitchedCards2And3 = 4,
        PlayerSwitchedCards2And4 = 5,
        PlayerSwitchedCards3And4 = 6,

        PlayerDroppedCardOnIdenticalCard = 10,

        PlayerTookCardAndReturnedCard = 20
    }
}