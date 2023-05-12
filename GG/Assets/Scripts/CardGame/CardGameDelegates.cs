public delegate void CardAction(CardInfo card);

public delegate int GetPower(CardInfo card);

public delegate CardInfo ChangeCardAction(CardInfo card, int value);

public delegate CardInfo ChangeCard(CardInfo card);

public delegate void GameEvent(CardGameManager manager);

public delegate void AttackEvent(Participant attacker, Participant defender, int damage);